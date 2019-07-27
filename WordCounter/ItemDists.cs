using System;
using System.Collections.Generic;

namespace WordCounter {
	public class ItemDists {
		public double SumDists = 0; // sum distances
		public List<int> Positions;
		public ItemDists(int pos) {
			Positions = new List<int>();
			Positions.Add(pos);
		} // //////////////////////////////////////////////////////////////////////////////////
		public void Add(int pos) {
			double sum = 0;
			foreach (int x in Positions) {
				if (x == pos)
					return;
				double tmp = x - pos;
				sum += Math.Abs(tmp); // * tmp;
			}
			SumDists += Math.Sqrt(sum);
			Positions.Add(pos);
		} // /////////////////////////////////////////////////////////////////////////////
		public void Add(ItemDists add_item) {
			foreach (int y in add_item.Positions)
				Add(y);
		} // /////////////////////////////////////////////////////////////////////////////
	} // ****************************************************************************************
} // -----------------------------------------------------------------------------------------
