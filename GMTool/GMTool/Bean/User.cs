using System;
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

        public User(long ID, int UID, int CharacterSN,string Name,int Class, int level)
        {
            this.CID = ID;
            this.UID = UID;
            this.CharacterSN = CharacterSN;
            this.Name = Name;
            this.Class = Class.GetClass();
            this.level = level;
        }

        public override string ToString()
        {
        	return  "角色："+Name + " 职业：" + Class+"["+Class.Index()+"] 等级:"+level;
        }
    }
}
