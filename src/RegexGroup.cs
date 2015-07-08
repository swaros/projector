using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Projector
{
    public class RegexGroup
    {

        public static List<string> getMatch(string input, int readLevel = 1, char left = '{', char right = '}')
        {
            /*
            string pattern = "^[^"+ left + right +"]*" +
                          "(" +
                          "((?'Open'"+left+")[^" + left + right + "]*)+" +
                          "((?'Close-Open'"+right+")[^" + left + right + "]*)+" +
                          ")*" +
                          "(?(Open)(?!))$";
            */

            string pattern = @"^[^{}]*\{((?:[^{}]|(?<open>\{)|(?<-open>\}))+(?(open)(?!)))\}"; 

            List<string> result = new List<string>();
            Match m = Regex.Match(input, pattern);
            if (m.Success)
            {
                
                int level = 0;
                foreach (Group grp in m.Groups)
                {
                    if (grp.Success)
                    {
                        //result.Add(grp.Value);
                        int grCap = 0;
                        foreach (Capture cap in grp.Captures)
                        {
                            grCap++;
                            if (level >= readLevel || readLevel < 0)
                            {
                                result.Add(cap.Value);
                            }
                            
                        }
                        /*
                        if (level >= readLevel && readLevel >= 0)
                        {
                            return result;
                        }
                         */
                    }
                    level++;
                }
            }
            return result;
        }
       
    }
}
