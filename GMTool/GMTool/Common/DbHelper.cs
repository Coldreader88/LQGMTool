/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/21
 * 时间: 15:20
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */


namespace GMTool.Helper
{
	using System;
	using System.Data;
	using System.Data.Common;

	public abstract class DbHelper<T> where T :DbConnection
	{
		#region
		protected T conn;
		protected string connStr;
		public bool IsOpen
		{
			get { return conn != null && conn.State == ConnectionState.Open; }
		}
		public bool IsClose
		{
			get { return conn == null; }
		}
		
		public DbHelper()
		{
			this.connStr = null;
		}
		public DbHelper(string connStr)
		{
			this.connStr = connStr;
		}
		
		
		protected abstract T ConnectDataBase(string connStr);
		
		public T Connection{
			get{return conn;}
		}
		
		protected abstract DbCommand CreateCommand(string SQL, T conn);


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
			try
			{
				conn = ConnectDataBase(connStr);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return false;
			}
			return IsOpen;
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
		#endregion
		
		#region reader
		public DbDataReader GetReader(string strSQL, CommandType cmdtype= CommandType.Text, DbParameter[] paras=null)
		{
			if (!IsOpen) return null;
			using(DbCommand command = CreateCommand(strSQL, conn)){
				command.CommandType = cmdtype;
				if (paras != null)
				{
					command.Parameters.AddRange(paras);
				}
				return command.ExecuteReader(CommandBehavior.SingleResult);
			}
		}
        #endregion

        #region exec

        public int ExcuteSQLs(params string[] SQLs)
        {
            if (!IsOpen) return 0;
            int result = 0;
            using (DbTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    using (DbCommand cmd = CreateCommand("", conn))
                    {
                        foreach (string SQLstr in SQLs)
                        {
                            cmd.CommandText = SQLstr;
                            result += cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch
                {
                    transaction.Rollback();//出错，回滚
                    result = -1;
                }
                finally
                {
                    transaction.Commit();
                }
            }
            return  result;
        }
        public int ExcuteSQL(string strSQL, CommandType cmdtype = CommandType.Text, DbParameter[] paras=null)
		{
			if (!IsOpen) return 0;
			int num = 0;
			using(DbCommand command = CreateCommand(strSQL, conn)){
				command.CommandType = cmdtype;
				if (paras != null)
				{
					command.Parameters.AddRange(paras);
				}
				num = command.ExecuteNonQuery();
			}
			return num;
		}

		public int ExcuteScalarSQL(string strSQL, CommandType cmdtype= CommandType.Text, DbParameter[] paras=null)
		{
			if (!IsOpen) return 0;
			int num = 0;
			using(DbCommand command = CreateCommand(strSQL, conn)){
				command.CommandType = cmdtype;
				if (paras != null)
				{
					command.Parameters.AddRange(paras);
				}
				num = Convert.ToInt32(command.ExecuteScalar());
			}
			return num;
		}
		#endregion
	}
	
	public static class DbExtension{
		public static string ReadString(this DbDataReader reader,string col,string def=null){
			object obj = reader[col];
			if (obj == DBNull.Value)
				return def;
			return Convert.ToString(obj);
		}
		public static short ReadInt16(this DbDataReader reader,string col,short def=0){
			object obj = reader[col];
			if (obj == DBNull.Value)
				return 0;
			return Convert.ToInt16(obj);
		}
		public static int ReadInt32(this DbDataReader reader,string col,int def=0){
			object obj = reader[col];
			if (obj == DBNull.Value)
				return def;
			return Convert.ToInt32(obj);
		}
		public static bool ReadBoolean(this DbDataReader reader,string col,bool def = false){
			object obj = reader[col];
			if (obj == DBNull.Value)
				return def;
			string val = Convert.ToString(obj).ToLower();
			return "1" == val || "true"==val;
		}
		public static long ReadInt64(this DbDataReader reader,string col,long def=0){
			object obj = reader[col];
			if (obj == DBNull.Value)
				return def;
			return Convert.ToInt64(obj);
		}

		public static T ReadEnum<T>(this DbDataReader reader,string col,T def=default(T)){
			object obj = reader[col];
			string val  ="";
			if (obj != DBNull.Value){
				val = Convert.ToString(obj);
			}
			T e= def;
			try{
				e = (T)Enum.Parse(typeof(T), val);
			}catch(Exception){
				e = def;
			}
			return e;
		}

	}
}
