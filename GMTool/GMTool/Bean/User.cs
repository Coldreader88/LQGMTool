using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMTool.Enums;
using GMTool.Extensions;
using System.Data.Common;
using GMTool.Helper;

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

		public int AP{ get; private set; }
		public StatInfo Stat{ get; private set; }
		public GroupInfo Group { get; private set; }
		public int GroupLevel { get; private set; }
		private string _txt;
		public User(){
			
		}
		public User(DbDataReader reader)
		{
			this.CID = reader.ReadInt64("ID");
			this.UID =  reader.ReadInt32("UID");
			this.CharacterSN = reader.ReadInt32("CharacterSN");
			this.Name =  reader.ReadString("Name");
			this.Class =  reader.ReadInt32("Class").ToClassInfo();
			this.level = reader.ReadInt32("Level");
			this.AP = reader.ReadInt32("AP");
			this.Stat= new StatInfo(reader);
			this.Group = GroupInfo.Unknown;
		}
		
		public void UpdateGroup(DbDataReader reader){
			int group = reader.ReadInt32("vocationClass", -1);
			this.Group = group.ToGroupInfo();
			this.GroupLevel = reader.ReadInt32("VocationLevel", -1);
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
