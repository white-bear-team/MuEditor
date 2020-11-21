using MuEditor.Utils.Account;
using System.Collections.Generic;

namespace MuEditor
{
    public class DbModel
    {
        public static List<Account> GetAccounts()
        {
            List<Account> accounts = new List<Account>();
            DbLite.DbU.Read("select memb___id from MEMB_INFO order by memb___id");
            while (DbLite.DbU.Fetch())
            {
                Account account = new Account();
                account.AccountName = DbLite.DbU.GetAsString("memb___id");
                accounts.Add(account);
            }
            DbLite.DbU.Close();
            return accounts;
        }

        public static Account GetAccount(string accountName)
        {
            Account account = new Account();
            DbLite.DbU.Read("select memb_guid, memb__pwd,mail_addr,sno__numb from MEMB_INFO where memb___id = '" + accountName + "'");
            DbLite.DbU.Fetch();
            account.AccountPassword = DbLite.DbU.GetAsString("memb__pwd");
            account.Email = DbLite.DbU.GetAsString("mail_addr");
            account.Id = DbLite.DbU.GetAsString("sno__numb");
            return account;
        }

        public static void RemoveAccount(string account)
        {   
            DbLite.DbU.Exec("delete from MEMB_INFO where memb___id = '" + account + "'");
            DbLite.DbU.Close();
            DbLite.Db.Exec("delete from Character where AccountID = '" + account + "'");
            DbLite.Db.Close();
            DbLite.Db.Exec("delete from AccountCharacter where Id = '" + account + "'");
            DbLite.Db.Close();
            DbLite.Db.Exec("delete from warehouse where AccountID = '" + account + "'");
            DbLite.Db.Close();
        }

        public static string AddAccount(Account account)
        {
            int num1 = DbLite.DbU.ExecWithResult("select count(*) from MEMB_INFO where memb___id = '" + account.AccountName + "'");
            if (account.AccountName.Length < 2)
            {
                DbLite.DbU.Close();
                return "Could not create an account, check name field";
            }
            else if (account.AccountPassword.Length < 2)
            {
                DbLite.DbU.Close();
                return "Could not create an account, check password field";
            }
            else if (num1 > 0)
            {
                DbLite.DbU.Close();
                return "Could not create account because the name is occupied";
            }
            else
            {
                DbLite.DbU.Close();
                bool a = DbLite.DbU.Exec("insert into MEMB_INFO (memb___id,memb__pwd,memb_name,sno__numb,mail_addr,fpas_ques,fpas_answ,appl_days,modi_days,out__days,true_days,mail_chek,bloc_code,ctl1_code) values ('" + account.AccountName + "','" + account.AccountPassword + "','','1','" + account.Email + "','','','20140101','20140101','20140101','20140101','1','0','0')");
                DbLite.DbU.Close();
                if (a)
                    return "Account was created";
                else
                    return "Could not create an account";
            }
        }
        public static List<Character> GetCharacters(string user)
        {
            List<Character> characters = new List<Character>();
            DbLite.Db.Read("select Name from Character where AccountID = '" + user + "' order by Name");
            while (DbLite.Db.Fetch())
            {
                Character character = new Character();
                character.CharacterName = DbLite.Db.GetAsString("Name");
                characters.Add(character);
            }
            DbLite.Db.Close();
            return characters;
        }

        public static List<string> SearchAccounts(string input)
        {
            List<string> accounts = new List<string>();
            DbLite.Db.Read("select memb___id from MEMB_INFO where memb___id Like '" + "%" + input + "%' order by memb___id");
            while (DbLite.Db.Fetch())
            {
                accounts.Add(DbLite.Db.GetAsString("memb___id"));
            }
            DbLite.Db.Close();
            return accounts;
        }

        public static List<string> SearchCharacters(string input)
        {
            List<string> characters = new List<string>();
            DbLite.Db.Read("select Name from Character where Name Like '" + "%" + input + "%' order by Name");
            while (DbLite.Db.Fetch())
            {
                characters.Add(DbLite.Db.GetAsString("Name"));
            }
            return characters;
        }

        public static string GetUserByCharName(string charName)
        {
            string account = "";

            return account;
        }

        
    }
}