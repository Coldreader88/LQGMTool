/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/21
 * 时间: 16:49
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using GMTool.Helper;
using System.Data.SQLite;
using GMTool.Bean;
using GMTool.Helpers;
using System.Data.Common;
using GMTool.Enums;
using GMTool.Extensions;

namespace GMTool.Helper
{
	/// <summary>
	/// Description of SearchHelper.
	/// </summary>
	public class SearchHelper
	{
		private List<ItemClassInfo> Infos;
		
		public Dictionary<string, ItemClassInfo> Items {get;private set;}
		private ItemClassInfo tmp;
		public SearchHelper()
		{
			this.Items = new Dictionary<string, ItemClassInfo>();
			this.Infos=new List<ItemClassInfo>();
		}
		
		public void Add(ItemClassInfo info){
			if(!Items.TryGetValue(info.ItemClass, out tmp)){
				Infos.Add(info);
				Items.Add(info.ItemClass, info);
			}
		}
		
		public int Count{
			get{
				return Infos.Count;
			}
		}
		public List<ItemClassInfo> SearchItems(string name, string id=null,
		                                       string maincategory=null, string subcategory=null, User user=null)
		{
			List<ItemClassInfo> rs = new List<ItemClassInfo>();
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
            }
			foreach (ItemClassInfo info in Infos)
			{
				if (!string.IsNullOrEmpty(name))
				{
					if (info.Name == null || !info.Name.Contains(name))
					{
						continue;
					}
				}
				if (!string.IsNullOrEmpty(id))
				{
					if (info.ItemClass == null || !info.ItemClass.Contains(id))
					{
						continue;
					}
				}
				if (!string.IsNullOrEmpty(subcategory)
				    && subcategory != SubCategory.NONE.Name())
				{
					if (info.SubCategory.Name() != subcategory)
					{
						continue;
					}
				}
				if (!string.IsNullOrEmpty(maincategory)
				    && maincategory != MainCategory.NONE.Name())
				{
					if (info.MainCategory.Name() != maincategory)
					{
						continue;
					}
				}
				if(user!=null && info.ClassRestriction !=0 && !user.IsEnable(info.ClassRestriction)){
					continue;
				}
				rs.Add(info);
			}
			return rs;
		}
	}
}
