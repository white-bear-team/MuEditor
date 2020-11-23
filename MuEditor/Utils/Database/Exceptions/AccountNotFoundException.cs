using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuEditor.Utils.Database.Exceptions
{
    class AccountNotFoundException : Exception
    {
        public AccountNotFoundException()
        { }

        public AccountNotFoundException(string message)
        { }

        public AccountNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
