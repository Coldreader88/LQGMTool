﻿
using System;
using System.Linq;
using Vindictus.Bean;
using Vindictus.Enums;
using System.Collections.Generic;
using System.Data.Common;

namespace Vindictus.Helper
{
	public class DataHelper
	{
		private CoreConfig config;
		public Dictionary<int, SkillBonusInfo> SynthesisSkillBonues{get;private set;}
		public Dictionary<string, EnchantInfo> Enchants{get;private set;}
		public Dictionary<long, TitleInfo> Titles{get;private set;}
		public Dictionary<string, ItemClassInfo> Items{get;private set;}
		public DataHelper(CoreConfig config)
		{
			this.config=config;
			SynthesisSkillBonues=new  Dictionary<int, SkillBonusInfo>();
			Titles=new Dictionary<long, TitleInfo>();
			Enchants = new Dictionary<string, EnchantInfo>();
			Items =new Dictionary<string, ItemClassInfo>();
		}
		public void ReadData(HeroesTextHelper HeroesText){
			using(SQLiteHelper db=new SQLiteHelper(PathHelper.Combine(
				config.ServerPath, "bin", "heroesContents.db3"))){
				if(db.Open()){
					ReadItems(db, HeroesText);
					ReadEnchants(db,HeroesText);
					ReadTitles(db, HeroesText);
					ReadSkillBonuds(db, HeroesText);
				}
			}
		}
		public TitleInfo[] GetTitles(){
			return Titles.Values.ToArray<TitleInfo>();
		}
		public EnchantInfo[] GetEnchantInfos(){
			return Enchants.Values.ToArray<EnchantInfo>();
		}
		public EnchantInfo GetEnchant(string name){
			EnchantInfo info;
			if(Enchants.TryGetValue(name, out info)){
				return info;
			}
			return null;
		}
		
		public ItemClassInfo getItemClassInfo(string itemclass){
			ItemClassInfo info;
			if(Items.TryGetValue(itemclass, out info)){
				return info;
			}
			return null;
		}
		public SkillBonusInfo getSkillBonusInfo(int id){
			SkillBonusInfo info;
			if(SynthesisSkillBonues.TryGetValue(id, out info)){
				return info;
			}
			return null;
		}
		
		#region synskillbonuds
		private void ReadSkillBonuds(SQLiteHelper db,HeroesTextHelper HeroesText){
			using (DbDataReader reader = db.GetReader("select * from synthesisskillbonus order by classRestriction;"))
			{
				// MessageBox.Show("count=" + HeroesText.SynSkillBonuds.Count);
				while (reader != null && reader.Read())
				{
					SkillBonusInfo info = new SkillBonusInfo(reader, HeroesText);
					if (!SynthesisSkillBonues.ContainsKey(info.ID))
					{
						SynthesisSkillBonues.Add(info.ID, info);
					}
				}
			}
		}
		#endregion#
		
		#region title
		//select titleid,ts.description,targetcount,ispositive,isparty,category,autogivelevel,requiredlevel,classrestriction from ( titlegoalinfo as ts left join  titleinfo as ti on  ti.id=ts.titleid) order by requiredlevel;
		
		private void ReadTitles(SQLiteHelper db,HeroesTextHelper HeroesText){
			List<TitleInfo> titles=new List<TitleInfo>();
			using (DbDataReader reader = db.GetReader(
				"select ts.titleid,ti.description as name,feature,tg.description,category,autogivelevel,requiredlevel,classrestriction" +
				" from (titlestatinfo as ts left join titleinfo as ti on  ts.titleid = ti.id )"+
				" left join titlegoalinfo as tg on tg.titleid=ts.titleid"+
				" group by ts.titleid order by requiredlevel"))
			{
				while (reader != null && reader.Read())
				{
					TitleInfo info = new TitleInfo(reader, HeroesText);
					if (info.ClassRestriction > 0)
					{
						titles.Add(info);
					}
				}
			}
			foreach(TitleInfo info in titles){
				if(!Titles.ContainsKey(info.TitleID)){
					using(DbDataReader reader = db.GetReader("select * from titlestatinfo where titleID = "+info.TitleID)){
						while (reader != null && reader.Read())
						{
							info.UpdateEffect(reader);
						}
						//if(string.IsNullOrEmpty(info.Effect))
						//	continue;
					}
					info.Trim();
					Titles.Add(info.TitleID, info);
				}
			}
		}
		#endregion
		
		#region items
		private void ReadItems(SQLiteHelper db,HeroesTextHelper HeroesText){
			using (DbDataReader reader = db.GetReader("SELECT * FROM ItemClassInfo left join equipiteminfo on ItemClassInfo.itemclass = equipiteminfo.itemclass order by RequiredLevel;"))
			{
				while (reader != null && reader.Read())
				{
					var info=new ItemClassInfo(reader, HeroesText);
					if(!Items.ContainsKey(info.ItemClass)){
						Items.Add(info.ItemClass, info);
					}
				}
			}
		}
		#endregion
		
		#region enchant
		private void ReadEnchants(SQLiteHelper db,HeroesTextHelper HeroesText){
			
			using (DbDataReader reader2 = db.GetReader("SELECT * FROM EnchantInfo ORDER BY EnchantLevel;"))
			{
				while (reader2 != null && reader2.Read())
				{
					EnchantInfo info = new EnchantInfo(reader2,HeroesText);
					if (info.Class.EndsWith("_100"))
					{
						continue;
					}
					if (!Enchants.ContainsKey(info.Class))
					{
						Enchants.Add(info.Class, info);
					}

				}
			}
		}
		#endregion
	}
}
