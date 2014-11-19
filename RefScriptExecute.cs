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

        private int lastErrorCode = Projector.RefSrcStates.EXEC_ERROR_UNKNOWNREASON;

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
                       this.execMethod(objectReferences[scrLine.namedReference],scrLine);
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
            object[] mParam = new object[countOfparams];
            for (int i = 0; i < countOfparams; i++)
            {
                mParam[i] = refObj.parameters[i];
            }
            return myMethodInfo.Invoke(obj, mParam);            
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
