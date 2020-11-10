using System.Collections.Generic;

namespace MuEditor
{
    public class DbModel
    {
        public static List<string> GetAccounts()
        {
            List<string> accounts = new List<string>();
            DbLite.DbU.Read("select memb___id from MEMB_INFO order by memb___id");
            while (DbLite.DbU.Fetch())
            {
                accounts.Add(DbLite.DbU.GetAsString("memb___id"));
            }
            DbLite.DbU.Close();
            return accounts;
        }

        public static List<string> GetCharacters(string user)
        {
            List<string> characters = new List<string>();
            DbLite.Db.Read("select Name from Character where AccountID = '" + user + "' order by Name");
            while (DbLite.Db.Fetch())
            {
                characters.Add(DbLite.Db.GetAsString("Name"));
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


    }
}