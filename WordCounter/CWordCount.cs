using System;
// for sorting dictionary
namespace WordCounter {
	class CWordCount : IComparable<CWordCount> {
		public string word;
		public double dists = 0;
		public int cnt;
		private const int desc = -1;
		public int CompareTo(CWordCount other) {
			if(dists > other.dists)
				return desc;
			if(dists < other.dists)
				return -desc;

			//if (word.Length > other.word.Length)
			//	return desc;
			//if (word.Length < other.word.Length)
			//	return -desc;
			return -desc * string.Compare(word, other.word);
			throw new NotImplementedException();
		} // ////////////////////////////public int CompareTo(CWordCount other)//////////////////////////////
	} // ******************************class CWordCount : IComparable<CWordCount>*************************
} // ---------------------------------namespace WordCounter-------------------------------------------------