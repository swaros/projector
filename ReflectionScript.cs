using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using Projector.Script.Vars;


namespace Projector.Script
{
    public class ReflectionScript
    {
        /// <summary>
        /// INTEGER Set the maximum time in seconds any WAITFOR will waiting
        /// </summary>
        public const string SETUP_MAXWAIT = "MAX_WAIT";
        /// <summary>
        /// BOOLEAN enables parsing parent objects by parent. mofifier
        /// </summary>
        public const string SETUP_GLOBAL = "GLOBAL";
        /// <summary>
        /// STRING sets the label for Script widgets
        /// </summary>
        public const string SETUP_LABEL = "LABEL";
        /// <summary>
        /// STRING sets the Descrition for Script Widgets
        /// </summary>
        public const string SETUP_DESC = "DESCRIPTION";
        /// <summary>
        /// BOOLEAN enable on-the-fly creation of visual objects
        /// </summary>
        public const string SETUP_PREVIEW = "CODE.PREVIEW";

        /// <summary>
        /// brackets to get code DEPRECATED
        /// </summary>
        private const string REGEX_BRACKETS = "({[^}]*})";
        private const string REGEX_CALC_BRACKETS = @"(\([^\)]*\))";
       // private const string REGEX_CALC_BRACKETS = "((.+)\\)/U";
        private const string REGEX_STRING = "\"([^\"]*)\"";

        public const string MASK_DELIMITER = "#";

        /// <summary>
        /// the token for the script in the current scope and all assigned subscripts
        /// used as key for all created objects
        /// </summary>
        private String scopeToken;

        /// <summary>
        /// name for this script
        /// </summary>
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

        public Variables scrVars = new Variables();
        
        /// <summary>
        /// Reflection script
        /// </summary>
        public ReflectionScript()
        {
            this.init();
        }

        
        /// <summary>
        /// initialize basics.
        /// creates base commands and set
        /// base environment variables and base default settings
        /// </summary>
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
             * STR_OBJECT create a Object of type string
             * INT_OBJECT create a Object from type Int
             * 
             */

            //base commands
            this.mask.Add("NEW § %" + Projector.Script.ReflectionScript.MASK_DELIMITER + "OBJECT");
            this.mask.Add("PROCEDURE % ?" + Projector.Script.ReflectionScript.MASK_DELIMITER + "PARSE");
            this.mask.Add("EXEC ? ?" + Projector.Script.ReflectionScript.MASK_DELIMITER + "PARSE");    

            // hard coded native stufff
            this.mask.Add("MESSAGEBOX ?" + Projector.Script.ReflectionScript.MASK_DELIMITER);
            this.mask.Add("REG ?" + Projector.Script.ReflectionScript.MASK_DELIMITER);
            this.mask.Add("UNREG ?" + Projector.Script.ReflectionScript.MASK_DELIMITER);
            this.mask.Add("WAITFOR ?" + Projector.Script.ReflectionScript.MASK_DELIMITER);

            //this.mask.Add("VAR % = ?" + Projector.Script.ReflectionScript.MASK_DELIMITER + "VAR OBJECT" + Projector.Script.ReflectionScript.MASK_DELIMITER + "var . = STR");
            this.mask.Add("STRING % = ?" + Projector.Script.ReflectionScript.MASK_DELIMITER + "VAR OBJECT" + Projector.Script.ReflectionScript.MASK_DELIMITER + "string . = STR");
            this.mask.Add("INTEGER % = ?" + Projector.Script.ReflectionScript.MASK_DELIMITER + "VAR OBJECT" + Projector.Script.ReflectionScript.MASK_DELIMITER + "integer . = INT");
            this.mask.Add("DOUBLE % = ?" + Projector.Script.ReflectionScript.MASK_DELIMITER + "VAR OBJECT" + Projector.Script.ReflectionScript.MASK_DELIMITER + "double . = Double");

            this.mask.Add("% = ?" + Projector.Script.ReflectionScript.MASK_DELIMITER + "ASSIGN" + Projector.Script.ReflectionScript.MASK_DELIMITER + ". = ?");
            this.mask.Add("% += ?" + Projector.Script.ReflectionScript.MASK_DELIMITER + "ASSIGN SELFINC" + Projector.Script.ReflectionScript.MASK_DELIMITER + ". += ?");

            // environment change
            this.mask.Add("SET % ?" + Projector.Script.ReflectionScript.MASK_DELIMITER + "VAR SETUP");


            // access to parent if exists
            this.mask.Add("PARENT % = ?" + Projector.Script.ReflectionScript.MASK_DELIMITER + "ASSIGN PARENT" + Projector.Script.ReflectionScript.MASK_DELIMITER + "PARENT . = ?");
            this.mask.Add("PARENT % += ?" + Projector.Script.ReflectionScript.MASK_DELIMITER + "ASSIGN PARENT SELFINC" + Projector.Script.ReflectionScript.MASK_DELIMITER + "PARENT . += ?");

           // this.mask.Add("DOWN % = ?" + Projector.Script.ReflectionScript.MASK_DELIMITER + "ASSIGN DOWNCOPY" + Projector.Script.ReflectionScript.MASK_DELIMITER + "PARENT . = ?");

            // exection controlls
            this.mask.Add("STOP" + Projector.Script.ReflectionScript.MASK_DELIMITER + "WAIT" + Projector.Script.ReflectionScript.MASK_DELIMITER + "STOP");
            this.mask.Add("RUN" + Projector.Script.ReflectionScript.MASK_DELIMITER + "RUN" + Projector.Script.ReflectionScript.MASK_DELIMITER + "RUN");
            this.mask.Add("EXIT" + Projector.Script.ReflectionScript.MASK_DELIMITER + "EXIT" + Projector.Script.ReflectionScript.MASK_DELIMITER + "EXIT");

            this.mask.Add("PARENT STOP" + Projector.Script.ReflectionScript.MASK_DELIMITER + "WAIT PARENT" + Projector.Script.ReflectionScript.MASK_DELIMITER + "PARENT STOP");
            this.mask.Add("PARENT RUN" + Projector.Script.ReflectionScript.MASK_DELIMITER + "RUN PARENT" + Projector.Script.ReflectionScript.MASK_DELIMITER + "PARENT RUN");
            this.mask.Add("PARENT EXIT" + Projector.Script.ReflectionScript.MASK_DELIMITER + "EXIT PARENT" + Projector.Script.ReflectionScript.MASK_DELIMITER + "PARENT EXIT");

            // just to store something
            // this.mask.Add("VAR § SET ?=STRINGVAR STR");
        }
        
        /// <summary>
        /// creates default settings
        /// </summary>
        private void initDefaultSetups()
        {
            this.addSetupIfNotExists(ReflectionScript.SETUP_MAXWAIT, 0); // max wait 10 seconds for finishing threads
            this.addSetupIfNotExists(ReflectionScript.SETUP_GLOBAL, true);
            
        }

        /// <summary>
        /// add a setup with with an key and content
        /// if this setup not already exists.
        /// If this setup still present, the content
        /// will not be updated
        /// </summary>
        /// <param name="name">keyname for this setting</param>
        /// <param name="value">the content for this setup</param>
        public void addSetupIfNotExists(string name, Object value)
        {
            if (this.Setup.ContainsKey(name))
            {                
                return;
            }

            this.Setup.Add(name, value);
        }

        /// <summary>
        /// add an setup if not exists
        /// or update existing settings
        /// </summary>
        /// <param name="name">keyname for these setting</param>
        /// <param name="value">value for these setting</param>
        private void addSetup(string name, Object value)
        {
            if (this.Setup.ContainsKey(name))
            {
                this.Setup[name] = value;
                return;
            }

            this.Setup.Add(name, value);
        }

        /// <summary>
        /// get a setup content as Integer.
        /// Note: there is no casting. the content 
        /// will be returned only if the setting
        /// content type is Integer
        /// </summary>
        /// <param name="name">keyname</param>
        /// <returns>the setting content if type is Integer</returns>
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

        /// <summary>
        /// get a setup content as String.
        /// Note: there is no casting. the content 
        /// will be returned only if the setting
        /// content type is String
        /// </summary>
        /// <param name="name">keyname</param>
        /// <returns>the setting content if the content is string</returns>
        public String SetupStringValue(string name)
        {
            if (this.Setup.ContainsKey(name))
            {
                if (this.Setup[name] is String)
                {
                    return this.Setup[name].ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// get a setup content as Boolean.
        /// Note: there is no casting. the content 
        /// will be returned only if the setting
        /// content type is Boolean
        /// </summary>
        /// <param name="name">keyname</param>
        /// <returns>the setting content if the content is Boolean</returns>
        public Boolean SetupBoolValue(string name)
        {
            if (this.Setup.ContainsKey(name))
            {
                if (this.Setup[name] is Boolean)
                {
                    return (Boolean)this.Setup[name];
                }
                if (this.Setup[name] is String && this.Setup[name].ToString().ToUpper() == "TRUE")
                {
                    return true;
                }
            }
            return false;
        }

        // -------------------- end setup stuff ----------------------

        /// <summary>
        /// returns compiled script
        /// </summary>
        /// <returns></returns>
        public List<ReflectionScriptDefines> getScript()
        {
            return this.buildedSource;
        }

        /// <summary>
        /// returns compiled script including
        /// all this.scrVars.subScripts
        /// </summary>
        /// <returns></returns>
        public List<ReflectionScriptDefines> getFullScript()
        {

            List<ReflectionScriptDefines> res = new List<ReflectionScriptDefines>();
            res.AddRange( this.getScript());
            foreach (DictionaryEntry subScr in this.scrVars.subScripts)
            {
                ReflectionScript refScr = (ReflectionScript)subScr.Value;
                List<ReflectionScriptDefines> subSource = refScr.getFullScript();                
                res.AddRange(subSource);
            }

            return res;
        }


        /// <summary>
        /// reset all local containers
        /// and reset default values
        /// </summary>
        private void reset()
        {
            this.commentedLines.Clear();
            this.lineOffset = 0;
            this.errorMessages.Clear();

          
            this.buildedSource.Clear();
        
            this.errorLines.Clear();
            this.scrVars.clear();

            this.parsedObjects.Clear();

            this.setDefaultVars();

        }

        /// <summary>
        /// set default values
        /// </summary>
        private void setDefaultVars()
        {
            this.createOrUpdateStringVar("&PATH.DOCUMENTS", System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonDocuments) + System.IO.Path.DirectorySeparatorChar.ToString());
            this.createOrUpdateStringVar(".PATH.SEP", System.IO.Path.DirectorySeparatorChar.ToString());
            this.createOrUpdateStringVar(".NL.", "\n");
        }


        /// <summary>
        /// injects Reflection script sourcecode
        /// and start JIT if code changed
        /// </summary>
        /// <param name="code">Reflection Script Source Code</param>
        public void setCode(string code)
        {
            this.setCode(code, false);
        }

        /// <summary>
        /// injects Reflection script sourcecode.
        /// </summary>
        /// <param name="code">Reflection Script Source Code</param>
        /// <param name="forceRebuild">Force the build</param>
        public void setCode(string code, Boolean forceRebuild)
        {
            if (this.storedCode != code)
            {
                System.Guid guid = System.Guid.NewGuid();
                this.scopeToken = guid.ToString();
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

        /// <summary>
        /// rebuild the Script. not needed if code is changed
        /// because this case will automatically triggers an rebuild.
        /// use this for changes that depends the runtime environmet
        /// </summary>
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

        public void updateScopeTocken(string token)
        {
            this.scopeToken = token;
        }

        // debugging

        /// <summary>
        /// registers a method from outside, that will be informed about any
        /// executed step til execution
        /// </summary>
        /// <param name="debugWatcher">the Object that handles the message</param>
        /// <param name="varChange">the name of the method</param>
        public void registerDebugWatcher(Object debugWatcher, string varChange)
        {
            this.parentWatcher = debugWatcher;
            this.parentWatcherMethod = varChange;
            this.debugMode = true;
        }

        /// <summary>
        /// send debugging informations to an registered watcher
        /// </summary>
        /// <param name="varName">name of variable</param>
        /// <param name="value">the value as string</param>
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
        /// <summary>
        /// update current watchers to the submited
        /// Reflection Script with the current debug watcher
        /// </summary>
        /// <param name="child">The Reflectionscript that have to register the watcher </param>
        public void updateDebugInfo(ReflectionScript child)
        {
            child.registerDebugWatcher(this.parentWatcher, this.parentWatcherMethod);
        }

        // -------------------- methods to get Infomations about the current Situation of the script ------------

        /// <summary>
        /// returns all existing Instances from the given
        /// Objecttype
        /// </summary>
        /// <param name="objectType">Name of the Object (like ReflectForm)</param>
        /// <returns>A List of all existing Objects from type of Objecttype</returns>
        public List<string> getCurrentObjectsByType(string objectType)
        {
            List<string> usedBy = new List<string>();
            if (this.scrVars.getObjectsReference().ContainsValue(objectType))
            {
               
                foreach (DictionaryEntry de in this.scrVars.getObjectsReference())
                {
                    if (de.Value != null && de.Value.ToString() == objectType)
                    {
                        usedBy.Add(de.Key.ToString());
                    }
                }              
            }
            foreach (DictionaryEntry scrpt in this.scrVars.subScripts)
            {
                ReflectionScript refScr = (ReflectionScript)scrpt.Value;
                List<string> subUsed = refScr.getCurrentObjectsByType(objectType);
                usedBy.AddRange(subUsed);
            }

            return usedBy;
        }



        // -------------------  error handlings, methods for add and get the errors -----------------------------



        
        /// <summary>
        /// return an string with all error informations.
        /// usabel if you have only one string for display
        /// errormessages
        /// </summary>
        /// <returns>All errors in one String</returns>
        public string getErrors()
        {
            string error = "";
            for (int i = 0; i < errorMessages.Count; i++)
            {
                error +=  "line:" + errorMessages[i].lineNumber + "|" + errorMessages[i].errorMessage + System.Environment.NewLine;
            }
            return error;
        }

        /// <summary>
        /// returns all Errors
        /// </summary>
        /// <returns>all Errors as a List of Type ScriptErrors</returns>
        public List<ScriptErrors> getAllErrors()
        {
            List<ScriptErrors> returnList = this.errorMessages;
            return returnList;
        }

        /// <summary>
        /// Add an default parsing Error just by message
        /// </summary>
        /// <param name="Errormessage">The message for the user</param>
        private void addError(String Errormessage)
        {
            ScriptErrors error = new ScriptErrors();
            error.errorCode = 0;
            error.errorMessage = Errormessage;
            error.runtimeError = false;
            error.lineNumber = this.getLineNumber();
            this.errorMessages.Add(error);
        }

        /// <summary>
        /// Add an Error by an ErrorMessage and an ErrorCode
        /// </summary>
        /// <param name="Errormessage">The Message for the User</param>
        /// <param name="ErrorCode">The Code of Error. 0 for unknown</param>
        private void addError(String Errormessage, int ErrorCode)
        {
            ScriptErrors error = new ScriptErrors();
            error.errorCode = ErrorCode;
            error.errorMessage = Errormessage;
            error.lineNumber = this.getLineNumber();
            error.runtimeError = false;
            this.errorMessages.Add(error);
        }

        /// <summary>
        /// Add an Error by ScriptError, so the Error have to full defined
        /// </summary>
        /// <param name="error">The Error Object</param>
        public void addError(ScriptErrors error)
        {
            if (this.errorLines.Contains(error.lineNumber))
            {
                return;
            }
            this.errorLines.Add(error.lineNumber);
            this.errorMessages.Add(error);
        }

        /// <summary>
        /// returns the Full count of all Errors
        /// </summary>
        /// <returns></returns>
    
        public int getErrorCount()
        {
            return this.errorMessages.Count();
        }

        /// <summary>
        /// Returns count of all Errors they are NOT
        /// Runtime errors.
        /// </summary>
        /// <returns></returns>
        public int getNotRuntimeErrorCount()
        {
            int errCount = 0;

            foreach (ScriptErrors err in this.errorMessages)
            {
                if (!err.runtimeError)
                {
                    errCount++;
                }
            }

            return errCount;
        }

        // -------------------------------- end of error handling --------------------------------------       

       /// <summary>
       /// returns an List of linenumbers that contains just comments
       /// </summary>
       /// <returns>List of Line Numbers</returns>
        public List<int> getCommentLines()
        {
            List<int> returnList = this.commentedLines;
            
            foreach (DictionaryEntry subScrDic in this.scrVars.subScripts)
            {
                ReflectionScript subScr = (ReflectionScript)subScrDic.Value;
                int parentOffset = subScr.parentLineNumber;
                List<int> subComments = subScr.getCommentLines();
                int offsets = 0;
                foreach (int subLnr in subComments)
                {
                    returnList.Add(subLnr + parentOffset);
                    offsets = subScr.getLineOffset();
                }
            }

            return returnList;
        }

        /// <summary>
        /// get the numbers of lines that used by strings or subscripts
        /// and not counted as lines of code
        /// </summary>
        /// <returns>the number of lines that used for multilines</returns>
        public int getLineOffset()
        {
            return this.lineOffset;
        }

       /// <summary>
       /// get the current Line Number including 
       /// offset
       /// </summary>
       /// <returns></returns>
        private int getLineNumber()
        {
            return this.currentReadLine + this.lineOffset;
        }

        /// <summary>
        /// Replaces all variables and return the
        /// Result as String. That includes all maths, codelines and
        /// Strings
        /// </summary>
        /// <param name="source">string including placeholders</param>
        /// <returns>string with replaced placeholders</returns>
        public String fillUpAll(string source)
        {
            return 
                this.scrVars.fillUpMaths(
                    this.fillUpCodeLines(
                        this.fillUpStrings(source)
                    )
                );
        }

        /// <summary>
        /// Fills all Placeholder and returns the 
        /// content
        /// </summary>
        /// <param name="source">string with placeholders</param>
        /// <returns>string with replaced placeholders</returns>
        public String fillUpStrings(string source)
        {
            string myStrings = this.fillUpStrings(source, "", "");            
            return myStrings;
        }

        /// <summary>
        /// Fills all PlaceHolder and Returns
        /// the Content
        /// </summary>
        /// <param name="source">String wuth Placeholder</param>
        /// <param name="pre">String that will be added before</param>
        /// <param name="post">String that will be added at the end</param>
        /// <returns>String with replaced Placeholders and the Pre and Poststring</returns>
        public String fillUpStrings(string source, String pre, String post)
        {
            return this.fillUpVars(source,this.scrVars.globalRenameHash, pre, post);
        }
        
        /// <summary>
        /// Returns the full Code of code with placeholders
        /// or an Placeholder himself
        /// </summary>
        /// <param name="source">Placeholder or Code with placeholders</param>
        /// <returns>Full Code</returns>
        public String fillUpCodeLines(string source)
        {
            return this.fillUpVars(source, this.scrVars.namedSubScripts);
        }


        /// <summary>
        /// Fill up Placeholder by the given Hashtable
        /// as source for key and value. The key will be the
        /// name of the Placeholder that will be replaced by the 
        /// value.
        /// This Method works recusiv
        /// </summary>
        /// <param name="source">Source with placeholder</param>
        /// <param name="useThis">Hashtable that contans the keys and values</param>
        /// <returns>return filled source</returns>
        public String fillUpVars(string source, Hashtable useThis)
        {
           return this.fillUpVars(source, useThis, "", "");
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
            if (this.scrVars.namedSubScripts.ContainsKey(name))
            {
                return this.fillUpStrings( this.scrVars.namedSubScripts[name].ToString(),"\"","\"");
            }
            return null;
        }

        public Hashtable getAllStrings()
        {
            Hashtable fullStrings =this.scrVars.globalRenameHash;
            foreach (DictionaryEntry subScr in this.scrVars.subScripts)
            {
                ReflectionScript refScr = (ReflectionScript)subScr.Value;
                foreach (DictionaryEntry subVars in refScr.getAllStrings())
                {
                    if (!fullStrings.ContainsKey(subVars.Key))
                    {
                        fullStrings.Add(subVars.Key, subVars.Value);
                    }
                }
            }
            return fullStrings;
            //returnthis.scrVars.globalRenameHash;
        }
        // check if the variable exists from the given object

        public Boolean varExists(string name)
        {
            return this.scrVars.globalRenameHash.ContainsKey("&" + name);
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
      

        public Hashtable getRuntimeObjects(Boolean alsoIfNotRunning)
        {
            Hashtable result = new Hashtable();
            if (this.CurrentExecuter != null)
            {
                foreach (DictionaryEntry tmpRes in this.CurrentExecuter.getRuntimeObjects())
                {
                    result.Add(tmpRes.Key, tmpRes.Value);
                }

            }
            
            if (alsoIfNotRunning)
            {
               foreach (DictionaryEntry tmpRes in this.scrVars.getObjects())
               {
                   if (!result.ContainsKey(tmpRes.Key))
                   {
                       result.Add(tmpRes.Key, tmpRes.Value);
                   }                   
               }
            }
            
            if (result == null)
            {
                result = new Hashtable();
            }

            foreach (DictionaryEntry scrpt in this.scrVars.subScripts)
            {
                ReflectionScript refScr = (ReflectionScript)scrpt.Value;
                Hashtable subResult = refScr.getRuntimeObjects(alsoIfNotRunning);

                foreach (DictionaryEntry subInfo in subResult)
                {
                    if (!result.ContainsKey(subInfo.Key))
                        result.Add(subInfo.Key, subInfo.Value);
                }
            }


            return result;
        }


        public void updateSubScripts()
        {
            foreach (DictionaryEntry Obj in this.scrVars.getObjects())
            {
                string name = Obj.Key.ToString();
                Object obj = Obj.Value;
                if (this.scrVars.objectIsStored(name))
                {
                    string type = this.scrVars.getObjectsReference(name);
                    foreach (DictionaryEntry scrpt in this.scrVars.subScripts)
                    {
                        ReflectionScript refScr = (ReflectionScript)scrpt.Value;
                        if (refScr.scrVars.objectIsStored("parent." + name))
                        {
                            refScr.scrVars.updateExistingObject("parent." + name, obj);
                        }
                        else
                        {
                            refScr.scrVars.createObject("parent." + name, obj, type);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// checks if an form still visible
        /// so this means the script is still running
        /// </summary>
        /// <returns>true if a form is visible</returns>
        private Boolean testOnOpenForms()
        {
            Hashtable allObjects = this.getRuntimeObjects(false);
            if (allObjects == null)
            {
                return false;
            }

            foreach (DictionaryEntry runObj in allObjects)
            {
                if (runObj.Value is Form)
                {
                    Form testForm = (Form)runObj.Value;
                    if (testForm.Visible)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if any process running.
        /// Checks the states and open Forms.
        /// Running Threads not collected
        /// </summary>
        /// <returns></returns>
        public Boolean imRunning()
        {
            if (this.CurrentExecuter != null)
            {
                if (this.CurrentExecuter.runState != RefScriptExecute.STATE_FINISHED)
                {
                    return true;
                }

                // next check if a form still visible
                if (this.testOnOpenForms())
                {
                    return true;
                }

                // check before not valid so we are in the state FINISHED...BUT waht abeout oure subscripts?
                Boolean somIsRunning = false;
                foreach (DictionaryEntry scrpt in this.scrVars.subScripts)
                {
                    ReflectionScript refScr = (ReflectionScript)scrpt.Value;
                    if (refScr.imRunning())
                    {
                        somIsRunning = true;
                    }
                }
                return somIsRunning;
            }
            return false;
        }

        public void StopAll()
        {
            if (this.CurrentExecuter != null)
            {
                // check before not valid so we are in the state FINISHED...BUT waht abeout oure subscripts?
                foreach (DictionaryEntry scrpt in this.scrVars.subScripts)
                {
                    ReflectionScript refScr = (ReflectionScript)scrpt.Value;
                    refScr.StopAll();
                   
                }

                this.CurrentExecuter.StopNow();
                
            }
        }


        public void updateSubScripts(ReflectionScript refScr)
        {
            foreach (DictionaryEntry Obj in this.scrVars.getObjects())
            {
                string name = Obj.Key.ToString();
                Object obj = Obj.Value;
                if (this.scrVars.objectIsStored(name))
                {
                    string type = this.scrVars.getObjectsReference(name);
                    
                    List<string> aliases = new List<string>();
                    aliases.Add("parent.");
                    aliases.Add("");
                    /*
                    if (this.Parent == null)
                    {
                        aliases.Add(this.name + ".");
                    }
                    else
                    {
                        aliases.Add("");
                    }*/
                    foreach (string addName in aliases)
                    {
                        if (refScr.scrVars.objectIsStored(addName + name))
                        {
                            refScr.scrVars.updateExistingObject(addName + name, obj);
                        }
                        else
                        {
                            refScr.scrVars.createObject(addName + name, obj, type);
                        }
                    }
                    refScr.updateMeByObject(obj);

                }               
            }
        }

       


      

      

       // ---------------

        public List<int> getEmptyLines()
        {
            return this.emptyLines;
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

            if (this.scrVars.globalRenameHash.ContainsKey(name))
            {
               this.scrVars.globalRenameHash[name] = value;
            }
            else
            {
               this.scrVars.globalRenameHash.Add(name, value);
            }

            if (SetupBoolValue(ReflectionScript.SETUP_GLOBAL))
            {
                foreach (DictionaryEntry  scrpt in this.scrVars.subScripts)
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
            MatchCollection match = Regex.Matches(this.code, Projector.Script.ReflectionScript.REGEX_STRING);

            System.Guid  guid = System.Guid.NewGuid();
            guid.ToString();
            for (int i = 0; i < match.Count; i++)
            {
                string str = match[i].Value;
                string key = "%str_" + i + "%" + guid.ToString(); ;
                this.scrVars.globalRenameHash.Add(key, str.Replace("\"",""));
                this.scrVars.globalParamRenameHash.Add(key, str);
                code = code.Replace(str, key);
            }

            

            // get all subelemets by brackets {} independend from what kind of usage
            // just cut it out, add an marker for later usage
            //MatchCollection bracketMatch = Regex.Matches(this.code, Projector.Script.ReflectionScript.REGEX_BRACKETS);
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
                    this.scrVars.namedSubScripts.Add(
                            key,
                            str
                        );

                    code = code.Replace("{" + str + "}", key);
                    this.scrVars.globalParamRenameHash.Add(key, str);
                    /*    
                        int lastCut = str.LastIndexOf('}');                
                        string nCode = str.Substring(1, lastCut - 1);
                        this.scrVars.namedSubScripts.Add(
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
            MatchCollection cBracketsMatch = Regex.Matches(this.code, Projector.Script.ReflectionScript.REGEX_CALC_BRACKETS);
            for (int i = 0; i < cBracketsMatch.Count; i++)
            {
                string str = cBracketsMatch[i].Value;
                string key = "%cbracket_" + i + "%";                
                code = code.Replace(str, key);
                string nCode = str.Substring(1, str.Length - 2);

                RefScrMath math = new RefScrMath(this, key, nCode);

                this.scrVars.addMath(key, math);
                this.scrVars.globalParamRenameHash.Add(key, str);
                
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
                if (this.scrVars.mathIsStored(tparam))
                {
                    RefScrMath mathObj = this.scrVars.getMath(tparam);
                    mathObj.calc();
                    if (testObj.isParentAssigned)
                    {
                        // update for the parent script
                        if (this.Parent != null)
                        {                            
                            this.Parent.scrVars.updateVarByMath(testObj.name, mathObj);
                            if (this.Parent.scrVars.isErrorOccured())
                            {
                                this.addError(this.Parent.scrVars.lastErrorMessage);
                            }
                        }
                        else
                        {
                            this.addError("There is no Parent Instance");
                        }
                        
                    }
                    else
                    {
                        // update for the current script
                        this.scrVars.updateVarByMath(testObj.name, mathObj);
                        if (this.scrVars.isErrorOccured())
                        {
                            this.addError(this.scrVars.lastErrorMessage);
                        }
                    }
                }
                else
                {
                    if (testObj.isParentAssigned)
                    {
                        if (this.Parent != null)
                        {
                            this.Parent.scrVars.updateVarByObject(testObj.name, 
                                    this.fillUpAll( 
                                       tparam
                                    ),
                                    testObj.isSelfInc
                                );
                            // some errors
                            if (this.Parent.scrVars.isErrorOccured())
                            {
                                this.addError(this.Parent.scrVars.lastErrorMessage);
                            }
                        }
                        else
                        {
                            this.addError("There is no Parent Instance");
                        }
                    }
                    else
                    {
                        this.scrVars.updateVarByObject(testObj.name, this.fillUpAll(tparam), testObj.isSelfInc);
                        if (this.scrVars.isErrorOccured())
                        {
                            this.addError(this.scrVars.lastErrorMessage);
                        }
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
            if (obj.ToString() == "" || obj is String || obj is int)
            {
                return;
            }
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

            if (testObj.isMethod)
            {
                if (testObj.namedReference == null)
                {
                    this.addError("this method is invalid because of a unknown Reference" + testObj.code);
                }
                else
                {
                    testObj.ReflectObject = this.scrVars.getRegisteredObject(testObj.namedReference);
                }
                
            }




            if (testObj.isAssignement && testObj.name != null)
            {
                // for any assignement a variable must be exists and allready defined
                if (this.varExists(testObj))
                {
                    foreach (string tparam in testObj.scriptParameters)
                    {
                        if (this.scrVars.mathIsStored(tparam))
                        {
                            RefScrMath mathObj = this.scrVars.getMath(tparam);
                            mathObj.calc();
                            if (testObj.isParentAssigned)
                            {
                                // update for the parent script
                                if (this.Parent != null)
                                {
                                    this.Parent.scrVars.updateVarByMath(testObj.name, mathObj);
                                    if (this.Parent.scrVars.isErrorOccured())
                                    {
                                        this.addError(this.Parent.scrVars.lastErrorMessage);
                                    }
                                }
                                else
                                {
                                    this.addError("There is no Parent Instance");
                                }
                            }
                            else
                            {
                                // update for the current script
                                this.scrVars.updateVarByMath(testObj.name, mathObj);
                                if (this.scrVars.isErrorOccured())
                                {
                                    this.addError(this.scrVars.lastErrorMessage);
                                }
                            }
                        }
                        else
                        {

                        }
                        
                 
                    }
                }
                else
                {
                    // no plain varibale found
                    if (this.scrVars.objectIsStored(testObj.name))
                    {

                    }
                    else
                    {
                        this.addError("Referenz Object not exists " + testObj.name);
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
                if (obVar == null)
                {
                    obVar = this.fillUpAll(var);
                }

                this.addSetup(testObj.name, obVar);
            }


            // variable assinements
            if (testObj.isVariable && !testObj.isSetup)
            {
                if (testObj.name != null && testObj.scriptParameters.Count() == 1)
                {
                    //this.updateParam()
                    
                    string name = this.fillUpStrings(testObj.name);
                    int readPos =0;
                    foreach (string varValue in testObj.scriptParameters)
                    {
                        string varName = testObj.name;
                        
                        string type = "?";
                        if (testObj.scriptParameterTypes.Count() > readPos)
                        {
                            type = testObj.scriptParameterTypes[readPos];
                        }

                        string varStr = this.fillUpStrings(varValue);
                      
                        if (this.scrVars.globalRenameHash.ContainsKey("&" + varName) && !testObj.isSetup)
                        {
                            this.addError("variable " + varName + " allready defined");
                        }
                        else
                        {
                          
                            this.scrVars.globalRenameHash.Add("&" + testObj.name, varStr);
                            

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
                                    this.scrVars.createObject(testObj.name, intValue, type);

                                }
                                catch (Exception ex)
                                {
                                    this.addError(ex.Message);
                                }
                            }
                            else if (type == "Double" || type == "DOUBLE")
                            {
                                Double intValue = 0;
                                try
                                {
                                    //this.int32Founds.Add(testObj.name, Int32.Parse(varStr));
                                    intValue = Double.Parse(varStr);
                                    this.scrVars.createObject(testObj.name, intValue, type);

                                }
                                catch (Exception ex)
                                {
                                    this.addError(ex.Message);
                                }
                            }
                            else if (type == "STR" || type == "String")
                            {
                                this.scrVars.createObject(testObj.name, varStr, type);
                            }
                            else
                            {
                                this.scrVars.createObject(testObj.name, testObj.parameters[readPos], type);
                            }
                            
                        }


                        readPos++;
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
                            //this.createObject(testObj.name, testObject, testObj.typeOfObject);
                            this.scrVars.createObject(testObj.name, testObj.ReflectObject, testObj.typeOfObject);

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
                            foreach (DictionaryEntry parScr in this.scrVars.globalRenameHash)
                            {
                                testObj.subScript.scrVars.globalRenameHash.Add("&parent." + parScr.Key.ToString(), parScr.Value);
                            }
                            updateSubScripts(testObj.subScript);
                        }

                        testObj.subScript.setCode(fullCode);
                        
                        updateDebugInfo(testObj.subScript);
                        this.lineOffset += testObj.subScript.getLineOffset();

                        if (testObj.namedReference != null) {
                            if (this.scrVars.subScripts.ContainsKey(testObj.namedReference))
                            {
                                System.Guid uuid = System.Guid.NewGuid();
                                this.scrVars.subScripts.Add(testObj.namedReference + uuid.ToString(), testObj.subScript);
                            }
                            else
                            {
                                this.scrVars.subScripts.Add(testObj.namedReference, testObj.subScript);
                            }

                        }
                        testObj.parameters.Add(testObj.subScript);

                       
                        if (testObj.subScript.getNotRuntimeErrorCount() > 0)
                        {
                            //this.addError("Invalid Code in subLogic: " + testObj.code);
                          
                            foreach (ScriptErrors err in testObj.subScript.getAllErrors())
                            {
                                // add my own line number
                                ScriptErrors copyErr = err;
                                copyErr.lineNumber += testObj.lineNumber;
                                this.addError(copyErr);

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
                    foreach (DictionaryEntry rp in this.scrVars.globalRenameHash)
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


        private Object getParamValue(string type, string partOrigin)
        {
            Object retValue = null;
            string part = this.fillUpAll(partOrigin);
            switch (type)
            {
                case "String": case "STR":
                    retValue = part;
                    break;
                case "INT": case "Int32":
                    try
                    {
                        string tmpStr = part;
                        int parInt = int.Parse(tmpStr);
                        retValue = parInt;
                    }
                    catch (Exception e)
                    {
                        this.addError("Parameter must be an number" + e.Message);
                        return null;
                    }
                    break;
                case "Double":
                case "DOUBLE":
                    try
                    {
                        string tmpStr = part;
                        Double parInt = Double.Parse(tmpStr);
                        retValue = parInt;
                    }
                    catch (Exception e)
                    {
                        this.addError("Parameter must be an double number" + e.Message);
                        return null;
                    }
                    break;
                case "Decimal":
                
                    try
                    {
                        string tmpStr = part;
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
                    retValue = this.getObjectForParam(part);
                    break;
            }
            return retValue;
        }

        public Object getObjectForParam(string param)
        {
            
            string[] pars = param.Split('.');
            if (pars.Count() > 1)
            {
                if (pars[0].ToLower() == "parent" && this.Parent != null)
                {
                    string chk = "";
                    for (int i = 1; i < pars.Count(); i++)
                    {
                        chk += pars[i];
                    }
                    return this.Parent.getObjectForParam(chk);
                }

                if (pars[0].ToLower() == "root" && this.Parent != null)
                {
                    string chk = "";
                    for (int i = 1; i < pars.Count(); i++)
                    {
                        chk += pars[i];
                    }
                    return this.Parent.getObjectForParam(chk);
                }
            }

            if (this.scrVars.objectIsStored(param))
            {               
                return this.scrVars.getRegisteredObject(param);
                
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
                case "ListView": case "ResultList":
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
                    this.scrVars.updateExistingObject(cmdResult.name, cmdResult.parameters[i]);
                }

                if (this.debugMode && cmdResult.parameters[i] != null)
                {
                    this.updateDebugMessage(cmdResult.name + i + type, cmdResult.parameters[i].ToString());
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
                else if (definedType == "Double" || definedType == "DOUBLE")
                {
                    string replaced = this.fillUpStrings(part);

                    try
                    {
                        Double parInt = Double.Parse(replaced);
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
                    if (this.scrVars.objectIsStored(part))
                    {
                        cmdResult.parameters.Add(this.scrVars.getRegisteredObject(part));
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
            string bestMatchMask = "";
            int matchingWordCount = 0;
            string missingNextWord = "";
            foreach (string hash in this.mask)
            {

                RefScriptMaskMatch maskMatch = new RefScriptMaskMatch(hash, this);
                int bestmatch = maskMatch.possibleMatch(words);
                if (bestMatchFound < bestmatch)
                {
                    bestMatchFound = bestmatch;
                    bestMatchMask = hash;
                    matchingWordCount = maskMatch.matchingUntil;
                    missingNextWord = maskMatch.lastMatchDefinition;
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
                    string[] def = hash.Split(Projector.Script.ReflectionScript.MASK_DELIMITER[0]);

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
                            cmdResult.varDefines = varDefines;
                            cmdResult.codeToken = this.scopeToken;

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

                            if (this.scrVars.objectIsStored(part))
                            {
                                if (objectType == "" || this.scrVars.getObjectsReference(part) == objectType)
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
                        if (this.scrVars.objectIsStored(objName))
                        {
                            string error = "a Object " + objName + " already exists";
                            return null;
                        } else {
                           

                           //this.scrVars.createObject(objName, referenceObject, typeOfObject);
                        }
                    }                    
                    // build parameters

                    

                    return cmdResult;
                }
            }
            if (bestMatchFound == RefScriptMaskMatch.MAYBE_MATCH)
            {
                this.addError("not sure what this should be (" + bestMatchMask + ")" + matchingWordCount);
            }
            else if (bestMatchFound == RefScriptMaskMatch.PARTIAL_MATCH)
            {
                //this.addError("incomplete (" + bestMatchMask + ")" + matchingWordCount + "  " + missingNextWord);

                ScriptErrors maskError = new ScriptErrors();
                maskError.errorMessage = "Incomplete Source";
                maskError.wordPosition = matchingWordCount;
                maskError.lineNumber = this.getLineNumber();
                maskError.errorType = ScriptErrors.TYPO_ERROR;
                maskError.internalMessage = bestMatchMask;
                this.addError(maskError);

            }
            else
            {
                this.addError("INVALID ");
            }
            

            return null;
        }


    }
}
