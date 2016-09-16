using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GMTool.Helper
{
    public class MSSqlHelper
    {
        private SqlConnection conn;
        private string connStr;
        public MSSqlHelper()
        {
            this.connStr = null;
        }
        public MSSqlHelper(string connStr)
        {
            this.connStr = connStr;
        }
        public bool Open()
        {
            return Open(this.connStr);
        }
        public bool Open(string connStr)
        {
            if (connStr == null)
            {
                return false;
            }
            if (IsOpen)
            {
                Close();
            }
            this.connStr = connStr;
            conn = new SqlConnection(connStr);
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
        public SqlDataReader GetReader(string strSQL)
        {
            return GetReader(strSQL, null);
        }

        public SqlDataReader GetReader(string strSQL, SqlParameter[] paras)
        {
            return GetReader(strSQL, paras, CommandType.Text);
        }
        public SqlDataReader GetReader(string strSQL, SqlParameter[] paras, CommandType cmdtype)
        {
            SqlCommand command = new SqlCommand(strSQL, conn)
            {
                CommandType = cmdtype
            };
            if (paras != null)
            {
                command.Parameters.AddRange(paras);
            }
            return command.ExecuteReader(CommandBehavior.SingleResult);
        }


    }
}
