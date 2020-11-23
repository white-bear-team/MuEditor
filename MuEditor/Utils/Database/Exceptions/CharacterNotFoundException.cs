using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuEditor.Utils.Database.Exceptions
{
    class CharacterNotFoundException : Exception
    {
        public CharacterNotFoundException()
        { }

        public CharacterNotFoundException(string message)
        { }

        public CharacterNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
