/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/30
 * 时间: 17:09
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using Vindictus.Extensions;
using System.Data.Common;
using Vindictus.Helper;

namespace Vindictus.Bean
{
    /// <summary>
    /// Description of SynthesisSkillBonus.
    /// </summary>
    public class SkillBonusInfo
    {
        public int ID { get; private set; }
        public string Grade { get; private set; }
        public int ClassRestriction { get; private set; }
        //描述
        public string DESC { get; private set; }
        //技能id
        public string SkillID { get; private set; }
        //效果类型
        public string Type { get; private set; }
        //百分比
        public int Value { get; private set; }
        public SkillBonusInfo()
        {
        }
        public SkillBonusInfo(DbDataReader reader, HeroesTextHelper HeroesText)
        {
            this.ID = reader.ReadInt32("ID");
            this.Grade = reader.ReadString("Grade");
            this.ClassRestriction = reader.ReadInt32("ClassRestriction");
            this.DESC = reader.ReadString("DESCID");
            string tmp;
            if (HeroesText.SynSkillBonuds.TryGetValue("" + ID, out tmp))
            {
                this.DESC = tmp;
            }
            this.SkillID = reader.ReadString("SkillID");
            this.Type = reader.ReadString("Type");
            this.Value = reader.ReadInt32("Value");
        }
        public string GetKey()
        {
            if (ID > 0)
            {
                return Grade + "/" + ID;
            }
            return Grade;
        }
        public override string ToString()
        {
            return GetKey() + " " +(string.IsNullOrEmpty(DESC)?( SkillID + " " + Type + " " + Value): DESC);
        }

    }
}
