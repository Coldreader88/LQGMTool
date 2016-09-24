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
using GMTool.Helper;

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
							string val =name+"\t"+ oldname+"\t"+m.Groups[2].Value;
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
		
		#region convert
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
			Dictionary<string, string> dic=new Dictionary<string, string>();
			using (FileStream fs = new FileStream(checkfile, FileMode.Open))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
				{
					string line = null;
					Regex regex = new Regex("\"(\\S*?_HEROES_\\S+?)\"\\s+\"([\\s\\S]+?)\"");
					while ((line = sr.ReadLine()) != null)
					{
						Match m = regex.Match(line);
						if(m.Groups.Count > 2){
							string name = m.Groups[1].Value;
							string val = m.Groups[2].Value;
							if(dic.ContainsKey(name)){
								dic.Add(name, val);
							}
						}
					}
				}
			}
			File.Delete(outfile);
			//以file为蓝本，根据list从check复制行出来到out
			using (FileStream fs = new FileStream(file, FileMode.Open))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
				{
					using (FileStream fs2 = new FileStream(outfile, FileMode.OpenOrCreate))
					{
						using (StreamWriter sw = new StreamWriter(fs2, Encoding.Unicode))
						{
							string line = null;
							Regex regex = new Regex("\"(\\S*?_HEROES_\\S+?)\"\\s+\"([\\s\\S]+?)\"");
							while ((line = sr.ReadLine()) != null)
							{
								Match m = regex.Match(line);
								if(m.Groups.Count > 2){
									string name = m.Groups[1].Value;
									string oldname = name;
									string val;
									name = GetName(name);
									if(keys.Contains(name)){
										//从checkfile复制该行，用oldname去找
										if(dic.TryGetValue(name, out val)){
											//
											line = line.Replace(m.Groups[1].Value, val);
										}
									}
								}
								sw.WriteLine(line);
							}
							//原始
						}
					}
				}
			}
		}
		#endregion
		
		/// <summary>
		/// 繁体转简体
		/// </summary>
		/// <param name="outfile">输出文本</param>
		/// <param name="listfile">规则文本，不转简体</param>
		public void TW2CN(string outfile,string listfile){
			Dictionary<string, bool> rules=new Dictionary<string, bool>();
			using (FileStream fs = new FileStream(listfile, FileMode.Open))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
				{
					string line = null;
					while ((line = sr.ReadLine()) != null)
					{
						string[] keys = line.Split(' ');
						string name = keys[0];
						bool isall = (keys.Length>1&&"all".Equals(keys[1]));
						if(!rules.ContainsKey(name)){
							Console.WriteLine("rule:"+isall+":"+name);
							rules.Add(name, isall);
						}
					}
				}
			}
			File.Delete(outfile);
			//以file为蓝本，根据list从check复制行出来到out
			using (FileStream fs = new FileStream(file, FileMode.Open))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
				{
					using (FileStream fs2 = new FileStream(outfile, FileMode.OpenOrCreate))
					{
						using (StreamWriter sw = new StreamWriter(fs2, Encoding.Unicode))
						{
							string line = null;
							Regex regex = new Regex("\"(\\S*?_\\S+?)\"\\s+\"([\\s\\S]+?)\"");
							while ((line = sr.ReadLine()) != null)
							{
								bool ignore= false;
								Match m = regex.Match(line);
								if(m.Groups.Count > 2){
									string name = m.Groups[1].Value;
									foreach(string key in rules.Keys){
										bool all = rules[key];
										if(all){
											if(name.Equals(key)){
												ignore=true;
												Console.WriteLine("ignore:"+name);
												break;
											}
										}else{
											if(name.StartsWith(key)){
												ignore=true;
												Console.WriteLine("ignore:"+name);
												break;
											}
										}
										
									}
								}
								if(!ignore){
									line = ChineseTextHelper.ToSimplified(line);
								}
								sw.WriteLine(line);
							}
						}
					}
				}
			}
		}
		
		#region common
		private string GetKey(string line){
			return line.Split('\t')[0];
		}
		private string GetName(string name){
			string head = "HEROES_";
			int i = name.IndexOf(head);
			if(i>=0){
				int j = name.IndexOf("_", i+head.Length);
				//如果是最后一个_
				int k = name.LastIndexOf("_");
				if(j == k || (i+head.Length-1)==k){
					name = name.Substring(0, i+head.Length);
				}else  if(j>=0){
					name = name.Substring(0, j+1);
				}
			}
			return name;
		}
		#endregion
	}
}