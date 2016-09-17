﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMTool.Bean
{
    public class Mail
    {
        public long MailID { get; private set; }
        public string Title { get; private set; }
        public string Text { get; private set; }

        public int Count { get; set; }
        public Mail(long mailID,string title,string content)
        {
            this.MailID = mailID;
            this.Title = title;
            this.Text = content;
        }
        public override string ToString()
        {
            string txt="标题："+Title+"\n内容："+Text;
            if (Count > 0)
            {
                txt += "\n数量：" + Count;
            }
            return txt;
        }
    }
}
