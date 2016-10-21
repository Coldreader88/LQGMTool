using System;
using System.Collections.Generic;
using System.Linq;

using Vindictus.Bean;
using Vindictus.Enums;
using Vindictus.Extensions;

namespace Vindictus.Helper
{
	public class SearchHelper
	{
		private List<ItemClassInfo> Infos;
		
		public Dictionary<string, ItemClassInfo> Items {get;private set;}
		private ItemClassInfo tmp;
		public SearchHelper(Dictionary<string, ItemClassInfo> Items)
		{
			this.Items = Items;
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
