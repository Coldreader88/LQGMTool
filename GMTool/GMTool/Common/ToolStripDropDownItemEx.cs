/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/26
 * 时间: 11:15
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
	/// <summary>
	/// 解决悬停的工具提示
	/// </summary>
	public class ToolStripMenuItemEx : ToolStripMenuItem
	{
//		DateTime enterTime;
//		DateTime outTime;
//		Timer timer;
		public ToolStripMenuItemEx(string text):base(text)
		{
//			timer=new Timer();
//			timer.Interval = 500;
//			timer.Tick += (object sender, EventArgs e) =>{
//				TimeSpan span = (TimeSpan)(DateTime.Now - outTime);
//				if(span.Milliseconds>=500){
//					base.OnMouseLeave(null);
//				}
//				timer.Stop();
//			};
//			outTime = DateTime.Now;
//			enterTime =DateTime.Now;
		}
//		protected override void OnMouseEnter(EventArgs e)
//		{
//			base.OnMouseEnter(e);
//			enterTime = DateTime.Now;
//			base.OnMouseHover(e);
//		}
//		protected override void OnMouseHover(EventArgs e)
//		{
//			//base.OnMouseHover(e);
//		}
//		protected override void OnMouseLeave(EventArgs e)
//		{
//			outTime = DateTime.Now;
//			TimeSpan span = (TimeSpan)(DateTime.Now - enterTime);
//			if(span.Milliseconds>=500){
//				timer.Enabled = false;
//				base.OnMouseLeave(e);
//			}else{
//				//过一段时间，则隐藏
//				timer.Start();
//			}
//			//重复触发
//		}
//		protected override void OnMouseMove(MouseEventArgs e)
//		{
//			outTime = DateTime.Now;
////			ToolStrip owner= this.Owner;
////			if(owner!=null){
////				Rectangle rect= owner.DisplayRectangle;
////				if(rect!=null && !rect.IsEmpty){
////					if(e.X >= rect.Left && e.X <= rect.Right
////					  && e.Y >= rect.Top && e.Y<=rect.Bottom){
////						base.OnMouseHover(null);
////					}
////				}
////			}
//			//MouseEventArgs e2= new MouseEventArgs(e.Button, e.Clicks, e.X-100,e.Y,e.Delta);
//			base.OnMouseMove(e);
//		}

		
	}
}
