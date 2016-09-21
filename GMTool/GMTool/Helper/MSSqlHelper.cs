using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace GMTool.Helper
{
	public class MSSqlHelper : DbHelper<SqlConnection>
	{
		public MSSqlHelper():base()
		{
		}
		public MSSqlHelper(string connStr):base(connStr)
		{
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
	}
}
