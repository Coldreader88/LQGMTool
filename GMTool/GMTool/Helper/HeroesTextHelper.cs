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
	/// Description of HeroesTextHelper.
	/// </summary>
	public class HeroesTextHelper
	{
		public Dictionary<string, string> PrefixNames{get; private set;}
		public Dictionary<string, string> SuffixNames{get; private set;}
		public Dictionary<string, string> EnchantDescs{get; private set;}
		public Dictionary<string, string> EnchantEffects{get; private set;}
		public Dictionary<string, string> EnchantEffectIfs{get; private set;}
		public Dictionary<string, string> ItemNames{get; private set;}
		public Dictionary<string, string> ItemDescs{get; private set;}
		public Dictionary<string, string> TitleNames{get; private set;}
		public Dictionary<string, string> StatNames{get; private set;}
		public Dictionary<string, string> TitleDescs{get; private set;}
		private bool TW2CN = true;
		private Dictionary<Regex, Dictionary<string, string>> Regexs
			=new Dictionary<Regex, Dictionary<string, string>>();
		
		public HeroesTextHelper()
		{
			IniHelper helper = new IniHelper(Program.INT_FILE);
				string val  =helper.ReadValue("data", "forceZhCN");
			if(val!=null){
				val  = val.ToLower();
			}
			TW2CN = "开" == val ||"true" == val;
			PrefixNames =new Dictionary<string, string>();
			SuffixNames =new Dictionary<string, string>();
			EnchantDescs =new Dictionary<string, string>();
			EnchantEffects =new Dictionary<string, string>();
			EnchantEffectIfs =new Dictionary<string, string>();
			ItemNames =new Dictionary<string, string>();
			ItemDescs =new Dictionary<string, string>();
			TitleNames = new Dictionary<string, string>();
			StatNames=new Dictionary<string, string>();
			TitleDescs=new Dictionary<string, string>();
			Regexs.Add(new Regex("HEROES_ATTRIBUTE_PREFIX_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), PrefixNames);
			Regexs.Add(new Regex("HEROES_ATTRIBUTE_SUFFIX_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), SuffixNames);
			Regexs.Add(new Regex("HEROES_ITEMCONSTRAINT_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), EnchantDescs);
			Regexs.Add(new Regex("HEROES_ENCHANTSTAT_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), EnchantEffects);
			Regexs.Add(new Regex("HEROES_ENCHANTCONDITION_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), EnchantEffectIfs);
			Regexs.Add(new Regex("HEROES_ITEM_NAME_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), ItemNames);
			Regexs.Add(new Regex("HEROES_ITEM_DESC_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), ItemDescs);
			Regexs.Add(new Regex("(HEROES_TITLE_NAME_\\S+?)\"\\s+\"([\\s\\S]+?)\""), TitleNames);
			Regexs.Add(new Regex("HEROES_MARKETDIALOG_DETAILOPTION_TITLE_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), StatNames);
			Regexs.Add(new Regex("(HEROES_TITLE_GOAL_NAME_\\S+?)\"\\s+\"([\\s\\S]+?)\""), TitleDescs);
		}
		
		public void Read(string file){
			if(!File.Exists(file))return;
			using (FileStream fs = new FileStream(file, FileMode.Open))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
				{
					string line = null;
					
					while ((line = sr.ReadLine()) != null)
					{
						foreach(Regex regex in Regexs.Keys){
							if(Add(regex, line, Regexs[regex])){
								break;
							}
						}
					}
				}
			}
		}
		
		private bool Add(Regex regex, string line,Dictionary<string, string> dic){
			Match m = regex.Match(line);
			if (m!= null)
			{
				if (m.Groups.Count > 2)
				{
					string v;
					string k = m.Groups[1].Value.ToLower();
					if (!dic.TryGetValue(k, out v))
					{
						dic.Add(k, ToCN(m.Groups[2].Value));
					}
					return true;
				}
			}
			return false;
		}
		private string ToCN(string tw)
		{
			if (tw == null)
			{
				return string.Empty;
			}
			if (TW2CN)
			{
				tw = tw.Replace("\\n", "\n").Trim();
				return ChineseTextHelper.ToSimplified(tw);
			}
			return tw;
		}

	}
}
