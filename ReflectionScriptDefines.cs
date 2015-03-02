using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector.Script
{
    public class ReflectionScriptDefines
    {    
        /// <summary>
        /// contains the command as uppercase 
        /// </summary>
        public string code;
        /// <summary>
        /// this is the written code part
        /// </summary>
        public string originCode;
        /// <summary>
        /// flag for Objects
        /// </summary>
        public Boolean isObject = false;
        public Boolean isVariable = false;
        public Boolean isMethod = false;
        public Boolean isLinked = false;
        public Boolean isAssignement = false;
        public Boolean isSetup = false;
        public Boolean isSelfInc = false;
        public Boolean isSelfDec = false;
        public Boolean isReflection = false;
        public Boolean isParentAssigned = false;
        public Boolean parseable = false;
        public List<string> scriptParameters = new List<string>();
        public List<String> scriptParameterTypes = new List<String>();
        public List<Object> parameters;
        public string name;
        public string typeOfObject;
        public string namedReference;
        public Object Referenz;
        public Object ReflectObject;
        public int lineNumber = 0;
        public int lineCount = 1;
        public int setState = 0;
        public String codeToken = "UNSET";

        public ReflectionScript subScript;

    }
}
