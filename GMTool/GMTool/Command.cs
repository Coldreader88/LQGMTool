/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/10/12
 * 时间: 12:30
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.IO;
using System.Text;
using GMTool.Helper;
using System.Data.Common;
using GMTool.Common;
using System.Windows.Forms;

namespace GMTool
{
	public class Command
	{
		#region member
		private string[] args;
		public Command(string[] args)
		{
			this.args=args;
		}
		
		public void run(){
			if(args==null || args.Length==0){
				return;
			}
			switch(args[0]){
				case "-tw2cn":
					if(args.Length < 2){
						return;
					}else if(args.Length == 2){
						TW2CN(args[1]);
						return;
					}
					string file = args[2];
					switch(args[1]){
						case "db":
							DbTW2CN(file);
							break;
						case "text":
							TextTW2CN(file);
							break;
						default:
							//解压db，text
							TW2CN(file);
							break;
					}
					break;
				case "/?":
				case "help":
				case "/help":
					Console.WriteLine("-tw2cn db [dbpath] 使得台服以zh-cn启动");
					Console.WriteLine("-tw2cn text [textpath] 繁体转简体");
					Console.WriteLine("-tw2cn [gamepath] 使得台服以zh-cn启动，和简体字");
					break;
			}
		}
		#endregion
		
		#region 繁体转简体
		public void TW2CN(string path){
			//font
			Console.WriteLine("复制字体");
			string fontpath = PathHelper.Combine(path ,"scaleform", "font");
			if(!Directory.Exists(fontpath)){
				Directory.CreateDirectory(fontpath);
			}
			string curfont = PathHelper.Combine(Application.StartupPath, "fonts_ch.swf");
			string cnfont = PathHelper.Combine(fontpath, "fonts_ch.swf");
			if(!File.Exists(cnfont)){
				if(File.Exists(curfont)){
					File.Copy(curfont, cnfont);
				}else{
					if(!VZip.ExtractAllHfsFindFile(PathHelper.Combine(path, "hfs"),
					                               "fonts_ch.swf",
					                               fontpath)){
						if(VZip.ExtractAllHfsFindFile(PathHelper.Combine(path, "hfs"),
						                              "fonts_tw.swf",
						                              fontpath)){
							string text = PathHelper.Combine(fontpath, "fonts_tw.swf");
							File.Move(text, cnfont);
						}
					}
				}
			}
			//db
			string textpath = PathHelper.Combine(path, "resource","localized_text","chinese");
			string cntext = PathHelper.Combine(textpath, "heroes_text_chinese.txt");
			if(!File.Exists(cntext)){
				if(VZip.ExtractAllHfsFindFile(PathHelper.Combine(path, "hfs"),
				                              "heroes_text_taiwan.txt",
				                              textpath)){
					string text = PathHelper.Combine(textpath, "heroes_text_taiwan.txt");
					
					File.Delete(cntext);
					File.Move(text, cntext);
					TextTW2CN(cntext);
				}
			}
			Console.WriteLine("处理数据库");
			string dbpath = PathHelper.Combine(path, "sql");
			string cndb  =PathHelper.Combine(dbpath, "heroes.db3");
			if(!File.Exists(cndb)){
				if(!VZip.ExtractAllHfsFindFile(PathHelper.Combine(path, "hfs"),
				                              "heroes.db3", dbpath)){
					return;
				}
			}
			if(File.Exists(cndb)){
				DbTW2CN(cndb);
			}
		}
		public void DbTW2CN(string dbfile){
			//alter table featurematrix add column zhcn_bak text;
			SQLiteHelper db=new SQLiteHelper(dbfile);
			if(db.Open()){
				try{
					db.ExcuteSQL("alter table featurematrix add column zhcn_bak text;");
					db.ExcuteSQL("update featurematrix set zhcn_bak =\"zh-CN\";");
				}catch{
					
				}
				db.ExcuteSQL("update featurematrix set "+
				             "\"zh-CN\" = NULL,"+
				             "\"zh-CN-x-gm\"= NULL WHERE feature='UseGMFont';");
				db.ExcuteSQL("update featurematrix set "+
				             "\"zh-CN\" =\"zh-TW\","+
				             "\"zh-CN-x-gm\"=\"zh-TW-x-gm\";");
			}
		}
		/// <summary>
		/// 繁体转简体
		/// </summary>
		public void TextTW2CN(string infile)
		{
			string outfile = infile+".tmp";
			if(!File.Exists(infile)){
				Console.WriteLine("文件不存在："+infile);
				return;
			}
			Console.WriteLine("繁体翻译变简体");
			File.Delete(outfile);
			//以file为蓝本，根据list从check复制行出来到out
			using (FileStream fs = new FileStream(infile, FileMode.Open))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
				{
					using (FileStream fs2 = new FileStream(outfile, FileMode.OpenOrCreate))
					{
						using (StreamWriter sw = new StreamWriter(fs2, Encoding.Unicode))
						{
							string line = null;
							while ((line = sr.ReadLine()) != null)
							{
								line = ChineseTextHelper.ToSimplified(line);
								sw.WriteLine(line);
							}
						}
					}
				}
			}
			File.Delete(infile);
			File.Move(outfile, infile);
		}
		#endregion
	}
	
}
