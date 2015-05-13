using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using Projector.Script.Vars;

namespace Projector.Script
{
    public class RefScriptExecute
    {

        public const int STATE_NOTHING = 0;
        public const int STATE_RUN = 1;
        public const int STATE_RUN_STEPWISE = 2;
        public const int STATE_WAIT = 5;
        public const int STATE_FINISHED = 10;
        public const int STATE_WAITFOR_SUBS = 6;

        /// <summary>
        /// static name as ID Prey-key for global procIDent
        /// </summary>
        public const string PROC_NAME = "ReflectionScript";

        /// <summary>
        /// at runtime generated Proc-key added to PROC_NAME ti identify the process.
        /// all childs have to use the same id
        /// </summary>
        private string ProcID = "unset";

        /// <summary>
        /// watch to get the runtime
        /// </summary>
        private Stopwatch RunTime = new Stopwatch();
        private Hashtable WaitingTimers = new Hashtable();

        /// <summary>
        /// the level of executers. Root is always 0
        /// </summary>
        public int runlevel = 0;

        /// <summary>
        /// the current state of the current scop (see const STATE_xxx)
        /// </summary>
        public int runState = 0;

        /// <summary>
        /// current assigned script
        /// </summary>
        private ReflectionScript currentScript;


        private Hashtable objectDefines = new Hashtable();
        /// <summary>
        /// storage for all created objectes til run
        /// </summary>
        // private Hashtable objectReferences = new Hashtable();

        private Object parentObject;

        private Boolean internalError = false;

        private int lastErrorCode = 0;

        private String lastErrorMessage = "";

        public Boolean debugMode = false;
        public Object parentWatcher;
        public string watcherMethod;

        /// <summary>
        /// one time flag to force an abort of execution
        /// </summary>
        private Boolean forceAbort = false;

        /// <summary>
        /// variable container
        /// </summary>
        private Variables scrVars = new Variables();

        /// <summary>
        /// the current executed script line stored for debugging
        /// </summary>
        private ReflectionScriptDefines currentDebugLine;

        /// <summary>
        /// the current executed line
        /// </summary>
        private int currentExecLine = 0;

        public int startLine = 0;

        public String getRunIdent()
        {
            if (this.ProcID != "unset")
            {
                return this.ProcID;
            }

            if (this.currentScript.Parent != null && this.currentScript.Parent.CurrentExecuter != null)
            {
                return this.currentScript.Parent.CurrentExecuter.getRunIdent();
            }
            System.Guid uuid = System.Guid.NewGuid();
            return uuid.ToString();

        }

        public RefScriptExecute(ReflectionScript script, Object parent)
        {
            this.parentObject = parent;
            this.currentScript = script;
            this.currentScript.CurrentExecuter = this;
            this.ProcID = this.getRunIdent();
            this.init();
        }

        public void setWatcher(Object watcher, String MethodName)
        {
            // must have parameters   ReflectionScriptDefines, int , int
            this.watcherMethod = MethodName;
            this.parentWatcher = watcher;
            this.debugMode = true;
        }


        private void init()
        {
            this.objectDefines.Add("NEW", new ReflectNew());
        }

        public void StopNow()
        {
            // first abort all childs
            this.forceAbort = true;
            this.Stop();
            this.clearing();

        }

        public Boolean aborting()
        {
            return this.forceAbort;
        }

        private void clearing()
        {
            ProcSync.removeMainProc(RefScriptExecute.PROC_NAME + this.ProcID);
            /*
            foreach (DictionaryEntry existingObjects in this.objectReferences)
            {
                Object obj = (object)existingObjects.Value;
                
            }
            */
        }

        private Boolean checkWaitingTimer(string name)
        {
            int maxRunTime = this.currentScript.SetupIntValue(ReflectionScript.SETUP_MAXWAIT);
            if (maxRunTime < 1)
            {
                return true;
            }
            // existing timers
            if (this.WaitingTimers.ContainsKey(name))
            {
                Stopwatch running = (Stopwatch)this.WaitingTimers[name];
                if (running.ElapsedMilliseconds < maxRunTime)
                {
                    return true;
                }
            }
            else
            {
                // new timer ...so it is true
                Stopwatch newRunning = new Stopwatch();
                newRunning.Start();
                this.WaitingTimers.Add(name, newRunning);
                return true;
            }
            return false;
        }

        private void removeWaitTimerIfExists(string name)
        {
            if (this.WaitingTimers.ContainsKey(name))
            {
                this.WaitingTimers.Remove(name);
            }
        }

        public Boolean run()
        {

            if (this.currentScript.Parent != null && this.currentScript.Parent.CurrentExecuter != null)
            {
                this.watcherMethod = this.currentScript.Parent.CurrentExecuter.watcherMethod;
                this.parentWatcher = this.currentScript.Parent.CurrentExecuter.parentWatcher;
                this.debugMode = this.currentScript.Parent.CurrentExecuter.debugMode;

                this.runlevel += this.currentScript.Parent.CurrentExecuter.runlevel + 1;
                this.startLine += this.currentScript.Parent.CurrentExecuter.startLine;
            }

            if (!ProcSync.isRegistered(RefScriptExecute.PROC_NAME + this.ProcID))
            {
                ProcSync.registerProc(RefScriptExecute.PROC_NAME + this.ProcID);
            }

            this.internalError = false;
            if (this.currentScript.getNotRuntimeErrorCount() == 0)
            {
                this.runState = RefScriptExecute.STATE_RUN;
                Boolean runSucceed = this.exec();
                return (runSucceed == true && this.internalError == false);
            }
            else
            {
                this.internalError = true;
                if (this.currentScript.Parent != null)
                {
                    this.currentScript.updateErrorsToParent();
                }

            }

            return false;
        }


        public void setDebuRun()
        {
            this.internalError = false;
            if (this.currentScript.getNotRuntimeErrorCount() == 0)
            {
                this.currentExecLine = 0;
            }
        }

        public Boolean Next()
        {

            if (this.currentExecLine >= this.currentScript.getScript().Count)
            {
                this.runState = RefScriptExecute.STATE_FINISHED;
                if (this.debugMode && this.currentDebugLine != null)
                {
                    this.updateMessage(this.currentDebugLine);
                }
                return false;
            }

            this.runState = RefScriptExecute.STATE_RUN;
            Boolean execRes = this.execSingleLine();
            this.runState = RefScriptExecute.STATE_WAIT;


            return execRes;

        }

        public void Stop()
        {
            this.runState = RefScriptExecute.STATE_WAIT;

        }

        public Boolean Continue()
        {


            if (this.currentExecLine >= this.currentScript.getScript().Count)
            {
                this.runState = RefScriptExecute.STATE_FINISHED;
                if (this.debugMode)
                {
                    if (this.currentDebugLine != null)
                    {
                        this.updateMessage(this.currentDebugLine);
                    }
                }

                return true;
            }

            this.runState = RefScriptExecute.STATE_RUN;
            Boolean execRes = this.execSingleLine();
            //this.currentExecLine++;
            return execRes;

        }

        public int getCurrentLineNumber()
        {
            return this.currentExecLine;
        }

        public void changedState()
        {
            if (this.runState == RefScriptExecute.STATE_RUN)
            {
                this.exec();
            }
        }

        public Hashtable getRuntimeObjects()
        {
            return this.scrVars.getObjects();
        }


        public Object getObjectFromRoot(string name)
        {
            if (this.currentScript.Parent != null)
            {
                return this.currentScript.Parent.CurrentExecuter.getObjectFromRoot(name);
            }
            if (this.scrVars.objectIsStored(name))
            {
                return this.scrVars.getRegisteredObject(name);
            }
            return null;
        }

        public Object findObject(string name)
        {
            if (this.scrVars.objectIsStored(name))
            {
                return this.scrVars.getRegisteredObject(name);
            }
            if (this.currentScript.Parent != null)
            {
                return this.currentScript.Parent.CurrentExecuter.findObject(name);
            }

            return null;
        }


        public Object getRegisteredObject(string name)
        {
            if (this.scrVars.objectIsStored(name))
            {
                return this.scrVars.getRegisteredObject(name);
            }

            if (this.currentScript.Parent != null)
            {
                string[] parts = name.Split('.');
                if (parts.Count() > 1)
                {
                    if (parts[0] == "this")
                    {
                        return this.getObjectFromRoot(parts[1]);
                    }
                    else
                    {
                        string newName = "";
                        string add = "";
                        for (int i = 1; i < parts.Count(); i++)
                        {
                            newName += add + parts[i];
                            add = ".";
                        }
                        return this.currentScript.Parent.CurrentExecuter.getRegisteredObject(newName);
                    }
                }
                else
                {
                    return this.findObject(name);
                }
            }
            return null;
        }



        private Boolean execLine(ReflectionScriptDefines scrLine)
        {

            // what ever happens ..tis line is executed
            this.currentExecLine++;

            if (this.forceAbort)
            {
                return true;
            }

            if (this.currentScript.Parent != null && this.currentScript.Parent.CurrentExecuter != null)
            {
                if (this.currentScript.Parent.CurrentExecuter.aborting())
                {
                    return true;
                }
            }


            string cmd = scrLine.code.ToUpper();
            //this.currentScript.updateParam(scrLine);


            if (cmd == "MESSAGEBOX")
            {
                string message = "";
                foreach (string parStr in scrLine.scriptParameters)
                {
                    message += this.currentScript.fillUpAll(parStr);
                }
                MessageBox.Show(message);
            }

            if (cmd == "REG")
            {
                string procIdent = "";
                foreach (string parStr in scrLine.scriptParameters)
                {
                    procIdent += this.currentScript.fillUpAll(parStr);
                }
                ProcSync.addSubProc(RefScriptExecute.PROC_NAME + this.ProcID, procIdent);
            }

            if (cmd == "UNREG")
            {
                string procIdent = "";
                foreach (string parStr in scrLine.scriptParameters)
                {
                    procIdent += this.currentScript.fillUpAll(parStr);
                }
                if (ProcSync.getProcCount(RefScriptExecute.PROC_NAME + this.ProcID, procIdent) > 0)
                {
                    ProcSync.removeSubProc(RefScriptExecute.PROC_NAME + this.ProcID, procIdent);
                }

            }

            if (cmd == "EXEC")
            {
                List<string> execPars = new List<string>();
                string externalScript = "";
                foreach (string parStr in scrLine.scriptParameters)
                {
                    if (externalScript == "")
                    {
                        externalScript = this.currentScript.fillUpAll(parStr);
                    }
                    else
                    {
                        execPars.Add(this.currentScript.fillUpAll(parStr));
                    }

                }


                PConfig seting = new PConfig();
                string scrPath = seting.getSettingWidthDefault("client.scriptpath", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
                string scrFileName = scrPath + System.IO.Path.DirectorySeparatorChar.ToString() + externalScript;
                if (System.IO.File.Exists(scrFileName))
                {

                    string code = System.IO.File.ReadAllText(scrFileName);
                    ReflectionScript exScrpt = new ReflectionScript();
                    exScrpt.setCode(code);
                    exScrpt.addSetupIfNotExists(ReflectionScript.SETUP_PREVIEW, true);
                    RefScriptExecute subExec = new RefScriptExecute(exScrpt, this.parentObject);
                    int ppp = 0;
                    foreach (string parToExec in execPars)
                    {
                        ppp++;
                        exScrpt.createOrUpdateStringVar("&PARAM." + ppp, parToExec);
                    }
                    subExec.run();

                }
                else
                {
                    ScriptErrors error = new ScriptErrors();
                    error.errorMessage = "script not found:" + scrFileName;
                    error.lineNumber = scrLine.lineNumber;
                    error.runtimeError = true;
                    error.errorCode = Projector.RefSrcStates.ERROR_TYPE_WARNING;
                    this.currentScript.addError(error);
                }

            }

            if (cmd == "WAITFOR")
            {
                Application.DoEvents();
                string procIdent = "";
                foreach (string parStr in scrLine.scriptParameters)
                {
                    procIdent += this.currentScript.fillUpAll(parStr);
                }

                if (ProcSync.getProcCount(RefScriptExecute.PROC_NAME + this.ProcID, procIdent) > 0)
                {
                    // max time reached ?
                    if (this.checkWaitingTimer(procIdent))
                    {
                        // get back to my self
                        currentExecLine--;
                        Application.DoEvents();
                        return true;
                    }
                    else
                    {
                        ScriptErrors error = new ScriptErrors();
                        error.errorMessage = "Max Waiting Time reached. Check code or increase max waiting Time";
                        error.lineNumber = scrLine.lineNumber;
                        error.runtimeError = true;
                        error.errorCode = Projector.RefSrcStates.ERROR_TYPE_WARNING;
                        this.currentScript.addError(error);
                        this.removeWaitTimerIfExists(procIdent);
                    }
                }
                else
                {
                    this.removeWaitTimerIfExists(procIdent);
                }
            }

            if (scrLine.setState != 0)
            {
                if (scrLine.isParentAssigned)
                {
                    if (this.currentScript.Parent != null && this.currentScript.Parent.CurrentExecuter != null)
                    {
                        //this.currentScript.Parent.
                        this.currentScript.Parent.CurrentExecuter.runState = scrLine.setState;
                        this.currentScript.Parent.CurrentExecuter.changedState();
                    }
                    else
                    {
                        ScriptErrors error = new ScriptErrors();
                        error.errorMessage = "Parent can be used in subscripts only ";
                        error.lineNumber = scrLine.lineNumber;
                        error.runtimeError = true;
                        error.errorCode = Projector.RefSrcStates.EXEC_ERROR_INVALIDOBJECT;
                        this.currentScript.addError(error);
                    }
                }
                else
                {
                    this.runState = scrLine.setState;
                }

            }

            // execute object assignements that NOT are methods. so just code assignements
            if (scrLine.isAssignement && scrLine.name != null && scrLine.isMethod == false)
            {
                // for any assignement a variable must be exists and allready defined
                this.currentScript.recalcBrackets(scrLine);


            }


            if (scrLine.isVariable && !scrLine.isSetup)
            {
                this.currentScript.updateParam(scrLine, true);
            }

            if (scrLine.isObject && this.objectDefines.ContainsKey(cmd))
            {
                scrLine.Referenz = objectDefines[cmd];
                this.execReflectObject(scrLine);
                if (scrLine.ReflectObject == null)
                {
                    ScriptErrors error = new ScriptErrors();
                    error.errorMessage = "object " + scrLine.typeOfObject + " not createable";
                    error.lineNumber = scrLine.lineNumber;
                    error.errorCode = Projector.RefSrcStates.EXEC_ERROR_NONOBJECT;
                    error.runtimeError = true;
                    this.currentScript.addError(error);

                    lastErrorCode = Projector.RefSrcStates.EXEC_ERROR_INVALIDOBJECT;
                    return false;
                }
                if (!this.scrVars.objectIsStored(scrLine.name))
                {
                    this.scrVars.createObject(scrLine.name, scrLine.ReflectObject, scrLine.typeOfObject);
                }
                else
                {
                    ScriptErrors error = new ScriptErrors();
                    error.errorMessage = "object " + scrLine.typeOfObject + " allready added. Check Script";
                    error.lineNumber = scrLine.lineNumber;
                    error.errorCode = Projector.RefSrcStates.EXEC_ERROR_NONOBJECT;
                    error.runtimeError = true;
                    this.currentScript.addError(error);

                    lastErrorCode = Projector.RefSrcStates.EXEC_ERROR_INVALIDOBJECT;
                    return false;
                }


            }

            if (scrLine.isMethod && scrLine.namedReference != null)
            {

                Object useObject = this.getRegisteredObject(scrLine.namedReference);
                //Object useObject = scrLine.ReflectObject;
                //Object useObject = this.currentScript.getRegisteredObject(scrLine.namedReference);
                if (useObject != null)
                {
                    this.lastErrorCode = 0;
                    this.currentScript.updateParam(scrLine, true);

                    Object execResult = this.execMethod(useObject, scrLine);
                    scrLine.ReflectObject = useObject;
                    this.currentScript.scrVars.updateExistingObject(scrLine.namedReference, useObject);
                    if (this.lastErrorCode > 0)
                    {
                        ScriptErrors error = new ScriptErrors();
                        error.errorMessage = "object " + scrLine.typeOfObject + " reports an error on execution " + this.lastErrorCode + this.lastErrorMessage;
                        error.lineNumber = scrLine.lineNumber;
                        error.runtimeError = true;
                        error.errorCode = this.lastErrorCode;
                        this.currentScript.addError(error);
                    }
                    else
                    {

                        this.currentScript.updateMeByObject(scrLine);

                        if (scrLine.isAssignement && execResult != null)
                        {
                            if (scrLine.isParentAssigned)
                            {
                                if (this.currentScript.Parent != null)
                                {
                                    this.currentScript.Parent.scrVars.updateVarByObject(scrLine.name, execResult);
                                }
                            }
                            else
                            {
                                this.currentScript.scrVars.updateVarByObject(scrLine.name, execResult);
                            }
                        }
                    }

                }
                else
                {
                    ScriptErrors error = new ScriptErrors();
                    error.errorMessage = "execution Fail: " + scrLine.namedReference + " is not registered as an executable Object ";
                    error.lineNumber = scrLine.lineNumber;
                    error.errorCode = this.lastErrorCode;
                    error.runtimeError = true;
                    this.currentScript.addError(error);
                }
            }

            // last trigger call
            if (this.debugMode)
            {
                this.currentDebugLine = scrLine;
                this.updateMessage(scrLine);
            }

            return true;
        }

        private Boolean exec()
        {

            if (this.runState == RefScriptExecute.STATE_RUN)
            {
                while (this.runState == RefScriptExecute.STATE_RUN)
                {
                    Boolean execResult = this.Continue();
                    if (execResult == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        private Boolean execSingleLine()
        {
            List<ReflectionScriptDefines> src = this.currentScript.getScript();
            if (src.Count > this.currentExecLine)
            {
                return this.execLine(src[this.currentExecLine]);
            }
            return false;
        }


        private Object execMethod(Object obj, ReflectionScriptDefines refObj)
        {

            if (obj == null)
            {
                lastErrorCode = Projector.RefSrcStates.EXEC_ERROR_NONOBJECT;
                lastErrorMessage = "Object is NULL";
                this.internalError = true;
                return null;
            }

            Type executeableObj = obj.GetType();
            MethodInfo myMethodInfo = executeableObj.GetMethod(refObj.originCode);
            if (myMethodInfo == null)
            {
                lastErrorCode = Projector.RefSrcStates.EXEC_ERROR_NONMETHOD;
                lastErrorMessage = "invalid Method (null)";
                this.internalError = true;
                return null;
            }

            int countOfparams = refObj.parameters.Count();
            if (countOfparams > 0)
            {
                object[] mParam = new object[countOfparams];
                for (int i = 0; i < countOfparams; i++)
                {
                    mParam[i] = refObj.parameters[i];
                }

                try
                {
                    return myMethodInfo.Invoke(obj, mParam);
                }
                catch (TargetParameterCountException te)
                {
                    lastErrorCode = Projector.RefSrcStates.EXEC_ERROR_INVALID_PARAMETER_COUNT;
                    lastErrorMessage = te.Message;
                    this.internalError = true;
                    return null;
                }
                catch (TargetInvocationException te)
                {
                    lastErrorCode = Projector.RefSrcStates.EXEC_ERROR_UNKNOWNREASON;
                    lastErrorMessage = te.Message;
                    this.internalError = true;
                    return null;
                }
                catch (ArgumentException te)
                {
                    lastErrorCode = Projector.RefSrcStates.EXEC_ERROR_UNKNOWNREASON;
                    lastErrorMessage = te.Message;
                    this.internalError = true;
                    return null;
                }
            }
            else
            {
                try
                {
                    return myMethodInfo.Invoke(obj, null);
                }
                catch (TargetParameterCountException te)
                {
                    lastErrorCode = Projector.RefSrcStates.EXEC_ERROR_INVALID_PARAMETER_COUNT;
                    lastErrorMessage = te.Message;
                    this.internalError = true;
                    return null;
                }

            }


        }

        public int getCurrentExecutionLine()
        {
            return this.currentExecLine;
        }

        private void updateMessage(ReflectionScriptDefines refObj)
        {
            if (this.parentWatcher != null && watcherMethod != null)
            {
                int startLn = 0;

                if (this.currentScript.Parent != null)
                {
                    startLn = this.currentScript.parentLineNumber;
                }

                Type queryWinType = this.parentWatcher.GetType();
                MethodInfo myMethodInfo = queryWinType.GetMethod(this.watcherMethod);
                object[] mParam = new object[] { refObj, refObj.lineNumber + startLn, this.runState, this.runlevel };
                refObj.ReflectObject = myMethodInfo.Invoke(this.parentWatcher, mParam);
            }
            Application.DoEvents();
        }


        private ReflectionScriptDefines execReflectObject(ReflectionScriptDefines refObj)
        {
            Type queryWinType = refObj.Referenz.GetType();
            MethodInfo myMethodInfo = queryWinType.GetMethod("getObject");
            object[] mParam = new object[] { refObj, this.parentObject };
            refObj.ReflectObject = myMethodInfo.Invoke(refObj.Referenz, mParam);
            return refObj;
        }

    }
}
