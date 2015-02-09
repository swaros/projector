using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    public class ScriptErrors
    {

        public const int UNDECLARED = 0;
        public const int TYPO_ERROR = 1;

        public int errorCode = 0;
        public String errorMessage = "";
        public int lineNumber = 0;
        public int wordPosition = 0;
        public Boolean wrongSpelling = false;
        public String internalMessage = "";
        public int errorType = 0;
    }
}
