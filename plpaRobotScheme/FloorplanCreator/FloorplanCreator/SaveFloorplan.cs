using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FloorplanCreator
{
    class SaveAndLoadFloorplan
    {
        Dictionary<string,int> dictionary;

        public SaveAndLoadFloorplan()
        {
            dictionary = new Dictionary<string, int>();
            createDictionary();
            
        }

        private void createDictionary()
        {

            dictionary.Add(Colors.White.ToString(),0);
            dictionary.Add(Colors.Green.ToString(),1);
            dictionary.Add(Colors.Red.ToString(),2);
            dictionary.Add(Colors.Yellow.ToString() + "0",3);
            dictionary.Add(Colors.Yellow.ToString() + "1", 4);
            dictionary.Add(Colors.Yellow.ToString() + "2", 5);
            dictionary.Add(Colors.Yellow.ToString() + "3", 6);
            dictionary.Add(Colors.Yellow.ToString() + "4", 7);
            dictionary.Add(Colors.Yellow.ToString() + "0v", 8);
            dictionary.Add(Colors.Yellow.ToString() + "0^", 9);
            dictionary.Add(Colors.Yellow.ToString() + "1v", 10);
            dictionary.Add(Colors.Yellow.ToString() + "1^", 11);
            dictionary.Add(Colors.Yellow.ToString() + "2v", 12);
            dictionary.Add(Colors.Yellow.ToString() + "2^", 13);
            dictionary.Add(Colors.Yellow.ToString() + "3v", 14);
            dictionary.Add(Colors.Yellow.ToString() + "3^", 15);
            dictionary.Add(Colors.Yellow.ToString() + "4v", 16);
            dictionary.Add(Colors.Yellow.ToString() + "4^", 17);
        }




        private List<List<int>> parseToLists(Grid floorplan)
        {
            List<List<int>> lists = new List<List<int>>();
            foreach (RowDefinition r in floorplan.RowDefinitions)
            {
                List<int> column = new List<int>();
                foreach (ColumnDefinition c in floorplan.ColumnDefinitions)
                {
                    string dictionaryKey = "";
                    List<UIElement> controls = 
                        floorplan.Children.Cast<UIElement>().Where(i => 
                            floorplan.RowDefinitions[Grid.GetRow(i)] == r && floorplan.ColumnDefinitions[Grid.GetColumn(i)] == c).ToList();
                    dictionaryKey += controls.Where(i => i.GetType() == typeof(Canvas)).Cast<Canvas>().ElementAt(0).Background;
                    
                    List<TextBlock> textblocks = controls.Where(i => i.GetType() == typeof(TextBlock)).Cast<TextBlock>().ToList();
                    if(textblocks.Where(i => i.Name == "wsnr").Count() > 0)
                        dictionaryKey+= textblocks.Where(i => i.Name == "wsnr").ElementAt(0).Text;
                    if(textblocks.Where(i => i.Name == "output").Count() > 0)
                        dictionaryKey+="^";
                    else if(textblocks.Where(i => i.Name == "input").Count() > 0)
                        dictionaryKey+="v";

                   int colVal;
                   if(dictionary.TryGetValue(dictionaryKey, out colVal))
                       column.Add(colVal);
                   else
                   {
                       lists.Clear();
                       return lists;
                   }            
                }
                lists.Add(column);
            }
            return lists;
        }

        public string Save(Grid floorplan, string path = "")
        {
            string fileName = "";
            List<List<int>> valuesToSave = parseToLists(floorplan);
            if (path == "")
            {
                SaveFileDialog saveDia = new SaveFileDialog();
                saveDia.Filter = "Scheme file | *.ss";
                saveDia.Title = "Save floorplan as scheme file";
                saveDia.ShowDialog();
                fileName = saveDia.FileName;
            }
            else
            {
                fileName = path;
            }

            if (fileName != "")
            {
                System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate);
                string toWrite = "(define floorplan '(\n";
                foreach (List<int> column in valuesToSave)
                {
                    toWrite+= "(";
                    foreach (int val in column)
                    {
                        toWrite += val.ToString() + " ";
                    }
                    toWrite+= ")\n";
                }
                toWrite += "))";
                byte[] toWriteByte = Encoding.ASCII.GetBytes(toWrite);
                fs.Write(toWriteByte,0,toWriteByte.Length);
                fs.Close();
                return fileName;
            }
            else
                return "";
        }

        
    }
}
