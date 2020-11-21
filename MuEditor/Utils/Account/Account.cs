using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuEditor.Utils.Account
{
    public class Account
    {
        public string AccountName;
        public string AccountPassword;
        public string Email;
        public string Id;

        public Account() { }
        public Account(string AccountName, string AccountPassword, string Email)
        {
            this.AccountName = AccountName;
            this.AccountPassword = AccountPassword;
            this.Email = Email;
        }
    }
}
