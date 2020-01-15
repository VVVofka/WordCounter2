using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace WordCounter {
	public class TextProc {
		public int lenText = 0;     // Длина необработанного текста
		public int allChars = 0;    // Учитываемых букв
		public int allWord = 0;     // Учитываемых слов
		public int uniqWord = 0;    // Уникальных слов
		public int uniqMiddle = 0;  // Уникальных однокоренных слов

		public Dictionary<string, ItemDists> lst = new Dictionary<string, ItemDists>();
		public bool irrVerb = true;
		List<CWordCount> lstord = new List<CWordCount>();
		public ObservableCollection<OutGridData> grdata;
		private IrregularVerbs irr = new IrregularVerbs();
		// //////////////////////////////////////////////////////////////////
		public void Run(string inp_text) {
			string buf = "";
			lenText = +inp_text.Length;
			for (int i = 0; i < lenText; i++) {
				char c = Char.ToLower(inp_text[i]);
				if (Char.IsLetter(c) || c == '\'')
					buf += c;
				else {
					if (buf.Length != 0) {
						if (buf.Length > 2)
							run1(buf, i);
						buf = "";
					}
				}
			} // for (int i = 0; i < lenText; i++)
			if (buf.Length > 2) // for last word
				run1(buf, lenText - 1);
			uniqMiddle = uniqWord;
		} // ////////////// public void Run(string inp_text) ///////////////////////
		private void run1(string s, int pos) {
			ItemDists itdst;
			if (irrVerb)
				s = irr.getInfinitive(s);
			allWord++;
			allChars += s.Length;
			if (lst.TryGetValue(s, out itdst)) {
				itdst.Add(pos); //lst[s] = cnt + 1;
			} else if (s != "the" && s != "and" && s != "you") {
				itdst = new ItemDists(pos);
				lst.Add(s, itdst);
				uniqWord++;
			}
		} // //////////////////////////////////////////////////////////////////////
		public void Sorting() {
			lstord.Clear();
			foreach (KeyValuePair<string, ItemDists> item in lst) {
				CWordCount wc = new CWordCount();
				wc.cnt = item.Value.Positions.Count;
				wc.dists = item.Value.SumDists;
				wc.word = item.Key;
				lstord.Add(wc);
			}
			if (lstord.Count > 0) {
				lstord.Sort();
				double mindist = lstord[lstord.Count - 1].dists;
				double maxdist = lstord[0].dists;
				foreach (CWordCount item in lstord) {
					item.dists = 99 * (item.dists - mindist) / (maxdist - mindist);
				}
			}
		} // ///////////////////////////////////////////////////////////////////////
		public WordCnt At(int pos) {
			CWordCount wc = lstord[pos];
			WordCnt ret = new WordCnt();
			ret.word = wc.word;
			ret.cnt = wc.cnt;
			return ret;
		} // /////////////////////////////////////////////////////////////////////
		public void DelStart(string ex) {
			Dictionary<string, ItemDists> keys4Add = new Dictionary<string, ItemDists>(); // keys for add 
			List<string> keys4Del = new List<string>(); // keys for delete 
			foreach (KeyValuePair<string, ItemDists> itemEx in lst) {
				string s = itemEx.Key;
				if (s.Length < ex.Length + 2)
					continue;
				if (s.Substring(0, ex.Length) == ex) {
					string sNoEx = s.Substring(ex.Length);
					if (sNoEx.Length < 3)
						continue;
					if (lst.ContainsKey(sNoEx)) {
						keys4Add.Add(sNoEx, itemEx.Value);
						keys4Del.Add(s);
					}
				}
			}
			uniqMiddle -= keys4Del.Count;
			foreach (KeyValuePair<string, ItemDists> p in keys4Add)
				lst[p.Key].Add(p.Value);
			foreach (string keyDel in keys4Del)
				lst.Remove(keyDel);
		} // /////////////// public void DelStart(string ex) /////////////////////////////
		public void DelEnd(string ex) {
			Dictionary<string, ItemDists> keys4Add = new Dictionary<string, ItemDists>(); // keys for add 
			List<string> keys4Del = new List<string>(); // keys for delete 
			foreach (KeyValuePair<string, ItemDists> itemEx in lst) {
				string s = itemEx.Key;
				if (s.Length < ex.Length + 2)
					continue;

				if (s.Substring(s.Length - ex.Length, ex.Length) == ex) {
					string sNoEx = s.Substring(0, s.Length - ex.Length);
					if (sNoEx.Length < 3)
						continue;
					if (lst.ContainsKey(sNoEx)) {
						keys4Add.Add(sNoEx, itemEx.Value);
						keys4Del.Add(s);
					}
				}
			}
			uniqMiddle -= keys4Del.Count;
			foreach (KeyValuePair<string, ItemDists> i in keys4Add)
				lst[i.Key].Add(i.Value);
			foreach (string keyDel in keys4Del)
				lst.Remove(keyDel);
		} // /////////////// public void DelUn() /////////////////////////////
		public void ReplEnd(string ex, string root_end) { //
			Dictionary<string, ItemDists> keys4Add = new Dictionary<string, ItemDists>(); // keys for add 
			List<string> keys4Del = new List<string>(); // keys for delete 
			foreach (KeyValuePair<string, ItemDists> itemEx in lst) {
				string s = itemEx.Key;
				if (s.Length < ex.Length + 2)
					continue;

				if (s.Substring(s.Length - ex.Length, ex.Length) == ex) {
					string sNoEx = s.Substring(0, s.Length - ex.Length);
					if (sNoEx.Length < 3)
						continue;
					if (lst.ContainsKey(sNoEx + root_end)) {
						keys4Add.Add(sNoEx + root_end, itemEx.Value);
						keys4Del.Add(s);
					}
				}
			}
			uniqMiddle -= keys4Del.Count;
			foreach (KeyValuePair<string, ItemDists> p in keys4Add)
				lst[p.Key].Add(p.Value);
			foreach (string keyDel in keys4Del)
				lst.Remove(keyDel);
		} // /////////////// public void DelUn() /////////////////////////////
		public void ReplEnd2(string ex) { //
			Dictionary<string, ItemDists> keys4Add = new Dictionary<string, ItemDists>(); // keys for add 
			List<string> keys4Del = new List<string>(); // keys for delete 
			foreach (KeyValuePair<string, ItemDists> itemEx in lst) {
				string s = itemEx.Key;
				if (s.Length < ex.Length + 2)
					continue;

				if (s.Substring(s.Length - ex.Length, ex.Length) == ex) {
					string sNoEx = s.Substring(0, s.Length - ex.Length);
					if (sNoEx.Length < 3)
						continue;
					string s1 = sNoEx.Substring(sNoEx.Length - 2, 1);
					string s2 = sNoEx.Substring(sNoEx.Length - 1, 1);
					if (s1 != s2)
						continue;
					sNoEx = sNoEx.Substring(0, sNoEx.Length - 1);
					if (lst.ContainsKey(sNoEx)) {
						keys4Add.Add(sNoEx, itemEx.Value);
						keys4Del.Add(s);
					}
				}
			}
			uniqMiddle -= keys4Del.Count;
			foreach (KeyValuePair<string, ItemDists> p in keys4Add)
				lst[p.Key].Add(p.Value);
			foreach (string keyDel in keys4Del)
				lst.Remove(keyDel);
		} // /////////////// public void DelUn() /////////////////////////////
		public void report(Dictionary<string, int> dict_db, bool? know, bool? unknow, int min_cnt, string path = "") {
			Sorting();
			grdata = new ObservableCollection<OutGridData>();
			int n = 0;
			double sum = 0, min = 0, max = 0;
			for (int i = 0; i < lstord.Count; i++) {
				CWordCount wc = lstord[i];
				sum += wc.cnt;
				//int val = dict_db[wc.word];
				bool bknow = false, bunknow = false, needAdd = true;
				if (dict_db != null) {
					if (!dict_db.ContainsKey(wc.word))
						continue;
					KnownUnknown.Pop(dict_db, wc.word, out bknow, out bunknow);
					n++;
					if (know == null && unknow == null) {
						needAdd = true;
					} else if (know != null && unknow != null) {
						needAdd = ((bunknow == unknow) && (bknow == know));
					} else if (unknow != null && know == null) {
						needAdd = (bunknow == unknow);
					} else if (know != null && unknow == null)
						needAdd = (bknow == know);
				}
				if (needAdd == true) {
					if (wc.dists < min)
						min = wc.dists;
					if (wc.dists > max)
						max = wc.dists;
					// Не загружать, если количество менее
					if (wc.cnt >= min_cnt)
						grdata.Add(new OutGridData(n, wc.cnt, wc.dists, wc.word, bknow, bunknow));
				}
			}
			foreach (OutGridData i in grdata) {
				i.Percent = (double)(0.1 * (int)(1000 * (i.Percent - min) / (max - min)));
			}

			if (path.Length == 0) {
				for (int i = 0; i < lstord.Count; i++) {
					CWordCount wc = lstord[i];
					Console.WriteLine("{0} Cnt = {1}, Word = {2}, Dists = {3}",
						i, wc.cnt, wc.word, wc.dists);
				}
			} else {
				try {
					if (File.Exists(path))
						File.Delete(path);
					using (StreamWriter sw = File.CreateText(path)) {
						sw.WriteLine("N;Count;Word");
						int sumf = 0;
						for (int i = 0; i < lstord.Count; i++) {
							CWordCount wc = lstord[i];
							sumf += wc.cnt;
							sw.WriteLine("{0};{1};{2};{3};{4}", i, (int)(100 * sum / allWord), wc.cnt, wc.word, wc.dists);
							Console.WriteLine("{0} {1} {2} {3} {4}", i, (int)(100 * sum / allWord), wc.cnt, wc.word, wc.dists);
						}
					}
				} catch (Exception ex) {
					Console.WriteLine(ex.ToString());
				}
			}
		} // ///////////////// public void report(string path = "") ////////////////////
	} // ******************** public class TextProc ************************************
} // ----------------------- namespace WordCounter -------------------------------------
