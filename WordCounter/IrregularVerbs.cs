using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter {
	class IrregularVerbs {
		const string fname = "IrregularVerb.txt";
		public Dictionary<string, string> db;
		public IrregularVerbs() {
			Load(fname);
		} // /////////////////////////////////////////////////////////////////////////////
		private void Load(string fname) {
			string dbfname;
			if (!File.Exists(fname)) {
				dbfname = @"..\..\" + fname;
				if (!File.Exists(dbfname)) {
					dbfname = @"..\" + dbfname;
					if (!File.Exists(dbfname)) {
						Console.WriteLine("No IrregularVerbs file '" + fname + "' !");
						return;
					}
				}
			} else {
				dbfname = fname;
			}
			db = new Dictionary<string, string>();
			using (StreamReader sr = new StreamReader(dbfname, Encoding.Default)) {
				string key, val;
				for (; ; ) {
					val = sr.ReadLine();
					if (val == null)
						break;
					key = sr.ReadLine();
					if (key == null)
						break;
					db.Add(key, val);
				}
			}
		} // ///////////////////////////////////////////////////////////////////////////////
		public string getInfinitive(string s) {
			if (db.ContainsKey(s))
				return db[s];
			return s;
		} // ////////////////////////////////////////////////////////////////////////////////
	} // ****************************************************************************
}
