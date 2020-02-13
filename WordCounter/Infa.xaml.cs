using System.Reflection;
using System.Windows;
using System.Windows.Controls;

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
			if(fiComWebBrowser == null)
				return;
			object objComWebBrowser = fiComWebBrowser.GetValue(wb);
			if(objComWebBrowser == null)
				return;
			objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
		} // //////////////////////////////////////////////////////////////////////////////////////////////////////
	} //  public partial class Infa : Window {
} // namespace WordCounter {
