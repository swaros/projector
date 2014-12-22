using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Projector
{
    public class RegexGroup
    {
        public static List<string> getMatch(string input, int readLevel = 1)
        {
            string pattern = "^[^{}]*" +
                          "(" +
                          "((?'Open'{)[^{}]*)+" +
                          "((?'Close-Open'})[^{}]*)+" +
                          ")*" +
                          "(?(Open)(?!))$";
            
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
                        string subContent = "";
                        foreach (Capture cap in grp.Captures)
                        {
                            grCap++;
                            if (level == readLevel)
                            {
                                result.Add(cap.Value);
                            }
                            
                        }
                        if (level == readLevel)
                        {
                            return result;
                        }
                    }
                    level++;
                }
            }
            return result;
        }
       
    }
}
