
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vindictus.Helper;
using System.ComponentModel;
using System.Xml;
using ServerManager;
using Vindictus.UI;
using System.Data.Common;
using System.IO;
using System.Threading;
using Vindictus.Enums;
using DataBasePatch;

namespace Vindictus.Extensions
{
	public static class MainFormToolExtension
	{
		#region 沙龙价格
		public static void PricePatch(this MainForm form,CoreConfig Config){
			string db3 = PathHelper.Combine(Config.GamePath ,"sql/heroes.db3");
			string srvdb3 = PathHelper.Combine(Config.BinPath ,"heroesContents.db3");
			if(!File.Exists(db3)){
				form.Warnning(string.Format(R.TipFileNotExist, db3));
				return;
			}
			if(!File.Exists(srvdb3)){
				form.Warnning(string.Format(R.TipFileNotExist, srvdb3));
				return;
			}
			if(form.Question(string.Format(R.TipDealDb, db3, srvdb3))){
				form.PostTask((WaitDialog arg)=>{
				              	arg.SetInfo(string.Format(R.TipDealClientDb, db3));
				              	using(SQLiteHelper db=new SQLiteHelper(db3)){
				              		new PircePatch(db, Config.GameCode)
				              			.SetLogCat(arg.SetInfo)
				              			.Patch();
				              	}
				              	arg.SetInfo(string.Format(R.TipDealServerDb, srvdb3));
				              	using(SQLiteHelper db=new SQLiteHelper(srvdb3)){
				              		new PircePatch(db, Config.GameCode)
				              			.SetLogCat(arg.SetInfo)
				              			.Patch();
				              	}
				              	arg.Info(R.TipTaskCompleted);
				              });
			}
		}
		#endregion
		
		#region 沙龙，发型，妆容，内衣，眉毛
		public static void AllSaLonPatch(this MainForm form,CoreConfig Config){
			string db3 = PathHelper.Combine(Config.GamePath ,"sql/heroes.db3");
			string srvdb3 = PathHelper.Combine(Config.BinPath ,"heroesContents.db3");
			if(!File.Exists(db3)){
				form.Warnning(string.Format(R.TipFileNotExist, db3));
				return;
			}
			if(!File.Exists(srvdb3)){
				form.Warnning(string.Format(R.TipFileNotExist, srvdb3));
				return;
			}
			if(form.Question(string.Format(R.TipDealDb, db3, srvdb3))){
				form.PostTask((WaitDialog arg)=>{
				              	arg.SetInfo(string.Format(R.TipDealClientDb, db3));
				              	AllSalonPatchDb(db3, Config.GameCode, arg.SetInfo);
				              	arg.SetInfo(string.Format(R.TipDealServerDb, srvdb3));
				              	AllSalonPatchDb(srvdb3, Config.GameCode, arg.SetInfo);
				              	arg.Info(R.TipTaskCompleted);
				              });
			}
		}
		private static void AllSalonPatchDb(string dbfile, string country,Action<string> logcat){
			using(SQLiteHelper db=new SQLiteHelper(dbfile)){
				List<BasePatch> patchs = new List<BasePatch>();

				//时装
				// patchs.Add(new AvatarPatch(db, country));
				//发型
				patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Fiona, country));
				patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Evy, country));
				patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Lynn, country));
				patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Vella, country));
				patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Arisha, country));
				patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Delia, country));
				//眉毛
				patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Fiona, country));
				patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Evy, country));
				patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Lynn, country));
				patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Vella, country));
				patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Arisha, country));
				patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Delia, country));
				//妆容
				patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Fiona, country));
				patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Evy, country));
				patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Lynn, country));
				patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Vella, country));
				patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Arisha, country));
				patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Delia, country));
				//内衣
				patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Fiona, country));
				patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Evy, country));
				patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Lynn, country));
				patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Vella, country));
				patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Arisha, country));
				patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Delia, country));
				//价格必须放在最后
				patchs.Add(new PircePatch(db, country));
				foreach (BasePatch patch in patchs)
				{
					patch.SetLogCat(logcat);
					if (patch.Read())
					{
						patch.Patch();
					}
				}
			}
		}
		#endregion
	}
}
