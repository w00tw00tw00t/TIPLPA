using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace plpaRobot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RunProgramClicked(object sender, RoutedEventArgs e)
        {
            var content = ProgramEditor.Text;

            MessageBox.Show(content);
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
            var child = this.FindName("FloorPlan");
            if (child != null)
            {
                this.RemoveLogicalChild(child);
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
                    var txt = new TextBlock
                    {
                        Text = " "
                    };

                    switch (data[i, l])
                    {
                        case 1:
                            txt.Background = Brushes.Green;
                            break;
                        case 2:
                            txt.Background = Brushes.Red;
                            break;
                        case 3:
                            txt.Background = Brushes.Yellow;
                            break;
                    }

                    Grid.SetRow(txt, i);
                    Grid.SetColumn(txt, l);

                    var border = new Border
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Black,
                        Child = txt
                    };

                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, l);

                    newFloorPlan.Children.Add(border);
                }
            }

            Grid.SetColumn(newFloorPlan, 0);
            //ContentGrid.Children.Add(border);
            ContentGrid.Children.Add(newFloorPlan);
        }

        // Test Data!!!!!!
        private int[,] GetDummyData()
        {
            return new int[,]
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
