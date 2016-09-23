/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/23
 * 时间: 17:12
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TextConvert
{
	/// <summary>
	/// Description of Converter.
	/// </summary>
	public class Converter
	{
		private string file;
		
		public Converter(string file)
		{
			this.file=file;
		}
		
		#region list
		public void GetList(string outfile){
			Dictionary<string, string> lines=new Dictionary<string, string>();
			using (FileStream fs = new FileStream(file, FileMode.Open))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
				{
					string line = null;
					Regex regex = new Regex("\"(\\S*?_HEROES_\\S+?)\"\\s+\"([\\s\\S]+?)\"");
					while ((line = sr.ReadLine()) != null)
					{
						Match m = regex.Match(line);
						if(m.Groups.Count>2){
							string name = m.Groups[1].Value;
							string oldname = name;
							name = GetName(name);
							string val = oldname+"\t"+m.Groups[2].Value;
							if(!lines.ContainsKey(name)){
								Console.WriteLine(name+"\t"+oldname);
								lines.Add(name, val);
							}
						}
					}
				}
			}
			File.Delete(outfile);
			using (FileStream fs = new FileStream(outfile, FileMode.OpenOrCreate))
			{
				using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
				{
					foreach(string name in lines.Keys){
						sw.WriteLine(lines[name]);
					}
				}
			}
		}
		#endregion
		
		
		public void Start(string list,string checkfile,string outfile){
			List<string> keys=new List<string>();
			using (FileStream fs = new FileStream(list, FileMode.Open))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
				{
					string line = null;
					while ((line = sr.ReadLine()) != null)
					{
						string key = GetKey(line);
						if(!keys.Contains(key)){
							keys.Add(key);
						}
					}
				}
			}
			using (FileStream fs = new FileStream(file, FileMode.Open))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
				{
					string line = null;
					Regex regex = new Regex("\"(\\S*?_HEROES_\\S+?)\"\\s+\"([\\s\\S]+?)\"");
					while ((line = sr.ReadLine()) != null)
					{
						Match m = regex.Match(line);
						if(m.Groups.Count>2){
							string name = m.Groups[1].Value;
							string oldname = name;
							name = GetName(name);
							if(keys.Contains(name)){
								//从checkfile复制该行，用oldname去找
								continue;
							}
						}
						File.AppendText(line+"\r\n");
					}
				}
			}
		}
		private string GetKey(string line){
			return line.Split('\t')[0];
		}
		private string GetName(string name){
			int i = name.IndexOf("HEROES_");
			if(i>=0){
				int j = name.IndexOf("_", i+8);
				//如果是最后一个_
				int k = name.LastIndexOf("_");
				if(j == k){
					name = name.Substring(0, i+6);
				}else  if(j>=0){
					name = name.Substring(0, j);
				}
			}
			return name;
		}
	}
}