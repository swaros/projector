using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Projector
{
    [Serializable()]
    class PConfig : ISerializable
    {
        private String NameSpace = "client";
        private static PConfigContent Configuration;
        private PConfigContent WalkingLive;
        private ConfigSerializer fileHandle = new ConfigSerializer();

        public PConfig() { }

        public String getNameSpace()
        {
            return this.NameSpace;
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
