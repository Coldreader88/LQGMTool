
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
					case "-l":
						string file = "heroes_text_taiwan.txt";
						if(args.Length>1){
							file = args[1];
						}
						string outfile="list.txt";
						if(args.Length>2){
							outfile = args[2];
						}
						new Converter(file).GetList(outfile);
						break;
				}
			}
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}