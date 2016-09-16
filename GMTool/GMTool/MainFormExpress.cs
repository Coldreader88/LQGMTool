using GMTool.Bean;
using GMTool.Helper;
using LY;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GMTool
{
    static class MainFormExpress
    {
        private static MSSqlHelper db = new MSSqlHelper();
        private const string SQL_QUERY_USERS = "select * from CharacterInfo where CreateTime >= '2016-01-01' ORDER BY CreateTime;";

        public static bool IsOpen(this MainForm main)
        {
            return db.IsOpen;
        }

        public static void ReadSettings(this MainForm main)
        {
            IniHelper helper = new IniHelper(Application.StartupPath + "/DBIni.ini");
            main.tb_mssql_server.Text = helper.ReadValue("连接信息", "IP");
            main.tb_mssql_db.Text = helper.ReadValue("连接信息", "数据库名");
            main.tb_mssql_user.Text = helper.ReadValue("连接信息", "用户名");
            main.tb_mssql_pwd.Text = helper.ReadValue("连接信息", "密码");
        }

        public static bool connectDataBase(this MainForm main, string server, string user, string pwd, string dbname)
        {
            string connect = "Data Source=" + server + ";User Id=" + user + ";Password=" + pwd + ";Initial Catalog=" + dbname;
            bool rs =  db.Open(connect);
            if (rs)
            {
                IniHelper helper = new IniHelper(Environment.CurrentDirectory + "/DBIni.ini");
                helper.WriteValue("连接信息", "IP", server.Trim());
                helper.WriteValue("连接信息", "数据库名", dbname.Trim());
                helper.WriteValue("连接信息", "用户名", user.Trim());
                helper.WriteValue("连接信息", "密码", pwd.Trim());
            }
            return true;
        }

        public static void CloseDataBase(this MainForm main)
        {
            db.Close();
        }

        public static List<User> ReadUserList(this MainForm main)
        {
            List<User> userList = new List<User>();
            using (SqlDataReader reader = db.GetReader(SQL_QUERY_USERS))
            {
                while (reader.Read())
                {
                    User item = new User(Convert.ToInt64(reader["ID"]),
                         Convert.ToInt32(reader["UID"]),
                         Convert.ToInt32(reader["CharacterSN"]),
                        Convert.ToString(reader["Name"]),
                        Convert.ToInt32(reader["Class"]),
                        Convert.ToInt32(reader["Level"])
                    );
                    userList.Add(item);
                }
            }
            return userList;
        }

        public static List<Mail> ReadSendMailList(this MainForm main,User user)
        {
            List<Mail> mails = new List<Mail>();
            if (user == null)
            {
                return mails;
            }
            using (SqlDataReader reader = db.GetReader("SELECT * FROM QueuedItem WHERE CID =" + user.CID))
            {
                while (reader.Read())
                {
                    Mail item = new Mail(
                        Convert.ToInt64(reader["RowID"]),
                        Convert.ToString(reader["MailTitle"]),
                        Convert.ToString(reader["MailContent"])
                        );
                    mails.Add(item);
                }
            }
            return mails;
        }
        public static List<Mail> ReadUserMailList(this MainForm main,User user)
        {
            List<Mail> mails = new List<Mail>();
            if (user == null)
            {
                return mails;
            }
            using (SqlDataReader reader = db.GetReader("select mi.itemCount,m.* from Mail as m LEFT JOIN MailItem as mi ON mi.MailID = m.MailID"
                + " where m.ToCID =" + user.CID))
            {
                while (reader.Read())
                {
                    Mail item = new Mail(
                        Convert.ToInt64(reader["mailID"]),
                        Convert.ToString(reader["title"]),
                        Convert.ToString(reader["content"])
                        );
                    mails.Add(item);
                }
            }
            return mails;
        }
        public static List<Item> ReadUserItems(this MainForm main, User user)
        {
            return new List<Item>();
        }

    }
}
