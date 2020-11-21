using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuEditor.Utils.Items
{
    public class Item
    {
        public int Category;
        public int Id;
        public string Name;
        public int X;
        public int Y;
        public bool TwoHanded;


        public string ShowString()
        {
            return ("Item Name" + Name + " Item category: " + Category.ToString());
        }
    }
}
