using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vindictus.Enums;
using Vindictus.Extensions;
using System.Data.Common;
using Vindictus.Helper;

namespace Vindictus.Bean
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
				sb.Append(R.UserName+"：\t" + Name 
				          + "\n"+R.UserClass+"：\t" + Class.Name()
				          +"\n"+R.UserLevel+"：\t" + level);
				sb.Append(Stat.ToString());
				sb.Append("\nAP:\t"+AP);
				if (Group != GroupInfo.Unknown)
				{
					sb.Append("\n["+R.UserGroup+"：" + Group.Name() + " lv." + GroupLevel+"]");
				}
				
				_txt= sb.ToString();;
			}
			return _txt;
		}
		public override string ToString()
		{
			return Name + " [" + Class.Name() + "] lv." + level;
		}
	}
}
