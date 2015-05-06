using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace plpaRobot
{
    public class Robot
    {
        private uint _x;
        private uint _y;
        private bool _placed;

        public Grid Grid;

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
                Width = 20,
                Height = 20,
                Stretch = Stretch.Fill
            };

            canvas.Children.Add(ellipse);

            _x = x;
            _y = y;
            _placed = true;
        }

        public void MoveRobotAbsolute(string message)
        {
            if(message == "spawn")
                SetRobot(5, 1);

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

        private Canvas GetCanvasFromCoordinates(uint row, uint column)
        {
            return Grid.Children.Cast<Border>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column).Child as Canvas;
        }

    }
}
