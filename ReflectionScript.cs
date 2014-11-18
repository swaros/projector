using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace Projector
{
    class ReflectionScript
    {

        private const string REGEX_BRACKETS = "({[^}]*})";
        private const string REGEX_STRING = "\"([^\"]*)\"";

        private const string MASK_DELIMITER = "#";

        private Boolean followRecusives = true;

        private String code = "";

        private String[] lines;

        private List<String> mask = new List<String>();

        private List<String> objectList = new List<string>();

        private List<ScriptErrors> errorMessages = new List<ScriptErrors>();

        private Hashtable stringFounds = new Hashtable();

        private Hashtable namedSubScripts = new Hashtable();       

        private List<ReflectionScriptDefines> buildedSource = new List<ReflectionScriptDefines>();

        private List<int> emptyLines = new List<int>();

        private int currentReadLine = 0;

        private int lineOffset = 0;

        

        public ReflectionScript()
        {
            this.init();
        }

        public List<ReflectionScriptDefines> getScript()
        {
            return buildedSource;
        }
        /**
         * the getters
         */ 
       
        

        /**
         * resets all elements to starts a clear iteration
         */ 
        private void reset()
        {
            this.lineOffset = 0;
            this.errorMessages.Clear();
            this.objectList.Clear();
            this.stringFounds.Clear();
            this.buildedSource.Clear();
            this.namedSubScripts.Clear();
        }


        public void setCode(string code)
        {
            this.reset();
            this.code = code;
            if (this.prepareCodeLines() == true)
            {
                this.build();
            }
        }

        public string getErrors()
        {
            string error = "";
            for (int i = 0; i < errorMessages.Count; i++)
            {
                error +=  "line:" + errorMessages[i].lineNumber + "|" + errorMessages[i].errorMessage + System.Environment.NewLine;
            }
            return error;
        }

        public List<ScriptErrors> getAllErrors()
        {
            return errorMessages;
        }

        private int getLineNumber()
        {
            return this.currentReadLine + this.lineOffset;
        }


        private void addError(String Errormessage)
        {
            ScriptErrors error = new ScriptErrors();
            error.errorCode = 0;
            error.errorMessage = Errormessage;
            error.lineNumber = this.getLineNumber();
            this.errorMessages.Add(error);
        }

        /**
         * add an error in to the backlog
         */ 
        private void addError(String Errormessage, int ErrorCode)
        {
            ScriptErrors error = new ScriptErrors();
            error.errorCode = ErrorCode;
            error.errorMessage = Errormessage;
            error.lineNumber = this.getLineNumber();
            this.errorMessages.Add(error);
        }

        /**
         * returns the amount of errors
         */
        public int getErrorCount()
        {
            return this.errorMessages.Count();
        }

        private void addError(ScriptErrors error)
        {
            this.errorMessages.Add(error);
        }

        public String fillUpVars(string source)
        {
            string newStr = this.fillUpVarsBack(source);
            string chkStr = source;
            while (newStr != source)
            {
                source = newStr;
                newStr = fillUpVarsBack(source);
            }
            return newStr;
        }


        public String fillUpVarsBack(string source)
        {
            string newSrc = source;
            foreach (DictionaryEntry de in this.stringFounds)
            {
                newSrc = newSrc.Replace(de.Key.ToString(), de.Value.ToString());
            }
            return newSrc.Replace("\"","");
        }

        public String getCodeByName(string name)
        {
            if (namedSubScripts.ContainsKey(name))
            {
                return namedSubScripts[name].ToString();
            }
            return null;
        }

        public Hashtable getAllStrings()
        {
            return this.stringFounds;
        }

        private void init()
        {
            /**
             * mask for define the expected commands.
             * 
             *   - any keyword must be written in upercase and on the place who it is expected
             *  § the type of an object 
             *  % here is the name excpected. also this is the variable name
             *  ? this is an parameter AFTER THE KEY WORD
             *  & must be an existing variable. the name is expected
             *  
             *  = delimiter for the mask definition 
             *  
             * mask definitions
             * 
             * OBJECT   means this is an object
             *   
             */

            //base commands
            this.mask.Add("NEW § %" + Projector.ReflectionScript.MASK_DELIMITER + "OBJECT");
            this.mask.Add("PROCEDURE % ?" + Projector.ReflectionScript.MASK_DELIMITER + "PARSE");

            this.mask.Add("VAR % = ?" + Projector.ReflectionScript.MASK_DELIMITER + "VAR OBJECT");
            //this.mask.Add("STR § = ?" + Projector.ReflectionScript.MASK_DELIMITER + "VAR"  + Projector.ReflectionScript.MASK_DELIMITER + ". . . STR");
            //this.mask.Add("INT § = ?" + Projector.ReflectionScript.MASK_DELIMITER + "VAR" + Projector.ReflectionScript.MASK_DELIMITER + ". . . INT");

            //object depended commands
            this.mask.Add("& SETCOORDS ? ? ? ?" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". setCoords INT INT INT INT");
            this.mask.Add("& SELECTTABLE ?" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". selectTable STR");
            this.mask.Add("& FIREQUERY" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". fireQuery");
            this.mask.Add("& SETWHERE ? ?" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". setWhere STR STR");

            this.mask.Add("& SETSQL ?" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". setSql STR");
            this.mask.Add("& SHOWTABLELIST ?" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". showTableList BOOL");


            // just to store something
            // this.mask.Add("VAR § SET ?=STRINGVAR STR");
        }

        public List<int> getEmptyLines()
        {
            return this.emptyLines;
        }


        public string getSourceInfo()
        {
            string info = "";
            for (int i = 0; i < this.buildedSource.Count; i++ )
            {

                ReflectionScriptDefines tmpCode = (ReflectionScriptDefines)buildedSource[i];
                info += this.getSrcInfo(tmpCode);
            }
            return info;
        }

        public string getSrcInfo(ReflectionScriptDefines tmpCode)
        {
            string info ="";
            info += tmpCode.code;

            if (tmpCode.name != null)
            {
                info += "<" + tmpCode.name + "> ";
            }

            if (tmpCode.scriptParamaters != null)
            {
                info += " (";
                string add = "";
                foreach (String param in tmpCode.scriptParamaters)
                {
                    info += add + this.fillUpVars(param);
                    add = ", ";
                }
                info += ")";
            }

            info += System.Environment.NewLine;

            return info;
        }

        /**
         * show the Script properties as user readable string
         */ 
        public string dump(ReflectionScriptDefines tmpCode)
        {
            string info = "";
            info += strDump("code", tmpCode.code);
            info += strDump("name", tmpCode.name);
            info += strDump("namedReference", tmpCode.namedReference);
            info += strDump("originCode", tmpCode.originCode);
            info += strDump("typeOfObject", tmpCode.typeOfObject);
            info += strDump("isLinked", tmpCode.isLinked);
            info += strDump("isMethod", tmpCode.isMethod);
            info += strDump("isObject", tmpCode.isObject);
            info += strDump("isReflection", tmpCode.isReflection);
            info += strDump("isVariable", tmpCode.isVariable);
            info += strDump("lineNumber", tmpCode.lineNumber.ToString());
            info += strDump("count of Lines", tmpCode.lineCount.ToString());
            info += strDump("parseabale", tmpCode.parseable);
            info += strDump("Referenz", tmpCode.Referenz);
            info += strDump("ReflectedObject", tmpCode.ReflectObject);
            if (tmpCode.subScript != null)
            {
                info += strDump("SubSource", tmpCode.subScript.getSourceInfo());
            }
            info += "--parameters ---<" + System.Environment.NewLine;
            string info2 = "";
            if (tmpCode.scriptParamaters != null)
            {
                
                
                foreach (String param in tmpCode.scriptParamaters)
                {
                    info += param + System.Environment.NewLine;
                    info2 += this.fillUpVars(param) + System.Environment.NewLine;
                
                }
                info += System.Environment.NewLine;
            }

            info += System.Environment.NewLine + ">---- filed params ------< " + System.Environment.NewLine + info2;

            return info;
        }

        /**
         * some dump methods for different kind of parameters
         * 
         */ 
        private string strDump(string label, string value)
        {
            if (value == null)
            {
                value = "null";
            }

            return label + ": " + value + System.Environment.NewLine;
        }

        private string strDump(string label, Boolean value)
        {
            string strVal = "true";
            if (value == false)
            {
                strVal = "false";
            }

            return label + ": " + strVal + System.Environment.NewLine;
        }

        private string strDump(string label, Object value)
        {
            if (value == null)
            {
                return label + ": NULL" + System.Environment.NewLine;
            }
            return label + ": " + value.GetType() + System.Environment.NewLine;
        }

        /**
         * find strings ("xxx") store these in string memory. replace these string with placeholder
         * so following string methods do not affect them.
         * split lines depending on endline code
         * INFO System.Environment.Newline dosn't work for some reason (??) so \n is used
         */ 
        private Boolean prepareCodeLines()
        {
            // get out on empty code. maybe until writing
            if (this.code == null)
            {
                return false;
            }
            //Match match = Regex.Match(this.code, @"'([^']*)");

            // parse all strings so at the end code only will be in the source
            MatchCollection match = Regex.Matches(this.code, Projector.ReflectionScript.REGEX_STRING);
            
            
            for (int i = 0; i < match.Count; i++)
            {
                string str = match[i].Value;
                string key = "%str_" + i + "%";
                stringFounds.Add(key, str);
                code = code.Replace(str, key);
            }

            // get all subelemets by brackets {} independend from what kind of usage
            // just cut it out, add an marker for later usage
            MatchCollection bracketMatch = Regex.Matches(this.code, Projector.ReflectionScript.REGEX_BRACKETS);
            for (int i = 0; i < bracketMatch.Count; i++)
            {
                string str = bracketMatch[i].Value;
                string key = "%subscr_" + i + "%";
                stringFounds.Add(key, str);
                code = code.Replace(str, key);
                string nCode = str.Substring(1, str.Length - 2);
                namedSubScripts.Add(
                        key, 
                        nCode
                    );
            }

            //lines = Regex.Split(code.Replace(";", System.Environment.NewLine), System.Environment.NewLine);
            this.lines = Regex.Split(code, "\n");
            return true;
        }

        private void build()
        {
            int line = 0;
            emptyLines.Clear();

            foreach (String currentLine in lines)
            {
                this.currentReadLine = line;   
                string[] words = currentLine.Split(new char[] { ' ', '(', ')', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
                ReflectionScriptDefines buildRes = this.buildDefine(words);
                if (null != buildRes && this.validate(buildRes))
                {
                    this.buildedSource.Add(buildRes);
                }
                else
                {
                    emptyLines.Add(line);
                }
                line++;
            }
        }

        /**
         * validate object, and executes some needed operations (parse for example) just to go deeper
         * and found errors
         */ 
        private Boolean validate( ReflectionScriptDefines testObj)
        {

            if (testObj.isObject && testObj.name == null)
            {
                this.addError("No name defined for Object " + testObj.code);
            }

            if (testObj.isMethod && testObj.namedReference == null)
            {
                this.addError("this method is invalid because of a unknown Reference" + testObj.code);
            }

            if (testObj.isVariable)
            {
                if (testObj.name != null && testObj.scriptParamaters.Count() == 1)
                {
                    string name = this.fillUpVars(testObj.name);
                    foreach (string varValue in testObj.scriptParamaters)
                    {
                        string varName = testObj.name;
                        string varStr = this.fillUpVars(varValue);
                        if (stringFounds.ContainsKey(varName))
                        {
                            this.addError("variable " + varName + " allready defined");
                        } else {
                            stringFounds.Add(testObj.name, varStr);
                            stringFounds.Add("&" + testObj.name, varStr);
                            stringFounds.Add("&&" + testObj.name, "'" + varStr + "'");
                        }
                    }
                    //stringFounds.Add(name,
                }
                else
                {
                    this.addError("invalid paramatercount or invalid name for assignement ");
                }
            }


            // just something to do

            // this opbject is parseable so it must have some part of code as param
            if (testObj.parseable)
            {
                if (testObj.scriptParamaters.Count < 1)
                {
                    this.addError("Missing Code Exception: " + testObj.code);
                }
                else if (this.followRecusives)
                {
                    foreach (String subCode in testObj.scriptParamaters)
                    {
                        string fullCode = this.getCodeByName(subCode);
                        testObj.subScript = new ReflectionScript();
                        testObj.subScript.setCode(fullCode);

                        if (testObj.subScript.getErrorCount() > 0)
                        {
                            this.addError("Invalid Code in " + testObj.code);
                            this.addError(testObj.subScript.getErrors());
                        }
                    }
                }
            }

            // check how many lines of sourcecode is used to write this. so check strings because this is
            // the first valid source of linebreaks

            if (testObj.scriptParamaters.Count > 0)
            {
                string parFull = "";
                foreach (String subCode in testObj.scriptParamaters)
                {
                    parFull += this.fillUpVars(subCode);
                }

                string[] testOfCnt = parFull.Split('\n');
                int cnt = testOfCnt.Count();
                if (cnt > 1)
                {
                    testObj.lineCount = cnt;
                    this.lineOffset += cnt -1;
                }
            }


            return true;
        }


        private ReflectionScriptDefines buildDefine(string[] words)
        {
           


            // look on any mask
            foreach (string hash in this.mask)
            {

                // the reference ..alias the command
                string foundRef = null;

                // name of the object
                string objName = null;

                // assigned to an object (for methods for example) 
                string referenceObject = null;

                // assigned to an object (for methods for example) 
                string typeOfObject = null;

                ReflectionScriptDefines cmdResult = null;
                
                // reset reader position
                int partPosition = 0;

                int lastMaskElementCounts = 0;

                // get word by word from code
                foreach (string part in words)
                {
                    // for checking keywords transform it to upper case
                    string highPart = part.ToUpper();


                    // split mask into the mask himself (left) and the props (right)
                    string[] def = hash.Split(Projector.ReflectionScript.MASK_DELIMITER[0]);

                    // get the mask as words
                    string[] maskPart = def[0].Split(' ');
                   
                    lastMaskElementCounts = maskPart.Count();


                    // get the props as array
                    string[] definePart = def[1].Split(' ');

                     // is there some defines for the kind of parameters?
                    string[] varDefines = null;
                    if (def.Count() > 2)
                    {
                        varDefines = def[2].Split(' ');
                    }


                    // go to the current word position in the codeline if possible
                    if (maskPart.Count() > partPosition)
                    {

                        // comments ?
                        if (partPosition == 0 && part == "#")
                        {
                            return null;
                        }

                        // check if on the expected position an keyword
                        if (maskPart[partPosition] == highPart)
                        {

                            if (varDefines != null && varDefines.Count() > partPosition)
                            {
                                string definedSpelling = varDefines[partPosition];
                                if (definedSpelling != part)
                                {
                                    this.addError("wrong spelling of " + part + ". must be " + definedSpelling);
                                    return null;
                                }
                            }

                            foundRef = maskPart[partPosition];
                            cmdResult = new ReflectionScriptDefines();
                            cmdResult.lineNumber = getLineNumber();
                            cmdResult.isObject = definePart.Contains("OBJECT");
                            cmdResult.isMethod = definePart.Contains("METHOD");
                            cmdResult.parseable = definePart.Contains("PARSE");
                            cmdResult.isVariable = definePart.Contains("VAR");
                            cmdResult.code = highPart;
                            cmdResult.originCode = part;
                            cmdResult.parameters = new List<object>();
                        }
                        // a parameter
                        else if (cmdResult != null && maskPart[partPosition] == "?")
                        {
                            cmdResult.scriptParamaters.Add(part);

                            if (varDefines != null && varDefines.Count() > partPosition)
                            {
                                string definedType = varDefines[partPosition];
                                if (definedType == "INT"){
                                    try
                                    {
                                        int parInt = int.Parse(part);
                                        cmdResult.parameters.Add(parInt);
                                    }
                                    catch (Exception e)
                                    {
                                        this.addError("Parameter must bee an number" + e.Message);
                                        return null;
                                    }
                                    
                                }
                                else if (definedType == "STR")
                                {
                                    cmdResult.parameters.Add( this.fillUpVars(part) );
                                }
                                else if (definedType == "BOOL")
                                {
                                    if (part.ToUpper() == "TRUE" || part == "1")
                                    {
                                        cmdResult.parameters.Add(true);
                                    }
                                    else
                                    {
                                        cmdResult.parameters.Add(false);
                                    }
                                }                                
                            }

                        }
                        // a name definition
                        else if (maskPart[partPosition] == "%")
                        {
                            objName = part;
                        }
                        // define the type 
                        else if (maskPart[partPosition] == "§")
                        {
                            typeOfObject = part;
                        }
                        // use an reference to a named object
                        else if (maskPart[partPosition] == "&")
                        {
                            if (objectList.Contains(part))
                            {
                                referenceObject = part;
                            }
                            else
                            {
                                this.addError("invalid object reference. Object " + part + " not exists.");
                            }


                        }
                    }
                   
                    partPosition++;
                }
                               
                if (cmdResult != null)
                {

                    if (partPosition != lastMaskElementCounts)
                    {
                        
                        string error = "Invalid count of Parameters for " + foundRef;
                                                
                        this.addError(error);
                        return null;
                    }

                    cmdResult.name = objName;
                    cmdResult.namedReference = referenceObject;
                    cmdResult.typeOfObject = typeOfObject;

                    if (cmdResult.isObject && objName != null)
                    {
                        objectList.Add(objName);
                    }                    
                    // build parameters

                    

                    return cmdResult;
                }
            }
            
            return null;
        }


    }
}
