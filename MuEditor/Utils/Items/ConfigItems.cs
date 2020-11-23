using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace MuEditor.Utils.Items
{
    public class ConfigItems
    {


        const string CONFIG_SEPARATOR = "\t"; //Don't forget to change that if you are changing config
        const int NAME_POSITION = 8;
        const int WIDTH_POSITION = 3;
        const int HEIGHT_POSITION = 4;
        const int TWO_HANDED_POSITION = 9;
        const int ID_POSITION = 0;

        public static List<Item> Load()
        {
            List<Item> items = new List<Item>();
            int category = -1;
            try
            {
                foreach (string line in File.ReadLines("Items.txt"))
                {
                    if (line.StartsWith("//"))
                    {
                        continue;
                    }
                    if (line.StartsWith("end"))
                    {
                        category = -1;
                        continue;
                    }
                    else
                    {
                        string catline = line.Split(CONFIG_SEPARATOR.ToCharArray())[0];
                        if (category == -1 && !catline.Equals("end"))
                        {
                            MessageBox.Show("Category is negative");
                            category = int.Parse(catline);
                            continue;
                        }
                        if (!catline.Equals("end") && category != -1)
                        {
                            Item item = new Item();
                            item.Category = category;
                            string[] splitContent = line.Split(CONFIG_SEPARATOR.ToCharArray());
                            int id = int.Parse(splitContent[ID_POSITION]);
                            item.Name = splitContent[NAME_POSITION];
                            item.Id = int.Parse(splitContent[ID_POSITION]);
                            item.Width = int.Parse(splitContent[WIDTH_POSITION]);
                            item.Height = int.Parse(splitContent[HEIGHT_POSITION]);
                            item.TwoHanded = Convert.ToBoolean(int.Parse(splitContent[TWO_HANDED_POSITION]));
                            items.Add(item);
                        }
                    }
                }
                return items;
            }catch(Exception)
            {
                MessageBox.Show("Please check that there is Items.txt file.", "[Mu Editor] File error.");
                return items;
            }
        }
    }
}
