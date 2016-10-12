using System;
using System.IO;
using System.Xml.Serialization;

namespace GMTool.Common
{
    public class XmlHelper
    {
        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml)
        {
            T t = default(T);
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(typeof(T));
                    t = (T)xmldes.Deserialize(sr);
                }
            }
            catch (Exception)
            {
            }
            return t;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(typeof(T));
            return (T)xmldes.Deserialize(stream);
        }
        public static T DeserializeByFile<T>(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                return Deserialize<T>(fs);
            }
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer<T>(T obj)
        {
            string str = "";
            using (MemoryStream Stream = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                try
                {
                    //序列化对象
                    xml.Serialize(Stream, obj);
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
                Stream.Position = 0;
                using (StreamReader sr = new StreamReader(Stream))
                {
                    str = sr.ReadToEnd();
                };
            }
            return str;
        }
        public static void Serializer<T>(T obj, string file)
        {
            using (FileStream Stream = new FileStream(file, FileMode.OpenOrCreate))
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                try
                {
                    //序列化对象
                    xml.Serialize(Stream, obj);
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
            }
        }
        #endregion
    }
}
