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
                queryBrowser browser = new queryBrowser();
                browser.Name = refObject.name;

                if (parent is MdiForm){
                    MdiForm mdi = (MdiForm)parent;
                    mdi.addQueryWindow(browser);
                }

                return browser;
            }
            return null;
        }
    }
}
