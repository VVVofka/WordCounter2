using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WordCounter {
	public static class KnownUnknown {
		public static int NO_DEF = 0;
		public static int KNOWN = 1;
		public static int UNKNOWN = 2;
		public static int UNSERTAINLY = 3;

		public static int getN(string s) {
			switch (s) {
				case "0":
					return NO_DEF;
				case "1":
					return KNOWN;
				case "2":
					return UNKNOWN;
				case "3":
					return UNSERTAINLY;
				default:
					return -1;
			}
		} // ///////////////////////////////////////////////////////////////////////////
		public static int getN(bool know, bool unknow) {
			if (know)
				if (unknow)
					return UNSERTAINLY;
				else
					return KNOWN;
			else if (unknow)
				return UNKNOWN;
			return NO_DEF;
		} // ///////////////////////////////////////////////////////////////////////////
		public static void Pop(Dictionary<string, int> db, string key, out bool know, out bool unknow) {
			if (!db.ContainsKey(key)) {
				know = false;
				unknow = false;
			} else if (db[key] == NO_DEF) {
				know = false;
				unknow = false;
			} else if (db[key] == KNOWN) {
				know = true;
				unknow = false;
			} else if (db[key] == UNKNOWN) {
				know = false;
				unknow = true;
			} else if (db[key] == UNSERTAINLY) {
				know = true;
				unknow = true;
			} else {
				know = false;
				unknow = false;
			}
		} // ////////////////////////////////////////////////////////////////////////////////
	} // *******************************************************************************
	public class DbWords {
		public string defFName = "DBWords.txt";
		public Dictionary<string, int> db;
		public void Load(string fname = "") {
			if (fname.Length == 0)
				fname = defFName;

			string dbfname = fname;
			if (!File.Exists(dbfname)) {
				dbfname = @"..\" + dbfname;
				if (!File.Exists(dbfname)) {
					dbfname = @"..\" + dbfname;
					if (!File.Exists(dbfname)) {
						dbfname = @"..\" + dbfname;
						if (!File.Exists(dbfname)) {
							Console.WriteLine("No DB file '" + fname + "' !");
							return;
						}
					}
				}
			}
			db = new Dictionary<string, int>();
			using (StreamReader sr = new StreamReader(dbfname, Encoding.Default)) {
				string line;
				while ((line = sr.ReadLine()) != null) {
					string sval = line.Substring(0, 1);
					string skey = line.Substring(1, line.Length - 1);
					try {
						db.Add(skey, KnownUnknown.getN(sval));
					} catch (ArgumentException) {
						Console.WriteLine("An element with Key = " + skey + " already exists.");
					}
				}
			}
		} // ///////////////////////////////////////////////////////////////////////////////
		public void Save(string fname = "") {
			if (fname.Length == 0)
				fname = defFName;
			using (StreamWriter sw = new StreamWriter(fname, false, System.Text.Encoding.Default)) {
				IEnumerable<KeyValuePair<string, int>> query = db.OrderBy(i => i.Key);
				foreach (KeyValuePair<string, int> i in query) {
				//foreach (KeyValuePair<string, int> i in db) {
					string s = i.Value.ToString() + i.Key;
					sw.WriteLine(s);
				}
			}
		} // ///////////////////////////////////////////////////////////////////////////////
		public void Push(string key, bool know, bool unknow) {
			if (db == null)
				db = new Dictionary<string, int>();
			int val = KnownUnknown.getN(know, unknow);
			if (db.ContainsKey(key))
				db[key] = val;
			else
				db.Add(key, val);
		} // //////////////////////////////////////////////////////////////////////////////
	} // ************************************************************************************
}

