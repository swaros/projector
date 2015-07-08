using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class CodeStringPrepaerator
    {

        private static string spaceFill = "(){}=";

        public static String PrePareString(string unprepared)
        {
            foreach (char checkChr in CodeStringPrepaerator.spaceFill.ToCharArray())
            {
                unprepared = unprepared.Replace(checkChr.ToString(), " " + checkChr.ToString() + " ");
            }
            return unprepared;
        }
    }
}
