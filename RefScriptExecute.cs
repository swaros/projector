using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;

namespace Projector
{
    class RefScriptExecute
    {
        private ReflectionScript currentScript;

        private Hashtable objectDefines = new Hashtable();

        private Hashtable objectReferences = new Hashtable();

        private Object parentObject;

        private Boolean internalError = false;

        private int lastErrorCode = 0;

        private String lastErrorMessage = "";

        public RefScriptExecute(ReflectionScript script, Object parent)
        {
            this.parentObject = parent;
            this.currentScript = script;
            this.init();
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
                Boolean runSucceed = this.exec();
                return (runSucceed == true && this.internalError == false);
            }
            return false;
        }


        private Boolean exec()
        {
            foreach (ReflectionScriptDefines scrLine in this.currentScript.getScript())
            {
                string cmd = scrLine.code.ToUpper();

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
                        Object execResult = this.execMethod(objectReferences[scrLine.namedReference],scrLine);

                        if (this.lastErrorCode > 0)
                        {
                            ScriptErrors error = new ScriptErrors();
                            error.errorMessage = "object " + scrLine.typeOfObject + " reports an error on execution " + this.lastErrorCode;
                            error.lineNumber = scrLine.lineNumber;
                            error.errorCode = this.lastErrorCode;
                            this.currentScript.addError(error);
                        }
                        else
                        {
                            if (scrLine.isAssignement && execResult != null)
                            {
                                this.currentScript.updateVarByObject(scrLine.name, execResult);
                            }
                        }

                    }
                }
            }
            return true;
        }

        private Object execMethod(Object obj, ReflectionScriptDefines refObj)
        {

            if (obj == null)
            {
                lastErrorCode = Projector.RefSrcStates.EXEC_ERROR_NONOBJECT;
                this.internalError = true;
                return null;
            }

            Type executeableObj = obj.GetType();
            MethodInfo myMethodInfo = executeableObj.GetMethod(refObj.originCode);
            if (myMethodInfo == null)
            {
                lastErrorCode = Projector.RefSrcStates.EXEC_ERROR_NONMETHOD;
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
                return myMethodInfo.Invoke(obj, mParam);
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
                    this.internalError = true;
                    return null;
                }
                
            }
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
