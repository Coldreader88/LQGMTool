﻿using System;
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
                return false;
            }
            return IsOpen;
        }
        public bool IsOpen
        {
            get { return conn != null && conn.State == ConnectionState.Open; }
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
            if (!IsOpen) return null;
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
            if (!IsOpen) return 0;
            int num = 0;
            using (SqlCommand command = new SqlCommand(strSQL, this.conn)
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
            if (!IsOpen) return 0;
            int num = 0;
            using (SqlCommand command = new SqlCommand(strSQL, this.conn)
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
