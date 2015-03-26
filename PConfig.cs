using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Projector.Crypt;

namespace Projector
{
    [Serializable()]
    class PConfig : ISerializable
    {
        /// <summary>
        /// the main profil key
        /// </summary>
        public const string KEY_MAIN = "client";

        /// <summary>
        /// the key for all the  database profiles.
        /// </summary>
        public const string KEY_PROFILS = PConfig.KEY_MAIN + ".profiles";

        /// <summary>
        /// the main key for groups
        /// </summary>
        public const string KEY_GROUPS = PConfig.KEY_MAIN + ".groups";

        /// <summary>
        /// the main key for groups names
        /// </summary>
        public const string KEY_GROUPS_NAMES = PConfig.KEY_MAIN + "." + PConfig.KEY_GROUPS + ".names";

        /// <summary>
        /// the main key for groups names
        /// </summary>
        public const string KEY_GROUPS_MEMBERS = PConfig.KEY_MAIN + "." + PConfig.KEY_GROUPS + ".members";

        private String NameSpace = PConfig.KEY_MAIN;
        private static PConfigContent Configuration;
        private PConfigContent WalkingLive;
        private ConfigSerializer fileHandle = new ConfigSerializer();

        public Boolean removeEmptyChilds = false;

        public PConfig() { }

        public String getNameSpace()
        {
            return this.NameSpace;
        }

        public void setPassword(string password)
        {
            PConfigContent.password = password;
        }

        public PConfig(SerializationInfo info, StreamingContext ctxt)
        {

        }

        public void loadRuntimeConfig()
        {
            if (System.IO.File.Exists( this.getDefaultFilename() ))
            {
                PConfig.Configuration = fileHandle.DeSerializeObject(this.getDefaultFilename());
            }

            if (PConfig.Configuration == null)
            {
                this.initBaseConfig();
            }
            this.resetReader();
        }

        private void initBaseConfig(){
            PConfig.Configuration = new PConfigContent();
            PConfig.Configuration.SetFlatConfig(this.NameSpace, null);
            
        }

        public void saveRuntimeConfig()
        {
            if (PConfig.Configuration != null)
            {                
                fileHandle.SerializeObject(this.getDefaultFilename(), PConfig.Configuration);                
            }
            
        }

        public Boolean loadConfigFromFile(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                PConfig.Configuration = fileHandle.DeSerializeObject(filename);
                if (PConfig.Configuration == null)
                {
                    this.initBaseConfig();
                }
                this.resetReader();
                return true;
            }

            return false;
        }

        public void saveConfigToFile(string filename)
        {
            if (PConfig.Configuration != null)
            {
                fileHandle.SerializeObject(filename, PConfig.Configuration);
            }
        }

        public PConfigContent getConfig()
        {
            return PConfig.Configuration;
        }

        public void resetReader()
        {
            this.WalkingLive = PConfig.Configuration;

        }

        public int getIntSettingWidthDefault(string name, int StoreValue)
        {
            string strVal = this.getSettingWidthDefault(name, StoreValue.ToString());            
            try
            {
                return int.Parse(strVal);
            }
            catch (Exception e)
            {
                return StoreValue;
            }
        }

        public Boolean getBooleanSettingWidthDefault(string name, Boolean StoreValue)
        {
            String value = "0";
            if (StoreValue)
            {
                value = "1";
            }
            string strVal = this.getSettingWidthDefault(name, value);
            if (strVal == "1")
                return true;
            else
                return false;
        }

        public string getSettingWidthDefault(string name, string StoreValue)
        {
            return getSettingWidthDefault(name, StoreValue, true);
        }

        public string getSettingWidthDefault(string name, string StoreValue, Boolean resetReader){
            if (resetReader)
            {
                this.resetReader();
            }
       
            string[] keyChain = name.Split('.');
            string parentKey = keyChain[0];
            if (keyChain.Count() > 1)
            {
                if (parentKey == this.WalkingLive.getName())
                {
                    string nextStepKey = keyChain[1];
                    this.WalkingLive = this.WalkingLive.addOrGetGroupChild(nextStepKey);
                    
                    string nextKey = "";
                    string add = "";
                    for (int i = 1; i < keyChain.Count(); i++)
                    {
                        nextKey += add + keyChain[i];
                        add = ".";
                    }
                    return this.getSettingWidthDefault(nextKey, StoreValue, false);
                }
                else
                {
                    throw new Exception("There is no Setting named " + name);
                }
            }
            else
            {
                if (this.WalkingLive.getName() == name)
                {
                    string upd = this.WalkingLive.getContent();
                    if (upd == null)
                    {
                        this.WalkingLive.Update(StoreValue);
                        return StoreValue;
                    }
                    return upd;
                }
                else
                {
                    throw new Exception("There is no Setting named " + name);
                }
                
            }
        }

        public List<String> getListWidthDefault(string name, List<String> StoreValue)
        {
            return this.getListWidthDefault(name, StoreValue, true);
        }

        public List<String> getListWidthDefault(string name, List<String> StoreValue, Boolean resetReader)
        {
            if (resetReader)
            {
                this.resetReader();
            }

            string[] keyChain = name.Split('.');
            string parentKey = keyChain[0];
            if (keyChain.Count() > 1)
            {
                if (parentKey == this.WalkingLive.getName())
                {
                    string nextStepKey = keyChain[1];
                    this.WalkingLive = this.WalkingLive.addOrGetGroupChild(nextStepKey);

                    string nextKey = "";
                    string add = "";
                    for (int i = 1; i < keyChain.Count(); i++)
                    {
                        nextKey += add + keyChain[i];
                        add = ".";
                    }
                    return this.getListWidthDefault(nextKey, StoreValue, false);
                }
                else
                {
                    throw new Exception("There is no Setting named " + name);
                }
            }
            else
            {
                if (this.WalkingLive.getName() == name)
                {
                    List<string> upd = this.WalkingLive.getContentAsList();
                    if (upd == null || upd.Count() == 0)
                    {
                        this.WalkingLive.Update(StoreValue);
                        return StoreValue;
                    }
                    return upd;
                }
                else
                {
                    throw new Exception("There is no Setting named " + name);
                }

            }
        }



        public void setValue(string name, Boolean StoreValue)
        {
            if (StoreValue)
                this.setValue(name, "1", true);
            else
                this.setValue(name, "0", true);
        }

        public void setValue(string name, int StoreValue)
        {
            this.setValue(name, StoreValue.ToString(), true);
        }

        public void setValue(string name, string StoreValue)
        {
            this.setValue(name, StoreValue, true);
        }

        

        public void setValue(string name, string StoreValue, Boolean resetReader)
        {
            if (resetReader)
            {
                this.resetReader();
            }

            // add this node if not exists
            if (this.WalkingLive.getName() == name)
            {
                this.WalkingLive.Update(StoreValue);
                return;
            }

            string[] keyChain = name.Split('.');
            string parentKey = keyChain[0];
            if (keyChain.Count() > 1)
            {

                if (parentKey == this.WalkingLive.getName())
                {
                    string nextStepKey = keyChain[1];
                    this.WalkingLive = this.WalkingLive.addOrGetGroupChild(nextStepKey);

                    string nextKey = "";

                    string add = "";
                    for (int i = 1; i < keyChain.Count(); i++)
                    {
                        nextKey += add + keyChain[i];
                        add = ".";
                    }
                    this.setValue(nextKey, StoreValue, false);
                }
                else
                {
                    throw new Exception("There is no Setting named " + name);
                }

            }
            else
            {
                if (this.WalkingLive.getName() == name)
                {
                    this.WalkingLive.Update(StoreValue);

                }
                else
                {
                    throw new Exception("There is no Setting named " + name);
                }
            }
        }

        public void setName(string name, string newName)
        {
            this.setName(name, newName, true);
        }

        public void setName(string name, string newName, Boolean resetReader)
        {
            if (resetReader)
            {
                this.resetReader();
            }

            // add this node if not exists
            if (this.WalkingLive.getName() == name)
            {
                this.WalkingLive.changeName(newName);
                return;
            }

            string[] keyChain = name.Split('.');
            string parentKey = keyChain[0];
            if (keyChain.Count() > 1)
            {

                if (parentKey == this.WalkingLive.getName())
                {
                    string nextStepKey = keyChain[1];
                    this.WalkingLive = this.WalkingLive.addOrGetGroupChild(nextStepKey);

                    string nextKey = "";

                    string add = "";
                    for (int i = 1; i < keyChain.Count(); i++)
                    {
                        nextKey += add + keyChain[i];
                        add = ".";
                    }
                    this.setName(nextKey, newName, false);
                }
                else
                {
                    throw new Exception("There is no Setting named " + name);
                }

            }
            else
            {
                if (this.WalkingLive.getName() == name)
                {
                    this.WalkingLive.changeName(newName);

                }
                else
                {
                    throw new Exception("There is no Setting named " + name);
                }
            }
        }



        public void setList(string name, List<String> StoreValue)
        {
            this.setList(name, StoreValue, true);
        }

        public void setList(string name, List<String> StoreValue, Boolean resetReader)
        {
            if (resetReader)
            {
                this.resetReader();
            }

            // add this node if not exists
            if (this.WalkingLive.getName() == name)
            {
                this.WalkingLive.Update(StoreValue);
                return;
            }

            string[] keyChain = name.Split('.');
            string parentKey = keyChain[0];
            if (keyChain.Count() > 1)
            {

                if (parentKey == this.WalkingLive.getName())
                {
                    string nextStepKey = keyChain[1];
                    this.WalkingLive = this.WalkingLive.addOrGetGroupChild(nextStepKey);

                    string nextKey = "";

                    string add = "";
                    for (int i = 1; i < keyChain.Count(); i++)
                    {
                        nextKey += add + keyChain[i];
                        add = ".";
                    }
                    this.setList(nextKey, StoreValue, false);
                }
                else
                {
                    throw new Exception("There is no Setting named " + name);
                }

            }
            else
            {
                if (this.WalkingLive.getName() == name)
                {
                    this.WalkingLive.Update(StoreValue);                    
                    
                }
                else
                {
                    throw new Exception("There is no Setting named " + name);
                }
            }
        }

        public void RemoveChild(string name, string childName)
        {
            this.RemoveChild(name, childName, true);
        }
        public void RemoveChild(string name,string childName, Boolean resetReader)
        {
            if (resetReader)
            {
                this.resetReader();
            }

            // add this node if not exists
            if (this.WalkingLive.getName() == name)
            {
                this.WalkingLive.RemoveChild(childName);
                return;
            }

            string[] keyChain = name.Split('.');
            string parentKey = keyChain[0];
            if (keyChain.Count() > 1)
            {

                if (parentKey == this.WalkingLive.getName())
                {
                    string nextStepKey = keyChain[1];
                    this.WalkingLive = this.WalkingLive.addOrGetGroupChild(nextStepKey);

                    string nextKey = "";

                    string add = "";
                    for (int i = 1; i < keyChain.Count(); i++)
                    {
                        nextKey += add + keyChain[i];
                        add = ".";
                    }
                    this.RemoveChild(nextKey, childName, false);
                }
                else
                {
                    throw new Exception("There is no Setting named " + name);
                }

            }
            else
            {
                if (this.WalkingLive.getName() == name)
                {
                    this.WalkingLive.RemoveChild(childName);

                }
                else
                {
                    throw new Exception("There is no Setting named " + name);
                }
            }
        }

     
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //info.AddValue("bookMarks", this.bookMarks);
        }

        public string getDefaultFilename()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) 
                + System.IO.Path.DirectorySeparatorChar 
                + "projector_runtime.config";
        }


    }
}
