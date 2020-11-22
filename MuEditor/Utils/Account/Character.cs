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

        public override string ToString()
        {
            return Name;
        }
    }
}
