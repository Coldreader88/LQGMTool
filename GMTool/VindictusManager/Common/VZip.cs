using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Hfs;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using Vindictus.Helper;

namespace Vindictus.Common
{
	public class StreamLocalSourceHfs : ICSharpCode.SharpZipLib.Hfs.IStaticDataSource
	{
		private Stream stream;
		public StreamLocalSourceHfs(Stream x)
		{
			this.stream = x;
		}

		public Stream GetSource()
		{
			return this.stream;
		}
	};

	public class StreamLocalSourceZip : ICSharpCode.SharpZipLib.Zip.IStaticDataSource
	{
		private Stream stream;
		public StreamLocalSourceZip(Stream x)
		{
			this.stream = x;
		}

		public Stream GetSource()
		{
			return this.stream;
		}
	};
	public class VZip
	{
		public static bool ExtractAllHfsFindFile(string dir, string filename,string path=null)
		{
			string[] files = Directory.GetFiles(dir);
			if(files != null){
				if(path == null){
					path = Path.GetDirectoryName(dir);
				}
				foreach(string file in files){
					try{
						using (HfsFile hfs = new HfsFile(file)){
							foreach (HfsEntry hfsEntry in hfs)
							{
								if(filename == hfsEntry.Name||filename+".comp" == hfsEntry.Name){
									using(Stream read = hfs.GetInputStream(hfsEntry)){
										string fullname = PathHelper.Combine(path, filename);
										string fullpath = Path.GetDirectoryName(fullname);
										if(!Directory.Exists(fullpath)){
											Directory.CreateDirectory(fullpath);
										}
										using(FileStream fs=new FileStream(
											fullname, FileMode.Create)){
											byte[] data=new byte[4096];
											long count = read.Length;
											long max = count/data.Length;
											for(long i = 0;i<max;i++){
												read.Read(data, 0, data.Length);
												fs.Write(data,0 , data.Length);
											}
											if(max * data.Length < count){
												int c = (int)(count-(max*data.Length));
												read.Read(data, 0, c);
												fs.Write(data,0 , c);
											}
										}
									}
									return true;
								}
							}
						}
					}catch(Exception e){
						Console.WriteLine("Couldn't process " + e.Message);
					}
				}
			}
			return false;
		}
		public static void ExtractHfs(string filename)
		{
			string basep = Path.GetDirectoryName(filename);
			string plain = Path.GetFileNameWithoutExtension(filename);
			using (HfsFile hfs = new HfsFile(filename))
				using (ZipFile zip = ZipFile.Create(basep + @"\" + plain + "_.zip"))
			{

				zip.BeginUpdate();

				foreach (HfsEntry hfsEntry in hfs)
				{
					Console.WriteLine("Processing " + hfsEntry.Name);
					try
					{
						Stream read = hfs.GetInputStream(hfsEntry);

						zip.Add(new StreamLocalSourceZip(read), hfsEntry.Name);
					}
					catch (Exception e)
					{
						Console.WriteLine("Couldn't process " + hfsEntry.Name + ": " + e.Message);
					}
				}

				if (hfs.ObfuscationKey != 0)
				{
					zip.SetComment("extra_obscure");
				}

				Console.WriteLine("Compressing..");
				zip.CommitUpdate();
			}

			Console.WriteLine("Wrote to " + basep + @"\" + plain + "_.zip");
		}

		public static void ConvertZip(string filename)
		{
			string basep = Path.GetDirectoryName(filename);
			string plain = Path.GetFileNameWithoutExtension(filename);

			using (HfsFile hfs = HfsFile.Create(basep + @"\" + plain + "_.hfs"))
				using (ZipFile zip = new ZipFile(filename))
			{
				hfs.BeginUpdate();

				if (zip.ZipFileComment == "extra_obscure")
				{
					// could be better implemented
					hfs.ObfuscationKey = -1;
				}

				foreach (ZipEntry zipEntry in zip)
				{
					Console.WriteLine("Processing " + zipEntry.Name);

					Stream read = zip.GetInputStream(zipEntry);


					hfs.Add(new StreamLocalSourceHfs(read), zipEntry.Name);
				}

				Console.WriteLine("Compressing..");
				hfs.CommitUpdate();
			}

			Console.WriteLine("Wrote to " + basep + @"\" + plain + "_.hfs");
		}

		public static void Delete(string hfsfile,string filename){
			if(!filename.EndsWith(".comp")){
				filename += ".comp";
			}
			string basep = Path.GetDirectoryName(filename);
			string plain = Path.GetFileNameWithoutExtension(filename);
			using (HfsFile hfs = new HfsFile(hfsfile)){
				foreach (HfsEntry hfsEntry in hfs)
				{
					if(filename != hfsEntry.Name){
						Console.WriteLine("skip "+hfsEntry.Name);
						continue;
					}
					hfs.BeginUpdate();
					Console.WriteLine("hfs rm "+filename);
					hfs.Delete(hfsEntry);
					hfs.CommitUpdate();
					break;
				}
				Console.WriteLine("completed...");
			}
		}
		public static void AddOrReplace(string hfsfile,string file,string filename=null){
			if(string.IsNullOrEmpty(filename)){
				filename = Path.GetFileName(file);
			}
			if(!filename.EndsWith(".comp")){
				filename += ".comp";
			}
			string basep = Path.GetDirectoryName(filename);
			string plain = Path.GetFileNameWithoutExtension(filename);
			using (HfsFile hfs = new HfsFile(hfsfile)){
				foreach (HfsEntry hfsEntry in hfs)
				{
					if(filename != hfsEntry.Name){
						Console.WriteLine("skip "+hfsEntry.Name);
						continue;
					}
					hfs.BeginUpdate();
					Console.WriteLine("hfs rm "+filename);
					hfs.Delete(hfsEntry);
					Console.WriteLine("hfs add "+filename);
					hfs.Add(file, filename);
					hfs.CommitUpdate();
					break;
				}
				Console.WriteLine("completed...");
			}
		}
	}
}
