
using System;
using GMTool.Common;
using System.IO;

namespace VZipTool
{
	class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("Options:");
				Console.WriteLine("a [dir]  Copies all .hfs in the current directory to .zip");
				Console.WriteLine("h [file]  Convert .zip to .hfs");
				Console.WriteLine("z [file]  Convert .hfs to .zip");
				Console.WriteLine("r [hfs] [file] Add/Replace file to .hfs");
				Console.WriteLine("d [hfs] [name] Delete name to .hfs");
				return;
			}

			try
			{
				string cmd = args[0];
				if(cmd.StartsWith("-")){
					cmd = cmd.Substring(1);
				}
				switch (cmd)
				{
					case "a":
						{
							string dir = Directory.GetCurrentDirectory();

							if (args.Length > 1)
								dir = args[1];

							string[] files = Directory.GetFiles(dir, @"*.hfs");

							foreach (string file in files)
							{
								VZip.ExtractHfs(file);
							}
						}
						break;
					case "z":
						{
							if (args.Length < 2)
							{
								return;
							}

							VZip.ExtractHfs(args[1]);
						}
						break;
					case "h":
						{
							if (args.Length < 2)
							{
								return;
							}

							VZip.ConvertZip(args[1]);
						}
						break;
					case "r":
						{
							if (args.Length < 3)
							{
								return;
							}

							VZip.AddOrReplace(args[1],args[2]);
						}
						break;
					case "d":
						{
							if (args.Length < 3)
							{
								return;
							}

							VZip.Delete(args[1],args[2]);
						}
						break;
					default:
						Console.WriteLine("Options:");
						Console.WriteLine("a [dir]  Copies all .hfs in the current directory to .zip");
						Console.WriteLine("h [file]  Convert .zip to .hfs");
						Console.WriteLine("z [file]  Convert .hfs to .zip");
						Console.WriteLine("r [hfs] [file] Add/Replace file to .hfs");
							Console.WriteLine("d [hfs] [name] Delete name to .hfs");
						break;
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			Console.ReadKey();
		}
	}
}