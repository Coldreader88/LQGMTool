
using System;

namespace TextConvert
{
	class Program
	{
		public static void Main(string[] args)
		{
			#if DEBUG
			args = new string[]{"-s","heroes_text_taiwan.txt","heroes_text_taiwan_out.txt","ignore.txt"};
			#endif
			if(args.Length > 0){
				switch(args[0]){
						case "-l":{
							string file = "heroes_text_taiwan.txt";
							if(args.Length>1){
								file = args[1];
							}
							string outfile="list.txt";
							if(args.Length>2){
								outfile = args[2];
							}
							new Converter(file).GetList(outfile);
						}
						break;
						case "-c":{
							string infile = "heroes_text_taiwan.txt";
							if(args.Length>1){
								infile = args[1];
							}
							string checkfile = "heroes_text_chinese.txt";
							if(args.Length>2){
								checkfile = args[2];
							}
							string listfile = "list_out.txt";
							if(args.Length>3){
								listfile = args[3];
							}
							string outfile=infile.Replace(".txt","_out.txt");
							//"heroes_text_taiwan_out.txt";
							if(args.Length>4){
								outfile = args[4];
							}
							new Converter(infile).Start(listfile,checkfile, outfile);
						}
						break;
						case "-s":{
							string infile = "heroes_text_taiwan.txt";
							if(args.Length>1){
								infile = args[1];
							}
							string outfile=infile.Replace(".txt","_out.txt");
							//"heroes_text_taiwan_out.txt";
							if(args.Length>2){
								outfile = args[2];
							}
							string listfile = "ignore.txt";
							if(args.Length>3){
								listfile = args[3];
							}
							new Converter(infile).TW2CN(outfile, listfile);
						}
                        break;
                    case "-e":
                        {
                            string infile = "heroes_text_taiwan.txt";
                            if (args.Length > 1)
                            {
                                infile = args[1];
                            }
                            string outfile = infile.Replace(".txt", "_out.txt");
                            //"heroes_text_taiwan_out.txt";
                            if (args.Length > 2)
                            {
                                outfile = args[2];
                            }
                            string listfile = "ignore.txt";
                            if (args.Length > 3)
                            {
                                listfile = args[3];
                            }
                            new Converter(infile).TW2CN(outfile, listfile, false);
                        }
                        break;
				}
			}else{
				Console.WriteLine("获取替换规则:-l 台服文本 规则文本");
				Console.WriteLine("替换文本:-c 台服文本 国服文本 规则文本 输出文本");
				Console.WriteLine("繁体转简体:-s 台服文本  输出文本 排除规则文本");
                Console.WriteLine("繁体转简体:-e 台服文本  输出文本 包含规则文本");
            }
			Console.Write("完成任务");
			Console.ReadKey(true);
		}
	}
}