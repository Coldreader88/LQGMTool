﻿/*
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
			catch (Exception)
			{
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
		public DbDataReader GetReader(string strSQL)
		{
			return GetReader(strSQL, null);
		}

		public DbDataReader GetReader(string strSQL, DbParameter[] paras)
		{
			return GetReader(strSQL, paras, CommandType.Text);
		}
		public DbDataReader GetReader(string strSQL, DbParameter[] paras, CommandType cmdtype)
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
		public int ExcuteSQL(string strSQL)
		{
			return ExcuteSQL(strSQL, null);
		}

		public int ExcuteSQL(string strSQL, params DbParameter[] paras)
		{
			return ExcuteSQL(strSQL, CommandType.Text, paras);
		}

		public int ExcuteSQL(string strSQL, CommandType cmdtype, params DbParameter[] paras)
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
		public int ExcuteScalarSQL(string strSQL)
		{
			return ExcuteScalarSQL(strSQL, null);
		}

		public int ExcuteScalarSQL(string strSQL,params DbParameter[] paras)
		{
			return ExcuteScalarSQL(strSQL,CommandType.Text, paras);
		}

		public int ExcuteScalarSQL(string strSQL, CommandType cmdtype,params DbParameter[] paras)
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
		public static string ReadString(this DbDataReader reader,string col){
			return ReadString(reader, col, null);
		}
		public static string ReadString(this DbDataReader reader,string col,string def){
			object obj = reader[col];
			if (obj == DBNull.Value)
				return def;
			return Convert.ToString(obj);
		}
		public static short ReadInt16(this DbDataReader reader,string col){
			object obj = reader[col];
			if (obj == DBNull.Value)
				return 0;
			return Convert.ToInt16(obj);
		}
		public static int ReadInt32(this DbDataReader reader,string col){

			return ReadInt32(reader, col, 0);
		}
		public static int ReadInt32(this DbDataReader reader,string col,int def){
			object obj = reader[col];
			if (obj == DBNull.Value)
				return def;
			return Convert.ToInt32(obj);
		}
		public static bool ReadBoolean(this DbDataReader reader,string col){
			object obj = reader[col];
			if (obj == DBNull.Value)
				return false;
			string val = Convert.ToString(obj).ToLower();
			return "1" == val || "true"==val;
		}
		public static long ReadInt64(this DbDataReader reader,string col){
			object obj = reader[col];
			if (obj == DBNull.Value)
				return 0;
			return Convert.ToInt64(obj);
		}

		public static T ReadEnum<T>(this DbDataReader reader,string col,T def){
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
