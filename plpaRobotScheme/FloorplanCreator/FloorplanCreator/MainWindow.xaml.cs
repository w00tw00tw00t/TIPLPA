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

namespace FloorplanCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<List<int>> valuesToParse;
        string path;

        public MainWindow()
        {
            InitializeComponent();
            path = "";
        }

        private void ReloadClick(object sender, RoutedEventArgs e)
        {
            int width;
            int height;
            if (!int.TryParse(WidthBox.Text, out width) || !int.TryParse(HeightBox.Text, out height))
            {
                MessageBox.Show("Use integer values for height and widths only");
                return;
            }

            valuesToParse = new List<List<int>>();
            
            FloorplanGrid.Children.Clear();
            FloorplanGrid.RowDefinitions.Clear();
            FloorplanGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < width; i++)
            {
                valuesToParse.Add(new List<int>());
                GridLength g = new GridLength(1, GridUnitType.Star);
                ColumnDefinition coldef = new ColumnDefinition
                {
                    Width = g
                };

                FloorplanGrid.ColumnDefinitions.Add(coldef);
            }

            for (int i = 0; i < height; i++)
            {
                
                GridLength g = new GridLength(1,GridUnitType.Star);
                RowDefinition rowdef = new RowDefinition
                {
                    Height = g   
                };

                FloorplanGrid.RowDefinitions.Add(rowdef);
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Border b = new Border
                    {
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(1),
                    };


                    Canvas c = new Canvas
                    {
                        Background = new SolidColorBrush(Colors.White),
                       
                    };

                    c.MouseDown +=c_MouseDown;

                    Grid.SetRow(c, i);
                    Grid.SetColumn(c, j);
                    Grid.SetRow(b, i); 
                    Grid.SetColumn(b, j);
                    FloorplanGrid.Children.Add(c);
                    FloorplanGrid.Children.Add(b);
                }
            }
        }

        private void c_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas c = (Canvas)sender;
            if (e.ChangedButton == MouseButton.Left && TileValueBox.SelectedItem != null)
            {
                var textblocksToDelete = FloorplanGrid.Children.OfType<TextBlock>().Where(i => Grid.GetRow(i) == Grid.GetRow(c) && Grid.GetColumn(i) == Grid.GetColumn(c));
                int count = textblocksToDelete.Count();
                for (int i = 0; i < count; i++)
                {
                    FloorplanGrid.Children.Remove(textblocksToDelete.ElementAt(0));
                } 
                c.Background = ((ComboBoxItem)TileValueBox.SelectedItem).Background;
            }
            else
            {
                int children = FloorplanGrid.Children.Cast<UIElement>().Count(i => Grid.GetRow(i) == Grid.GetRow(c) && Grid.GetColumn(i) == Grid.GetColumn(c));
                ContextMenu context = new ContextMenu();
                if(children > 2)
                {
                    MenuItem addInput = new MenuItem{
                        Header = "Add input"};
                    addInput.Click += addInput_Click;
                    MenuItem addOutput = new MenuItem
                    {
                        Header = "Add output"
                    };
                    MenuItem removeInput = new MenuItem
                    {
                        Header = "Remove input"
                    };
                    MenuItem removeOutput = new MenuItem
                    {
                        Header = "Remove output"
                    };
                    MenuItem removeNumber = new MenuItem
                    {
                        Header = "Remove number"
                    };
                    addOutput.Click += addOutput_Click;
                    addInput.Click += addInput_Click;
                    removeInput.Click += removeInput_Click;
                    removeOutput.Click += removeOutput_Click;
                    removeNumber.Click += removeNumber_Click;

                    context.Items.Add(addInput);
                    context.Items.Add(addOutput);
                    context.Items.Add(removeInput);
                    context.Items.Add(removeOutput);
                    context.Items.Add(removeNumber);
                    c.ContextMenu = context;

                    //
                    //MenuItem RemoveInput = new MenuItem("Remove output");
                    MenuItem RemoveNumber;
                }
                int ws;
                if (int.TryParse(WorkspaceBox.Text, out ws) && ws >= 0 && ws < 5)
                    loadWorkspace(c, ws.ToString());
                else
                    MessageBox.Show("Only use workspaces from 0 to 4 and integer values only");
            }
           
        }

        private void removeNumber_Click(object sender, RoutedEventArgs e)
        {
            Canvas c = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Canvas;
            try
            {
                TextBlock numberBlock = FloorplanGrid.Children.OfType<TextBlock>().Where(i =>
                    Grid.GetColumn(i) == Grid.GetColumn(c) && Grid.GetRow(i) == Grid.GetRow(c) && i.GetValue(FrameworkElement.NameProperty).ToString() == "wsnr").ElementAt<TextBlock>(0);
                FloorplanGrid.Children.Remove(numberBlock);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void removeOutput_Click(object sender, RoutedEventArgs e)
        {
            Canvas c = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Canvas;
            try
            {
                TextBlock outputblock = FloorplanGrid.Children.OfType<TextBlock>().Where(i =>
                    Grid.GetColumn(i) == Grid.GetColumn(c) && Grid.GetRow(i) == Grid.GetRow(c) && i.GetValue(FrameworkElement.NameProperty).ToString() == "output").ElementAt<TextBlock>(0);
                FloorplanGrid.Children.Remove(outputblock);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void removeInput_Click(object sender, RoutedEventArgs e)
        {
            Canvas c = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Canvas;
            try
            {
                TextBlock inputBlock = FloorplanGrid.Children.OfType<TextBlock>().Where(i =>
                    Grid.GetColumn(i) == Grid.GetColumn(c) && Grid.GetRow(i) == Grid.GetRow(c) && i.GetValue(FrameworkElement.NameProperty).ToString() == "input").ElementAt<TextBlock>(0);
                FloorplanGrid.Children.Remove(inputBlock);
            }
            catch (Exception ex)
            {
                return;
            }
            
        }

        void addOutput_Click(object sender, RoutedEventArgs e)
        {
            Canvas c = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Canvas;
            if (FloorplanGrid.Children.Cast<UIElement>().Count(i => 
                i.GetValue(FrameworkElement.NameProperty).ToString() == "output" && Grid.GetColumn(i) == Grid.GetColumn(c) && Grid.GetRow(i) == Grid.GetRow(c)) != 0)
                return;
            TextBlock inputBlock = new TextBlock
            {
                Name = "output",
                Text = "^",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 25, 0, 0)
            };

            Grid.SetRow(inputBlock, Grid.GetRow(c));
            Grid.SetColumn(inputBlock, Grid.GetColumn(c));
            FloorplanGrid.Children.Add(inputBlock);
        }

        void addInput_Click(object sender, RoutedEventArgs e)
        {

            Canvas c = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Canvas;
            if (FloorplanGrid.Children.Cast<UIElement>().Count(i =>
                i.GetValue(FrameworkElement.NameProperty).ToString() == "input" && Grid.GetColumn(i) == Grid.GetColumn(c) && Grid.GetRow(i) == Grid.GetRow(c)) != 0)
                return;
            TextBlock inputBlock = new TextBlock
            {
                Name = "input",
                Text = "v",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0,25,0,0)
            };

            Grid.SetRow(inputBlock, Grid.GetRow(c));
            Grid.SetColumn(inputBlock, Grid.GetColumn(c));
            FloorplanGrid.Children.Add(inputBlock);
 
           
        }

        private void loadWorkspace(Canvas C, string wsNr)
        {
            int col = Grid.GetColumn(C);
            int row = Grid.GetRow(C);
            
            // Set number space
            int children = FloorplanGrid.Children.Cast<UIElement>().Count(i => Grid.GetRow(i) == row && Grid.GetColumn(i) == col);
            if (children > 2 || C.Background !=  Brushes.Yellow)
                return;

            TextBlock t = new TextBlock
            {
                Name = "wsnr",
                Text = wsNr,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
            Grid.SetColumn(t, col);
            Grid.SetRow(t, row);
            FloorplanGrid.Children.Add(t);
            if (row < FloorplanGrid.RowDefinitions.Count - 1)
            {
                Canvas nc = FloorplanGrid.Children.OfType<Canvas>().Where(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col).ElementAt<Canvas>(0);
                loadWorkspace(nc, wsNr);
            }
            if (row > 0)
            {
                Canvas nc = (Canvas)FloorplanGrid.Children.OfType<Canvas>().Where(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col).ElementAt<Canvas>(0);
                loadWorkspace(nc, wsNr);
            }
            if (col < FloorplanGrid.ColumnDefinitions.Count - 1)
            {
                Canvas nc = (Canvas)FloorplanGrid.Children.OfType<Canvas>().Where(i => Grid.GetRow(i) == row && Grid.GetColumn(i) == col + 1).ElementAt<Canvas>(0);
                loadWorkspace(nc, wsNr);
            }
            if (col > 0)
            {
                Canvas nc = (Canvas)FloorplanGrid.Children.OfType<Canvas>().Where(i => Grid.GetRow(i) == row && Grid.GetColumn(i) == col - 1).ElementAt<Canvas>(0);
                loadWorkspace(nc, wsNr);
            }
            return;
                      
        }

        private void saveas_click(object sender, RoutedEventArgs e)
        {
            SaveAndLoadFloorplan save = new SaveAndLoadFloorplan() ;
            path = save.Save(FloorplanGrid);
            if (path != "")
            {
                Uri temp = new Uri(path);
                Title = "Floorplan builder - " + System.IO.Path.GetFileName(temp.LocalPath);
            }
        }

        private void load_click(object sender, RoutedEventArgs e)
        {
           
        }

        private void save_click(object sender, RoutedEventArgs e)
        {
            if (path == "")
            {
                saveas_click(sender, e);
            }
            else
            {
                SaveAndLoadFloorplan save = new SaveAndLoadFloorplan();
                save.Save(FloorplanGrid, path);
            }

        }
    }
}
