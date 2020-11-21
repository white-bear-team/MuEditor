using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MuEditor.Utils.Items
{
    public class Items
    {
        public static List<Item> items = new List<Item>();
        public static void Load()
        {
            int category = -1;
            try
            {
                foreach (string line in File.ReadLines("Items.txt"))
                {
                    string sep = "\t";
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
                        string catline = line.Split(sep.ToCharArray())[0];
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
                            string[] splitContent = line.Split("\t".ToCharArray());
                            int id = int.Parse(splitContent[0]);
                            item.Name = splitContent[8];
                            item.Id = int.Parse(splitContent[0]);
                            item.X = int.Parse(splitContent[3]);
                            item.Y = int.Parse(splitContent[4]);
                            item.TwoHanded = Convert.ToBoolean(int.Parse(splitContent[9]));
                            items.Add(item);
                        }
                    }
                }
                foreach (Item item in items)
                {
                    if (item.Category == 11)
                        MessageBox.Show(item.ShowString());
                }
            }catch(Exception ee)
            {
                MessageBox.Show("File doesn't exist");
            }
        }
    }
}
