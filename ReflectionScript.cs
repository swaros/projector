using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;


namespace Projector
{
    public class ReflectionScript
    {

        public const string SETUP_MAXWAIT = "MAX_WAIT";
        public const string SETUP_GLOBAL = "GLOBAL";


        private const string REGEX_BRACKETS = "({[^}]*})";
        private const string REGEX_CALC_BRACKETS = @"(\([^\)]*\))";
       // private const string REGEX_CALC_BRACKETS = "((.+)\\)/U";
        private const string REGEX_STRING = "\"([^\"]*)\"";

        public const string MASK_DELIMITER = "#";

        public string name = "this";

        public int parentLineNumber = 0;

        // parent reflection script
        public ReflectionScript Parent;

        // parent reflectionScript Owner
        public ReflectionScriptDefines ParentOwner;

        //public RefScriptExecute ParentExecuter;
        public RefScriptExecute CurrentExecuter;

        private Boolean followRecusives = true;

        private String code = "";

        private String storedCode = "";

        // current code lines
        private String[] lines;

        // contains all settings
        private Hashtable Setup = new Hashtable();

        // VARIABLES: the list of Objects
        private List<String> objectList = new List<string>();

        // VARIABLES: contains the refrences for all objects
        private Hashtable objectReferences = new Hashtable();

        private Hashtable objectStorage = new Hashtable();

        // VARIABLES: list of all Int Variables
        private Hashtable globalRenameHash = new Hashtable();

        // VARIABLES: list of all variables that cutted out so we clean the sourcecode
        private Hashtable globalParamRenameHash = new Hashtable();


        // VARIABLES: list of all string Variables
        private Hashtable stringFounds = new Hashtable();

        // VARIABLES: list of all Long Variables
        private Hashtable int64Founds = new Hashtable();

        // VARIABLES: list of all Long Variables
        private Hashtable int32Founds = new Hashtable();

        // VARIABLES: list of all Long Variables
        private Hashtable floatFounds = new Hashtable();

        // VARIABLES: list of all Long Variables
        private Hashtable doubleFounds = new Hashtable();

        // VARIABLES: list of all Boolean Variables
        private Hashtable BoolFounds = new Hashtable();

        // VARIABLES: list of all subscripts defined by methods
        private Hashtable namedSubScripts = new Hashtable();

        // CONTROL: all subscripts
        private Hashtable subScripts = new Hashtable();

        // VARIABLES: list of entries between () brackets that used for calculations

        // ---- this one contains the Maths himself. this will be used for calculations
        private Hashtable calcingBrackets = new Hashtable();

        // ---- this one contains the results only so it will be easier to fill up vars
        private Hashtable calcingBracketsResults = new Hashtable();   

        // CONTROL: list of objects that be already parsed. (all methods and paramaters extracted)
        private List<string> parsedObjects = new List<string>();

        // CONTROL: the mask that defines how to parse the code
        private List<String> mask = new List<String>();

        // CONTROL: the jit result
        private List<ReflectionScriptDefines> buildedSource = new List<ReflectionScriptDefines>();

        // contains all errors
        private List<ScriptErrors> errorMessages = new List<ScriptErrors>();

        // stores allready reported errors. so only the first one is used
        private List<int> errorLines = new List<int>();

        // list of full emtpty lines
        private List<int> emptyLines = new List<int>();

        // list of comment lines
        private List<int> commentedLines = new List<int>();

        // current line that actualy parsed
        private int currentReadLine = 0;

        // offest by mutlilines (line breaking strings or subcode)
        private int lineOffset = 0;
        
        // debuggig stuff
        private Object parentWatcher;

        private string parentWatcherMethod;

        private Boolean debugMode = false;
        

        public ReflectionScript()
        {
            this.init();
        }

        

        public void init()
        {

            this.initDefaultSetups();

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
             * PARSE    any parameter is a reflectionScript
             * VAR      will create an Variable
             * ASSIGN   change the value of a given Object. so an Type of Object must be an Part of this (%)
             * SELFINC  for assignements. means the value is increased by self
             * SELFDEC  for assignements. means the value is decreased by self
             * PARENT   means an operation on the parent script.
             * DOWNCOPY copy an value from an parent value
             * WAIT     set the execution state to wait (for this script part)
             * RUN      set the execution state to continue
             * SETUP    use value to change setup value
             * 
             */

            //base commands
            this.mask.Add("NEW § %" + Projector.ReflectionScript.MASK_DELIMITER + "OBJECT");
            this.mask.Add("PROCEDURE % ?" + Projector.ReflectionScript.MASK_DELIMITER + "PARSE");

            // hard coded native stufff
            this.mask.Add("MESSAGEBOX ?" + Projector.ReflectionScript.MASK_DELIMITER);
            this.mask.Add("REG ?" + Projector.ReflectionScript.MASK_DELIMITER);
            this.mask.Add("UNREG ?" + Projector.ReflectionScript.MASK_DELIMITER);
            this.mask.Add("WAITFOR ?" + Projector.ReflectionScript.MASK_DELIMITER);

            this.mask.Add("VAR % = ?" + Projector.ReflectionScript.MASK_DELIMITER + "VAR OBJECT" + Projector.ReflectionScript.MASK_DELIMITER + "var . = STR");
            this.mask.Add("STRING % = ?" + Projector.ReflectionScript.MASK_DELIMITER + "VAR OBJECT" + Projector.ReflectionScript.MASK_DELIMITER + "string . = STR");
            this.mask.Add("INTEGER % = ?" + Projector.ReflectionScript.MASK_DELIMITER + "VAR OBJECT" + Projector.ReflectionScript.MASK_DELIMITER + "integer . = INT");

            this.mask.Add("% = ?" + Projector.ReflectionScript.MASK_DELIMITER + "ASSIGN" + Projector.ReflectionScript.MASK_DELIMITER + ". = ?");
            this.mask.Add("% += ?" + Projector.ReflectionScript.MASK_DELIMITER + "ASSIGN SELFINC" + Projector.ReflectionScript.MASK_DELIMITER + ". += ?");

            // environment change
            this.mask.Add("SET % ?" + Projector.ReflectionScript.MASK_DELIMITER + "VAR SETUP");


            // access to parent if exists
            this.mask.Add("PARENT % = ?" + Projector.ReflectionScript.MASK_DELIMITER + "ASSIGN PARENT" + Projector.ReflectionScript.MASK_DELIMITER + "PARENT . = ?");
            this.mask.Add("PARENT % += ?" + Projector.ReflectionScript.MASK_DELIMITER + "ASSIGN PARENT SELFINC" + Projector.ReflectionScript.MASK_DELIMITER + "PARENT . += ?");

           // this.mask.Add("DOWN % = ?" + Projector.ReflectionScript.MASK_DELIMITER + "ASSIGN DOWNCOPY" + Projector.ReflectionScript.MASK_DELIMITER + "PARENT . = ?");

            // exection controlls
            this.mask.Add("STOP" + Projector.ReflectionScript.MASK_DELIMITER + "WAIT" + Projector.ReflectionScript.MASK_DELIMITER + "STOP");
            this.mask.Add("RUN" + Projector.ReflectionScript.MASK_DELIMITER + "RUN" + Projector.ReflectionScript.MASK_DELIMITER + "RUN");
            this.mask.Add("EXIT" + Projector.ReflectionScript.MASK_DELIMITER + "EXIT" + Projector.ReflectionScript.MASK_DELIMITER + "EXIT");

            this.mask.Add("PARENT STOP" + Projector.ReflectionScript.MASK_DELIMITER + "WAIT PARENT" + Projector.ReflectionScript.MASK_DELIMITER + "PARENT STOP");
            this.mask.Add("PARENT RUN" + Projector.ReflectionScript.MASK_DELIMITER + "RUN PARENT" + Projector.ReflectionScript.MASK_DELIMITER + "PARENT RUN");
            this.mask.Add("PARENT EXIT" + Projector.ReflectionScript.MASK_DELIMITER + "EXIT PARENT" + Projector.ReflectionScript.MASK_DELIMITER + "PARENT EXIT");

            // just to store something
            // this.mask.Add("VAR § SET ?=STRINGVAR STR");
        }

        private void initDefaultSetups()
        {
            this.addSetupIfNotExists(ReflectionScript.SETUP_MAXWAIT, 0); // max wait 10 seconds for finishing threads
            this.addSetupIfNotExists(ReflectionScript.SETUP_GLOBAL, true);
        }

        private void addSetupIfNotExists(string name, Object value)
        {
            if (this.Setup.ContainsKey(name))
            {                
                return;
            }

            this.Setup.Add(name, value);
        }

        private void addSetup(string name, Object value)
        {
            if (this.Setup.ContainsKey(name))
            {
                this.Setup[name] = value;
                return;
            }

            this.Setup.Add(name, value);
        }

        public int SetupIntValue(string name)
        {
            if (this.Setup.ContainsKey(name))
            {
                if (this.Setup[name] is int)
                {
                    return (int)this.Setup[name];
                }
            }
            return 0;
        }

        public Boolean SetupBoolValue(string name)
        {
            if (this.Setup.ContainsKey(name))
            {
                if (this.Setup[name] is Boolean)
                {
                    return (Boolean)this.Setup[name];
                }
            }
            return false;
        }

        // -------------------- end setup stuff ----------------------

        public List<ReflectionScriptDefines> getScript()
        {
            return this.buildedSource;
        }

        public List<ReflectionScriptDefines> getFullScript()
        {

            List<ReflectionScriptDefines> res = new List<ReflectionScriptDefines>();
            res.AddRange( this.getScript());
            foreach (DictionaryEntry subScr in this.subScripts)
            {
                ReflectionScript refScr = (ReflectionScript)subScr.Value;
                List<ReflectionScriptDefines> subSource = refScr.getFullScript();                
                res.AddRange(subSource);
            }

            return res;
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
            this.objectStorage.Clear();
            this.globalRenameHash.Clear();
            this.globalParamRenameHash.Clear();
            this.buildedSource.Clear();
            this.namedSubScripts.Clear();
            this.subScripts.Clear();
            this.errorLines.Clear();
            this.calcingBrackets.Clear();
            this.calcingBracketsResults.Clear();
            this.parsedObjects.Clear();

            this.setDefaultVars();

        }

        private void setDefaultVars()
        {
            this.createOrUpdateStringVar("&PATH.DOCUMENTS", System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonDocuments) + System.IO.Path.DirectorySeparatorChar.ToString());
            this.createOrUpdateStringVar(".PATH.SEP", System.IO.Path.DirectorySeparatorChar.ToString());
            this.createOrUpdateStringVar(".NL.", "\n");
        }


        /**
         * assign text as Code and starts the Build if the code 
         * different from the last assigned
         */

        public void setCode(string code)
        {
            this.setCode(code, false);
        }

        public void setCode(string code, Boolean forceRebuild)
        {
            if (this.storedCode != code)
            {
                this.reset();
                this.code = code;
                // just to find out if the not the same code again will be set
                this.storedCode = code;

                if (this.Parent != null)
                {
                    this.Parent.updateSubScripts(this);
                    this.Parent.updateDebugInfo(this);
                }

                if (this.prepareCodeLines() == true)
                {
                    this.build();
                }

                
            }
        }

        public void reBuild()
        {
            this.reset(); 
            this.code = this.storedCode;
            if (this.prepareCodeLines() == true)
            {
                this.build();
            }
            this.build();
            
        }
        // debugging

        public void registerDebugWatcher(Object debugWatcher, string varChange)
        {
            this.parentWatcher = debugWatcher;
            this.parentWatcherMethod = varChange;
            this.debugMode = true;
        }

        private void updateDebugMessage(string varName, string value)
        {
            if (this.parentWatcher != null && parentWatcherMethod != null)
            {
                Type queryWinType = this.parentWatcher.GetType();
                MethodInfo myMethodInfo = queryWinType.GetMethod(this.parentWatcherMethod);
                object[] mParam = new object[] { this.getLineNumber(),  varName, value };
                myMethodInfo.Invoke(this.parentWatcher, mParam);
            }
            
        }

        public void updateDebugInfo(ReflectionScript child)
        {
            child.registerDebugWatcher(this.parentWatcher, this.parentWatcherMethod);
        }

        // -------------------- methods to get Infomations about the current Situation of the script ------------

        public List<string> getCurrentObjectsByType(string objectType)
        {
            if (this.objectReferences.ContainsValue(objectType))
            {
                List<string> usedBy = new List<string>();
                foreach (DictionaryEntry de in this.objectReferences)
                {
                    if (de.Value != null && de.Value.ToString() == objectType)
                    {
                        usedBy.Add(de.Key.ToString());
                    }
                }
                return usedBy;
            }
            return null;
        }



        // -------------------  error handlings, methods for add and get the errors -----------------------------


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
            if (Parent != null)
            {
                Parent.addError(error);
            }


            if (this.errorLines.Contains(error.lineNumber))
            {
                return;
            }
            this.errorLines.Add(error.lineNumber);
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
            return 
                this.fillUpMaths(
                    this.fillUpCodeLines(
                        this.fillUpStrings(source)
                    )
                );
        }

        public String fillUpStrings(string source)
        {
            string myStrings = this.fillUpStrings(source, "", "");            
            return myStrings;
        }

        public String fillUpStrings(string source, String pre, String post)
        {
            return this.fillUpVars(source, this.globalRenameHash, pre, post);
        }

        public String fillUpCodeLines(string source)
        {
            return this.fillUpVars(source, this.namedSubScripts);
        }

        public String fillUpMaths(string source)
        {
            return this.fillUpVars(source, this.calcingBracketsResults);
        }

        public String fillUpVars(string source, Hashtable useThis)
        {
           return this.fillUpVars(source, useThis, "", "");
        }


        // check if the variable exists from the given object

        public Boolean varExists(string name)
        {
            return this.globalRenameHash.ContainsKey("&"+name);
        }

        public Boolean varExists(ReflectionScriptDefines refObj)
        {
            if (refObj.isParentAssigned)
            {
                if (Parent != null)
                {
                    return this.Parent.varExists(refObj.name);
                }
                else
                {
                    this.addError("There is no Parent. use PARENT in subscript only");
                    return false;
                }
            }
            else
            {
                return this.varExists(refObj.name);
            }
        }

        /**
         * fills up all placeHolder
         * with content from assigned Hashtable
         */
        public String fillUpVars(string source, Hashtable useThis, string pre, string post)
        {
            string newStr = this.fillUpVarsBack(source, useThis, pre, post );
            string chkStr = source;
            while (newStr != source)
            {
                source = newStr;
                newStr = fillUpVarsBack(source, useThis, pre , post);
            }
            return newStr;
        }

        /**
         * fills placeholder recursiv
         */
        public String fillUpVarsBack(string source, Hashtable useThis, string pre, string post)
        {
            string newSrc = source;
            foreach (DictionaryEntry de in useThis)
            {                
               newSrc = newSrc.Replace(de.Key.ToString(), pre + de.Value.ToString() + post);
            }
            return newSrc;
        }

        public String getCodeByName(string name)
        {
            if (namedSubScripts.ContainsKey(name))
            {
                return this.fillUpStrings( namedSubScripts[name].ToString(),"\"","\"");
            }
            return null;
        }

        public Hashtable getAllStrings()
        {
            return this.globalRenameHash;
        }

        public void updateExistingObject(String name, Object value)
        {
            this.updateExistingObject(name, value, false);
        }

        public Boolean objectExists(string name)
        {
            return this.objectStorage.ContainsKey(name);
        }

        public Hashtable getObjects()
        {
            return this.objectStorage;
        }

        public void updateSubScripts()
        {
            foreach (DictionaryEntry Obj in this.getObjects())
            {
                string name = Obj.Key.ToString();
                Object obj = Obj.Value;
                if (this.objectReferences.ContainsKey(name))
                {
                    string type = this.objectReferences[name].ToString();
                    foreach (DictionaryEntry scrpt in this.subScripts)
                    {
                        ReflectionScript refScr = (ReflectionScript)scrpt.Value;
                        if (refScr.objectExists("parent." + name))
                        {
                            refScr.updateExistingObject("parent." + name, obj);
                        }
                        else
                        {
                            refScr.createObject("parent." + name, obj, type);
                        }
                    }
                }
            }
        }

        public void updateSubScripts(ReflectionScript refScr)
        {
            foreach (DictionaryEntry Obj in this.getObjects())
            {
                string name = Obj.Key.ToString();
                Object obj = Obj.Value;
                if (this.objectReferences.ContainsKey(name))
                {
                    string type = this.objectReferences[name].ToString();

                    if (refScr.objectExists("parent." + name))
                    {
                        refScr.updateExistingObject("parent." + name, obj);
                    }
                    else
                    {
                        refScr.createObject("parent." + name, obj, type);
                    }

                    refScr.updateMeByObject(obj);

                }               
            }
        }

        public void createObject(string name, Object obj, string type)
        {
            if (!objectStorage.ContainsKey(name))
            {
                objectStorage.Add(name, obj);               
            }
            if (!objectReferences.ContainsKey(name))
            {
                objectReferences.Add(name, type);
            }
        }

        public void updateExistingObject(String name, Object value, Boolean selfInc)
        {

            Boolean found = false;
            if (globalRenameHash.ContainsKey("&" + name))
            {
                found = true;
                if (selfInc)
                {
                    globalRenameHash["&" + name] += value.ToString();
                }
                else
                {
                    globalRenameHash["&" + name] = value.ToString();
                }
                if (this.debugMode)
                {
                    this.updateDebugMessage("&" + name, globalRenameHash["&" + name].ToString());
                }
            }


            if (objectStorage.ContainsKey(name) && this.objectStorage.ContainsKey(name))
            {
                found = true;
                if (!selfInc)
                {
                    this.objectStorage[name] = value;                   
                }
                else
                {
                    if (value is String && this.objectStorage[name] is String)
                    {
                        String store = (string) this.objectStorage[name];
                        store += value;
                        this.objectStorage[name] = value;
                    }
                    else if (value is int && this.objectStorage[name] is int)
                    {
                        int store = (int)this.objectStorage[name];
                        store += (int)value;
                        this.objectStorage[name] = value;
                    }
                    else
                    {
                        this.addError("this type can not be incremented");
                        found = false;
                    }
                }

                if (SetupBoolValue(ReflectionScript.SETUP_GLOBAL))
                {
                    foreach (DictionaryEntry scrpt in this.subScripts)
                    {
                        ReflectionScript refScr = (ReflectionScript)scrpt.Value;
                        if (refScr.objectExists("parent." + name))
                        {
                            refScr.updateExistingObject("parent." + name, this.objectStorage[name]);
                        }
                    }
                }


                if (this.debugMode)
                {
                    this.updateDebugMessage(name, value.ToString());
                }
            }
          
            if (found == false)
            {               
                this.addError("variable &" + name + " not existing/writable");
            }
        }


        public void updateVarByObject(String name, Object obj)
        {
            this.updateVarByObject(name, obj, false);
        }

        public void updateVarByObject(String name,Object obj, Boolean selfInc)
        {

            this.updateExistingObject(name, obj, selfInc);
           
        }


        public void updateVarByMath(String name, RefScrMath math)
        {
            this.updateExistingObject(name, math.getResult());
            
        }

        public void updateVarByMath(String name, RefScrMath math, Hashtable varTable)
        {
            
            if (varTable.ContainsKey("&" + name))
            {
                varTable["&" + name] = math.getResult();
            }
        }


       // ---------------

        public List<int> getEmptyLines()
        {
            return this.emptyLines;
        }


        public double getVarNumber(string name)
        {
            if (this.int32Founds.ContainsKey(name))
            {
                return (double)this.int32Founds[name];
            }
            return 0;
        }

        public string getVarNumberAsString(string name)
        {
            if (this.int32Founds.ContainsKey(name))
            {
                return this.int32Founds[name].ToString();
            }
            return "0";
        }

        // ---- inline debuginfos following ------------


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

            if (tmpCode.scriptParameters != null)
            {
                info += " (";
                string add = "";
                foreach (String param in tmpCode.scriptParameters)
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
            if (tmpCode.scriptParameters != null)
            {
                
                
                foreach (String param in tmpCode.scriptParameters)
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

        // --------------- creat, assign and update vars -------------------------------------------------------

        public void createOrUpdateStringVar(string name, string value)
        {

            if (this.debugMode)
            {
                this.updateDebugMessage( name, value );
            }

            if (this.globalRenameHash.ContainsKey(name))
            {
                this.globalRenameHash[name] = value;
            }
            else
            {
                this.globalRenameHash.Add(name, value);
            }

            if (SetupBoolValue(ReflectionScript.SETUP_GLOBAL))
            {
                foreach (DictionaryEntry  scrpt in this.subScripts)
                {
                    ReflectionScript refScr = (ReflectionScript)scrpt.Value;
                    refScr.createOrUpdateStringVar("&parent." + name, value);
                }
            }
        }



        // -------------------  here are all the stuff for building, parsing and so on -------------------------- 

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

            System.Guid  guid = System.Guid.NewGuid();
            guid.ToString();
            for (int i = 0; i < match.Count; i++)
            {
                string str = match[i].Value;
                string key = "%str_" + i + "%" + guid.ToString(); ;
                globalRenameHash.Add(key, str.Replace("\"",""));
                globalParamRenameHash.Add(key, str);
                code = code.Replace(str, key);
            }

            

            // get all subelemets by brackets {} independend from what kind of usage
            // just cut it out, add an marker for later usage
            //MatchCollection bracketMatch = Regex.Matches(this.code, Projector.ReflectionScript.REGEX_BRACKETS);
            List<string> bracketMatch = RegexGroup.getMatch(this.code, 1);
            int codeIdent = 1;
            while (bracketMatch.Count > 0)
            {
                for (int i = bracketMatch.Count - 1; i >= 0; i--)
                {
                    codeIdent++;
                    string str = bracketMatch[i];
                    string key = "%subscr_" + codeIdent + "%";
                    //globalRenameHash.Add(key, str);
                    namedSubScripts.Add(
                            key,
                            str
                        );

                    code = code.Replace("{" + str + "}", key);
                    globalParamRenameHash.Add(key, str);
                    /*    
                        int lastCut = str.LastIndexOf('}');                
                        string nCode = str.Substring(1, lastCut - 1);
                        namedSubScripts.Add(
                                key, 
                                nCode
                            );
                
                        code = code.Replace("{" + nCode + "}", key);
                        globalParamRenameHash.Add(key, nCode);
                     */
                }
                bracketMatch = RegexGroup.getMatch(this.code, 1);
            }
            // all content that in normal brackets, that will be used for calculations
            MatchCollection cBracketsMatch = Regex.Matches(this.code, Projector.ReflectionScript.REGEX_CALC_BRACKETS);
            for (int i = 0; i < cBracketsMatch.Count; i++)
            {
                string str = cBracketsMatch[i].Value;
                string key = "%cbracket_" + i + "%";                
                code = code.Replace(str, key);
                string nCode = str.Substring(1, str.Length - 2);

                RefScrMath math = new RefScrMath(this, key, nCode);
                
                this.calcingBrackets.Add(key, math);

                this.calcingBracketsResults.Add(key, math.getResult());
                globalParamRenameHash.Add(key, str);
                
            }

            //lines = Regex.Split(code.Replace(";", System.Environment.NewLine), System.Environment.NewLine);
            this.lines = Regex.Split(code, "\n");
            return true;
        }

        private void build()
        {
            int lineNr = 0;
            emptyLines.Clear();

            foreach (String currentLine in this.lines)
            {
                this.currentReadLine = lineNr;   
                string[] words = currentLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (words.Count() > 0)
                {
                    ReflectionScriptDefines buildRes = this.buildDefine(words);
                    if (null != buildRes && this.validate(buildRes))
                    {
                        this.buildedSource.Add(buildRes);
                    }
                }
                else
                {
                    emptyLines.Add(lineNr);
                }
                lineNr++;
            }
        }

        public void recalcBrackets(ReflectionScriptDefines testObj)
        {
           foreach (string tparam in testObj.scriptParameters)
           {
                if (this.calcingBrackets.ContainsKey(tparam))
                {
                    RefScrMath mathObj = (RefScrMath)this.calcingBrackets[tparam];
                    mathObj.calc();
                    if (testObj.isParentAssigned)
                    {
                        // update for the parent script
                        if (this.Parent != null)
                        {                            
                            this.Parent.updateVarByMath(testObj.name, mathObj);
                        }
                        else
                        {
                            this.addError("There is no Parent Instance");
                        }
                    }
                    else
                    {
                        // update for the current script
                        this.updateVarByMath(testObj.name, mathObj);
                    }
                }
                else
                {
                    if (testObj.isParentAssigned)
                    {
                        if (this.Parent != null)
                        {
                            this.Parent.updateVarByObject(testObj.name, 
                                    this.fillUpAll( 
                                       tparam
                                    ),
                                    testObj.isSelfInc
                                );
                        }
                        else
                        {
                            this.addError("There is no Parent Instance");
                        }
                    }
                    else
                    {
                        this.updateVarByObject(testObj.name, this.fillUpAll(tparam), testObj.isSelfInc);
                    }
                }
                        
                 
            }
                
        }

        private void updateStringsByHashtable(ReflectionScriptDefines testObj, Hashtable hTable)
        {
            foreach (DictionaryEntry strVar in hTable)
            {
                string localVarname = "&" + testObj.name + "." + strVar.Key.ToString();
                string strValue = "";
                if (strVar.Value != null)
                {
                    strValue = strVar.Value.ToString();
                }


                if (!varExists(localVarname))
                {
                    this.createOrUpdateStringVar(localVarname, strValue);
                }

                if (this.Parent != null && this.ParentOwner != null && this.ParentOwner.namedReference != null)
                {
                    string parenVarName = "&" + this.ParentOwner.namedReference + "." + testObj.name + "." + strVar.Key.ToString();
                    if (!this.Parent.varExists(parenVarName))
                    {
                        this.Parent.createOrUpdateStringVar("&" + this.ParentOwner.namedReference + "." + testObj.name + "." + strVar.Key.ToString(), strValue);
                    }
                }

            }
        }

        public void updateMeByObject(ReflectionScriptDefines  testObj)
        {
            ObjectInfo obInfo = new ObjectInfo();
            List<string> objMethods = obInfo.getObjectInfo(testObj.ReflectObject);
            if (objMethods != null)
            {
                foreach (string maskStr in objMethods)
                {
                    this.mask.Add(maskStr);
                }
                if (obInfo.lastObjectInfo != null)
                {
                    updateStringsByHashtable(testObj, obInfo.lastObjectInfo.Strings);
                    updateStringsByHashtable(testObj, obInfo.lastObjectInfo.Integers);
                    updateStringsByHashtable(testObj, obInfo.lastObjectInfo.Booleans);                
                }
            }
        }

        public void updateMeByObject(Object obj)
        {
            ObjectInfo obInfo = new ObjectInfo();
            List<string> objMethods = obInfo.getObjectInfo(obj);
            if (objMethods != null)
            {
                foreach (string maskStr in objMethods)
                {
                    this.mask.Add(maskStr);
                }               
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


            if (testObj.isAssignement && testObj.name != null)
            {
                // for any assignement a variable must be exists and allready defined
                if (this.varExists(testObj))
                {
                    foreach (string tparam in testObj.scriptParameters)
                    {
                        if (this.calcingBrackets.ContainsKey(tparam))
                        {
                            RefScrMath mathObj = (RefScrMath)this.calcingBrackets[tparam];
                            mathObj.calc();
                            if (testObj.isParentAssigned)
                            {
                                // update for the parent script
                                if (this.Parent != null)
                                {
                                    this.Parent.updateVarByMath(testObj.name, mathObj);
                                }
                                else
                                {
                                    this.addError("There is no Parent Instance");
                                }
                            }
                            else
                            {
                                // update for the current script
                                this.updateVarByMath(testObj.name, mathObj);
                            }
                        }
                        else
                        {

                        }
                        
                 
                    }
                }                

                if (testObj.isMethod)
                {
                    
                }

            }

            // setups 
            if (testObj.isVariable && testObj.isSetup && testObj.name != null && testObj.scriptParameters.Count() == 1)
            {
                string type = testObj.scriptParameterTypes[0];
                string var = testObj.scriptParameters[0];
                Object obVar = this.getParamValue(type, var);
                this.addSetup(testObj.name, obVar);
            }


            // variable assinements
            if (testObj.isVariable && !testObj.isSetup)
            {
                if (testObj.name != null && testObj.scriptParameters.Count() == 1)
                {
                    string name = this.fillUpStrings(testObj.name);
                    int readPos =0;
                    foreach (string varValue in testObj.scriptParameters)
                    {
                        string varName = testObj.name;
                        readPos++;
                        string type = "?";
                        if (testObj.scriptParameterTypes.Count() > readPos)
                        {
                            type = testObj.scriptParameterTypes[readPos];
                        }

                        string varStr = this.fillUpStrings(varValue);
                      
                        if (globalRenameHash.ContainsKey("&" + varName) && !testObj.isSetup)
                        {
                            this.addError("variable " + varName + " allready defined");
                        }
                        else
                        {
                          
                            globalRenameHash.Add("&" + testObj.name, varStr);

                            if (this.SetupBoolValue(ReflectionScript.SETUP_GLOBAL) && this.Parent != null)
                            {
                                this.Parent.createOrUpdateStringVar("&" + testObj.name, varStr);
                            }

                            if (type == "INT" || type == "Int32" || type == "Decimal")
                            {
                                int intValue = 0;
                                try
                                {
                                    //this.int32Founds.Add(testObj.name, Int32.Parse(varStr));
                                    intValue = Int32.Parse(varStr);

                                }
                                catch (Exception ex)
                                {
                                    this.addError(ex.Message);
                                }

                                RefScrVariable newVar = new RefScrVariable();
                                newVar.Name = testObj.name;
                                newVar.Value = intValue;
                                newVar.referenceOf = null;
                                newVar.TypeName = "INT";

                                this.int32Founds.Add(newVar.Name, newVar);
                                
                            }
                            
                        }
                        

                        
                    }                    
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
                string keyForObj = testObj.typeOfObject + "." + testObj.name;
                if (testObj.isObject && cmd == "NEW")
                {
                    if (!this.parsedObjects.Contains(keyForObj))
                    {
                        ReflectNew reflector = new ReflectNew();
                        Object testObject = reflector.getObject(testObj, this);
                        if (testObject != null)
                        {
                            testObj.ReflectObject = testObject;


                            //this.objectStorage.Add(testObj.name,testObject);
                            this.createObject(testObj.name, testObject, testObj.typeOfObject);

                            this.updateMeByObject(testObj);
                            this.parsedObjects.Add(keyForObj);
                        }
                        else
                        {
                            this.addError("Object " + keyForObj + " can not be created. Check Name");
                        }
                    }
                    else
                    {
                        this.addError("Object " + keyForObj + " was already created");
                    }
                }
            }

            // just something to do

            // this opbject is parseable so it must have some part of code as param
            if (testObj.parseable)
            {
                if (testObj.scriptParameters.Count < 1)
                {
                    this.addError("Missing Code Exception: " + testObj.code);
                }
                else if (this.followRecusives)
                {
                    testObj.parameters = new List<object>();
                    foreach (String subCode in testObj.scriptParameters)
                    {                        
                        string fullCode = this.getCodeByName(subCode);
                        testObj.subScript = new ReflectionScript();
                        testObj.subScript.Parent = this;
                        testObj.subScript.ParentOwner = testObj;
                        testObj.subScript.parentLineNumber = this.getLineNumber();

                        if (this.SetupBoolValue(ReflectionScript.SETUP_GLOBAL))
                        {
                            foreach (DictionaryEntry parScr in this.globalRenameHash)
                            {
                                testObj.subScript.globalRenameHash.Add("&parent." + parScr.Key.ToString(), parScr.Value);
                            }
                            updateSubScripts(testObj.subScript);
                        }

                        testObj.subScript.setCode(fullCode);
                        
                        updateDebugInfo(testObj.subScript);

                        if (testObj.namedReference != null) {
                            if (this.subScripts.ContainsKey(testObj.namedReference))
                            {
                                System.Guid uuid = System.Guid.NewGuid();
                                this.subScripts.Add(testObj.namedReference + uuid.ToString(), testObj.subScript);
                            }
                            else
                            {
                                this.subScripts.Add(testObj.namedReference, testObj.subScript);
                            }

                        }
                        testObj.parameters.Add(testObj.subScript);

                       

                        if (testObj.subScript.getErrorCount() > 0)
                        {
                            //this.addError("Invalid Code in subLogic: " + testObj.code);
                            foreach (ScriptErrors err in testObj.subScript.getAllErrors())
                            {
                                // add my own line number
                                err.lineNumber += this.getLineNumber();
                                this.addError(err);

                            }
                        }
                    }
                }
            }

            // check how many lines of sourcecode is used to write this. so check strings because this is
            // the first valid source of linebreaks

            if (testObj.scriptParameters.Count > 0)
            {
                string parFull = "";
                foreach (String subCode in testObj.scriptParameters)
                {
                   // parFull += this.fillUpAll(subCode);
                    foreach (DictionaryEntry rp in this.globalParamRenameHash)
                    {
                        parFull += subCode.Replace(rp.Key.ToString(), rp.Value.ToString());
                    }
                        

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


        private Object getParamValue(string type, string part)
        {
            Object retValue = null;
            switch (type)
            {
                case "String": case "STR":
                    retValue = this.fillUpAll(part);
                    break;
                case "INT": case "Int32":
                    try
                    {
                        string tmpStr = this.fillUpAll(part);
                        int parInt = int.Parse(tmpStr);
                        retValue = parInt;
                    }
                    catch (Exception e)
                    {
                        this.addError("Parameter must be an number" + e.Message);
                        return null;
                    }
                    break;
                case "Decimal":
                
                    try
                    {
                        string tmpStr = this.fillUpAll(part);
                        Decimal parInt = Decimal.Parse(tmpStr);
                        retValue = parInt;
                    }
                    catch (Exception e)
                    {
                        this.addError("Parameter must be an Decimal number" + e.Message);
                        return null;
                    }
                    break;
                case "Boolean":
                case "Bool":
                    if (part.ToUpper() == "TRUE" || part == "1")
                    {
                        retValue = true;
                    }
                    else
                    {
                        retValue = false;
                    }
                    break;
                case "ReflectionScript":
                    ReflectionScript tmpSript = new ReflectionScript();
                    tmpSript.setCode(this.fillUpCodeLines(part));
                    retValue = tmpSript;
                    break;
                default :
                    if (this.objectStorage.ContainsKey(part))
                    {
                        retValue = this.objectStorage[part];
                    }
                    break;
            }
            return retValue;
        }

        public Object getObjectForParam(string param)
        {
            
            string[] pars = param.Split('.');
            if (pars.Count() > 1 && pars[0].ToLower() == "parent" && this.Parent != null)
            {
                string chk = "";
                for (int i = 1; i < pars.Count(); i++)
                {
                    chk += pars[i];
                }
                return this.Parent.getObjectForParam(chk);
            }

            if (this.objectStorage.ContainsKey(param))
            {               
                return this.objectStorage[param];
                
            }
            return param;

        }


        private Object upateParamValue(Object param, string type, string defined)
        {
            Object tmpObject = param;
            switch (type)
            {
                case "String":
                case "STR":
                       tmpObject = this.fillUpAll(defined);
                    break;
                case "?":
                    if (param is string)
                    {
                       tmpObject = this.fillUpAll(defined);
                       return getObjectForParam(tmpObject.ToString());     
                    }
                    
                    break;
                case "ListView":
                    return getObjectForParam(this.fillUpAll(defined)); 
                    break;

            }
            
            return tmpObject;
        }


        /**
         * upateing the parameters with current State
         */

        public void updateParam(ReflectionScriptDefines cmdResult)
        {
            this.updateParam(cmdResult, false);

        }
        public void updateParam(ReflectionScriptDefines cmdResult, Boolean setOrigin)
        {

            for (int i = 0; i < cmdResult.parameters.Count; i++ )
            {
                string type = cmdResult.scriptParameterTypes[i];
                string defined = cmdResult.scriptParameters[i];
                cmdResult.parameters[i] = this.upateParamValue(cmdResult.parameters[i], type, defined);

                if (setOrigin && cmdResult.name != null)
                {
                    this.updateExistingObject(cmdResult.name, cmdResult.parameters[i]);
                }

                if (this.debugMode && cmdResult.parameters[i] != null)
                {
                    this.updateDebugMessage("PARAM UPD:" + type, cmdResult.parameters[i].ToString());
                }
            }

            /*
            cmdResult.parameters.Clear();
            int i=0;
            foreach (string pType in cmdResult.scriptParameterTypes)
            {
                cmdResult.parameters.Add(this.getParamValue(pType, cmdResult.scriptParameters[i]));
                i++;
            }
            */
        }

       




        /**
         * work on parameters
         * 
         */ 
        private ReflectionScriptDefines setParams(ReflectionScriptDefines cmdResult, int partPosition, String[] varDefines, String part)
        {
            cmdResult.scriptParameters.Add(part);

            if (varDefines != null && varDefines.Count() > partPosition)
            {
                cmdResult.scriptParameterTypes.Add(varDefines[partPosition]);
                string definedType = varDefines[partPosition];
                if (definedType == "INT" || definedType == "Int32")
                {
                    string replaced = this.fillUpStrings(part);

                    try
                    {
                        int parInt = int.Parse(replaced);
                        cmdResult.parameters.Add(parInt);
                    }
                    catch (Exception e)
                    {
                        this.addError(e.Message);
                        return null;
                    }

                }
                else if (definedType == "Decimal")
                {
                    string replaced = this.fillUpStrings(part);
                    try
                    {
                        Decimal parInt = Decimal.Parse(replaced);
                        cmdResult.parameters.Add(parInt);
                    }
                    catch (Exception e)
                    {
                        this.addError("Parameter must be an Decimal Number" + e.Message);
                        return null;
                    }

                }
                else if (definedType == "STR" || definedType == "String")
                {
                    cmdResult.parameters.Add(this.fillUpStrings(part));
                }
                else if (definedType == "BOOL" || definedType == "Boolean")
                {
                    string replaced = this.fillUpStrings(part);
                    if (replaced.ToUpper() == "TRUE" || replaced == "1")
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
                else if (definedType == "?")
                {
                    cmdResult.parameters.Add(this.fillUpStrings(part));
                }
                else
                {
                    if (this.objectStorage.ContainsKey(part))
                    {
                        cmdResult.parameters.Add(this.objectStorage[part]);
                    }
                    else
                    {
                        cmdResult.parameters.Add(getObjectForParam(part));
                    }
                }
            }
            else
            {
                cmdResult.scriptParameterTypes.Add("?");
            }
            return cmdResult;
        }

        private ReflectionScriptDefines buildDefine(string[] words)
        {

            if (words.Count() > 0 && words[0][0] == '#')
            {                
                this.commentedLines.Add(this.getLineNumber());
                return null;
            }

           // Boolean foundMatch = false;
            // look on any mask
            int bestMatchFound = 0;
            foreach (string hash in this.mask)
            {

                RefScriptMaskMatch maskMatch = new RefScriptMaskMatch(hash, this);
                int bestmatch = maskMatch.possibleMatch(words);
                if (bestMatchFound < bestmatch)
                {
                    bestMatchFound = bestmatch;
                }

                if (bestmatch != RefScriptMaskMatch.MATCH)
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
                            cmdResult.isAssignement = definePart.Contains("ASSIGN");
                            cmdResult.isVariable = definePart.Contains("VAR");
                            cmdResult.isSelfInc = definePart.Contains("SELFINC");
                            cmdResult.isSelfDec = definePart.Contains("SELFDEC");
                            cmdResult.isSetup = definePart.Contains("SETUP");
                            cmdResult.isParentAssigned = definePart.Contains("PARENT");

                            if (definePart.Contains("WAIT"))
                            {
                                cmdResult.setState = RefScriptExecute.STATE_WAIT;
                            }

                            if (definePart.Contains("RUN"))
                            {
                                cmdResult.setState = RefScriptExecute.STATE_RUN;
                            }

                            if (definePart.Contains("EXIT"))
                            {
                                cmdResult.setState = RefScriptExecute.STATE_FINISHED;
                            }



                            cmdResult.code = highPart;
                            cmdResult.originCode = part;


                            cmdResult.parameters = new List<object>();
                        }
                        // here we found an parameter. so we reed what type is expected;
                        else if (cmdResult != null && currentMaskPart == "?")
                        {
                            ReflectionScriptDefines cmdTmpResult = this.setParams(cmdResult, partPosition, varDefines, part);
                            if (cmdTmpResult == null)
                            {
                                return null;
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
            if (bestMatchFound == RefScriptMaskMatch.MAYBE_MATCH)
            {
                this.addError("not sure what this should be");
            }
            else if (bestMatchFound == RefScriptMaskMatch.PARTIAL_MATCH)
            {
                this.addError("incomplete");
            }
            else
            {
                this.addError("INVALID");
            }
            

            return null;
        }


    }
}
