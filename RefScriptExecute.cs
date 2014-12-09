using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;

namespace Projector
{
    public class RefScriptExecute
    {

        public const int STATE_NOTHING = 0;
        public const int STATE_RUN = 1;
        public const int STATE_RUN_STEPWISE = 2;
        public const int STATE_WAIT = 5;
        public const int STATE_FINISHED = 10;


        public int runState = 0;

        private ReflectionScript currentScript;

        private Hashtable objectDefines = new Hashtable();

        private Hashtable objectReferences = new Hashtable();

        private Object parentObject;

        private Boolean internalError = false;

        private int lastErrorCode = 0;

        private String lastErrorMessage = "";

        public Boolean debugMode = false;
        public Object parentWatcher;
        public string watcherMethod;

        // debugging stuff
        private ReflectionScriptDefines currentDebugLine;
        private int currentExecLine = 0;
        
        

        public RefScriptExecute(ReflectionScript script, Object parent)
        {
            this.parentObject = parent;
            this.currentScript = script;
            this.currentScript.CurrentExecuter = this;

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
            this.objectDefines.Add("NEW",new ReflectNew());
        }


        public Boolean run()
        {         
            this.internalError = false;
            if (this.currentScript.getErrorCount() == 0)
            {
                this.runState = RefScriptExecute.STATE_RUN;
                Boolean runSucceed = this.exec();
                return (runSucceed == true && this.internalError == false);
            }
            return false;
        }


        public void setDebuRun()
        {
            this.internalError = false;
            if (this.currentScript.getErrorCount() == 0)
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
            Boolean execRes = this.execSingleLine(this.currentExecLine);
            this.runState = RefScriptExecute.STATE_WAIT;
            this.currentExecLine++;
            
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
            Boolean execRes = this.execSingleLine(this.currentExecLine);            
            this.currentExecLine++;
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

        private Boolean execLine(ReflectionScriptDefines scrLine)
        {

            
            string cmd = scrLine.code.ToUpper();
            this.currentScript.updateParam(scrLine);
            if (this.debugMode)
            {
                this.currentDebugLine = scrLine;
                this.updateMessage(scrLine);
            }

            if (cmd == "MESSAGEBOX")
            {
                string message = "";
                foreach (string parStr in scrLine.scriptParameters)
                {
                    message += this.currentScript.fillUpAll(parStr);
                }
                MessageBox.Show(message);
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
                        error.errorCode = Projector.RefSrcStates.EXEC_ERROR_INVALIDOBJECT;
                    }
                }
                else
                {
                    this.runState = scrLine.setState;
                }
                
            }

            if (scrLine.isAssignement && scrLine.name != null)
            {
                // for any assignement a variable must be exists and allready defined
                this.currentScript.recalcBrackets(scrLine);

               
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

                    this.currentScript.addError(error);

                    lastErrorCode = Projector.RefSrcStates.EXEC_ERROR_INVALIDOBJECT;
                    return false;
                }
                this.objectReferences.Add(scrLine.name, scrLine.ReflectObject);

            }

            if (scrLine.isMethod && scrLine.namedReference != null)
            {
                if (objectReferences.ContainsKey(scrLine.namedReference))
                {
                    this.lastErrorCode = 0;
                    Object execResult = this.execMethod(objectReferences[scrLine.namedReference], scrLine);

                    if (this.lastErrorCode > 0)
                    {
                        ScriptErrors error = new ScriptErrors();
                        error.errorMessage = "object " + scrLine.typeOfObject + " reports an error on execution " + this.lastErrorCode + this.lastErrorMessage;
                        error.lineNumber = scrLine.lineNumber;
                        error.errorCode = this.lastErrorCode;
                        this.currentScript.addError(error);
                    }
                    else
                    {
                        if (scrLine.isAssignement && execResult != null)
                        {
                            if (scrLine.isParentAssigned)
                            {
                                if (this.currentScript.Parent != null)
                                {
                                    this.currentScript.Parent.updateVarByObject(scrLine.name, execResult);
                                }
                            }
                            else
                            {
                                this.currentScript.updateVarByObject(scrLine.name, execResult);
                            }
                        }
                    }

                }
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


        private Boolean execSingleLine(int nr)
        {
            List<ReflectionScriptDefines> src = this.currentScript.getScript();

            if (src.Count > nr)
            {
                return this.execLine(src[nr]);
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

        private void updateMessage(ReflectionScriptDefines refObj)
        {
            if (this.parentWatcher != null && watcherMethod != null)
            {
                Type queryWinType = this.parentWatcher.GetType();
                MethodInfo myMethodInfo = queryWinType.GetMethod(this.watcherMethod);
                object[] mParam = new object[] { refObj, this.currentExecLine, this.runState };
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
