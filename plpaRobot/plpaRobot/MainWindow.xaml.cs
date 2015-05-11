using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using plpaRobot.Enumerables;

namespace plpaRobot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Robot _robot;

        public MainWindow()
        {
            InitializeComponent();

            _robot = new Robot();

            var data = GetDummyData();// Do somthing to convert scheme from filepath to int[,]
            CreateFloorPlan(data);
            _robot.SetRobot(5,1);
        }

        private void RunProgramClicked(object sender, RoutedEventArgs e)
        {
            //var content = ProgramEditor.Text;

            //MessageBox.Show(content);
            //_robot.SetRobot(1, 5);

            var message = ProgramEditor.Text;

            switch (message)
            {
                case "spawn":
                    _robot.SetRobot(5, 1);
                    break;
                case "up":
                case "right":
                case "down":
                case "left":
                    _robot.MoveRobotAbsolute(message);
                    break;
                default:
                    _robot.MoveRobotRelative(message);
                    break;
            }
        }

        private void Menu_Open(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Scheme files (*.ss, *.scm)|*.ss;*.scm|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
            };

            if (openFileDialog.ShowDialog() != true) 
                return;

            var data = GetDummyData();// Do somthing to convert scheme from filepath to int[,]
            CreateFloorPlan(data);
        }

        private void Menu_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CreateFloorPlan(int[,] data)
        {
            var child = FindName("FloorPlan");
            if (child != null)
            {
                RemoveLogicalChild(child);
            }

            var newFloorPlan = new Grid
            {
                Margin = new Thickness(10, 10, 10, 10)
            };

            for (int i = 0; i < data.GetLength(0); i++)
            {
                newFloorPlan.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < data.GetLength(1); i++)
            {
                newFloorPlan.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int l = 0; l < data.GetLength(1); l++)
                {
                    var canvas = new Canvas();

                    switch ((FloorTypeEnum)data[i, l])
                    {
                        case FloorTypeEnum.Path:
                            canvas.Background = Brushes.Green;
                            break;
                        case FloorTypeEnum.Parking:
                            canvas.Background = Brushes.Red;
                            break;
                        case FloorTypeEnum.Workstation:
                            canvas.Background = Brushes.Yellow;
                            break;
                    }

                    Grid.SetRow(canvas, i);
                    Grid.SetColumn(canvas, l);

                    var border = new Border
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Black,
                        Child = canvas
                    };

                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, l);

                    newFloorPlan.Children.Add(border);
                }
            }

            _robot.Grid = newFloorPlan;
            Grid.SetColumn(newFloorPlan, 0);
            ContentGrid.Children.Add(newFloorPlan);
        }

        // Test Data!!!!!!
        private int[,] GetDummyData()
        {
            return new [,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 3, 3, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0},
                { 0, 0, 3, 3, 1, 0, 0, 3, 3, 3, 1, 0, 0, 0, 0},
                { 0, 0, 3, 3, 1, 0, 0, 3, 3, 3, 1, 0, 0, 0, 0},
                { 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0},
                { 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                { 0, 2, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0},
                { 0, 2, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 3, 0, 0},
                { 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 3, 3, 0, 0},
                { 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 3, 3, 0, 0},
                { 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0},
                { 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0},
                { 0, 0, 0, 0, 1, 0, 0, 3, 3, 3, 1, 0, 0, 0, 0},
                { 0, 0, 0, 0, 1, 0, 0, 3, 3, 3, 1, 0, 0, 0, 0},
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                { 0, 0, 3, 3, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                { 0, 0, 3, 3, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
                { 0, 0, 3, 3, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            };
            
        }
    }
}
