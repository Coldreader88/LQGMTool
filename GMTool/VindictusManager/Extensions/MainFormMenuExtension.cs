
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vindictus.Helper;
using System.ComponentModel;
using System.Xml;
using ServerManager;
using Vindictus.UI;
using System.Data.Common;
using System.IO;
using System.Threading;

namespace Vindictus.Extensions
{
	public static class MainFormMenuExtension
	{
		private static ServerForm serverForm;
		public static void ShowServerManager(this MainForm form)
		{
			if(serverForm==null){
				serverForm =new ServerForm();
				serverForm.Closing+= delegate(object sender, CancelEventArgs e) {
					serverForm.Hide();
					e.Cancel = true;
				};
			}
			serverForm.Show();
			serverForm.Activate();
		}
		public static void ShowAbout(this MainForm form){
			string str = R.AboutText;
			str=str.Replace("$author","QQ247321453");
			str=str.Replace("$name",R.Title);
			str=str.Replace("$vesion",Application.ProductVersion.ToString());
			MessageBox.Show(str,R.About,MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		public static bool CloseServer(this MainForm form){
			if(serverForm!=null && serverForm.isStart){
				if(form.Question(R.TipCloseServer)){
					serverForm.Close();
					return true;
				}
			}
			return false;
		}
	}
}
