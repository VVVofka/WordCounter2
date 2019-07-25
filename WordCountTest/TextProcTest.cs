using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCounter;
namespace WordCountTest {
	[TestClass]
	public class TextProcTest {
		[TestMethod]
		public void TestMethod1() {
			TextProc tp = new TextProc();
			string s = @".OnE aWo don't2oneone one !Unone-ones+awOED AWOing done doned slip funny fun slipped";
			tp.Run(s);
			tp.DelStart("un");
			tp.DelEnd("s");
			tp.DelEnd("ed");
			tp.ReplEnd("ed", "e");
			tp.DelEnd("ing");
			tp.ReplEnd2("y");
			tp.ReplEnd2("ed");
			tp.Sorting();
			Assert.AreNotEqual(tp.lenText, 0, "lenText");
			Assert.AreEqual(tp.lenText, s.Length, "lenText");

			Assert.AreEqual(68, tp.allChars, "allChars");
			Assert.AreEqual(15, tp.allWord, "allWord");
			Assert.AreEqual(14, tp.uniqWord, "uniqWord");
			Assert.AreEqual(7, tp.uniqMiddle, "uniqMiddle");

			int nord = -1;
			WordCnt p;

			p = tp.At(++nord);
			Assert.AreEqual(p.word, "one", "Sort word " + nord);
			Assert.AreEqual(p.cnt, 4, "Sort cnt " + nord);
			p = tp.At(++nord);
			Assert.AreEqual(p.word, "awo", "Sort word " + nord);
			Assert.AreEqual(p.cnt, 3, "Sort cnt " + nord);
			p = tp.At(++nord);
			Assert.AreEqual(p.word, "done", "Sort word " + nord);
			Assert.AreEqual(p.cnt, 2, "Sort cnt " + nord);
			p = tp.At(++nord);
			Assert.AreEqual(p.word, "fun", "Sort word " + nord);
			Assert.AreEqual(p.cnt, 2, "Sort cnt " + nord);
			p = tp.At(++nord);
			Assert.AreEqual(p.word, "slip", "Sort word " + nord);
			Assert.AreEqual(p.cnt, 2, "Sort cnt " + nord);
			p = tp.At(++nord);
			Assert.AreEqual(p.word, "don't", "Sort word " + nord);
			Assert.AreEqual(p.cnt, 1, "Sort cnt " + nord);
			p = tp.At(++nord);
			Assert.AreEqual(p.word, "oneone", "Sort word " + nord);
			Assert.AreEqual(p.cnt, 1, "Sort cnt " + nord);
		} // //////////////////////////////////////////////////////////////////////
		[TestMethod]
		public void TestDB() {
			const string fname = "testdb.txt";
			DbWords db = new DbWords();
			db.Push("Know", true, false);
			db.Push("NoDefine", false, false);
			db.Push("unKnow", false, true);
			db.Push("MayBe", true, true);
			db.Push("unKnowAdd", false, true);
			db.Save(fname);

			DbWords dbr = new DbWords();
			dbr.Load(fname);
			Assert.AreEqual(db.db.Count, 5, "Equal Len dict");
			Assert.AreEqual(db.db.Count, dbr.db.Count, "Equal Len dict");
			foreach (KeyValuePair<string, int> i in dbr.db) {
				string key = i.Key;
				Assert.AreEqual(i.Value, dbr.db[key], "Key: " + key);
			}
			Assert.AreEqual(dbr.db["NoDefine"], KnownUnknown.NO_DEF, "NoDefine");
			Assert.AreEqual(dbr.db["Know"], KnownUnknown.KNOWN, "Know");
			Assert.AreEqual(dbr.db["unKnow"], KnownUnknown.UNKNOWN, "unKnow");
			Assert.AreEqual(dbr.db["unKnowAdd"], KnownUnknown.UNKNOWN, "unKnowAdd");
			Assert.AreEqual(dbr.db["MayBe"], KnownUnknown.UNSERTAINLY, "MayBe");
		}
	} // **************************************************************************
} // ------------------------------------------------------------------------------
  //Assert.Fail("from Fail");
  //string message = String.Format("InitCheckBox wrong case parametr: `{0}`", readRegVal);
  //throw new ArgumentOutOfRangeException(message);

// throw new ArgumentOutOfRangeException("amount", amount, "ds");
//StringAssert.Contains(e.Message, BankAccount.DebitAmountExceedsBalanceMessage);

//try {
//	account.Debit(debitAmount);
//} catch (ArgumentOutOfRangeException e) {
//	// Assert
//	StringAssert.Contains(e.Message, BankAccount.DebitAmountExceedsBalanceMessage);
//}

