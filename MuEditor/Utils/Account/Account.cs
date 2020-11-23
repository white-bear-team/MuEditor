using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuEditor.Utils.Account
{
    public class Account
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public string Online { get; set; }

        public Account() { }
        public Account(string AccountName, string AccountPassword, string Email)
        {
            this.Name = AccountName;
            this.Password = AccountPassword;
            this.Email = Email;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
