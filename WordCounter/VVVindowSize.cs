using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace WordCounter {
	/// <summary>
	/// For set window size in % of active screen
	/// insert to Window_Loaded()
	/// </summary>
	public static class VVVindowSize {
		public static void ReSize(Window wnd, double k_width, double k_heigh, double k_hoirz, double k_vert, Window wndowner=null) {
			if (wndowner == null)
				wndowner = wnd;
			Rectangle rmain = new Rectangle((int)wndowner.Left, (int)wndowner.Top, (int)wndowner.Width, (int)wndowner.Height);
			Screen myScreen = Screen.FromRectangle(rmain);
			Rectangle area = myScreen.WorkingArea;

			wnd.Width = area.Width * k_width;
			wnd.Height = area.Height * k_heigh;
			wnd.Left = area.Left + (area.Width - wnd.Width) * k_hoirz;
			wnd.Top = area.Top + (area.Height - wnd.Height) * k_vert;
			//Console.WriteLine("wndowner Left" + (int)wndowner.Left + " Top" + (int)wndowner.Top + " Width" + (int)wndowner.Width + " Height" + (int)wndowner.Height);
			//Console.WriteLine("area Left" + (int)area.Left + " Top" + (int)area.Top + " Width" + (int)area.Width + " Height" + (int)area.Height);
			//Console.WriteLine("wnd Left" + (int)wnd.Left + " Top" + (int)wnd.Top + " Width" + (int)wnd.Width + " Height" + (int)wnd.Height);
		} // ////////////////////////////////////////////////////////////////////////////////////////////
	}
}
