﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using Microsoft.Win32;
using plpaRobot.Enumerables;
using System.Text.RegularExpressions;
using IronScheme;
using IronScheme.Runtime;

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

            Schemer.doImports();
            Schemer.loadSchemeFile("SchemeFiles/robotactions.ss");

            _robot = new Robot();
            
            object test = Schemer.Eval("(string-split \"do re mi\" \" \")");
            //var data = GetDummyData();// Do somthing to convert scheme from filepath to int[,]
            //CreateFloorPlan(data);
            //_robot.SetRobot(5,1);
        }

        private void RunProgramClicked(object sender, RoutedEventArgs e)
        {
            //var content = ProgramEditor.Text;

            //MessageBox.Show(content);
            //_robot.SetRobot(1, 5);
            string text = "(string-split \"" + ProgramEditor.Text + "\" \n)";

            //object a = Schemer.Eval("(evalFunctionInString \"(moveRight)\")");
            object tet = Schemer.Eval("(runProgram \"" + ProgramEditor.Text + "\")");
            
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

            Schemer.loadSchemeFile(openFileDialog.FileName);
            string floorplan = (Schemer.Eval("floorplan") as IronScheme.Runtime.Cons).PrettyPrint;


            int[,] floorplanvalues = ParseFloorplan(floorplan);



            CreateFloorPlan(floorplanvalues);
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



                    TextBlock wstext = new TextBlock
                    {
                        FontSize = 10,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    };
                    Panel.SetZIndex(wstext, 100);
                    Grid.SetColumn(wstext, l);
                    Grid.SetRow(wstext, i);

                    switch ((FloorTypeEnum)data[i, l])
                    {
                        case FloorTypeEnum.Path:
                            canvas.Background = Brushes.Green;
                            break;
                        case FloorTypeEnum.Parking:
                            canvas.Background = Brushes.Red;
                            break;
                        case FloorTypeEnum.ws0:
                            
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "0";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws1:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "1";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws2:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "2";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws3:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "3";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws4:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "4";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws0drop:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "0 d";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws0pick:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "0 p";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws1drop:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "1 d";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws1pick:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "1 p";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws2drop:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "2 d";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws2pick:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "2 p";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws3drop:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "3 d";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws3pick:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "3 p";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws4drop:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "4 d";
                            newFloorPlan.Children.Add(wstext);
                            break;
                        case FloorTypeEnum.ws4pick:
                            canvas.Background = Brushes.Yellow;
                            wstext.Text = "4 p";
                            newFloorPlan.Children.Add(wstext);
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
            var coords = ((Cons)Schemer.Eval("(initRobot 0 0)"));
            var a = coords.cdr.GetType();
            _robot.SetRobot((uint)((Int32)((Cons)coords.cdr).car),(uint)((Int32)coords.car));
        }

        private int[,] ParseFloorplan(string floorplanstring)
        {
            string[] rows = Regex.Matches(floorplanstring, @"\(([^\)]+)\)")
                .Cast<Match>()
                .Select(x => x.Value.TrimStart('(').TrimEnd(')'))
                .ToArray();

            int[,] floorplanvalues = new int[rows.Length, rows[0].Split(' ').Length];

            for(int i = 0; i < rows.Length; i++)
            {
                string[] rowsplit = rows[i].Split(' ');
                for (int j = 0; j < rowsplit.Length; j++)
                {
                    floorplanvalues[i, j] = int.Parse(rowsplit[j]);
                }
            }

            return floorplanvalues;
           
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
