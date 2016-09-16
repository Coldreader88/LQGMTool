using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Data.SqlClient;

namespace GMTool.Helper
{
    public class SQLiteHelper
    {
        private SQLiteConnection conn;
        private string path;
        public SQLiteHelper()
        {
            this.path = null;
        }
        public SQLiteHelper(string path)
        {
            this.path = path;
        }
        public bool Open()
        {
            return Open(this.path);
        }
        public bool Open(string path)
        {
            if (path == null)
            {
                return false;
            }
            if (IsOpen)
            {
                Close();
            }
            this.path = path;
            conn = new SQLiteConnection("Data Source =" + path);
            try
            {
                conn.Open();
            }
            catch (Exception)
            {

            }
            return IsOpen;
        }
        public bool IsOpen
        {
            get { return conn != null; }
        }

        public void Close()
        {
            if (IsOpen)
            {
                try
                {
                    conn.Close();
                    conn = null;
                }
                catch (Exception)
                {

                }
            }
        }
        public SQLiteDataReader GetReader(string strSQL)
        {
            return GetReader(strSQL, null);
        }

        public SQLiteDataReader GetReader(string strSQL, SqlParameter[] paras)
        {
            return GetReader(strSQL, paras, CommandType.Text);
        }
        public SQLiteDataReader GetReader(string strSQL, SqlParameter[] paras, CommandType cmdtype)
        {
            SQLiteCommand command = new SQLiteCommand(strSQL, conn)
            {
                CommandType = cmdtype
            };
            if (paras != null)
            {
                command.Parameters.AddRange(paras);
            }
            return command.ExecuteReader(CommandBehavior.SingleResult);
        }


        public int ExcuteSQL(string strSQL)
        {
            return ExcuteSQL(strSQL, null);
        }

        public int ExcuteSQL(string strSQL, SqlParameter[] paras)
        {
            return ExcuteSQL(strSQL, paras, CommandType.Text);
        }

        public int ExcuteSQL(string strSQL, SqlParameter[] paras, CommandType cmdType)
        {
            int num = 0;
            using (SQLiteCommand command = new SQLiteCommand(strSQL, this.conn)
            {
                CommandType = cmdType
            })
            {
                if (paras != null)
                {
                    command.Parameters.AddRange(paras);
                }
                num = command.ExecuteNonQuery();
            }
            return num;
        }
        public int ExcuteScalarSQL(string strSQL)
        {
            return ExcuteScalarSQL(strSQL, null);
        }

        public int ExcuteScalarSQL(string strSQL, SqlParameter[] paras)
        {
            return ExcuteScalarSQL(strSQL, paras, CommandType.Text);
        }

        public int ExcuteScalarSQL(string strSQL, SqlParameter[] paras, CommandType cmdType)
        {
            int num = 0;
            using (SQLiteCommand command = new SQLiteCommand(strSQL, this.conn)
            {
                CommandType = cmdType
            })
            {
                if (paras != null)
                {
                    command.Parameters.AddRange(paras);
                }
                num = Convert.ToInt32(command.ExecuteScalar());
            }
            return num;
        }
    }
}
