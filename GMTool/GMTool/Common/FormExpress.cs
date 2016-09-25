using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public static class FormExpress
    {
        public static void Info(this Form form,string text)
        {
            MessageBox.Show(text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void Error(this Form form, string text)
        {
            MessageBox.Show(text, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void Warnning(this Form form, string text)
        {
            MessageBox.Show( text, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static bool Question(this Form form, string text)
        {
           return MessageBox.Show(text, "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)==DialogResult.OK;
        }

        public static int GetSelectIndex(this ListView listView)
        {
            if (listView.SelectedItems != null)
            {
                if (listView.SelectedItems.Count > 0)
                {
                    return listView.SelectedItems[0].Index;
                }
            }
            return -1;
        }
        public static int[] GetSelectIndexs(this ListView listView)
        {
            if (listView.SelectedItems != null)
            {
                int count = listView.SelectedItems.Count;
                if (count > 0)
                {
                    int[] indexs = new int[count];
                    for (int i = 0; i < count; i++)
                    {
                        indexs[i] = listView.SelectedItems[i].Index;
                    }
                    return indexs;
                }
            }
            return null;
        }
        public static T GetSelectItem<T>(this ListView listView)
        {
            if (listView.SelectedItems != null){
                if (listView.SelectedItems.Count > 0)
                {
                    return (T)listView.SelectedItems[0].Tag;
                }
            }
            return default(T);
        }

        public static T[] GetSelectItems<T>(this ListView listView)
        {
            if (listView.SelectedItems != null)
            {
                int count = listView.SelectedItems.Count;
                if (count > 0)
                {
                    T[] indexs = new T[count];
                    for (int i = 0; i < count; i++)
                    {
                        indexs[i] = (T)listView.SelectedItems[i].Tag;
                    }
                    return indexs;
                }
            }
            return null;
        }
        public static T[] GetItems<T>(this ListView listView)
        {
            int count = listView.Items.Count;
            if (count > 0)
            {
                T[] indexs = new T[count];
                for (int i = 0; i < count; i++)
                {
                    indexs[i] = (T)listView.Items[i].Tag;
                }
                return indexs;
            }
            return null;
        }

        public static void Select(this ListView listView,int index)
        {
            int count = listView.Items.Count;
            if (index >= 0 && index < count)
            {
                listView.Items[index].Selected = true;
                listView.Items[index].EnsureVisible();
            }
        }
        public static void GoToRow(this ListView listView, int index)
        {
            int count = listView.Items.Count;
            if (index >= 0 && index < count)
            {
                listView.Items[index].EnsureVisible();
            }
        }

        public static Control GetMenuConrtol(this ToolStripItem menu)
        {
            if (menu.OwnerItem != null)
            {
                //多级菜单
                //MessageBox.Show("" + menu.OwnerItem.Text);
                return GetMenuConrtol(menu.OwnerItem);
            }
            if (menu.GetCurrentParent() is ContextMenuStrip)
            {
                //一级菜单
                ContextMenuStrip cm = (ContextMenuStrip)menu.GetCurrentParent();
                return cm.SourceControl;
            }
            else
            {
                MessageBox.Show("" + (menu.GetCurrentParent().GetType()));
            }
            return null;
        }
    }
}
