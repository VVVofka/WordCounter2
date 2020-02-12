using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace WordCounter {
	class MyDataBind {
		//public ObservableCollection<Point> PointList { get; private set; }
		public ObservableCollection<Point> PosLines { get; private set; }
		public MyDataBind() {
			PosLines = new ObservableCollection<Point>();
		} // //////////////////////////////////////////////////////////////////////////////////
		public int Clear() {
			PosLines.Clear();
			return PosLines.Count;
		} // //////////////////////////////////////////////////////////////////////////////////
	}
}
