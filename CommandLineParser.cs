using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Projector
{
    class CommandLineParser
    {
        private string origText = "";
        private string leftLimiter  = "[SCRIPT]";
        private string rightLimiter = "[/SCRIPT]";

        private string[] content;

        public CommandLineParser(String commandLine)
        {
            origText = commandLine;
        }

        private void getContent()
        {
            string[] splits = new string[2];
            splits[0] = leftLimiter;
            splits[1] = rightLimiter;
            if (origText!=null && origText != "")
            {                
                content = origText.Split(splits, StringSplitOptions.None);
            }
        }

        private void workOnIt()
        {

        }


    }
}
