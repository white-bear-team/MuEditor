using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuEditor.Utils.Account
{
    public class Character
    {
        public string Name { get; set; }
        public int Class { get; set; }

        public Character() { }
        public Character(string Name, int Class)
        {
            this.Name = Name;
            this.Class = Class;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
