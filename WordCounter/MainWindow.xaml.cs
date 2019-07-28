using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Security;
using System.Media;
using System.Windows.Controls;
using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Shapes;

// https://code.msdn.microsoft.com/windowsapps/Data-Binding-Demo-82a17c83 - привязка данных
//см. Интерфейс INotifyPropertyChanged https://metanit.com/sharp/wpf/11.2.php
namespace WordCounter {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private OpenFileDialog dlg;
		private OptionsReg options; // save controls state
		TextProc tp = new TextProc();
		DbWords db = new DbWords();
		string sReadFiles = "";
		Line lhor = new Line();

		// //////////////////////////////////////////////////////
		public MainWindow() {
			InitializeComponent();
			dlg = new OpenFileDialog();
			dlg.Multiselect = true;
			dlg.DefaultExt = ".txt"; // Default file extension
			dlg.Filter = "Text documents (.txt)|*.txt|All|*.*"; // Filter files by extension
			Control[] ctrls = new Control[] { chKnown, chUnknown, lstFileNames, txFileName, chzUn, chz_s, chz_ed, chz_ing, chz_en, chz_est, chz_ly, chz_er, chz_y, chz_less, chz_IrVerb };
			options = new OptionsReg(this, ctrls);
			db.Load();

			grid1.Children.Add(lhor);
			Grid.SetRow(lhor, 1);
			Grid.SetColumn(lhor, 2);
			lhor.VerticalAlignment = VerticalAlignment.Top;
			lhor.Stroke = Brushes.Red;
			lhor.StrokeThickness = 3;
		} // ////////////////////////////////////////////////////////////////////////////////
		private void KnSelectFile_Click(object sender, RoutedEventArgs e) {
			if (dlg.ShowDialog() == true) {
				try {
					lstFileNames.Items.Clear();
					foreach (string curfile in dlg.FileNames) {
						lstFileNames.Items.Add(curfile);
					}
					lstFileNames.SelectedIndex = 0;
					txFileName.Text = "Добавлено " + lstFileNames.Items.Count + " файлов";
					//var sr = new StreamReader(openFileDialog1.FileName);
					//SetText(sr.ReadToEnd());
				} catch (SecurityException ex) {
					MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
					$"Details:\n\n{ex.StackTrace}");
				}
			}
		} /////////////////////////////////////////////////////////////////////////////////
		private void BtLoad_Click(object sender, RoutedEventArgs e) {
			dtOut.ItemsSource = null;
			for (int i = 0; i < lstFileNames.Items.Count; i++) {
				string fname = lstFileNames.Items[i].ToString();
				if (File.Exists(fname))
					sReadFiles += File.ReadAllText(fname) + " ";
				else {
					MessageBox.Show($"IO error. File\n`" + fname + $"`\n not exist!");
					return;
				}
			}
			tp = new TextProc();
			tp.irrVerb = (bool)chz_IrVerb.IsChecked;
			tp.Run(sReadFiles);
			if (chzUn.IsChecked == true) {
				tp.DelStart("un");
				tp.DelStart("dis");
			}
			if (chz_ed.IsChecked == true) {
				tp.DelEnd("ed");
				tp.ReplEnd("ed", "e");
				tp.ReplEnd("ied", "y");
				tp.ReplEnd2("ed");
			}
			if (chz_est.IsChecked == true) {
				tp.DelEnd("est");
				tp.ReplEnd("estly", "y");
				tp.ReplEnd("iest", "e");
			}
			if (chz_ly.IsChecked == true) {
				tp.DelEnd("ly");
				tp.ReplEnd("ily", "y");
				tp.ReplEnd("edly", "y");
				tp.ReplEnd("ily", "e");
			}
			if (chz_en.IsChecked == true) {
				tp.DelEnd("en");
				tp.ReplEnd("en", "y");
				tp.ReplEnd2("en");
			}
			if (chz_er.IsChecked == true) {
				tp.DelEnd("er");
				tp.ReplEnd("er", "e");
				tp.ReplEnd("er", "y");
				tp.ReplEnd("ier", "e");
			}
			if (chz_y.IsChecked == true) {
				tp.DelEnd("y");
				tp.DelEnd("estly");
				tp.DelEnd("ry");
				tp.ReplEnd2("y");
			}
			if (chz_less.IsChecked == true) {
				tp.DelEnd("less");
				tp.DelEnd("lessly");
			}
			if (chz_ing.IsChecked == true) {
				tp.DelEnd("ing");
				tp.DelEnd("ings");
				tp.DelEnd("ening");
				tp.DelEnd("enings");
				tp.ReplEnd("ing", "e");
				tp.ReplEnd("ings", "e");
				tp.ReplEnd("ening", "e");
				tp.ReplEnd("enings", "e");
				tp.ReplEnd2("ing");
			}
			if (chz_s.IsChecked == true) {
				tp.DelEnd("s");
				tp.ReplEnd("ves", "f");
				tp.DelEnd("es");
				tp.ReplEnd("ies", "y");
				tp.DelEnd("ers");
			}
			if (chz_ed.IsChecked == true) {
				tp.ReplEnd("ed", "ing");
				tp.ReplEnd("ed", "s");
				tp.ReplEnd2("ed");
			}
			tp.DelEnd("'d");
			tp.DelEnd("'t");
			tp.DelEnd("'re");
			tp.DelEnd("'m");
			tp.DelEnd("'ve");
			tp.DelEnd("'ll");
			tp.DelEnd("'s");
			tp.DelEnd("n't");

			lbSummary.Content = $"Text lenght= " + tp.lenText + " bytes and " + tp.allChars + " chars\nWords:\n" +
			tp.allWord + " All\n" + tp.uniqWord + " Unique\n" + tp.uniqMiddle + " Unique middle\n" + (tp.uniqWord - tp.uniqMiddle) + " Duplicate";
			if (db.db == null) {
				btSaveDB.Foreground = Brushes.Red;
				btSaveDB.IsEnabled = true;
				chKnown.IsChecked = chUnknown.IsChecked = null;
			} else {
				btSaveDB.Foreground = Brushes.Black;
			}
			refresh();
			//SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav"); simpleSound.Play();
			SystemSounds.Asterisk.Play();
		} // ////////////////////////////////////////////////////////////////////////////
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			options.SaveAll();
		} // ///////////////////////////////////////////////////////////////////////
		private void BtSetKnown_Click(object sender, RoutedEventArgs e) {
			setKnown(true);
		} // //////////////////////////////////////////////////////////////////////////////
		private void BtSetUnknown_Click(object sender, RoutedEventArgs e) {
			setKnown(false);
		} // //////////////////////////////////////////////////////////////////////////////
		private void setKnown(bool val) {
			for (int i = 0; i < dtOut.SelectedItems.Count; i++) {
				OutGridData o = (OutGridData)dtOut.SelectedItems[i];
				o.Known = val;              //tp.grdata[o.N-1].Known = false;
				o.Unknown = !val;
			}
			btSaveDB.IsEnabled = true;
			//dtOut.ItemsSource = null; dtOut.ItemsSource = tp.grdata; - вместо этого порно
			//см. Интерфейс INotifyPropertyChanged https://metanit.com/sharp/wpf/11.2.php
		} // //////////////////////////////////////////////////////////////////////////////
		private void BtSaveDB_Click(object sender, RoutedEventArgs e) {
			foreach (OutGridData i in tp.grdata)
				db.Push(i.Word, i.Known, i.Unknown);
			db.Save();
			btSaveDB.IsEnabled = false;
			btSaveDB.Foreground = Brushes.Black;
			refresh();
		} // ////////////////////////////////////////////////////////////////////////////////
		private void DtOut_BeginningEdit(object sender, DataGridBeginningEditEventArgs e) {
			btSaveDB.IsEnabled = true;
		} // /////////////////////////////////////////////////////////////////////////////////
		private void ChUnknown_Click(object sender, RoutedEventArgs e) {
			refresh();
		} // ///////////////////////////////////////////////////////////////////////////////////////////
		private void ChKnown_Click(object sender, RoutedEventArgs e) {
			refresh();
		} // //////////////////////////////////////////////////////////////////////////////////
		private void refresh() {
			tp.report(db.db, chKnown.IsChecked, chUnknown.IsChecked, txFileName.Text + "~");
			dtOut.ItemsSource = tp.grdata;
		} // ////////////////////////////////////////////////////////////////////////////////
		private void DtOut_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			if (dtOut.SelectedItems.Count == 0 || sReadFiles.Length == 0) return;
			Infa infa = new Infa();

			VVVindowSize.ReSize(infa, 0.6, 0.8, 0.33, 0.5, this);
			//infa.WindowState = WindowState.Maximized;
			infa.Owner = this;
			infa.Show();

			OutGridData o = (OutGridData)dtOut.SelectedItems[dtOut.SelectedItems.Count - 1];
			string url = "https://translate.yandex.ru/?lang=en-ru&text=" + o.Word;
			url.Replace(" ", "%20");
			infa.Browse(url);

			ItemDists idsts = tp.lst[o.Word];
			foreach (int pos in idsts.Positions) {
				double k = rctLine.Width * pos / sReadFiles.Length;
			}

		} // /////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void Window_Loaded(object sender, RoutedEventArgs e) {
			VVVindowSize.ReSize(this, 0.4, 0.75, 0.1, 0.2);
		} // ///////////////////////////////////////////////////////////////////////////////////////////////
		private void DtOut_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			lhor.X1 = dtOut.Margin.Left;
			lhor.X2 = dtOut.Margin.Left + dtOut.ActualWidth;
			lhor.Y1 = dtOut.Margin.Top / 2;
			lhor.Y2 = lhor.Y1;
		} // ///////////////////////////////////////////////////////////////////////////////////////
		private void DtOut_SizeChanged(object sender, SizeChangedEventArgs e) {
			DtOut_SelectionChanged(sender, null);
		} // /////////////////////////////////////////////////////////////////////////////////////
	} // *************************************************************************************
	  // ??????????????????????????????????
} // -------------------------------------------------------------------------------------------


