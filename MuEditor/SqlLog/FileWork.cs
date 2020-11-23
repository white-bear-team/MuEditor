using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuEditor.SqlLog
{
    class FileWork
    {
        public static string ReadSqlLog()
        {
            try
            {
                return File.ReadAllText("sql.log");
            }
            catch (FileNotFoundException)
            {
                return "File was not found";
            }
        }

        public static void DeleteSqlLog()
        {
            File.Delete("sql.log");
        }

        public static void WriteSqlLog(string line)
        {
            File.AppendAllText("sql.log", (DateTime.Now.ToString() + " " + line + Environment.NewLine));
        }
    }
}
