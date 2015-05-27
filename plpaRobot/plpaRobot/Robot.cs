using IronScheme.Runtime;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace plpaRobot
{

    public class Robot
    {
        private int lineWidth = 25;

        private uint _x;
        private uint _y;
        private bool _placed;
        private Direction _direction;

        public Grid Grid;
        private TextBox ProgramOutput;

        public Robot(TextBox ProgramOutput)
        {
            this.ProgramOutput = ProgramOutput;
            Delay = 100;
            Debug = false;
            RobotColor = Brushes.Black;

        }

        public enum Direction
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3
        }

        public void SetRobot(uint x, uint y)
        {
            if (Grid == null)
                return;

            if (_placed)
                GetCanvasFromCoordinates(_x, _y).Children.Clear();

            var canvas = GetCanvasFromCoordinates(x, y);

            var ellipse = new Ellipse
            {
                Fill = RobotColor,
                Width = canvas.ActualWidth,
                Height = canvas.ActualHeight,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };

            canvas.Children.Add(ellipse);

            var width = canvas.ActualWidth / lineWidth;
            var margin = (canvas.ActualWidth / 2 - (width / 2));

            var rect = new Rectangle
            {
                Height = canvas.ActualHeight / 2,
                Width = width,
                Margin = new Thickness(margin, 0, margin, canvas.ActualHeight / 2),
                Fill = Brushes.White,
                RenderTransformOrigin = new Point(0.5, 1)
            };

            Grid.SizeChanged += (sender, args) =>
            {
                var widthEvent = canvas.ActualWidth / lineWidth;
                var marginEvent = (canvas.ActualWidth / 2 - (widthEvent / 2));

                ellipse.Width = canvas.ActualWidth;
                ellipse.Height = canvas.ActualHeight;
                rect.Height = canvas.ActualHeight / 2;
                rect.Width = widthEvent;
                rect.Margin = new Thickness(marginEvent, 0, marginEvent, canvas.ActualHeight / 2);
            };

            canvas.Children.Add(rect);

            _x = x;
            _y = y;
            _placed = true;
            
            if(_direction != Direction.Up)
                SetRobotDirection(_direction);


            canvas.UpdateLayout();
            
        }

        private void SetRobotDirection(Direction direction)
        {
            if (Grid == null)
                return;

            var canvas = GetCanvasFromCoordinates(_x, _y);
            var oldRect = (Rectangle)GetCanvasFromCoordinates(_x, _y).Children.Cast<UIElement>().First(x => x is Rectangle);
            canvas.Children.Remove(oldRect);

            var rect = new Rectangle();
            rect.Fill = Brushes.White;

            if (direction == Direction.Up || direction == Direction.Down)
            {
                var width = canvas.ActualWidth / lineWidth;
                var margin = (canvas.ActualWidth / 2 - (width / 2));

                rect.Height = canvas.ActualHeight/2;
                rect.Width = width;
                rect.Margin = new Thickness(margin, 0, margin, canvas.ActualHeight/2);
                rect.RenderTransformOrigin = new Point(0.5, 1);

                if(direction == Direction.Down)
                    rect.RenderTransform = new RotateTransform(180);
            }
            else
            {
                var width = canvas.ActualWidth/2;
                var heigth = canvas.ActualHeight / lineWidth;
                var margin = canvas.ActualHeight/2 - heigth/2;

                rect.Width = width;
                rect.Height = heigth;
                rect.Margin = new Thickness(canvas.ActualWidth / 2, margin, 0, margin);
                rect.RenderTransformOrigin = new Point(0, 0.5);

                if (direction == Direction.Left)
                    rect.RenderTransform = new RotateTransform(180);
            }

            canvas.Children.Add(rect);
            _direction = direction;
        }



        private Canvas GetCanvasFromCoordinates(uint row, uint column)
        {
            return Grid.Children.OfType<Border>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column).Child as Canvas;

        }

        internal  async void Parser(IronScheme.Runtime.Cons result)
        {
            var list = result.ToList();
            foreach(Object o in list)
            {

                if (o is Cons)
                {
                    var listOfCommands = Schemer.ConvertNestedConsToList((Cons)o);


                    var list2 = ((Cons)o).ToList();

                    foreach (Object command in list2)
                    {
                        ParseOneCons(command);
                        await Task.Delay(Delay);

                    }
                }
                else
                {
                     ParseIronToProgramOutput(o);
                }
            }
        }


        private void ParseOneCons(Object x)
        {

            if (x is Cons)
            {
                Cons d = (Cons)x;


                string cmd = (String)d.car;

                if(Debug)
                ProgramOutput.Text += "Debug:" + d.PrettyPrint;

                if (d.car is String)
                {
                    switch ((String)d.car)
                    {
                        case "pos":
                            SetRobot((Cons)d.cdr);
                            break;
                        case "dir":
                            SetRobotDirection((Cons)d.cdr);
                            break;
                        case "pickup":
                            PickUp((Int32)(((Cons)d.cdr).car));
                            break;
                        case "dropoff":
                            DropOff((Int32)(((Cons)d.cdr).car));
                            break;
                        default:

                            ProgramOutput.Text += "\n" + d.car;
                            break;
                    }

                }
                else
                {
                    ParseIronToProgramOutput(d.car);
                }

            } else
            ParseIronToProgramOutput(x);

        }

        private void DropOff(int p)
        {
            ProgramOutput.Text += "Dropped off: " + CarryingNext + " Pickup up: " + p + "\n";
            Carrying = false;
            CarryingNext = p;
            RobotColor = Brushes.Black;
            SetRobot(_x, _y);
        }

        private void PickUp(int p)
        {
            ProgramOutput.Text += "Picked up: " + CarryingNext + " drop off at: " + p + "\n";
            Carrying = true;
            CarryingNext = p;
            RobotColor = Brushes.Blue;
            SetRobot(_x, _y);

        }

        private void SetRobotDirection(Cons dir)
        {
            var direction = (Direction) ((Int32) dir.car);
            SetRobotDirection(direction);
        }

        private void ParseIronToProgramOutput(Object x)
        {
            if (x is String)
            {
                ProgramOutput.Text += "\n" + (String)x;
            }
            else if (x is int)
            {
                ProgramOutput.Text += "\n" + (int)x;
            }
            else
            {
                ProgramOutput.Text += "wtf: unhandled content : " + x.GetType().ToString() + "\n";
            }
        }

         private   void SetRobot(Cons d)
        {
            try
            {
                SetRobot((UInt32)((Int32)(((Cons)d.cdr).car)), (UInt32)((Int32)d.car));
            }
            catch (Exception e)
            {
                ProgramOutput.Text += "Moving robot error: " + e.Message + "\n";
            }
        }


         public int Delay { get; set; }

         public bool Debug { get; set; }

         public bool Carrying { get; set; }

         public int CarryingNext { get; set; }

         public Brush RobotColor { get; set; }
    }
}
