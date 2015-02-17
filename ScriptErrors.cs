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

        /// <summary>
        /// the code for the error
        /// </summary>
        public int errorCode = 0;

        /// <summary>
        /// The readable error message
        /// </summary>
        public String errorMessage = "";

        /// <summary>
        /// The Line number in sourcecode
        /// </summary>
        public int lineNumber = 0;

        /// <summary>
        /// if known the position of the word. 0 is unknown
        /// </summary>
        public int wordPosition = 0;

        /// <summary>
        /// is true if a word just wrong written.
        /// </summary>
        public Boolean wrongSpelling = false;

        /// <summary>
        /// an interal Error Message
        /// </summary>
        public String internalMessage = "";

        /// <summary>
        /// The type of Error defined in SriptErrors
        /// </summary>
        public int errorType = 0;

        /// <summary>
        /// is true if this error thrown while runtime.
        /// </summary>
        public Boolean runtimeError = false;
    }
}
