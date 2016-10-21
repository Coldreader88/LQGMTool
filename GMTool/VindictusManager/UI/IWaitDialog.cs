
using System;

namespace Vindictus.UI
{
	public interface IWaitDialog
	{
		void SetTitle(string text);
		void SetInfo(string text);
		void Message(string text);
		void CloseDialog();
	}
}
