using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using System.Data.Common;
using Vindictus.Common;
using Vindictus.Helper;

namespace Vindictus.Helper
{
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
		public Dictionary<string, string> TitleDescs{get; private set;}
		public static Dictionary<string, string> ItemStatNames{get; private set;}
		public static Dictionary<string, string> MailTitles { get; private set; }
		public Dictionary<string, string> SynSkillBonuds{get;private set;}

		private Dictionary<Regex, Dictionary<string, string>> Regexs
			=new Dictionary<Regex, Dictionary<string, string>>();
		
		public HeroesTextHelper()
		{
			PrefixNames =new Dictionary<string, string>();
			SuffixNames =new Dictionary<string, string>();
			EnchantDescs =new Dictionary<string, string>();
			EnchantEffects =new Dictionary<string, string>();
			EnchantEffectIfs =new Dictionary<string, string>();
			ItemNames =new Dictionary<string, string>();
			ItemDescs =new Dictionary<string, string>();
			TitleNames = new Dictionary<string, string>();
			TitleDescs=new Dictionary<string, string>();
			MailTitles = new Dictionary<string, string>();
			ItemStatNames=new Dictionary<string, string>();
			SynSkillBonuds=new Dictionary<string, string>();
			Regexs.Add(new Regex("HEROES_ATTRIBUTE_PREFIX_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), PrefixNames);
			Regexs.Add(new Regex("HEROES_ATTRIBUTE_SUFFIX_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), SuffixNames);
			Regexs.Add(new Regex("HEROES_ITEMCONSTRAINT_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), EnchantDescs);
			Regexs.Add(new Regex("HEROES_ENCHANTSTAT_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), EnchantEffects);
			Regexs.Add(new Regex("HEROES_ENCHANTCONDITION_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), EnchantEffectIfs);
			Regexs.Add(new Regex("HEROES_ITEM_NAME_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), ItemNames);
			Regexs.Add(new Regex("HEROES_ITEM_DESC_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), ItemDescs);
			Regexs.Add(new Regex("(HEROES_TITLE_NAME_\\S+?)\"\\s+\"([\\s\\S]+?)\""), TitleNames);
			Regexs.Add(new Regex("(HEROES_TITLE_GOAL_NAME_\\S+?)\"\\s+\"([\\s\\S]+?)\""), TitleDescs);
			Regexs.Add(new Regex("\"(\\S+?MAIL_TITLE)\"\\s+\"([\\s\\S]+?)\""), MailTitles);
			Regexs.Add(new Regex("HEROES_ITEMSTAT_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), ItemStatNames);
			Regexs.Add(new Regex("HEROES_DESC_AVATAR_SKILL_BONUS_(\\S+?)\"\\s+\"([\\s\\S]+?)\""), SynSkillBonuds);
		}
		
		public void Read(string file,string patch=null){
			if(!File.Exists(file))return;
            ReadText(file, false);
            if (!string.IsNullOrEmpty(patch) && File.Exists(patch))
            {
                ReadText(patch, true);
            }
		}
		public static string GetMailTitle(string title)
		{
			if (title!=null && title.StartsWith("#")) {
				string t;
				if (MailTitles.TryGetValue(title.Substring(1).ToLower(), out t))
				{
					return t;
				}
			}
			return title;
		}
        private int ReadText(string file,bool force)
        {
            int count = 0;
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
                {
                    string line = null;

                    while ((line = sr.ReadLine()) != null)
                    {
                        foreach (Regex regex in Regexs.Keys)
                        {
                            if (Add(regex, line, Regexs[regex], force))
                            {
                                count++;
                                break;
                            }
                        }
                    }
                }
            }
            return count;
        }

        private bool Add(Regex regex, string line,Dictionary<string, string> dic,bool force=false){
			Match m = regex.Match(line);
			if (m!= null)
			{
				if (m.Groups.Count > 2)
				{
					string k = m.Groups[1].Value.ToLower();
                    string v = ToCN(m.Groups[2].Value);
                    if (!dic.ContainsKey(k))
                    {
                        dic.Add(k, v);
                    }
                    else if(force)
                    {
                        //存在
                        dic[k] = v;
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
			return tw.Replace("\\n","\n").Replace("\\t","\t");
		}

	}
}
