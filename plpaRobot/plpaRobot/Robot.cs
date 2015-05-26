﻿using IronScheme.Runtime;
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
                Fill = Brushes.Black,
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
                /*var width = canvas.ActualHeight / 25;
                var margin = (canvas.ActualHeight / 2 - (width / 2));
                var top = canvas.ActualHeight/2;
                var bottom = canvas.ActualWidth/2;

                rect.Height = canvas.ActualHeight / 2;
                rect.Width = width;
                rect.Margin = new Thickness(margin, top, margin, bottom);
                rect.Fill = Brushes.White;
                rect.RenderTransformOrigin = new Point(0.5, 1);

                rect.RenderTransform = direction == Direction.Right ? new RotateTransform(90) : new RotateTransform(270);*/

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


        public void MoveRobotAbsolute(string message)
        {
            if (!_placed || Grid == null)
                return;

            switch (message)
            {
                case "up":
                    if (_x == 0)
                        break;
                    SetRobot(_x - 1, _y);
                    break;
                case "left":
                    if (_y == 0)
                        break;
                    SetRobot(_x, _y - 1);
                    break;
                case "down":
                    if (_x == Grid.RowDefinitions.Count - 1)
                        break;
                    SetRobot(_x + 1, _y);
                    break;
                case "right":
                    if (_y == Grid.ColumnDefinitions.Count - 1)
                        break;
                    SetRobot(_x, _y + 1);
                    break;
            }
        }

        public void MoveRobotRelative(string message)
        {
            if (!_placed || Grid == null)
                return;

            if (message == "forward")
            {
                switch (_direction)
                {
                    case Direction.Down:
                        MoveRobotAbsolute("down");
                        break;
                    case Direction.Left:
                        MoveRobotAbsolute("left");
                        break;
                    case Direction.Right:
                        MoveRobotAbsolute("right");
                        break;
                    case Direction.Up:
                        MoveRobotAbsolute("up");
                        break;
                }

                SetRobotDirection(_direction);
            }
            else if (message == "backward")
            {
                switch (_direction)
                {
                    case Direction.Down:
                        MoveRobotAbsolute("up");
                        break;
                    case Direction.Left:
                        MoveRobotAbsolute("right");
                        break;
                    case Direction.Right:
                        MoveRobotAbsolute("left");
                        break;
                    case Direction.Up:
                        MoveRobotAbsolute("down");
                        break;
                }

                SetRobotDirection(_direction);
            }
            else if (message == "turnl")
            {
                switch (_direction)
                {
                    case Direction.Down:
                        SetRobotDirection(Direction.Right);
                        break;
                    case Direction.Left:
                        SetRobotDirection(Direction.Down);
                        break;
                    case Direction.Right:
                        SetRobotDirection(Direction.Up);
                        break;
                    case Direction.Up:
                        SetRobotDirection(Direction.Left);
                        break;
                }
            }
            else if (message == "turnr")
            {
                switch (_direction)
                {
                    case Direction.Down:
                        SetRobotDirection(Direction.Left);
                        break;
                    case Direction.Left:
                        SetRobotDirection(Direction.Up);
                        break;
                    case Direction.Right:
                        SetRobotDirection(Direction.Down);
                        break;
                    case Direction.Up:
                        SetRobotDirection(Direction.Right);
                        break;
                }
            }
        }

        private Canvas GetCanvasFromCoordinates(uint row, uint column)
        {
            return Grid.Children.OfType<Border>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column).Child as Canvas;

        }


        internal async void SetRobot(IronScheme.Runtime.Cons result)
        {
            var list = result.ToList();

            foreach (Object x in list)
            {
                if (x is Cons)
                {
                    Cons d = (Cons)x;
                    ProgramOutput.Text += "\nDebug:" + d.PrettyPrint;

                    if(d.car is String)
                    {
                        switch((String) d.car)
                        {
                            case "pos":
                                    SetRobotReal((Cons)d.cdr);
                                    break;
                            case "dir":
                                    break;
                            default:
                                
                                    ProgramOutput.Text += "\n" + d.car;
                                    break;
                        }

                    } else {
                       ParseIronToProgramOutput(d.car);
                    }

                }
               ParseIronToProgramOutput(x);

                await Task.Delay(100);
            }
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
                ProgramOutput.Text += "\n" + "wtf: unhandled content";
            }
        }

        private void SetRobotReal(Cons d)
        {
            try
            {
                SetRobot((UInt32)((Int32)(((Cons)d.cdr).car)), (UInt32)((Int32)d.car));
            }
            catch (Exception e)
            {
                ProgramOutput.Text += "\n" + "Moving robot error: " + e.Message;
            }
        }
            
    }
}
