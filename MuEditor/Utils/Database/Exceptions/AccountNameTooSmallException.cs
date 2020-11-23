using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuEditor.Utils.Database.Exceptions
{
    class AccountParametrSizeException : Exception
    {
        public AccountParametrSizeException()
        { }

        public AccountParametrSizeException(string message)
        { }

        public AccountParametrSizeException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
