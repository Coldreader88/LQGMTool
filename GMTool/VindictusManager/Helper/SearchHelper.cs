using System;
using System.Collections.Generic;
using System.Linq;

using Vindictus.Bean;
using Vindictus.Enums;
using Vindictus.Extensions;
using System.Windows.Forms;

namespace Vindictus.Helper
{
	public class SearchHelper
	{
		private Dictionary<string, ItemClassInfo> Items;
		
		public SearchHelper(Dictionary<string, ItemClassInfo> Items)
		{
			this.Items = Items;
		}
		
		public int Count{
			get{
				return Items.Count;
			}
		}
		public List<ItemClassInfo> SearchItems(string name, string id=null,
		                                       string maincategory=null, string subcategory=null, User user=null)
		{
			var rs = new List<ItemClassInfo>();
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
            }
			foreach (ItemClassInfo info in Items.Values)
			{
				if (!string.IsNullOrEmpty(name))
				{
                    if (string.IsNullOrEmpty(info.Name))
                    {
                        continue;
                    }
                    string iname = info.Name.ToLower();
                    if (!iname.Contains(name))
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
