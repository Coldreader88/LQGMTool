using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace SkillPatch
{
    public class FilePatch
    {
        string basePath, path1, ex1, path2, ex2,Sep;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources">基础路径</param>
        /// <param name="path1">读取</param>
        /// <param name="path2">源</param>
        /// <param name="ex1"></param>
        /// <param name="ex2"></param>
        public FilePatch(string basePath, string path1, string ex1, string ex2, string path2,string Sep="\\")
        {
            this.basePath = basePath;
            this.path1 = path1;
            this.ex1 = ex1;
            this.path2 = path2;
            this.ex2 = ex2;
        }

        public void Patch()
        {
            List<string> losts = new List<string>();
            Each(path1, (string file) =>
            {
                if (!file.EndsWith(ex1)) return;
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            int last = line.LastIndexOf(ex2);
                            if (last < 0)
                            {
                                continue;
                            }
                            line = line.Replace("\t", " ");
                            int start = line.LastIndexOf(" ", last);
                            if (start >= 0)
                            {
                                string name = line.Substring(start + 1, last - start +3);
                              //  Console.WriteLine(line);
                                string fullname = PathHelper.Combine(basePath, name);
                                string srcname = PathHelper.Combine(path2, Path.GetFileName(fullname));
                                string srcDir = Path.GetDirectoryName(fullname);
                                if (!File.Exists(srcname))
                                {
                                    if (!losts.Contains(name))
                                    {
                                        losts.Add(name);
                                        Console.WriteLine(file + ",缺失" + name);
                                    }
                                }else  if (!File.Exists(fullname))
                                {
                                    if (!Directory.Exists(srcDir)){
                                        Directory.CreateDirectory(srcDir);
                                    }
                                    File.Copy(srcname, fullname, false);
                                    
                                }
                            }
                        }
                    }
                }
            });
        }

        protected void Each(string path, Action<string> Work, string prefix = "")
        {
            Console.WriteLine("prefix=" + prefix + ",path=" + path);
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles();
            if (files != null)
            {
                foreach (FileInfo file in files)
                {
                    Work(file.FullName);
                }
            }
            DirectoryInfo[] infos = dir.GetDirectories();
            if (infos != null)
            {
                foreach (DirectoryInfo info in infos)
                {
                    Each(info.FullName, Work, prefix += "/" + info.Name);
                }
            }
        }
    }
}
