using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WordCounter {
	public class OutGridData : INotifyPropertyChanged {
		private bool known;
		private bool unknown;
		public int N { get; set; }
		public int Count { get; set; }
		public double Percent { get; set; }
		public string Word { get; set; }
		public bool Known {
			get { return known; }
			set {
				known = value;
				OnPropertyChanged("Known");
			}
		} // ///////////////////////////////////////////////////////////////////////////
		public bool Unknown {
			get { return unknown; }
			set {
				unknown = value;
				OnPropertyChanged("Unknown");
			}
		} // ///////////////////////////////////////////////////////////////////////////
		public OutGridData(int N, int Count, double Percent, string Word, bool Known, bool Unknown) {
			this.N = N;
			this.Count = Count;
			this.Percent = Percent;
			this.Word = Word;
			this.Known = Known;
			this.Unknown = Unknown;
		} // ////////////////////////////////////////////////////////////////////////////
		public override string ToString() {
			return N + " " +
				Count.ToString() + " " +
				Word + " " +
				Known.ToString() +
				Unknown.ToString();
		} // ///////////////////////////////////////////////////////////////////////////
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "") {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	} // ********************************************************************************
}
