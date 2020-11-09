
using MuEditor.SqlLog;
using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace MuEditor
{
    class DbLite
    { 
        public static DbLite Db = new DbLite();
        public static DbLite DbU = new DbLite();
        public OdbcConnection OdbcCon;
        private OdbcDataReader Odbcdr;
        public SqlConnection SqlCon;
        private SqlDataReader Sqldr;
        private OleDbDataReader OleDbdr;
        public Exception ExError;
        private string Host;
        private string Pwd;
        private string Database;
        private string User;
        private OleDbConnection OleDbCon;
        private byte ConType;
        public DbLite()
        {
            OdbcCon = new OdbcConnection();
            OleDbCon = new OleDbConnection();
            this.ConType = 2;
        }

        public void connect(string connectionString)
        {
            
            OleDbCon = new OleDbConnection(connectionString);
            OleDbCon.Open();
            OleDbCon.Close();
        }

        public void Close()
        {
            if (this.OleDbCon.State != ConnectionState.Closed)
                this.OleDbCon.Close();
            if (this.OleDbdr == null || this.OleDbdr.IsClosed)
                return;
            this.OleDbdr.Close();
        }
        
        public bool Exec(string Query)
        {
            try
            {
                this.ExError = new Exception();
                if (!(this.OleDbCon.State == ConnectionState.Open))
                    this.OleDbCon.Open();
                new OleDbCommand(Query, this.OleDbCon).ExecuteNonQuery();
                FileWork.WriteSqlLog(Query + Environment.NewLine + "\tRESULT: " + "true");
                return true;
            }
            catch (Exception ex)
            {
                this.ExError = ex;
                FileWork.WriteSqlLog(Query + Environment.NewLine + "\tRESULT: " + "Exception " + ex.Message);
                return false;
            }
        }

        public int ExecWithResult(string Query)
        {
            int value = 0;
            try
            {
                this.ExError = new Exception();
                this.OleDbCon.Open();
                value = (int)new OleDbCommand(Query, this.OleDbCon).ExecuteScalar();
                FileWork.WriteSqlLog(Query + Environment.NewLine + "\tRESULT: " + value);
                this.OleDbCon.Close();
                FileWork.WriteSqlLog(Query + Environment.NewLine + "\tRESULT: " + value);
                return value;
            }
            catch (Exception ex)
            {
                this.ExError = ex;
                value = int.MaxValue;
                OleDbCon.Close();
                FileWork.WriteSqlLog(Query + Environment.NewLine + "\tRESULT: " + "Exception");
                return value;
            }
        }

        public bool Read(string Query)
        {
            this.ExError = new Exception();
            this.OleDbdr = (OleDbDataReader)null;
            OleDbCommand oleDbCommand = new OleDbCommand(Query, this.OleDbCon);
            if (!(OleDbCon.State == ConnectionState.Open))
                this.OleDbCon.Open();
            this.OleDbdr = oleDbCommand.ExecuteReader();
            FileWork.WriteSqlLog(Query + Environment.NewLine + "\tRESULT: " + "true");
            return true;
        }

        public bool Fetch()
        {
            try
            {
                if (this.OleDbdr != null)
                    return this.OleDbdr.Read();
                return false;
            }
            catch (Exception ex)
            {
                this.ExError = ex;
                return false;
            }
        }

        public string GetAsString(string Row)
        {
            try
            {
                this.ExError = new Exception();
                if (!this.OleDbdr.IsClosed)
                {
                    for (int ordinal = 0; ordinal < this.OleDbdr.FieldCount; ++ordinal)
                    {
                        if (this.OleDbdr.GetName(ordinal).ToUpper() == Row.ToUpper())
                            return this.OleDbdr[ordinal].ToString();
                    }
                }
                return (string)null;
            }
            catch (Exception ex)
            {
                this.ExError = ex;
                return (string)null;
            }
        }

        public int GetAsInteger(string Row)
        {
            try
            {
                this.ExError = new Exception();
                if (!this.OleDbdr.IsClosed)
                {
                    for (int ordinal = 0; ordinal < this.OleDbdr.FieldCount; ++ordinal)
                    {
                        if (this.OleDbdr.GetName(ordinal).ToUpper() == Row.ToUpper())
                        {
                            FileWork.WriteSqlLog("SQL Get as Integer from " + Row + Environment.NewLine + "\tRESULT: " + Convert.ToInt32(this.OleDbdr[ordinal]).ToString());
                            return Convert.ToInt32(this.OleDbdr[ordinal]);
                        }

                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                this.ExError = ex;
                return 0;
            }
        }

        public int GetAsInteger(int Pos)
        {
            try
            {
                this.ExError = new Exception();
                if (!this.OleDbdr.IsClosed)
                    return Convert.ToInt32(this.OleDbdr[Pos]);
                return 0;
            }
            catch (Exception ex)
            {
                this.ExError = ex;
                return 0;
            }
        }

        public long GetAsInteger64(string Row)
        {
            try
            {
                this.ExError = new Exception();
                if (!this.OleDbdr.IsClosed)
                {
                    for (int ordinal = 0; ordinal < this.OleDbdr.FieldCount; ++ordinal)
                    {
                        if (this.OleDbdr.GetName(ordinal).ToUpper() == Row.ToUpper())
                            return Convert.ToInt64(this.OleDbdr[ordinal]);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                this.ExError = ex;
                return 0;
            }
        }

        public float GetAsFloat(string Row)
        {
            try
            {
                this.ExError = new Exception();
                if (!this.OleDbdr.IsClosed)
                {
                    for (int ordinal = 0; ordinal < this.OleDbdr.FieldCount; ++ordinal)
                    {
                        if (this.OleDbdr.GetName(ordinal).ToUpper() == Row.ToUpper())
                            return float.Parse(this.OleDbdr[ordinal].ToString());
                    }
                }
                return 0.0f;
            }
            catch (Exception ex)
            {
                this.ExError = ex;
                return 0.0f;
            }
        }

        public byte[] GetAsBinary(string Row)
        {
            try
            {
                this.ExError = new Exception();
                if (!this.OleDbdr.IsClosed)
                {
                    for (int ordinal = 0; ordinal < this.OleDbdr.FieldCount; ++ordinal)
                    {
                        if (this.OleDbdr.GetName(ordinal).ToUpper() == Row.ToUpper())
                            return (byte[])this.OleDbdr[ordinal];
                    }
                }
                return (byte[])null;
            }
            catch (Exception ex)
            {
                this.ExError = ex;
                return (byte[])null;
            }
        }
    }
}
