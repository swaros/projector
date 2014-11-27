using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Projector
{
    public class ObjectInfo
    {
        public List<string> getObjectInfo(Object obj)
        {
            List<string> maskData = new List<string>();
            if (obj == null)
            {                
                return null;
            }

            // "& SETCOORDS ? ? ? ?" + Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + ". setCoords INT INT INT INT"
            
            Type executeableObj = obj.GetType();
            MethodInfo[] myMethodInfos = executeableObj.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (myMethodInfos.Count() < 1)
            {
                return null;
            }


            
            foreach (MethodInfo mInfo in myMethodInfos)
            {
                string objStr = obj.ToString();
                string[] objParts = objStr.Split(',');
                string[] nameParts = objParts[0].Split('.');

                string objname = nameParts[1];

                string maskStr = "&"+ objname + " " + mInfo.Name.ToUpper();
                string paramPart = ". " + mInfo.Name;
                ParameterInfo[] parameters = mInfo.GetParameters();
                foreach (ParameterInfo parInfo in parameters)
                {
                    string partype = parInfo.ParameterType.Name;
                    maskStr += " ?";
                    paramPart += " " + partype;
                }
                maskStr += Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + paramPart;
                maskData.Add(maskStr);
            }

            return maskData;
        }
    }
}
