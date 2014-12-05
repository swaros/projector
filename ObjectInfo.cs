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

        public RefScrObjectStorage lastObjectInfo;

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

        private String getReturnValue(string typeStr)
        {
            switch (typeStr)
            {
                case "System.Void":
                    return "";
                    break;
                case "System.Boolean":
                    return "Boolean";
                    break;
                case "System.String":
                    return "STR";
                    break;
                case "System.Int32":
                    return "INT";
                    break;

            }
            return "";
        }

        private void updatePropInfos(Type executeableObj, RefScrObjectStorage objStore)
        {
            PropertyInfo[] propInfo = executeableObj.GetProperties();

            foreach (PropertyInfo prop in propInfo)
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    switch (prop.PropertyType.Name)
                    {
                        case "In32":
                            objStore.Integers.Add(prop.Name);
                            break;
                        case "String":
                            objStore.Strings.Add(prop.Name);
                            break;
                        case "Boolean":
                            objStore.Booleans.Add(prop.Name);
                            break;
                    }
        
                }
            }

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

            this.updatePropInfos(executeableObj, objStore);

            string objStr = obj.ToString();
            string[] objParts = objStr.Split(',');
            string[] nameParts = objParts[0].Split('.');
            string objname = nameParts[nameParts.Count()-1];

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



                string keepMe = maskStr;
                maskStr += Projector.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.ReflectionScript.MASK_DELIMITER + paramPart;

                maskData.Add(maskStr);
                objStore.methodMask.Add(maskStr);

                string returnVar = mInfo.ReturnType.FullName;
                string retval = this.getReturnValue(returnVar);
                if (retval != "")
                {
                    //maskStr = retval + " " + maskStr;
                    maskStr = "% = " + keepMe + Projector.ReflectionScript.MASK_DELIMITER + "METHOD ASSIGN" + Projector.ReflectionScript.MASK_DELIMITER + retval + " = " + paramPart;
                    maskData.Add(maskStr);
                    objStore.methodMask.Add(maskStr);
                }

            }
            this.storeObjectInfo(objStore);
            this.lastObjectInfo = objStore;
            return maskData;
        }
    }
}
