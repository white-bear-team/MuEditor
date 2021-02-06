using System;

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
