using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace Projector
{
    public class ReflectionScript
    {

        private const string REGEX_BRACKETS = "({[^}]*})";
        private const string REGEX_STRING = "\"([^\"]*)\"";

        public const string MASK_DELIMITER = "#";

        private Boolean followRecusives = true;

        private String code = "";

        private String storedCode = "";

        // current code lines
        private String[] lines;

    
        // VARIABLES: the list of Objects
        private List<String> objectList = new List<string>();

        // VARIABLES: contains the refrences for all objects
        private Hashtable objectReferences = new Hashtable();

        // VARIABLES: list of all Int Variables
        private Hashtable globalRenameHash = new Hashtable();

        // VARIABLES: list of all string Variables
        private Hashtable stringFounds = new Hashtable();

        // VARIABLES: list of all Long Variables
        private Hashtable int64Founds = new Hashtable();

        // VARIABLES: list of all Long Variables
        private Hashtable intFloatFounds = new Hashtable();

        // VARIABLES: list of all Long Variables
        private Hashtable intDoubleFounds = new Hashtable();

        // VARIABLES: list of all Boolean Variables
        private Hashtable BoolFounds = new Hashtable();

        // VARIABLES: list of all subscripts defined by methods
        private Hashtable namedSubScripts = new Hashtable();       

        // CONTROL: list of objects that be already parsed. (all methods and paramaters extracted)
        private List<string> parsedObjects = new List<string>();

        // CONTROL: the mask that defines how to parse the code
        private List<String> mask = new List<String>();

        // CONTROL: the jit result
        private List<ReflectionScriptDefines> buildedSource = new List<ReflectionScriptDefines>();

        // contains all errors
        private List<ScriptErrors> errorMessages = new List<ScriptErrors>();

        // list of full emtpty lines
        private List<int> emptyLines = new List<int>();

        // list of comment lines
        private List<int> commentedLines = new List<int>();

        // current line that actualy parsed
        private int currentReadLine = 0;

        // offest by mutlilines (line breaking strings or subcode)
        private int lineOffset = 0;



        

        public ReflectionScript()
        {
            this.init();
        }

        public List<ReflectionScriptDefines> getScript()
        {
            return this.buildedSource;
        }

        /**
         * resets all elements to starts a 
         * clear iteration
         */ 
        private void reset()
        {
            this.commentedLines.Clear();
            this.lineOffset = 0;
            this.errorMessages.Clear();
            this.objectList.Clear();
            this.objectReferences.Clear();
            this.globalRenameHash.Clear();
            this.buildedSource.Clear();
            this.namedSubScripts.Clear();
        }

        /**
         * assign text as Code and starts the Build if the code 
         * different from the last assigned
         */
        public void setCode(string code)
        {
            if (this.storedCode != code)
            {
                this.reset();
                this.code = code;
                // just to find out if the not the same code again will be set
                this.storedCode = code;
                if (this.prepareCodeLines() == true)
                {
                    this.build();
                }
            }
        }

        /**
         * return an string with all error informations.
         * usabel if you have only one string for display
         * errormessages
         * 
         */
        public string getErrors()
        {
            string error = "";
            for (int i = 0; i < errorMessages.Count; i++)
            {
                error +=  "line:" + errorMessages[i].lineNumber + "|" + errorMessages[i].errorMessage + System.Environment.NewLine;
            }
            return error;
        }

        /**
         * return full List of all Errors
         * 
         */ 
        public List<ScriptErrors> getAllErrors()
        {
            return errorMessages;
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

        /**
         * add an error
         */ 
        public void addError(ScriptErrors error)
        {
            this.errorMessages.Add(error);
        }


        /**
         * return a list of linenumbers 
         * that are comments only         
         */
        public List<int> getCommentLines()
        {
            return this.commentedLines;
        }

        /**
         * return the current line number.
         * includes offest calculation
         * for linebreaking code
         */
        private int getLineNumber()
        {
            return this.currentReadLine + this.lineOffset;
        }

        public String fillUpAll(string source)
        {
            return this.fillUpCodeLines(
                    this.fillUpStrings(source)
                );
        }


        public String fillUpStrings(string source)
        {
            return this.fillUpVars(source, this.globalRenameHash);
        }

        public String fillUpCodeLines(string source)
        {
            return this.fillUpVars(source, this.namedSubScripts);
        }

        /**
         * fills up all placeHolder
         * with content from assigned Hashtable
         */ 
        public String fillUpVars(string source, Hashtable useThis)
        {
            string newStr = this.fillUpVarsBack(source, useThis);
            string chkStr = source;
            while (newStr != source)
            {
                source = newStr;
                newStr = fillUpVarsBack(source, useThis);
            }
            return newStr;
        }

        /**
         * fills placeholder recursiv
         */
        public String fillUpVarsBack(string source, Hashtable useThis)
        {
            string newSrc = source;
            foreach (DictionaryEntry de in useThis)
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
            return this.globalRenameHash;
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

            this.mask.Add("MESSAGE ?" + Projector.ReflectionScript.MASK_DELIMITER + "PARSE" + Projector.ReflectionScript.MASK_DELIMITER + ". STR");

            this.mask.Add("VAR % = ?" + Projector.ReflectionScript.MASK_DELIMITER + "VAR OBJECT");
            //this.mask.Add("STR § = ?" + Projector.ReflectionScript.MASK_DELIMITER + "VAR"  + Projector.ReflectionScript.MASK_DELIMITER + ". . . STR");
            //this.mask.Add("INT § = ?" + Projector.ReflectionScript.MASK_DELIMITER + "VAR" + Projector.ReflectionScript.MASK_DELIMITER + ". . . INT");

            //object depended commands
            /*
            this.mask.Add("&QueryBrowser SETCOORDS ? ? ? ?" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". setCoords INT INT INT INT");
            this.mask.Add("&QueryBrowser SELECTTABLE ?" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". selectTable STR");
            this.mask.Add("&QueryBrowser FIREQUERY" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". fireQuery");
            this.mask.Add("&QueryBrowser SETWHERE ? ?" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". setWhere STR STR");

            this.mask.Add("& SETSQL ?" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". setSql STR");
            this.mask.Add("& SHOWTABLELIST ?" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". showTableList BOOL");

            this.mask.Add("& INVOKE ?" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD PARSE");
            */


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
                    info += add + this.fillUpAll(param);
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
                    info2 += this.fillUpAll(param) + System.Environment.NewLine;
                
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
                globalRenameHash.Add(key, str);
                code = code.Replace(str, key);
            }

            // get all subelemets by brackets {} independend from what kind of usage
            // just cut it out, add an marker for later usage
            MatchCollection bracketMatch = Regex.Matches(this.code, Projector.ReflectionScript.REGEX_BRACKETS);
            for (int i = 0; i < bracketMatch.Count; i++)
            {
                string str = bracketMatch[i].Value;
                string key = "%subscr_" + i + "%";
                //globalRenameHash.Add(key, str);
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
                    string name = this.fillUpStrings(testObj.name);
                    foreach (string varValue in testObj.scriptParamaters)
                    {
                        string varName = testObj.name;
                        string varStr = this.fillUpStrings(varValue);
                        if (globalRenameHash.ContainsKey(varName))
                        {
                            this.addError("variable " + varName + " allready defined");
                        } else {                            
                            globalRenameHash.Add("&" + testObj.name, varStr);
                            globalRenameHash.Add("&&" + testObj.name, "'" + varStr + "'");
                        }
                    }
                    //stringFounds.Add(name,
                }
                else
                {
                    this.addError("invalid paramatercount or invalid name for assignement ");
                }
            }


            // get methods from an object
            if (testObj.isObject)
            {
                string cmd = testObj.code.ToUpper();

                if (testObj.isObject && cmd == "NEW" && !this.parsedObjects.Contains(testObj.typeOfObject))
                {
                    ReflectNew reflector = new ReflectNew();
                    Object testObject = reflector.getObject(testObj,this);
                    ObjectInfo obInfo = new ObjectInfo();
                    List<string> objMethods = obInfo.getObjectInfo(testObject);
                    if (objMethods != null)
                    {
                        foreach (string maskStr in objMethods)
                        {
                            this.mask.Add(maskStr);
                        }
                    }
                    this.parsedObjects.Add(testObj.typeOfObject);
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
                    testObj.parameters = new List<object>();
                    foreach (String subCode in testObj.scriptParamaters)
                    {                        
                        string fullCode = this.getCodeByName(subCode);
                        testObj.subScript = new ReflectionScript();
                        testObj.subScript.setCode(fullCode);

                        testObj.parameters.Add(testObj.subScript);

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
                    parFull += this.fillUpAll(subCode);
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
            if (words.Count() > 0 && words[0][0] == '#')
            {                
                this.commentedLines.Add(this.getLineNumber());
                return null;
            }

            // look on any mask
            foreach (string hash in this.mask)
            {

                RefScriptMaskMatch maskMatch = new RefScriptMaskMatch(hash);
                if (maskMatch.possibleMatch(words) != RefScriptMaskMatch.MATCH)
                {
                    continue;
                }
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

                        // comment ?
                        if (partPosition == 0 && part[0] == '#')
                        {
                            this.commentedLines.Add( this.getLineNumber() );
                            return null;
                        }

                        // check if on the expected position an keyword
                        string currentMaskPart = maskPart[partPosition];
                        if (currentMaskPart == highPart)
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
                        else if (cmdResult != null && currentMaskPart == "?")
                        {
                            cmdResult.scriptParamaters.Add(part);

                            if (varDefines != null && varDefines.Count() > partPosition)
                            {
                                string definedType = varDefines[partPosition];
                                if (definedType == "INT" || definedType == "Int32")
                                {
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
                                else if (definedType == "STR" || definedType == "String")
                                {
                                    cmdResult.parameters.Add( this.fillUpStrings(part) );
                                }
                                else if (definedType == "BOOL" || definedType == "Boolean")
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
                                else if (definedType == "ReflectionScript")
                                {
                                    cmdResult.parameters.Add(this.fillUpCodeLines(part));
                                    cmdResult.parseable = true;
                                }
                            }

                        }
                        // a name definition
                        else if (currentMaskPart == "%")
                        {
                            objName = part;
                        }
                        // define the type 
                        else if (currentMaskPart == "§")
                        {
                            typeOfObject = part;
                        }
                        // use an reference to a named object
                        else if (currentMaskPart[0] == '&')
                        {
                            string objectType = "";

                            if (currentMaskPart.Length > 1)
                            {
                                objectType = currentMaskPart.Remove(0, 1);
                            }

                            if (this.objectReferences.Contains(part))
                            {
                                if (objectType == "" || this.objectReferences[part].ToString() == objectType)
                                {
                                    referenceObject = part;
                                }
                                else
                                {
                                    this.addError("wrong Object Type. Must be an Instance of type: " + objectType + " .");
                                }
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
                        if (this.objectReferences.Contains(objName))
                        {
                            string error = "a Object " + objName + " already exists";
                            return null;
                        } else {
                            objectList.Add(objName);
                            this.objectReferences.Add(objName, typeOfObject);
                        }
                    }                    
                    // build parameters

                    

                    return cmdResult;
                }
            }
            
            return null;
        }


    }
}
