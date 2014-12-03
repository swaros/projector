using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    public class ReflectionScriptDefines
    {        
        public string code;
        public string originCode;
        public Boolean isObject = false;
        public Boolean isVariable = false;
        public Boolean isMethod = false;
        public Boolean isLinked = false;
        public Boolean isAssignement = false;
        public Boolean isReflection = false;
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

        public ReflectionScript subScript;

    }
}
