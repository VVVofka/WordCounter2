using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WordCounter {
	/// <summary>
	/// Логика взаимодействия для Infa.xaml
	/// </summary>
	public partial class Infa : Window {
		public string url;
		public Infa() {
			InitializeComponent();
		} // ///////////////////////////////////////////////////////
		public void Browse(string url) {
			HideScriptErrors(webBrowser1, true);  // webBrowser1.ScriptErrorsSuppressed = true;
			webBrowser1.Navigate(url);
		} // ///////////////////////////////////////////////////////////////////
		public void HideScriptErrors(WebBrowser wb, bool Hide) {
			FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
			if (fiComWebBrowser == null) return;
			object objComWebBrowser = fiComWebBrowser.GetValue(wb);
			if (objComWebBrowser == null) return;
			objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
		} // //////////////////////////////////////////////////////////////////////////////////////////////////////
	} //  public partial class Infa : Window {
} // namespace WordCounter {
