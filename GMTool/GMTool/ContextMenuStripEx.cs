using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public class ContextMenuStripEx : ContextMenuStrip
    {
        Control _SourceConrolEx;
        public ContextMenuStripEx(IContainer container):base(container)
        {
            this.Opening += ContextMenuStripEx_Opening;
        }

        private void ContextMenuStripEx_Opening(object sender, CancelEventArgs e)
        {
            if (base.SourceControl != null)
            {
                _SourceConrolEx = base.SourceControl;
            }
        }
        public Control SourceConrolEx
        {
            get
            {
                return _SourceConrolEx;
            }
        }
        public ContextMenuStripEx():base()
        {
            this.Opening += ContextMenuStripEx_Opening;
        }
    }
}
