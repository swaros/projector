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


        public void run()
        {
            if (this.currentScript.getErrorCount() == 0)
            {
                this.exec();
            }
        }


        private void exec()
        {
            foreach (ReflectionScriptDefines scrLine in this.currentScript.getScript())
            {
                string cmd = scrLine.code.ToUpper();
                if (scrLine.isObject && this.objectDefines.ContainsKey(cmd))
                {
                    scrLine.Referenz = objectDefines[cmd];
                    this.execReflectObject(scrLine);

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
        }

        private Object execMethod(Object obj, ReflectionScriptDefines refObj)
        {

            Type executeableObj = obj.GetType();
            MethodInfo myMethodInfo = executeableObj.GetMethod(refObj.originCode);
            
            int countOfparams = refObj.parameters.Count();
            object[] mParam = new object[countOfparams];
            for (int i = 0; i < countOfparams; i++)
            {
                mParam[i] = refObj.parameters[i];
            }
                //object[] mParam = new object[] { obj, refObj.parameters };
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
