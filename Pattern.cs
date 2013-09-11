using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections;

namespace Projector
{
    class Pattern
    {

        private string scanText;
        private String[] scanTokens;
        
        public void setText(string lines)
        {
            this.scanText = lines;
            this.scan();
        }

        private PatternTypes checkToken(string token)
        {

            HashSet<PatternTypes> usedPattern = new HashSet<PatternTypes>();

            // add patterns in order of check
            PatternVarchar stringPattern = new PatternVarchar();
            usedPattern.Add(stringPattern);

            PatternGroup groupPattern = new PatternGroup();
            usedPattern.Add(groupPattern);

            // check the patterns
            foreach (PatternTypes tmpPat in usedPattern)
            {
                if (applyStartToken(tmpPat, token))
                {
                    return tmpPat;
                }
            }

            return null;
        }

        private Boolean applyStartToken(PatternTypes checkPattern,string token)
        {
            if (checkPattern.useIdentChecker)
            {
                checkPattern.currentContent = token;
                return checkPattern.checkStart();
            }
            else
            {
                return (checkPattern.startident == token);
            }
        }

        private void scan()
        {
            Regex r = new Regex("([= \\t{}()<>:;+-/*,'])");
            this.scanTokens = r.Split(this.scanText);
            foreach (string token in this.scanTokens)
            {
                if (token.Length > 0)
                {
                   PatternTypes addPattern = this.checkToken(token);

                   // a pattern was found
                   if (null != addPattern)
                   {
                       if (addPattern.scanReverse)
                       {

                       }
                   } 
                }
                
            }

        }
    }
}
