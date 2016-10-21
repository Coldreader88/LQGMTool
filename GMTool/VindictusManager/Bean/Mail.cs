using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vindictus.Extensions;
using System.Data.Common;
using Vindictus.Helper;


namespace Vindictus.Bean
{
	public class Mail
	{
		public long MailID { get; private set; }
		public string Title { get; private set; }
		public string Text { get; private set; }

		public int Count { get;private set; }
		public Mail(){
			
		}
		public Mail(DbDataReader reader){
			//mailID
			this.MailID =reader.ReadInt64("RowID");
			string title = reader.ReadString("MailTitle");
			title = HeroesTextHelper.GetMailTitle(title);
			this.Title = title;
			//Content
			this.Text =	reader.ReadString("MailContent");
			this.Count = reader.ReadInt32("Count");
		}
		
		public Mail AttachBox(DbDataReader reader){
			this.MailID =reader.ReadInt64("MailID");
			string title = reader.ReadString("title");
			title = HeroesTextHelper.GetMailTitle(title);
			this.Title = title;
			//Content
			this.Text =	reader.ReadString("content");
			return this;
		}
		public Mail(long mailID,string title,string content,int count)
		{
			this.MailID = mailID;
			this.Title = title;
			this.Text = content;
			this.Count = count;
		}
		public override string ToString()
		{
			string txt="["+Title+"]";
			if (Count > 0)
			{
				txt +=  Count;
			}
			txt +="\n------------------------\n"+Text;
			return txt;
		}
	}
}
