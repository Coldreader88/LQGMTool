﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMTool.Enums;
using GMTool.Extensions;

namespace GMTool.Bean
{
	public class User
	{
		public long CID { get; private set; }
		public int UID { get; private set; }
		public int CharacterSN { get; private set; }
		public string Name { get; set; }
		public ClassInfo Class { get; private set; }
		public int level { get; private set; }

		public int AP{get;set;}
		public StatInfo Stat{get;set;}
		public GroupInfo Group { get; set; }
		public int GroupLevel {get; set;}
		private string _txt;
		public User(long ID, int UID, int CharacterSN,string Name,int Class, int level)
		{
			this.Stat=new StatInfo();
			this.CID = ID;
			this.UID = UID;
			this.CharacterSN = CharacterSN;
			this.Name = Name;
			this.Class = Class.ToClassInfo();
			this.level = level;
			this.Group = GroupInfo.Unknown;
		}
		public string ToLongString()
		{
			if(string.IsNullOrEmpty(_txt)){
				StringBuilder sb=new StringBuilder();
				sb.Append("CID:"+CID);
				sb.Append("\n--------------------------\n");
				sb.Append("角色：" + Name + "\n职业：" + Class.Name() + "\n等级：" + level);
				sb.Append(Stat.ToString());
				sb.Append("\n技能点(AP):"+AP);
				if (Group != GroupInfo.Unknown)
				{
					sb.Append("\n[阵营：" + Group.Name() + " lv." + GroupLevel+"]");
				}
				
				_txt= sb.ToString();;
			}
			return _txt;
		}
		public override string ToString()
		{
			string txt= "角色：" + Name + " 职业：" + Class.Name() + " lv." + level;
			return txt;
		}
	}
}
