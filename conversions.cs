using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TCore.Util
{
    public class Conversions
    {
        public static DateTime? DttmFromString(string sDateTime, int nUtcOffset, bool fAdjustDST)
        {
            DateTime? dttm;

            if (sDateTime == null || sDateTime == "")
                return null;

            try
                {
                dttm = DateTime.Parse(sDateTime);
                return DttmUTCFromDttm(dttm.Value, nUtcOffset, fAdjustDST);
                }
            catch
                {
                return null;
                }
        }

        public static int NFromString(string s, int nDefault)
        {
            if (s == null || s == "")
                return nDefault;

            try
                {
                return Int32.Parse(s);
                }
            catch
                {
                return nDefault;
                }
        }

        public static Guid? GuidFromString(string sGuid)
        {
            if (sGuid == null || sGuid == "")
                return null;

            try
                {
                return new Guid(sGuid);
                }
            catch {}

            return null;
        }

        public static bool BoolFromString(string s)
        {
            if (s == null || s == "")
                return false;

            if (s == "1" || String.Compare(s, "true", true) == 0)
                return true;

            return false;
        }
        public static DateTime DttmFromDttmUTC(DateTime dttm, int nUTCOffset, bool fDST)
        {
            if (fDST)
                nUTCOffset += GetDSTAdjust(dttm);

            return dttm.AddMinutes(nUTCOffset);
        }

        public static int GetDSTAdjust(DateTime dttm)
        {
            int nYearFirst = 2017;
            int[] rgdstBeginMonth = {3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3};
            int[] rgdstBeginDay = {12, 13, 8, 9, 10, 11, 13, 14, 13, 11, 10, 9, 8};
            int[] rgdstEndMonth = {11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11};
            int[] rgdstEndDay = {5, 6, 1, 2, 3, 4, 6, 7, 6, 4, 3, 2, 1};

            int i = nYearFirst - dttm.Year;

            if (i < 0 || i > rgdstBeginMonth.Length)
                throw new Exception($"DST NOT IMPLEMENTED FOR YEAR {dttm.Year}");

            DateTime dttmBegin = new DateTime(nYearFirst - i, rgdstBeginMonth[i], rgdstBeginDay[i], 3, 0, 0);
            DateTime dttmEnd = new DateTime(nYearFirst - i, rgdstEndMonth[i], rgdstEndDay[i], 2, 0, 0);

            if (dttm.CompareTo(dttmBegin) > 0
                && dttm.CompareTo(dttmEnd) < 0)
                {
                // we fall within DST, so this is a DST adjusted time. this means that we are 1 less hour away
                // from UTC
                return 60;
                }
            return 0;
        }

        [Test]
        [TestCase("3/9/2014 12:01", 60)]
        [TestCase("3/9/2014 0:01", 0)]
        [TestCase("3/9/2014", 0)]
        [TestCase("3/9/2014 1:00", 0)]
        [TestCase("3/9/2014 2:00", 0)]
        [TestCase("3/9/2014 3:01", 60)]
        [TestCase("3/13/2016 3:01", 60)]
        public static void TestGetDSTAdjust(string sDttm, int nExpected)
        {
            DateTime dttm = DateTime.Parse(sDttm);
            int nActual = GetDSTAdjust(dttm);
            Assert.AreEqual(nExpected, nActual);
        }

        /* D T T M  U  T  C  F R O M  D T T M */
        /*----------------------------------------------------------------------------
				%%Function: DttmUTCFromDttm
				%%Contact: rlittle
	
			----------------------------------------------------------------------------*/
        public static DateTime DttmUTCFromDttm(DateTime dttm, int nUTCOffset, bool fDST)
        {
            if (fDST)
                nUTCOffset += GetDSTAdjust(dttm);

            return dttm.AddMinutes(-nUTCOffset);
        }
    }
}
