using System;
using System.Windows.Forms;
using Vindictus.Bean;
using Vindictus.Extensions;
using Vindictus.Enums;

namespace Vindictus.UI
{
	/// <summary>
	/// 修改角色名字
	/// </summary>
	public class ItemClassDialog : InputDialog
	{
		private readonly MainForm mainForm;
		private Item item;
		private ItemClassInfo info;
		private ClassInfo Class;
		public ItemClassDialog(MainForm main):base()
		{
			this.mainForm = main;
			this.Title = R.ModName;
			this.ContentText = R.TipModName;
			this.OnCheckText  = OnCheckName;
		}
		
		public void SetItem(User user,Item item){
			if(user==null||item==null){
				return;
			}
			this.Class=user.Class;
			this.item = item;
			this.InputText = item.Name;
		}
		public ItemClassInfo ItemInfo{
			get{return info;}
		}
		protected bool OnCheckName(string text){
			if (mainForm == null)
			{
				mainForm.Error(R.NoUser);
				DialogResult = DialogResult.Cancel;
			}else{
				info = mainForm.DataHelper.getItemClassInfo(text);
				if(info == null){
					mainForm.Error(R.NoItem);
					return false;
				}
				if(!Class.IsEnable(info.ClassRestriction)){
					mainForm.Error(R.ClassDontUse);
					return false;
				}
				if(item.MainCategory == MainCategory.WEAPON){
					if(info.MainCategory != MainCategory.WEAPON){
						mainForm.Error(string.Format(R.ItemNotIs, info.MainCategory.Name()+","+info.SubCategory.Name(), MainCategory.WEAPON.Name()));
						return false;
					}
					if(item.SubCategory != info.SubCategory){
						mainForm.Error(string.Format(R.WeaponNotSame, info.MainCategory.Name()+","+info.SubCategory.Name()));
						return false;
					}
				}else {
					//衣服
					if((item.SubCategory == SubCategory.BOOTS||item.SubCategory== SubCategory.BOOTS)
					   && !(info.SubCategory == SubCategory.AVATAR_BOOTS||info.SubCategory== SubCategory.AVATAR_BOOTS)){
						//鞋子
						mainForm.Error(string.Format(R.ItemNotIs, info.MainCategory.Name()+","+info.SubCategory.Name(), SubCategory.BOOTS.Name()));
						return false;
					}
					if((item.SubCategory == SubCategory.PANTS||item.SubCategory== SubCategory.PANTS)
					   && !(info.SubCategory == SubCategory.AVATAR_PANTS||info.SubCategory== SubCategory.AVATAR_PANTS)){
						//鞋子
						mainForm.Error(string.Format(R.ItemNotIs, info.MainCategory.Name()+","+info.SubCategory.Name(), SubCategory.PANTS.Name()));
						return false;
					}
					if((item.SubCategory == SubCategory.GLOVES||item.SubCategory== SubCategory.GLOVES)
					   && !(info.SubCategory == SubCategory.AVATAR_GLOVES||info.SubCategory== SubCategory.AVATAR_GLOVES)){
						//鞋子
						mainForm.Error(string.Format(R.ItemNotIs, info.MainCategory.Name()+","+info.SubCategory.Name(), SubCategory.GLOVES.Name()));
						return false;
					}
					if((item.SubCategory == SubCategory.HELM||item.SubCategory== SubCategory.HELM)
					   && !(info.SubCategory == SubCategory.AVATAR_HELM||info.SubCategory== SubCategory.AVATAR_HELM)){
						//鞋子
						mainForm.Error(string.Format(R.ItemNotIs, info.MainCategory.Name()+","+info.SubCategory.Name(), SubCategory.HELM.Name()));
						return false;
					}
					if((item.SubCategory == SubCategory.TUNIC||item.SubCategory== SubCategory.TUNIC)
					   && !(info.SubCategory == SubCategory.AVATAR_TUNIC||info.SubCategory== SubCategory.AVATAR_TUNIC)){
						//鞋子
						mainForm.Error(string.Format(R.ItemNotIs, info.MainCategory.Name()+","+info.SubCategory.Name(), SubCategory.TUNIC.Name()));
						return false;
					}
				}
				if(mainForm.Question(string.Format(R.SetLookTip, info.Name))){
					return true;
				}
			}
			return false;
		}
	}
}
