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
    public enum SqlServer
    {
        V2000 = 0,
        V2005,
        V2008,
        V2008R2,
        V2012,
        V2014,
    }

    public class MSSqlHelper : DbHelper<SqlConnection>
    {
        private static string[] VERSIONS = new string[]{
            "8.00",
            "9.00",
            "10.00",
            "10.50",
            "11.00",
            "12.00",
        };

        public MSSqlHelper() : base()
        {
        }

        public MSSqlHelper(string server, string user = null, string pwd = null, string dbname = null) : base()
        {
            if (server.StartsWith("Data Source"))
            {
                this.connStr = server;
            }
            else
            {
                this.connStr = MakeConnectString(server, user, pwd, dbname);
            }
        }
        public SqlServer Version
        {
            get
            {
                if (IsOpen)
                {
                    string ver = conn.ServerVersion;
                    int i = 0;
                    if (!string.IsNullOrEmpty(ver))
                    {
                        foreach (string v in VERSIONS)
                        {
                            if (ver.StartsWith(v))
                            {
                                return (SqlServer)i;
                            }
                            i++;
                        }
                        return SqlServer.V2014;
                    }
                }
                return SqlServer.V2014;
            }
        }

        public static string MakeConnectString(string server, string user = null, string pwd = null, string dbname = null)
        {
            string connect = "Data Source=" + server + ";";
            if (string.IsNullOrEmpty(user))
            {
                connect += "Integrated Security=SSPI;Persist Security Info=False;";
            }
            else
            {
                if (!string.IsNullOrEmpty(user))
                {
                    connect += "User Id=" + user + ";";
                }
                if (!string.IsNullOrEmpty(pwd))
                {
                    connect += "Password=" + pwd + ";";
                }
            }
            if (!string.IsNullOrEmpty(dbname))
            {
                connect += "Initial Catalog=" + dbname + ";";
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

        public int BackUp(string dbname, string bakfile)
        {
            string sql = "BACKUP DATABASE " + dbname + " TO DISK=N'" + bakfile + "' WITH COMPRESSION;";
            return ExcuteSQL(sql);
        }
        /// <summary>
        /// 收缩数据库
        /// </summary>
        /// <param name="dbname"></param>
        public int ShrinkDataBase(string dbname)
        {
            string sql = "DBCC SHRINKDATABASE(" + dbname + ")";
            return ExcuteSQL(sql);
        }
        public int RestoreOrCreate(string bakfile, string path = null, string dbname = null)
        {
            string sql2;
            if (string.IsNullOrEmpty(dbname))
            {
                dbname = Path.GetFileNameWithoutExtension(bakfile);
                sql2 = "RESTORE DATABASE [" + dbname + "] FROM  DISK = N'" + bakfile + "' \nWITH";
                string sql = "RESTORE FILELISTONLY  FROM DISK = '" + bakfile + "';";
                if (string.IsNullOrEmpty(path))
                {
                    path = Path.GetDirectoryName(bakfile);
                }
                using (DbDataReader reader = GetReader(sql))
                {
                    while (reader.Read())
                    {
                        string name = reader.ReadString("LogicalName");
                        string db = reader.ReadString("PhysicalName");
                        //替换路径

                        string ex = Path.GetExtension(db);
                        string file = PathHelper.Combine(path, name + ex);
                        sql2 += "\nMOVE N'" + name + "' TO N'" + file + "',";
                    }
                    if (sql2.EndsWith(","))
                    {
                        sql2 = sql2.Substring(0, sql2.Length - 1);
                    }
                }
            }
            else
            {
                sql2 = "RESTORE DATABASE " + dbname + " FROM  DISK = N'" + bakfile + "'";
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
        public int AttachDataBase2005(string dbname, string dbfile, bool haslog = false)
        {
            string sql = "CREATE DATABASE [" + dbname + "] ON (FILENAME = N'" + dbfile + "')";
            if (haslog)
            {
                sql += " FOR ATTACH";
            }
            else
            {
                sql += " FOR attach_force_rebuild_log";
            }
            return ExcuteSQL(sql);
        }
        public int AttachDataBase2000(string dbname, string dbfile, bool haslog = false)
        {
            string sql = "EXEC sp_attach_db '" + dbname + "','" + dbfile + "';";
            return ExcuteSQL(sql);
        }
        public int AttachDataBase(string dbname, string dbfile, bool haslog = true)
        {
            int n = (int)Version;
            if (n >= (int)SqlServer.V2005)
            {
                return AttachDataBase2005(dbname, dbfile, haslog);
            }
            return AttachDataBase2000(dbname, dbfile, haslog);
        }
        /// <summary>
        /// 分离数据库
        /// </summary>
        /// <param name="dbname">数据库</param>
        /// <returns></returns>
        public void SplitDataBase(string dbname)
        {
            string sql = "ALTER DATABASE " + dbname + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;"
                + "EXEC sp_detach_db '" + dbname + "';";
            ExcuteSQL(sql);
        }

        public bool Exist(string dbname)
        {
            using (DbDataReader reader = GetReader("select * From master.dbo.sysdatabases where name='" + dbname + "';"))
            {
                return reader != null && reader.Read();
            }
        }

    }
}
