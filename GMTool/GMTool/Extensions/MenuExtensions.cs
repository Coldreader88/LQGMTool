using GMTool.Bean;
using GMTool.Enums;
using GMTool.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GMTool
{
    public static class MenuExtensions
    {
        public static void AddClasses(this MainForm main,User user,ToolStripDropDownItem menuitem)
        {
            menuitem.DropDownItems.Clear();
            Array classes = Enum.GetValues(typeof(ClassInfo));
            foreach (ClassInfo cls in classes)
            {
                if (cls != ClassInfo.UnKnown)
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(cls.Name());
                    tsmi.Tag = cls;
                    if (user != null && user.Class == cls)
                    {
                        tsmi.Checked = true;
                    }
                    tsmi.ToolTipText = cls.ToString() + " " + cls.Index();
                    tsmi.Click += (object sender, EventArgs e) => {
                        if (!main.CheckUser()) return;
                        ToolStripMenuItem menu = sender as ToolStripMenuItem;
                        if (menu != null && menu.Tag != null)
                        {
                            ClassInfo info = (ClassInfo)menu.Tag;
                            //
                            if (main.ModUserClass(main.CurUser, info))
                            {
                                main.ReadUsers();
                            }
                        }
                    };
                    menuitem.DropDownItems.Add(tsmi);
                }
            }
        }
        public static void AddTypes(this MainForm main, ComboBox mainmenuitem, ComboBox submenuitem)
        {
            mainmenuitem.Items.Clear();
            submenuitem.Items.Clear();
            mainmenuitem.Items.AddRange(MainCategoryEx.Values);
            submenuitem.Items.AddRange(SubCategoryEx.Values);
            mainmenuitem.SelectedIndex = 0;
            submenuitem.SelectedIndex = 0;
        }
        public static void AddTitles(this MainForm main, ToolStripDropDownItem menuitem,List<long> titleIds)
        {
            menuitem.DropDownItems.Clear();
            TitleInfo[] titles = main.DataHelper.GetTitles();
            int k = 0;
            User user = main.CurUser;
            if (user == null) return;
            ToolStripMenuItem level = new ToolStripMenuItem("lv." + (k * 10 + 1) + "-" + ((k + 1) * 10));
            menuitem.DropDownItems.Add(level);
            int max = 20;
            int i = 0;
            foreach (TitleInfo cls in titles)
            {
                if (!user.IsEnable(cls.ClassRestriction))
                {
                    continue;
                }
                if (cls.OnlyClass != ClassInfo.UnKnown)
                {
                    if (user.Class != cls.OnlyClass)
                    {
                        continue;
                    }
                }
                
                if (cls.RequiredLevel <= (k + 1) * 10)
                {
                    if (i % max == 0 && i >= max)
                    {
                        level = new ToolStripMenuItem("lv." + (k * 10 + 1) + "-" + ((k + 1) * 10) + "(" + (i / max + 1) + ")");
                        menuitem.DropDownItems.Add(level);
                    }
                    i++;
                    ToolStripMenuItem tsmi = new ToolStripMenuItem("lv." + cls.RequiredLevel + " " + cls.Name);
                    tsmi.Tag = cls;
                    tsmi.ToolTipText = cls.ToString();

                    if (titleIds.Contains(cls.TitleID))
                    {
                        tsmi.Checked = true;
                    }
                    else
                    {
                        tsmi.Click += (object sender, EventArgs e) => {
                            if (!main.CheckUser()) return;
                            ToolStripMenuItem menu = sender as ToolStripMenuItem;
                            if (menu != null && menu.Tag != null)
                            {
                                TitleInfo info = (TitleInfo)menu.Tag;
                                if (!main.CurUser.IsEnable(info.ClassRestriction))
                                {
                                    main.Info("该头衔不适合当前职业");
                                    return;
                                }
                                //
                                main.AddTitle(main.CurUser, info);
                                main.ReadUsers();
                            }
                        };
                    }
                    level.DropDownItems.Add(tsmi);
                }
                else
                {
                    i = 0;
                    k = cls.RequiredLevel / 10;
                    level = new ToolStripMenuItem("lv." + (k * 10 + 1) + "-" + ((k + 1) * 10));
                    menuitem.DropDownItems.Add(level);
                }
            }
        }

        public static void InitCashEnchantMenu(this MainForm main, ToolStripDropDownItem inner,ListView listview)
        {
            //contentMenuCashInnerEnchant
            inner.DropDownItems.Clear();
            EnchantInfo[] enchantinfos = main.DataHelper.GetEnchantInfos();
            foreach (EnchantInfo info in enchantinfos)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(info.Name);
                tsmi.Tag = info;
                tsmi.ToolTipText = info.ToString();//提示文字为真实路径
                tsmi.Click += (object sender, EventArgs e) => {
                    ToolStripMenuItem menu = sender as ToolStripMenuItem;
                    if (menu != null && menu.Tag != null)
                    {
                        EnchantInfo _info = menu.Tag as EnchantInfo;
                        Item item = listview.GetSelectItem<Item>();
                        if (_info != null && item!=null)
                        {
                            //附魔
                            if (main.Enchant(item, _info))
                            {
                                main.log(item.ItemName + " 附魔【" + _info.Name + "】成功。");
                                main.ReadPackage(item.Package);
                            }
                            else
                            {
                                //main.Warnning(item.ItemName + " 附魔【" + _info.Name + "】失败。");
                                main.log(item.ItemName + " 附魔【" + _info.Name + "】失败。");
                            }
                        }
                    }
                };
                if (info.Constraint!=null && info.Constraint.Contains(SubCategory.INNERARMOR.ToString()))
                {
                    if (!info.Class.EndsWith("day7"))
                    {
                        inner.DropDownItems.Add(tsmi);
                    }
                }
            }
        }
        public static void InitEnchantMenu(this MainForm main, ToolStripDropDownItem prefixmenuitem, ToolStripDropDownItem suffixmenuitem,ListView listview)
        {
            prefixmenuitem.DropDownItems.Clear();
            suffixmenuitem.DropDownItems.Clear();
            EnchantInfo[] enchantinfos = main.DataHelper.GetEnchantInfos();
            ToolStripMenuItem prelist = null;
            ToolStripMenuItem suflist = null;
            int li=0, lj = 0;
            int maxi = 0, maxj = 0;
            int max = 10;
            foreach (EnchantInfo info in enchantinfos)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(info.Name);
                tsmi.Tag = info;
                tsmi.ToolTipText = info.ToString();//提示文字为真实路径
                tsmi.Click += (object sender, EventArgs e)=> {
                    Item item = listview.GetSelectItem<Item>();
                    ToolStripMenuItem menu = sender as ToolStripMenuItem;
                    if (menu != null && menu.Tag != null && item!=null)
                    {
                        EnchantInfo _info = menu.Tag as EnchantInfo;
                        if (_info != null)
                        {
                            //附魔
                            if (main.Enchant(item, _info))
                            {
                                main.log(item.ItemName + " 附魔【" + _info.Name + "】成功。");
                                main.ReadPackage(item.Package);
                            }
                            else
                            {
                               // main.Warnning(item.ItemName + " 附魔【" + _info.Name + "】失败。");
                                main.log(item.ItemName + " 附魔【" + _info.Name + "】失败。");
                            }
                        }
                    }
                };
            
                if (info.IsPrefix)
                {
                    if (prelist == null || li != info.EnchantLevel)
                    {
                        maxi = 0;
                        li = info.EnchantLevel;
                        prelist = new ToolStripMenuItem("等级："+li);
                        prefixmenuitem.DropDownItems.Add(prelist);
                    }
                    if (maxi % max == 0 && maxi>=max)
                    {
                        prelist = new ToolStripMenuItem("等级：" + li + "-" + (maxi / max+1));
                        prefixmenuitem.DropDownItems.Add(prelist);
                    }
                    if (info.Constraint != SubCategory.INNERARMOR.ToString())
                    {
                        maxi++;
                        prelist.DropDownItems.Add(tsmi);
                    }
                }
                else
                {
                    if (suflist == null || lj != info.EnchantLevel)
                    {
                        maxj = 0;
                        lj = info.EnchantLevel;
                        suflist = new ToolStripMenuItem("等级：" + lj);
                        suffixmenuitem.DropDownItems.Add(suflist);
                    }
                    if (maxj % max == 0 && maxj>=max)
                    {
                        prelist = new ToolStripMenuItem("等级：" + lj + "-" + (maxj / max+1));
                        suffixmenuitem.DropDownItems.Add(suflist);
                    }
                    maxj++;
                    suflist.DropDownItems.Add(tsmi);
                }
            }
            
        }
    }
}
