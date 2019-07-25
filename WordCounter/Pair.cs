namespace WordCounter {
	public class Pair<T1, T2> {
		public T1 a;
		public T2 b;
		public Pair() { }
		public Pair(T1 new_a, T2 new_b){
			a = new_a; b = new_b;
			}
		public override string ToString() {
			string ret = a.ToString() + " " + b.ToString();
			return ret; // base.ToString();
		}
	} // ******************************************************************
	public class WordCnt {
		public string word = "";
		public double dists = 0;
		public int cnt = 0;
	} // ******************************************************************
}