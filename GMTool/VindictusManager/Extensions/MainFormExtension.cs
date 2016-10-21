
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
	public static class MainFormExtension
	{
		
		public static void PostTask(this Form form, Action<IWaitDialog> action){
			if(action==null)return;
			using(WaitDialog dlg=new WaitDialog()){
				dlg.Closing+= delegate(object sender, CancelEventArgs e) {
					if(!dlg.CanClose){
						form.Info(R.TipStopTask);
						e.Cancel=true;
					}
				};
				dlg.Shown += delegate {
					Thread thread=new Thread(()=>
					                         {
					                         	action(dlg);
					                         	dlg.CanClose = true;
					                         	dlg.CloseDialog();
					                         });
					thread.IsBackground=true;
					thread.Start();
				};
				dlg.ShowDialog();
			}
		}
	}
}
