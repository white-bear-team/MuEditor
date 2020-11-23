using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuEditor.Utils.Database.Exceptions
{
    [Serializable]
    public class AccountAlreadyExistsException : Exception
    {
        public AccountAlreadyExistsException()
        { }

        public AccountAlreadyExistsException(string message)
        { }

        public AccountAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
