using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;

namespace GMTool.Helper
{
    public class SQLiteHelper: DbHelper<SQLiteConnection>
	{
		public SQLiteHelper():base()
		{
		}
		public SQLiteHelper(string connStr):base(connStr)
		{
		}
		
		protected override SQLiteConnection ConnectDataBase(string connStr)
		{
			if(!connStr.StartsWith("")){
				connStr = "Data Source =" + connStr;
			}
			SQLiteConnection conn = new SQLiteConnection(connStr);
			conn.Open();
			return conn;
		}
		protected override DbCommand CreateCommand(string SQL, SQLiteConnection conn)
		{
			return new SQLiteCommand(SQL, conn);
		}
	}
}
