
using System;

namespace TextConvert
{
	class Program
	{
		public static void Main(string[] args)
		{
			args = new string[]{"-l"};
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
							string outfile="heroes_text_taiwan_out.txt";
							if(args.Length>4){
								outfile = args[4];
							}
							new Converter(infile).Start(listfile,checkfile, outfile);
						}
						break;
				}
			}
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}