using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector.Script.Vars
{
    public class Variables
    {
        /// <summary>
        /// an type can not be converted 
        /// </summary>
        public const int ERROR_INCOMPATIBLE_TYPE = 1002;

        /// <summary>
        /// This type can not increment by using +=
        /// </summary>
        public const int ERROR_TYPE_NOT_INC_COMPATIBLE = 1003;

        /// <summary>
        /// an assignement to an taget that not exists
        /// </summary>
        public const int ERROR_UNKNOW_TARGET = 1004;

        /// <summary>
        /// an math that should be added still exists
        /// </summary>
        public const int ERROR_MATH_ALREADY_EXISTS = 1005;

        /// <summary>
        /// an math that should be should still exists do's not
        /// </summary>
        public const int ERROR_MATH_NOT_EXISTS = 1006;

        /// <summary>
        /// VARIABLES: contains the refrences for all objects
        /// </summary>
        private Hashtable objectReferences = new Hashtable();

        private Hashtable objectStorage = new Hashtable();
        
        /// <summary>
        /// VARIABLES: list of all Int Variables 
        /// </summary>
        public Hashtable globalRenameHash = new Hashtable();
        
        /// <summary>
        /// VARIABLES: list of all variables that cutted out so we clean the sourcecode
        /// </summary>
        public Hashtable globalParamRenameHash = new Hashtable();
        
        /// <summary>
        /// VARIABLES: list of all subscripts defined by methods
        /// </summary>
        public Hashtable namedSubScripts = new Hashtable();
        
        /// <summary>
        /// CONTROL: all subscripts
        /// </summary>
        public Hashtable subScripts = new Hashtable();
        
        /// <summary>
        /// this one contains the Maths himself. this will be used for calculations
        /// </summary>
        private Hashtable calcingBrackets = new Hashtable();
        
        /// <summary>
        /// this one contains the results only so it will be easier to fill up vars
        /// </summary>
        private Hashtable calcingBracketsResults = new Hashtable();

        /// <summary>
        /// is true if some error occurs
        /// </summary>
        private Boolean isError = false;

        /// <summary>
        /// Last error Code
        /// </summary>
        public int lastErrorCode = 0;

        /// <summary>
        /// Last Error Message
        /// </summary>
        public String lastErrorMessage = "";

        /// <summary>
        /// informs about an occured error and resets the error state
        /// </summary>
        /// <returns></returns>
        public Boolean isErrorOccured()
        {
            if (this.isError)
            {
                this.isError = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// clears all stored variables
        /// </summary>
        public void clear()
        {
            this.objectReferences.Clear();
            this.objectStorage.Clear();
            this.globalRenameHash.Clear();
            this.globalParamRenameHash.Clear();

            this.namedSubScripts.Clear();
            this.subScripts.Clear();

            this.calcingBrackets.Clear();
            this.calcingBracketsResults.Clear();
            this.lastErrorCode = 0;
            this.lastErrorMessage = "";
            this.isError = false;
        }


        /// <summary>
        /// adds a Math and stores it by the name
        /// </summary>
        /// <param name="name">Name of the Math</param>
        /// <param name="math">the Math Object</param>
        /// <returns></returns>
        public Boolean addMath(string name, RefScrMath math)
        {
            if (calcingBrackets.ContainsKey(name))
            {
                this.lastErrorMessage = "Math " + name  + " already exists";
                this.lastErrorCode = Projector.Script.Vars.Variables.ERROR_MATH_ALREADY_EXISTS;
                this.isError = true;
                return false;
            }
            this.calcingBrackets.Add(name, math);
            this.calcingBracketsResults.Add(name, math.getResult());
            return true;
        }

        /// <summary>
        /// get an math by the name
        /// </summary>
        /// <param name="name">name of math</param>
        /// <returns></returns>
        public RefScrMath getMath(string name)
        {
            if (this.mathIsStored(name))
            {
                return (RefScrMath)this.calcingBrackets[name];
            }
            this.lastErrorMessage = "Math " + name + " not exists";
            this.lastErrorCode = Projector.Script.Vars.Variables.ERROR_MATH_NOT_EXISTS;
            this.isError = true;
            return null;
        }


        /// <summary>
        /// Check an math already added
        /// </summary>
        /// <param name="name">Name of the Math</param>
        /// <returns></returns>
        public Boolean mathIsStored(string name)
        {
            return this.calcingBrackets.ContainsKey(name);
        }


        /// <summary>
        /// Fiill all Placeholder for content in brackets
        /// </summary>
        /// <param name="source">Code with placeholder</param>
        /// <returns>Code with Brackets and content in brackets</returns>
        public String fillUpMaths(string source)
        {
            return this.fillUpVars(source, this.calcingBracketsResults);
        }

        /// <summary>
        /// Replaces all variables and return the
        /// Result as String. That includes all maths, codelines and
        /// Strings
        /// </summary>
        /// <param name="source">string including placeholders</param>
        /// <returns>string with replaced placeholders</returns>
        public String fillUpAll(string source)
        {
            return
                this.fillUpMaths(
                    this.fillUpCodeLines(
                        this.fillUpStrings(source)
                    )
                );
        }

        /// <summary>
        /// Fills all Placeholder and returns the 
        /// content
        /// </summary>
        /// <param name="source">string with placeholders</param>
        /// <returns>string with replaced placeholders</returns>
        public String fillUpStrings(string source)
        {
            string myStrings = this.fillUpStrings(source, "", "");
            return myStrings;
        }

        /// <summary>
        /// Fills all PlaceHolder and Returns
        /// the Content
        /// </summary>
        /// <param name="source">String wuth Placeholder</param>
        /// <param name="pre">String that will be added before</param>
        /// <param name="post">String that will be added at the end</param>
        /// <returns>String with replaced Placeholders and the Pre and Poststring</returns>
        public String fillUpStrings(string source, String pre, String post)
        {
            return this.fillUpVars(source, this.globalRenameHash, pre, post);
        }

        /// <summary>
        /// Returns the full Code of code with placeholders
        /// or an Placeholder himself
        /// </summary>
        /// <param name="source">Placeholder or Code with placeholders</param>
        /// <returns>Full Code</returns>
        public String fillUpCodeLines(string source)
        {
            return this.fillUpVars(source, this.namedSubScripts);
        }



        /// <summary>
        /// Fill up Placeholder by the given Hashtable
        /// as source for key and value. The key will be the
        /// name of the Placeholder that will be replaced by the 
        /// value.
        /// This Method works recusiv
        /// </summary>
        /// <param name="source">Source with placeholder</param>
        /// <param name="useThis">Hashtable that contans the keys and values</param>
        /// <returns>return filled source</returns>
        public String fillUpVars(string source, Hashtable useThis)
        {
            return this.fillUpVars(source, useThis, "", "");
        }


        /**
         * fills up all placeHolder
         * with content from assigned Hashtable
         */
        public String fillUpVars(string source, Hashtable useThis, string pre, string post)
        {
            string newStr = this.fillUpVarsBack(source, useThis, pre, post);
            string chkStr = source;
            while (newStr != source)
            {
                source = newStr;
                newStr = fillUpVarsBack(source, useThis, pre, post);
            }
            return newStr;
        }

        /**
         * fills placeholder recursiv
         */
        public String fillUpVarsBack(string source, Hashtable useThis, string pre, string post)
        {
            string newSrc = source;
            foreach (DictionaryEntry de in useThis)
            {
                newSrc = newSrc.Replace(de.Key.ToString(), pre + de.Value.ToString() + post);
            }
            return newSrc;
        }

        public String getCodeByName(string name)
        {
            if (namedSubScripts.ContainsKey(name))
            {
                return this.fillUpStrings(namedSubScripts[name].ToString(), "\"", "\"");
            }
            return null;
        }

        public Hashtable getAllStrings()
        {
            Hashtable fullStrings = this.globalRenameHash;
            foreach (DictionaryEntry subScr in this.subScripts)
            {
                ReflectionScript refScr = (ReflectionScript)subScr.Value;
                foreach (DictionaryEntry subVars in refScr.getAllStrings())
                {
                    if (!fullStrings.ContainsKey(subVars.Key))
                    {
                        fullStrings.Add(subVars.Key, subVars.Value);
                    }
                }
            }
            return fullStrings;
            //return this.globalRenameHash;
        }
        // check if the variable exists from the given object

        public Boolean varExists(string name)
        {
            return this.globalRenameHash.ContainsKey("&" + name);
        }

        /// <summary>
        /// check if an object key used
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Boolean objectIsStored(string name)
        {
            return this.objectStorage.ContainsKey(name);
        }

        /// <summary>
        /// return all Object refrences
        /// </summary>
        /// <returns></returns>
        public Hashtable getObjects()
        {
            return this.objectStorage;
        }

        /// <summary>
        /// return all object references
        /// </summary>
        /// <returns></returns>
        public Hashtable getObjectsReference()
        {
            return this.objectReferences;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public String getObjectsReference(string name)
        {
            if (this.objectIsStored(name))
                return (string)this.objectReferences[name];
            else
                return null;
        }

        public void createObject(string name, Object obj, string type)
        {
            if (!objectStorage.ContainsKey(name))
            {
                objectStorage.Add(name, obj);
            }
            if (!objectReferences.ContainsKey(name))
            {
                objectReferences.Add(name, type);
            }
        }

        public Object getRegisteredObject(string name)
        {
            if (this.objectStorage.ContainsKey(name))
            {
                return (object)this.objectStorage[name];
            }
            return null;
        }

        public Boolean updateExistingObject(String name, Object value, Boolean selfInc)
        {
            
            Boolean found = false;
            /*
            if (globalRenameHash.ContainsKey("&" + name))
            {
                found = true;
                if (selfInc)
                {
                    globalRenameHash["&" + name] += value.ToString();
                }
                else
                {
                    globalRenameHash["&" + name] = value.ToString();
                }            
            }
            */

            if (objectStorage.ContainsKey(name) && this.objectStorage.ContainsKey(name))
            {
                found = true;
                if (!selfInc)
                {
                    Type valueType = value.GetType();
                    Type originType = this.objectStorage[name].GetType();
                    if (valueType == originType)
                    {
                        this.objectStorage[name] = value;

                        if (globalRenameHash.ContainsKey("&" + name))
                            globalRenameHash["&" + name] = value;
                    }
                    else
                    {
                        this.lastErrorCode = Projector.Script.Vars.Variables.ERROR_INCOMPATIBLE_TYPE;
                        this.lastErrorMessage = "Cast Error " + value + " can not converted from " + valueType.Name + " in to " + originType.Name;
                        this.isError = true;
                        return false;
                    }
                  
                    /*
                    if (this.debugMode)
                    {
                        this.updateDebugMessage("&" + name, value.ToString());
                    }*/
                }
                else
                {
                    if (value is String && this.objectStorage[name] is String)
                    {
                        String store = (string)this.objectStorage[name];
                        store += value;
                        this.objectStorage[name] = value;

                        if (globalRenameHash.ContainsKey("&" + name))
                            globalRenameHash["&" + name] = store;
                    }
                    else if (value is String && this.objectStorage[name] is int)
                    {
                        int store = (int)this.objectStorage[name];
                        try
                        {
                            store += int.Parse(value.ToString());
                        }
                        catch (Exception ex)
                        {
                            this.lastErrorCode = Projector.Script.Vars.Variables.ERROR_INCOMPATIBLE_TYPE;
                            this.lastErrorMessage = "Cast Error. " + value + " can to be converted to INT. " + ex.Message;
                            this.isError = true;
                            return false;
                        }
                        
                        this.objectStorage[name] = store;
                        if (globalRenameHash.ContainsKey("&" + name))
                            globalRenameHash["&" + name] = store;
                    }
                    else if (value is int && this.objectStorage[name] is int)
                    {
                        int store = (int)this.objectStorage[name];
                        store += (int)value;
                        this.objectStorage[name] = store;
                        if (globalRenameHash.ContainsKey("&" + name))
                            globalRenameHash["&" + name] = store;
                    }
                    else if (value is Double && this.objectStorage[name] is int)
                    {
                        /*
                        int store = (int)this.objectStorage[name];
                        Double pVal = (Double) value;
                        store += (int) Math.Round( pVal );
                        this.objectStorage[name] = store;
                        if (globalRenameHash.ContainsKey("&" + name))
                            globalRenameHash["&" + name] = store;
                         */
                        this.lastErrorCode = Projector.Script.Vars.Variables.ERROR_INCOMPATIBLE_TYPE;
                        this.lastErrorMessage = "Type Double can not be used for Type Integer.";
                        this.isError = true;

                    }
                    else if (value is String && this.objectStorage[name] is Double)
                    {
                        Double store = (Double)this.objectStorage[name];
                        try
                        {
                            Double pVal = Double.Parse(value.ToString());
                            store += pVal;
                        }
                        catch (Exception ex)
                        {
                            this.lastErrorCode = Projector.Script.Vars.Variables.ERROR_INCOMPATIBLE_TYPE;
                            this.lastErrorMessage = "Cast Error. " + value + " can to be converted to Double. " + ex.Message;
                            this.isError = true;
                            return false;
                        }
                        
                        this.objectStorage[name] = store;
                        if (globalRenameHash.ContainsKey("&" + name))
                            globalRenameHash["&" + name] = store;
                    }
                    else
                    {
                        /* this.addError("this type can not be incremented");*/
                        this.lastErrorCode = Projector.Script.Vars.Variables.ERROR_TYPE_NOT_INC_COMPATIBLE;
                        this.lastErrorMessage = "This Type can't be incremented";
                        this.isError = true;
                        found = false;
                    }
                }


                foreach (DictionaryEntry scrpt in this.subScripts)
                {
                    ReflectionScript refScr = (ReflectionScript)scrpt.Value;
                    if (refScr.scrVars.objectIsStored("parent." + name))
                    {
                        refScr.scrVars.updateExistingObject("parent." + name, this.objectStorage[name]);
                    }
                    if (refScr.scrVars.objectIsStored(name))
                    {
                        refScr.scrVars.updateExistingObject(name, this.objectStorage[name]);
                    }
                }


                /*
                if (this.debugMode)
                {
                    this.updateDebugMessage(name, value.ToString());
                }*/
            }
            
            if (found == false)
            {                
                this.lastErrorCode = Projector.Script.Vars.Variables.ERROR_UNKNOW_TARGET;
                this.lastErrorMessage = "variable &" + name + " not existing/writable";
                this.isError = true;
            }
            return found;

        }

        public void updateVarByObject(String name, Object obj)
        {
            this.updateVarByObject(name, obj, false);
        }

        public void updateVarByObject(String name, Object obj, Boolean selfInc)
        {

            this.updateExistingObject(name, obj, selfInc);

        }


        public void updateVarByMath(String name, RefScrMath math)
        {
            this.updateExistingObject(name, math.getResult());

        }

        public void updateVarByMath(String name, RefScrMath math, Hashtable varTable)
        {

            if (varTable.ContainsKey("&" + name))
            {
                varTable["&" + name] = math.getResult();
            }
        }

        public void updateExistingObject(String name, Object value)
        {
            this.updateExistingObject(name, value, false);
        }


    }


}
