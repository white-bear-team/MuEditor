using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuEditor
{
    class IdWithNameModel
    {
        private KeyValuePair<int, string> keyValuePair;

        public IdWithNameModel(int id, string name)
        {
            keyValuePair = new KeyValuePair<int, string>(id, name);
        }

        public int GetId()
        {
            return keyValuePair.Key;
        }

        public override string ToString()
        {
            return keyValuePair.Value;
        }
    }
}