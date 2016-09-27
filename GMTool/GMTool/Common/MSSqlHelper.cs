using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using GMTool.Common;

namespace GMTool.Helper
{
	public class MSSqlHelper : DbHelper<SqlConnection>
	{
		public MSSqlHelper():base()
		{
		}

		public MSSqlHelper(string server, string user=null, string pwd=null, string dbname=null):base()
		{
			this.connStr = MakeConnectString(server,user,pwd, dbname);
		}
		
		public static string MakeConnectString(string server, string user=null, string pwd=null, string dbname=null){
			string connect = "Data Source=" + server + ";";
			if(!string.IsNullOrEmpty(dbname)){
				connect+="Initial Catalog=" + dbname+";";
			}
			if(string.IsNullOrEmpty(user)){
				connect+="Integrated Security=SSPI;Persist Security Info=False;";
			}else{
				if(!string.IsNullOrEmpty(user)){
					connect += "User Id=" + user + ";";
				}
				if(!string.IsNullOrEmpty(pwd)){
					connect += "Password=" + pwd+";";
				}
			}
			return connect;
		}
		
		protected override SqlConnection ConnectDataBase(string connStr)
		{
			SqlConnection conn = new SqlConnection(connStr);
			conn.Open();
			return conn;
		}
		protected override DbCommand CreateCommand(string SQL, SqlConnection conn)
		{
			return new SqlCommand(SQL, conn);
		}
		
		public int BackUp(string dbname,string bakfile){
			string sql = "BACKUP DATABASE "+dbname+" TO DISK=N'"+bakfile+"' WITH COMPRESSION;";
			return ExcuteSQL(sql);
		}
		
		public int RestoreOrCreate(string bakfile,string dbname=null,string path=null){
			string sql2;
			if(string.IsNullOrEmpty(dbname)){
				dbname = Path.GetFileNameWithoutExtension(bakfile);
				sql2 = "RESTORE DATABASE ["+dbname+"] FROM  DISK = N'"+bakfile+"' \nWITH";
				string sql="RESTORE FILELISTONLY  FROM DISK = '"+bakfile+"';";
				if(string.IsNullOrEmpty(path)){
					path = Path.GetDirectoryName(bakfile);
				}
				using(DbDataReader reader=GetReader(sql)){
					while(reader.Read()){
						string name =reader.ReadString("LogicalName");
						string db = reader.ReadString("PhysicalName");
						//替换路径
						
						string ex = Path.GetExtension(db);
						string file = PathHelper.Combine(path, name+ex);
						sql2 += "\nMOVE N'"+name+"' TO N'" +file+"',";
					}
					if(sql2.EndsWith(",")){
						sql2 = sql2.Substring(0, sql2.Length-1);
					}
				}
			}else{
				sql2 = "RESTORE DATABASE "+dbname+" FROM  DISK = N'"+bakfile+"'";
			}
			//Console.WriteLine(sql2);
			//File.WriteAllText("resotre.sql", sql2);
			return ExcuteSQL(sql2);
		}
		
		/// <summary>
		/// 附加数据库
		/// </summary>
		/// <param name="dbname">数据库名字</param>
		/// <param name="dbfile">数据库文件</param>
		/// <returns></returns>
		public int AttachDataBase(string dbname,string dbfile,bool haslog=false){
			string sql="CREATE DATABASE ["+dbname+"] ON (FILENAME = N'"+dbfile+"')";
			if(haslog){
				sql+=" FOR ATTACH";
			}else{
				sql+=" FOR attach_force_rebuild_log";
			}
			return ExcuteSQL(sql);
		}
		/// <summary>
		/// 分离数据库
		/// </summary>
		/// <param name="dbname">数据库</param>
		/// <returns></returns>
		public int SplitDataBase(string dbname){
			string sql="ALTER DATABASE "+dbname+" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;"
				+"EXEC sp_detach_db '"+dbname+"';";
			return ExcuteSQL(sql);
		}
	}
}
