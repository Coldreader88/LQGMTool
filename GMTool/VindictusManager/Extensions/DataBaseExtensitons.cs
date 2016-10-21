using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Vindictus.Common;
using System.Data.Common;
using Vindictus;
using Vindictus.Helper;

namespace ServerManager
{
    public static class DataBaseExtensitons
    {
        public static int ShrinkDataBase(this ServerForm main, CoreConfig config)
        {
            MSSqlHelper sql = new MSSqlHelper(config.ConnectionString);
            if (sql.Open())
            {
                List<string> dbs = config.DataBases;
                if (dbs != null)
                {
                    int i = 0;
                    foreach (string db in dbs)
                    {
                        try
                        {
                            if (sql.Exist(db))
                            {
                                sql.ShrinkDataBase(db);
                                i++;
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                    return i;
                }
            }
            return 0;
        }
        public static int SplitDb(this ServerForm main, CoreConfig config)
        {
            MSSqlHelper sql = new MSSqlHelper(config.ConnectionString);
            if (sql.Open())
            {
                List<string> dbs = config.DataBases;
                if (dbs != null)
                {
                    int i = 0;
                    foreach (string db in dbs)
                    {
                        try
                        {
                            if (sql.Exist(db))
                            {
                                sql.SplitDataBase(db);
                                i++;
                            }

                        }
                        catch (Exception e)
                        {
                            main.Error(R.TipServerDbSplitFail+"\n" + e);
                            break;
                        }
                    }
                    return i;
                }
            }
            return 0;

        }

        public static int CleanDataBaseLog(this ServerForm main, CoreConfig config)
        {
            List<string> dbs = config.DataBases;

            if (dbs != null)
            {
                DirectoryInfo dir = new DirectoryInfo(config.DatabasePath);
                if (!dir.Exists)
                {
                    return 0;
                }
                MSSqlHelper sql = new MSSqlHelper(config.ConnectionString);
                if (sql.Open())
                {
                    int i = 0;
                    foreach (string db in dbs)
                    {
                        if (sql.Exist(db))
                        {
                            return -1;
                        }
                        else
                        {
                            FileInfo[] files = dir.GetFiles(db + "*.ldf");
                            if (files != null && files.Length > 0)
                            {
                                files[0].Delete();
                                i++;
                            }
                        }
                    }
                    return i;
                }
            }
            return 0;
        }

        public static int AttachDataBase(this ServerForm main, CoreConfig config)
        {
            List<string> dbs = config.DataBases;
            if (dbs != null)
            {
                MSSqlHelper sql = new MSSqlHelper(config.ConnectionString);
                if (sql.Open())
                {
                    int i = 0;
                    foreach (string db in dbs)
                    {
                        string file = PathHelper.Combine(config.DatabasePath, db + ".mdf");
                        try
                        {
                            if (!sql.Exist(db))
                            {
                                sql.AttachDataBase(db, file);
                                i++;
                            }
                        }
                        catch (Exception e)
                        {
                            main.Error(R.TipServerDbAttachFail+"\n" + file+"\n"+e.Message);
                            break;
                        }
                    }
                    return i;
                }
            }
            return 0;
        }

        public static int CreateDataBase(this ServerForm main, CoreConfig config, string path)
        {
            string[] files = Directory.GetFiles(path, "*.bak");
            string targetpath = config.DatabasePath;
            if (!Directory.Exists(targetpath))
            {
                Directory.CreateDirectory(targetpath);
            }
            if (files != null)
            {
                MSSqlHelper sql = new MSSqlHelper(config.ConnectionString);
                if (sql.Open())
                {
                    int i = 0;
                    foreach (string file in files)
                    {
                        try
                        {
                            sql.RestoreOrCreate(file, targetpath);
                            i++;
                        }
                        catch (Exception e)
                        {
                            main.Error(R.TipServerDbCreateFail+"\n" + file + "\n" + e.Message);
                            break;
                        }
                    }
                    return i;
                }
            }
            return 0;
        }
    }
}
