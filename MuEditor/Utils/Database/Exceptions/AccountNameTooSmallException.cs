using System;

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
