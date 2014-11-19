using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class ReflectNew : ReflectObjectInterface
    {
        public Object execute()
        {
            return null;
        }

        public Object getObject(ReflectionScriptDefines refObject, Object parent)
        {
            if (refObject.typeOfObject == "QueryBrowser")
            {
                return this.getQueryBrowser(refObject,parent);
            }

            if (refObject.typeOfObject == "Form")
            {
                return this.getForm(refObject, parent);
            }

            return null;
        }

        private ReflectForm getForm(ReflectionScriptDefines refObject, Object parent)
        {
            ReflectForm newForm = new ReflectForm();
            newForm.ScriptIdent = refObject.name;

            if (parent is MdiForm)
            {
                MdiForm mdi = (MdiForm)parent;
                mdi.addSubWindow(newForm,refObject.name);
            }

            return newForm;
        }


        private queryBrowser getQueryBrowser(ReflectionScriptDefines refObject, Object parent)
        {
            queryBrowser browser = new queryBrowser();
            browser.Name = refObject.name;
            browser.ScriptIdent = refObject.name;

            if (parent is MdiForm)
            {

                MdiForm mdi = (MdiForm)parent;
                queryBrowser existingQb = (queryBrowser)mdi.getQueryForm(refObject.name);
                if (null == existingQb)
                {
                    mdi.addQueryWindow(browser);
                }
                else
                {
                    return existingQb;

                }
            }

            return browser;
        }
    }
}
