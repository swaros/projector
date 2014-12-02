using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Projector
{
    public class ObjectInfo
    {

        private static Hashtable knownOjectMethods = new Hashtable();

        private void storeObjectInfo(RefScrObjectStorage objectToStore)
        {
            if (!Projector.ObjectInfo.knownOjectMethods.ContainsKey(objectToStore.originObjectName))
            {
                Projector.ObjectInfo.knownOjectMethods.Add(objectToStore.originObjectName,objectToStore);
            }
        }


        public static RefScrObjectStorage getObjectInfo(string name)
        {
            if (Projector.ObjectInfo.knownOjectMethods.ContainsKey(name))
            {
                return (RefScrObjectStorage)Projector.ObjectInfo.knownOjectMethods[name];
            }

            return null;
        }

        public static List<RefScrObjectStorage> getAllObjects()
        {            
            List<RefScrObjectStorage> returnList = new List<RefScrObjectStorage>();
            foreach (RefScrObjectStorage tmpCopy in Projector.ObjectInfo.knownOjectMethods.Values)
            {
                returnList.Add(tmpCopy);
            }
            return returnList;
        }

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

            RefScrObjectStorage objStore = new RefScrObjectStorage();

            string objStr = obj.ToString();
            string[] objParts = objStr.Split(',');
            string[] nameParts = objParts[0].Split('.');
            string objname = nameParts[1];

            objStore.originObjectName = objname;

            foreach (MethodInfo mInfo in myMethodInfos)
            {
                // dirty way to get the name
                // TODO: find better soultion


                

                string maskStr = "&"+ objname + " " + mInfo.Name.ToUpper();
                string paramPart = ". " + mInfo.Name;
                objStore.methods.Add(mInfo);
                objStore.methodNames.Add(mInfo.Name);                

                ParameterInfo[] parameters = mInfo.GetParameters();
                foreach (ParameterInfo parInfo in parameters)
                {
                    string partype = parInfo.ParameterType.Name;
                    maskStr += " ?";
                    paramPart += " " + partype;
                }
                maskStr += Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + paramPart;
                maskData.Add(maskStr);
                objStore.methodMask.Add(maskStr);
            }
            this.storeObjectInfo(objStore);
            return maskData;
        }
    }
}
