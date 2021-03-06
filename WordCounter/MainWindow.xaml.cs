﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Media;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// https://code.msdn.microsoft.com/windowsapps/Data-Binding-Demo-82a17c83 - привязка данных
//см. Интерфейс INotifyPropertyChanged https://metanit.com/sharp/wpf/11.2.php
namespace WordCounter {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private readonly OpenFileDialog dlg;
		private readonly OptionsReg options; // save controls state
		TextProc tp = new TextProc();
		readonly DbWords db = new DbWords();
		string sReadFiles = "";

		//Line lhor = new Line();
		readonly MyDataBind bind;
		// //////////////////////////////////////////////////////
		public MainWindow() {
			InitializeComponent();
			bind = new MyDataBind();
			this.DataContext = bind;

			Control[] ctrls = new Control[] { chKnown, chUnknown, lstFileNames, txFileName, chzUn, chz_s, chz_ed, chz_ing, chz_en, chz_est, chz_ly, chz_er, chz_y, chz_less, chz_IrVerb, txMinCnt };
			options = new OptionsReg(this, ctrls);
			string defext = options.getReg("DefaultExt", ".srt");
			//defext = defext.Replace(".", "");
			//options.setReg("DefaultExtIndex", dlg.FilterIndex);
			dlg = new OpenFileDialog {
				Multiselect = true,
				DefaultExt = defext, // Default file extension
				Filter = "Texts|*.txt|Subtitles|*.srt|All|*.*", // Filter files by extension
				FilterIndex = options.getRegInt("DefaultExtIndex", 1)
		};
			db.Load();
			/*grid1.Children.Add(lhor);
						*/
			//Grid.SetRow(lhor, 1);
			//Grid.SetColumn(lhor, 2);
			//lhor.VerticalAlignment = VerticalAlignment.Top;
			//lhor.Stroke = Brushes.Red;
			//lhor.StrokeThickness = 3;
		} // ////////////////////////////////////////////////////////////////////////////////
		private void KnSelectFile_Click(object sender, RoutedEventArgs e) {
			if(dlg.ShowDialog() == true) {
				try {
					lstFileNames.Items.Clear();
					foreach(string curfile in dlg.FileNames) {
						lstFileNames.Items.Add(curfile);
						string s = Path.GetExtension(curfile);
						//s = s.Replace(".", "");
						dlg.DefaultExt = s;
						options.setReg("DefaultExt", s);
						options.setReg("DefaultExtIndex", dlg.FilterIndex);
					}
					lstFileNames.SelectedIndex = 0;
					txFileName.Text = "Добавлено " + lstFileNames.Items.Count + " файлов";
					//var sr = new StreamReader(openFileDialog1.FileName);
					//SetText(sr.ReadToEnd());
				} catch(SecurityException ex) {
					MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
					$"Details:\n\n{ex.StackTrace}");
				}
			}
		} /////////////////////////////////////////////////////////////////////////////////
		private void BtLoad_Click(object sender, RoutedEventArgs e) {
			sReadFiles = "";
			BtLoad_Add_Click(sender, e);
		} // ////////////////////////////////////////////////////////////////////////////
		private void BtLoad_Add_Click(object sender, RoutedEventArgs e) {
			dtOut.ItemsSource = null;
			for(int i = 0; i < lstFileNames.Items.Count; i++) {
				string fname = lstFileNames.Items[i].ToString();
				if(File.Exists(fname))
					sReadFiles += File.ReadAllText(fname) + " ";
				else {
					MessageBox.Show($"IO error. File\n`" + fname + $"`\n not exist!");
					return;
				}
			}
			sReadFiles = sReadFiles.Replace("`", "'");
			tp = new TextProc {
				irrVerb = (bool)chz_IrVerb.IsChecked
			};
			tp.Run(sReadFiles);
			if(chzUn.IsChecked == true) {
				tp.DelStart("un");
				tp.DelStart("dis");
			}
			if(chz_ed.IsChecked == true) {
				tp.DelEnd("ed");
				tp.ReplEnd("ed", "e");
				tp.ReplEnd("ied", "y");
				tp.ReplEnd2("ed");
			}
			if(chz_est.IsChecked == true) {
				tp.DelEnd("est");
				tp.ReplEnd("estly", "y");
				tp.ReplEnd("iest", "e");
			}
			if(chz_ly.IsChecked == true) {
				tp.DelEnd("ly");
				tp.ReplEnd("ily", "y");
				tp.ReplEnd("edly", "y");
				tp.ReplEnd("ily", "e");
			}
			if(chz_en.IsChecked == true) {
				tp.DelEnd("en");
				tp.ReplEnd("en", "y");
				tp.ReplEnd2("en");
			}
			if(chz_er.IsChecked == true) {
				tp.DelEnd("er");
				tp.ReplEnd("er", "e");
				tp.ReplEnd("er", "y");
				tp.ReplEnd("ier", "e");
			}
			if(chz_y.IsChecked == true) {
				tp.DelEnd("y");
				tp.DelEnd("estly");
				tp.DelEnd("ry");
				tp.ReplEnd2("y");
			}
			if(chz_less.IsChecked == true) {
				tp.DelEnd("less");
				tp.DelEnd("lessly");
			}
			if(chz_ing.IsChecked == true) {
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
			if(chz_s.IsChecked == true) {
				tp.DelEnd("s");
				tp.ReplEnd("ves", "f");
				tp.DelEnd("es");
				tp.ReplEnd("ies", "y");
				tp.DelEnd("ers");
			}
			if(chz_ed.IsChecked == true) {
				tp.ReplEnd("ed", "ing");
				tp.ReplEnd("ed", "s");
				tp.ReplEnd2("ed");
			}
			//tp.DelEnd("'d");
			//tp.DelEnd("n't");
			//tp.DelEnd("'re");
			//tp.DelEnd("'m");
			//tp.DelEnd("'ve");
			tp.DelEnd("'ll");
			tp.DelEnd("'s");
			//tp.DelEnd("n't");

			lbSummary.Content = $"Text lenght=\n" + tp.lenText + " bytes\n" + tp.allChars + " chars\n\nWords:\n" +
			tp.allWord + " All\n" + tp.uniqWord + " Unique\n" + tp.uniqMiddle + " Unique middle\n" + (tp.uniqWord - tp.uniqMiddle) + " Duplicate";
			if(db.db == null) {
				btSaveDB.Foreground = Brushes.Red;
				btSaveDB.IsEnabled = true;
				chKnown.IsChecked = chUnknown.IsChecked = null;
			} else {
				btSaveDB.Foreground = Brushes.Black;
			}
			Refresh();
			//SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav"); simpleSound.Play();
			SystemSounds.Asterisk.Play();
		} // ////////////////////////////////////////////////////////////////////////////
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(btSaveDB.IsEnabled) {
				// Configure the message box
				Window owner = ((bool)true ? this : null);
				var messageBoxText = "DB not saved! Save?";
				var caption = "Confirm";
				var button = (MessageBoxButton)Enum.Parse(typeof(MessageBoxButton), "YesNoCancel");
				var icon = (MessageBoxImage)Enum.Parse(typeof(MessageBoxImage), "Question");
				var defaultResult = (MessageBoxResult)Enum.Parse(typeof(MessageBoxResult), "Yes");
				var options = (MessageBoxOptions)Enum.Parse(typeof(MessageBoxOptions), "None");

				// Show message box, passing the window owner if specified
				MessageBoxResult result;
				if(owner == null) {
					result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, defaultResult, options);
				} else {
					result = System.Windows.MessageBox.Show(owner, messageBoxText, caption, button, icon, defaultResult,
						options);
				}
				if(result == MessageBoxResult.Yes) {
					btSaveDB.Focus();
					BtSaveDB_Click(null, null);
				}else if(result == MessageBoxResult.Cancel) {
					e.Cancel = true;
					return;
				}
			}
			options.SaveAll();
		} // ///////////////////////////////////////////////////////////////////////
		private void BtSetKnown_Click(object sender, RoutedEventArgs e) {
			SetKnown(true);
		} // //////////////////////////////////////////////////////////////////////////////
		private void BtSetUnknown_Click(object sender, RoutedEventArgs e) {
			SetKnown(false);
		} // //////////////////////////////////////////////////////////////////////////////
		private void SetKnown(bool val) {
			for(int i = 0; i < dtOut.SelectedItems.Count; i++) {
				OutGridData o = (OutGridData)dtOut.SelectedItems[i];
				o.Known = val;              //tp.grdata[o.N-1].Known = false;
				o.Unknown = !val;
			}
			btSaveDB.IsEnabled = true;
			//dtOut.ItemsSource = null; dtOut.ItemsSource = tp.grdata; - вместо этого порно
			//см. Интерфейс INotifyPropertyChanged https://metanit.com/sharp/wpf/11.2.php
		} // //////////////////////////////////////////////////////////////////////////////
		private void BtSaveDB_Click(object sender, RoutedEventArgs e) {
			foreach(OutGridData ogd in tp.grdata)
				db.Push(ogd.Word, ogd.Known, ogd.Unknown);  // TODO: not exit
			db.Save();
			btSaveDB.IsEnabled = false;
			btSaveDB.Foreground = Brushes.Black;
			Refresh();
		} // ////////////////////////////////////////////////////////////////////////////////
		private void DtOut_BeginningEdit(object sender, DataGridBeginningEditEventArgs e) {
			btSaveDB.IsEnabled = true;
		} // /////////////////////////////////////////////////////////////////////////////////
		private void ChUnknown_Click(object sender, RoutedEventArgs e) {
			Refresh();
		} // ///////////////////////////////////////////////////////////////////////////////////////////
		private void ChKnown_Click(object sender, RoutedEventArgs e) {
			Refresh();
		} // //////////////////////////////////////////////////////////////////////////////////
		private void Refresh() {
			if(int.TryParse(txMinCnt.Text.Trim(), out int min_cnt)) {
				txMinCnt.Text = min_cnt.ToString();
			} else {
				min_cnt = 1;
				txMinCnt.Text = "1";
			}
			tp.Report(db.db, chKnown.IsChecked, chUnknown.IsChecked, min_cnt, txFileName.Text + "~");
			dtOut.ItemsSource = tp.grdata;
		} // ////////////////////////////////////////////////////////////////////////////////
		private void DtOut_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			if(dtOut.SelectedItems.Count == 0 || sReadFiles.Length == 0)
				return;
			Infa infa = new Infa();
			System.Windows.Controls.DataGrid ctrl = (System.Windows.Controls.DataGrid)sender;
			if(ctrl.CurrentColumn.DisplayIndex == 3) {
				VVVindowSize.ReSize(infa, 0.6, 0.8, 0.33, 0.5, this);
				//infa.WindowState = WindowState.Maximized;
				infa.Owner = this;
				infa.Show();

				OutGridData o = (OutGridData)dtOut.SelectedItems[dtOut.SelectedItems.Count - 1];
				string url = "https://translate.yandex.ru/?lang=en-ru&text=" + o.Word;
				url = url.Replace(" ", "%20");
				infa.Browse(url);
			}
		} // /////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void Window_Loaded(object sender, RoutedEventArgs e) {
			VVVindowSize.ReSize(this, 0.4, 0.75, 0.1, 0.2);
		} // ///////////////////////////////////////////////////////////////////////////////////////////////
		private void DtOut_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if(dtOut.SelectedItems.Count == 0)
				return;
			bind.PosLines.Clear();

			OutGridData o = (OutGridData)dtOut.SelectedItems[dtOut.SelectedItems.Count - 1];
			ItemDists idsts = tp.lst[o.Word];
			foreach(int pos in idsts.Positions) {
				Point p = new Point {
					X = lhor.X1 + (lhor.X2 - lhor.X1) * pos / sReadFiles.Length,
					Y = -6
				};
				bind.PosLines.Add(p);
			}
		} // ///////////////////////////////////////////////////////////////////////////////////////
		private void DtOut_SizeChanged(object sender, SizeChangedEventArgs e) {
			lhor.X1 = lstFileNames.Margin.Left;
			lhor.X2 = lstFileNames.Margin.Left + lstFileNames.ActualWidth + knSelectFile.ActualWidth - 2;
			//lhor.Y1 = lstFileNames.ActualHeight + 29;
			//lhor.Y2 = lhor.Y1;
			DtOut_SelectionChanged(sender, null);
		} // /////////////////////////////////////////////////////////////////////////////////////
		private void BtExit_Click(object sender, RoutedEventArgs e) {
			int ret = db.Clear();
			ret += tp.Clear();
			ret += options.Clear();
			ret += bind.Clear();
			if(ret == 0)
				Console.WriteLine("Clear complete");
			this.Close();
		} // /////////////////////////////////////////////////////////////////////
	} // *************************************************************************************
} // -------------------------------------------------------------------------------------------

