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
            bool rs = db.Open(connect);
            if (rs)
            {
                IniHelper helper = new IniHelper(Environment.CurrentDirectory + "/DBIni.ini");
                helper.WriteValue("连接信息", "IP", server.Trim());
                helper.WriteValue("连接信息", "数据库名", dbname.Trim());
                helper.WriteValue("连接信息", "用户名", user.Trim());
                helper.WriteValue("连接信息", "密码", pwd.Trim());
            }
            return rs;
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
                while (reader != null && reader.Read())
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

        public static List<Mail> ReadSendMailList(this MainForm main, User user)
        {
            List<Mail> mails = new List<Mail>();
            if (user == null)
            {
                return mails;
            }
            using (SqlDataReader reader = db.GetReader("SELECT * FROM QueuedItem WHERE CID =" + user.CID))
            {
                while (reader != null && reader.Read())
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
        public static List<Mail> ReadUserMailList(this MainForm main, User user)
        {
            List<Mail> mails = new List<Mail>();
            if (user == null)
            {
                return mails;
            }
            using (SqlDataReader reader = db.GetReader("select mi.itemCount,m.* from Mail as m LEFT JOIN MailItem as mi ON mi.MailID = m.MailID"
                + " where m.ToCID =" + user.CID))
            {
                while (reader != null && reader.Read())
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
        public static List<Item> ReadUserItems(this MainForm main, User user, PackType type)
        {
            List<Item> items = new List<Item>();
            try
            {
                string sql;
                if (type == PackType.Normal)
                {
                    sql = "SELECT i.ID,i.ExpireDateTime,i.ItemClass,i.Collection,i.Count,i.Slot,e.Color1,e.Color2,e.Color3,ai.Attribute,ai.Value,ai.Arg,ai.Arg2 "
                        + "FROM  (Item as i  left join ItemAttribute as ai on i.ID = ai.itemID  and ai.Attribute='ENHANCE') left join Equippable e on e.ID = i.ID "
                        + "WHERE i.Collection<100 and i.OwnerID =" + user.CID + " ORDER BY i.Collection,i.Slot";
                }
                else if (type == PackType.Cash)
                {
                    sql = "SELECT i.ID,i.ExpireDateTime,i.ItemClass,i.Collection,i.Count,i.Slot,e.Color1,e.Color2,e.Color3,ai.Attribute,ai.Value,ai.Arg,ai.Arg2 "
                        + "FROM  (Item as i  left join ItemAttribute as ai on i.ID = ai.itemID  and ai.Attribute='PREFIX') left join Equippable e on e.ID = i.ID "
                        + "WHERE i.Collection>=100 and i.OwnerID =" + user.CID + " ORDER BY i.Collection,i.Slot";
                }
                else
                {
                    sql = "SELECT i.ID,i.ExpireDateTime,i.ItemClass,i.Collection,i.Count,i.Slot,e.Color1,e.Color2,e.Color3,ai.Attribute,ai.Value,ai.Arg,ai.Arg2 "
                        + "FROM  (Item as i  left join ItemAttribute as ai on i.ID = ai.itemID  and ai.Attribute='ENHANCE') left join Equippable e on e.ID = i.ID "
                        + "WHERE i.OwnerID =" + user.CID + " ORDER BY i.Collection,i.Slot";
                }
                using (SqlDataReader reader = db.GetReader(sql))
                {
                    while (reader != null && reader.Read())
                    {
                        Item item = new Item(Convert.ToInt64(reader[0]),
                            Convert.ToString(reader["itemClass"]),
                            ""// Convert.ToString(reader["itemType"])
                            );

                        if (reader[1] != DBNull.Value)
                        {
                            item.Time = Convert.ToString(reader[1]).Split(' ')[0];
                        }
                        else
                        {
                            item.Time = "无限期";
                        }
                        item.Collection = Convert.ToInt32(reader["Collection"]);
                        item.Slot = Convert.ToInt32(reader["Slot"]);
                        item.attrName = reader["Attribute"] == DBNull.Value ? null : Convert.ToString(reader["Attribute"]);
                        item.attrValue = reader["Value"] == DBNull.Value ? null : Convert.ToString(reader["Value"]);
                        item.Count = reader["Count"] == DBNull.Value ? 1 : Convert.ToInt32(reader["Count"]);
                        object o = reader["Color1"];
                        if (o != DBNull.Value)
                        {
                            item.Color1 = Convert.ToInt32(o);
                        }
                        o = reader["Color2"];
                        if (o != DBNull.Value)
                        {
                            item.Color2 = Convert.ToInt32(o);
                        }
                        o = reader["Color3"];
                        if (o != DBNull.Value)
                        {
                            item.Color3 = Convert.ToInt32(o);
                        }
                        items.Add(item);
                    }
                }
            }
            catch (Exception exception)
            {
                main.Error("" + exception);
            }
            return items;
        }

        public static void DeleteUserMail(this MainForm main, Mail mail)
        {
            try
            {
                db.ExcuteSQL("delete from mailItem where mailID = " + mail.MailID);
                db.ExcuteSQL("delete from mail where mailID = " + mail.MailID);
            }
            catch (Exception)
            {

            }
        }
        public static void DeleteSendMail(this MainForm main, Mail mail)
        {
            try
            {
                db.ExcuteSQL("DELETE FROM QueuedItem Where RowID=" + mail.MailID);
            }
            catch (Exception)
            {

            }
        }

        public static void MaxSecondClass(this MainForm main, User user, string className)
        {
            try
            {
                if (db.ExcuteScalarSQL(string.Concat(new object[] {
                "select count(*) from Manufacture where ManufacturelID = N'", className, "' AND CID=", user.CID })) == 0)
                {
                    db.ExcuteSQL(string.Concat(new object[] {
                    "insert into Manufacture(CID, ManufacturelID, Grade, [ExperiencePoint]) values(", user.CID, ",N'", className, "',4,100000000)" }));
                }
                else
                {
                    db.ExcuteSQL(string.Concat(new object[] {
                    "update Manufacture set Grade = 4,ExperiencePoint = 100000000 where ManufacturelID = N'", className, "' and CID = ", user.CID }));
                }
            }
            catch (Exception ex)
            {
                main.Error(ex.Message);
            }
        }
        public static void MaxAllSecondClass(this MainForm main, User user)
        {
            string[] strArray = new string[] { "cooking", "metal_weapon", "heavy_armor", "light_armor", "sewing", "workmanship", "armor", "armor_dc", "spirit_stone", "gathering", "metal_weapon_dc", "sewing_dc", "workmanship_dc" };
            for (int i = 0; i < strArray.Length; i++)
            {
                main.MaxSecondClass(user, strArray[i]);
            }
        }

        public static void ModUserLevel(this MainForm main, User user, int level)
        {
            if (user.level == level)
            {
                return;
            }
            try
            {
                db.ExcuteSQL(string.Concat(new object[] { "update characterInfo set level = ", level, " where id = ", user.CID }));
            }
            catch (Exception exception)
            {
                main.Error(exception.Message);
            }
        }

        public static bool ModUserName(this MainForm main, User user, string name)
        {
            if (db.ExcuteScalarSQL("select count(*) from characterInfo where Name = N'" + name + "'") == 0)
            {
                db.ExcuteSQL(string.Concat(new object[] { "update characterInfo set name = N'", name, "' where id = ", user.CID }));
                // this.output("将角色 [" + this.userList[this.userIndex].name + "] 角色名修改为 [" + this.txtUserName.Text + "] 成功!");
                return true;
            }
            else
            {
                return false;
                // this.output("角色名 [" + this.txtUserName.Text + "] 已存在!");
            }
        }

        public static bool CheckName(this MainForm main, string name)
        {
            return db.ExcuteScalarSQL("select count(*) from characterInfo where Name = N'" + name + "'") == 0;
        }

        public static void MaxLightLevel(this MainForm main, User user)
        {
            MaxGroupLevel(main, user, 0);
        }
        public static void MaxDarkLevel(this MainForm main, User user)
        {
            MaxGroupLevel(main, user, 1);
        }
        /// <summary>
        /// 
        /// 0 light
        /// 1 dark
        /// </summary>
        /// <param name="main"></param>
        /// <param name="user"></param>
        /// <param name="group"></param>
        public static void MaxGroupLevel(this MainForm main, User user, int group)
        {
            try
            {
                if (db.ExcuteScalarSQL("select count(*) from vocation where cid=" + user.CID) == 0)
                {
                    db.ExcuteSQL(string.Concat(new object[] { "insert into vocation(CID,vocationClass,VocationLevel,VocationEXP,LastTransform) values(", user.CID, ",", group, ",40,0,'", DateTime.Now.ToString(), "')" }));
                }
                else
                {
                    db.ExcuteSQL("update vocation set vocationClass = 0,VocationLevel = 40 where cid =" + user.CID);
                }
                //this.output("角色 [" + this.userList[this.userIndex].name + "] 光明骑士等级修改成功!");
            }
            catch (Exception exception)
            {
                main.Error(exception.Message);
            }
        }

        public static void ResetGroupSkill(this MainForm main, User user)
        {
            string strSQL = "delete from VocationSkill WHERE CID =" + user.CID;
            try
            {
                db.ExcuteSQL(strSQL);
            }
            catch (Exception exception)
            {
                main.Error(exception.Message);
            }
        }

        public static void SendItems(this MainForm main, User user, int count, params Item[] items)
        {

        }
    }
}
