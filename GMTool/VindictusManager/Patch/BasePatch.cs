using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vindictus.Common;
using Vindictus.Helper;
using System.Data.Common;

namespace DataBasePatch
{
    public enum Category
    {
        APPEARANCE,
        AVATAR_BOOTS,
        AVATAR_GLOVES,
        AVATAR_HELM,
        AVATAR_PACKAGE,
        AVATAR_PANTS,
        AVATAR_TUNIC,
        BEARD,
        BODYPAINTING,
        EYEBROW,
        FACEPAINTING,
        HAIR,
        INNERARMOR,
        MAKEUP,
        SCAR,
    }
    public static class CategoryEx
    {
        public static string Key(this Category type)
        {
            if (type == Category.INNERARMOR)
            {
                return "inner";
            }
            else if (type == Category.EYEBROW)
            {
                return "brow";
            }
            return type.ToString().ToLower() ;
        }
        public static string Name(this Category type)
        {
            return type.ToString();
        }
    }
    public abstract class BasePatch
    {
        protected SQLiteHelper db { get; private set; }
       
        protected Dictionary<int, string> Items = new Dictionary<int, string>();
        public BasePatch(SQLiteHelper db)
        {
            this.db = db;
        }

        public BasePatch(string db)
        {
            this.db = new SQLiteHelper(db);
        }

        public bool Read()
        {
            if (db == null) return false;
            if (!db.IsOpen)
            {
                if (!db.Open()) return false;
            }
            //读取旧的
            return ReadData();
        }

        protected abstract bool ReadData();
        public abstract int Patch();
    }
}
