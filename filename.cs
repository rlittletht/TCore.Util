using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TCore.Util
{
    public class Filename
    {
        /*----------------------------------------------------------------------------
        	%%Function: SBuildTempFilename
        	%%Qualified: TCore.Util.Filename.SBuildTempFilename
        	%%Contact: rlittle
        	
        ----------------------------------------------------------------------------*/
        public static string SBuildTempFilename(string sBaseName, string sExt)
        {
            if (sBaseName == null)
                sBaseName = "";

            if (sExt == null)
                return $"{Environment.GetEnvironmentVariable("Temp")}\\{sBaseName}{System.Guid.NewGuid().ToString()}";
            return $"{Environment.GetEnvironmentVariable("Temp")}\\{sBaseName}{System.Guid.NewGuid().ToString()}.{sExt}";
        }


    }
}