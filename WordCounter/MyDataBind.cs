﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace WordCounter {
	class MyDataBind {
		//public ObservableCollection<Point> PointList { get; private set; }
		public ObservableCollection<Line> PosLines { get; private set; }
		public MyDataBind() {
			PosLines = new ObservableCollection<Line>();
		} // //////////////////////////////////////////////////////////////////////////////////
	}
}
