using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Projector
{
    /// <summary>
    /// Class to get all Infomations of Objects, an manage the infomation
    /// </summary>
    public class ObjectInfo
    {
        /// <summary>
        /// The last Object Information that we got
        /// </summary>
        public RefScrObjectStorage lastObjectInfo;

        /// <summary>
        /// all Methods from this Object
        /// </summary>
        private static Hashtable knownOjectMethods = new Hashtable();

        /// <summary>
        /// Store ObjectInfo if not exists
        /// </summary>
        /// <param name="objectToStore">The ObjectStorage that should be stored</param>
        private void storeObjectInfo(RefScrObjectStorage objectToStore)
        {
            if (!Projector.ObjectInfo.knownOjectMethods.ContainsKey(objectToStore.originObjectName))
            {
                Projector.ObjectInfo.knownOjectMethods.Add(objectToStore.originObjectName,objectToStore);
            }
        }

        /// <summary>
        /// get ObjectInfo if there are stored
        /// </summary>
        /// <param name="name">the Name of the Type like queryBrowser</param>
        /// <returns></returns>
        public static RefScrObjectStorage getObjectInfo(string name)
        {
            if (Projector.ObjectInfo.knownOjectMethods.ContainsKey(name))
            {
                return (RefScrObjectStorage)Projector.ObjectInfo.knownOjectMethods[name];
            }

            return null;
        }

        /// <summary>
        /// return a list of all stored object infomation
        /// </summary>
        /// <returns>Stored Object Information</returns>
        public static List<RefScrObjectStorage> getAllObjects()
        {            
            List<RefScrObjectStorage> returnList = new List<RefScrObjectStorage>();
            foreach (RefScrObjectStorage tmpCopy in Projector.ObjectInfo.knownOjectMethods.Values)
            {
                returnList.Add(tmpCopy);
            }
            return returnList;
        }

        /// <summary>
        /// Get a simple version of the Real 
        /// SystemType
        /// </summary>
        /// <param name="typeStr">Original System Type</param>
        /// <returns>Basic Type name</returns>
        private String getReturnValue(string typeStr)
        {
            switch (typeStr)
            {
                case "System.Void":
                    return "";

                case "System.Boolean":
                    return "Boolean";

                case "System.String":
                    return "STR";

                case "System.Int32":
                    return "INT";

                default:
                    string[] parts = typeStr.Split('.');
                    int cnt = parts.Count()-1;
                    if (cnt > 0)
                    {
                        return parts[cnt];
                    }
                    break;

            }
            return "";
        }

        /// <summary>
        /// Updates the current collected Infomation in objStore about
        /// the current State.
        /// usefull after some changes happens
        /// </summary>
        /// <param name="execObject">the object that are executable</param>
        /// <param name="executeableObj"></param>
        /// <param name="objStore"></param>
        private void updatePropInfos(Object execObject, Type executeableObj, RefScrObjectStorage objStore)
        {
            PropertyInfo[] propInfo = executeableObj.GetProperties();

            foreach (PropertyInfo prop in propInfo)
            {
                if (prop.CanRead && prop.CanWrite)
                {

                    Object val = null;
                    ParameterInfo[] pInfo = prop.GetIndexParameters();
                    if (pInfo.Count() < 1 && executeableObj != null)
                    {
                        try
                        {
                            val = prop.GetValue(execObject, null);
                        }
                        catch (Exception e) { }
                        
                    }

                    switch (prop.PropertyType.Name)
                    {
                        case "In32":
                            objStore.Integers.Add(prop.Name, val);
                            break;
                        case "String":
                            objStore.Strings.Add(prop.Name, val);
                            break;
                        case "Boolean":
                            objStore.Booleans.Add(prop.Name, val);
                            break;
                    }

                }
            }

        }

        public List<string> getObjectInfo(Object obj)
        {
            return this.getObjectInfo(obj, true);
        }

        public List<string> getObjectInfo(Object obj, Boolean getFullProp)
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
            if (getFullProp)
            {
                this.updatePropInfos(obj, executeableObj, objStore);
            }
            else
            {
                this.updatePropInfos(null, executeableObj, objStore);
            }

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
                maskStr += Projector.Script.ReflectionScript.MASK_DELIMITER + "METHOD" + Projector.Script.ReflectionScript.MASK_DELIMITER + paramPart;

                maskData.Add(maskStr);
                objStore.methodMask.Add(maskStr);

                string returnVar = mInfo.ReturnType.FullName;
                string retval = this.getReturnValue(returnVar);
                if (retval != "")
                {
                    //maskStr = retval + " " + maskStr;
                    maskStr = "% = " + keepMe + Projector.Script.ReflectionScript.MASK_DELIMITER + "METHOD ASSIGN" + Projector.Script.ReflectionScript.MASK_DELIMITER + retval + " = " + paramPart;
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
