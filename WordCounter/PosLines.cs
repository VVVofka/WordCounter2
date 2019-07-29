using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter {
	class PosLines {
		ObservableCollection<Line> poslines = new ObservableCollection<Line>();
		public void Add(List<int> lst, Line lhor) {
			//poslines.Clear();
			int n = poslines.Count;
			for (int i = n - 1; i >= 0; i--) {
				poslines.RemoveAt(i);
			}

			OutGridData o = (OutGridData)dtOut.SelectedItems[dtOut.SelectedItems.Count - 1];
			ItemDists idsts = tp.lst[o.Word];
			foreach (int pos in idsts.Positions) {
				Line ln = new Line();
				ln.X1 = ln.X2 = lhor.X1 + (lhor.X2 - lhor.X1) * pos / sReadFiles.Length;
				ln.Y1 = 1;
				ln.Y2 = 3;
				poslines.Add(ln);

		/*		grid1.Children.Add(ln);
				Grid.SetRow(ln, 1);
				Grid.SetColumn(ln, 2);
				ln.VerticalAlignment = VerticalAlignment.Top;
				ln.Stroke = Brushes.Blue;
				ln.StrokeThickness = 3;		*/
			}
		} // /////////////////////////////////////////////////////////////////////////
	}
}
