using GMTool.Bean;
using GMTool.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GMTool.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="listview"></param>
        /// <param name="mails"></param>
        /// <returns>is send</returns>
        public static int AddMails(this MainForm main, string tag, ListView listview, List<Mail> mails)
        {
            //TODO
            int count = mails.Count;
            //TODO
            listview.BeginUpdate();
            listview.Items.Clear();

            if (count >= 0)
            {
                ListViewItem[] items = new ListViewItem[count];
                for (int i = 0; i < count; i++)
                {
                    Mail u = mails[i];
                    items[i] = new ListViewItem();
                    items[i].Tag = u;
                    items[i].Text = u.Title;
                    if (i % 2 == 0)
                        items[i].BackColor = Color.GhostWhite;
                    else
                        items[i].BackColor = Color.White;
                    items[i].ToolTipText = u.ToString();
                }
                listview.Items.AddRange(items);
            }
            listview.EndUpdate();
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main"></param>
        /// <param name="t"></param>
        /// <param name="info"></param>
        /// <param name="i">位置</param>
        /// <param name="listview"></param>
        /// <param name="selectindex">选择索引</param>
        /// <returns></returns>
        public static ListViewItem GetItemView(this MainForm main, Item t,int i,ListView listview,bool fullname,bool hascolor,bool has2category,out int selectindex)
        {
            selectindex = -1;
            ListViewItem vitem = new ListViewItem();
            ItemClassInfo info = main.DataHelper.GetItemInfo(t.ItemClass);
            t.Attach(info);
            vitem.Tag = t;
            vitem.Text = (t.ItemName == null ? t.ItemClass : t.ItemName);
            if (fullname && t.Attributes != null)
            {
                string head = "";
                foreach (ItemAttribute attr in t.Attributes)
                {
                    if (attr.Type == ItemAttributeType.ENHANCE)
                    {
                        head = "+" + attr.Value + " ";
                        break;
                    }
                }

                foreach (ItemAttribute attr in t.Attributes)
                {
                    if (attr.Type == ItemAttributeType.PREFIX)
                    {
                        EnchantInfo einfo = main.DataHelper.GetEnchant(attr.Value);
                        head += "【" + (einfo == null ? attr.Value : einfo.Name) + "】";
                        break;
                    }
                }
                foreach (ItemAttribute attr in t.Attributes)
                {
                    if (attr.Type == ItemAttributeType.SUFFIX)
                    {
                        EnchantInfo einfo = main.DataHelper.GetEnchant(attr.Value);
                        head += "【" + (einfo == null ? attr.Value : einfo.Name) + "】";
                        break;
                    }
                }
                foreach (ItemAttribute attr in t.Attributes)
                {
                    if (attr.Type == ItemAttributeType.QUALITY)
                    {
                        head += "★" + attr.Arg + " ";
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(head))
                {
                    vitem.Text = head + vitem.Text;
                }
            }
            if (i % 2 == 0)
                vitem.BackColor = Color.GhostWhite;
            else
                vitem.BackColor = Color.White;
            if (main.NormalCurItem == i)
            {
                selectindex = i;
                vitem.Checked = true;
                vitem.Selected = true;
            }
            vitem.SubItems.Add("" + t.Count);
            if (has2category)
            {
                vitem.SubItems.Add("" + t.MainCategory);
            }
            vitem.SubItems.Add("" + t.SubCategory);
            if (hascolor)
            {
                vitem.UseItemStyleForSubItems = false;
                int colorIndex = vitem.SubItems.Count;
                vitem.SubItems.Add("");// + (t.Color1 == 0 ? "" : t.Color1.ToString("x")));
                vitem.SubItems.Add("");// + (t.Color1 == 0 || t.Color2 == 0 ? "" : t.Color2.ToString("x")));
                vitem.SubItems.Add("");// + (t.Color1 == 0 || t.Color3 == 0 ? "" : t.Color3.ToString("x")));
                vitem.SubItems[colorIndex].BackColor = t.Color1.GetColor();
                vitem.SubItems[colorIndex + 1].BackColor = t.Color2.GetColor();
                vitem.SubItems[colorIndex + 2].BackColor = t.Color3.GetColor();
                vitem.SubItems[colorIndex].ForeColor = vitem.SubItems[colorIndex].BackColor;
                vitem.SubItems[colorIndex + 1].ForeColor = vitem.SubItems[colorIndex + 1].BackColor;
                vitem.SubItems[colorIndex + 2].ForeColor = vitem.SubItems[colorIndex + 2].BackColor;
            }
            vitem.SubItems.Add("" + t.Time);
            vitem.ToolTipText = t.ToString();
            return vitem;
        }
    }
}
