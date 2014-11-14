using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    interface ReflectObjectInterface
    {
        Object getObject(ReflectionScriptDefines refObject, Object parent);
        Object execute();
        
    }
}
