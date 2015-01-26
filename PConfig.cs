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
        private String NameSpace = "projector";
        private PConfigContent Configuration;
        private PConfigContent WalkingLive;
        private ConfigSerializer fileHandle = new ConfigSerializer();

        public PConfig() { }
       
        public PConfig(SerializationInfo info, StreamingContext ctxt)
        {

        }

        public void loadRuntimeConfig()
        {
            if (System.IO.File.Exists( this.getDefaultFilename() ))
            {
                this.Configuration = fileHandle.DeSerializeObject(this.getDefaultFilename());
            }

            if (this.Configuration == null)
            {
                this.initBaseConfig();
            }
            this.resetReader();
        }

        private void initBaseConfig(){
            this.Configuration = new PConfigContent();
            this.Configuration.addGroupChild(this.NameSpace);
        }

        public void saveRuntimeConfig()
        {
            if (this.Configuration != null)
            {
                fileHandle.SerializeObject(this.getDefaultFilename(), this.Configuration);
            }
            
        }

        public PConfigContent getConfig()
        {
            return this.Configuration;
        }

        public void resetReader()
        {
            this.WalkingLive = this.Configuration;

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

            
           
   
            // add this node if not exists
            if (this.WalkingLive.getName() == name)
            {
                return this.WalkingLive.getContent();
               
            }
            

         
            string[] keyChain = name.Split('.');
            string parentKey = keyChain[0];
            if (keyChain.Count() > 1)
            {
            
                PConfigContent pTmp = this.WalkingLive.getChildByName(parentKey);
                if (pTmp == null)
                {
                    this.WalkingLive.addGroupChild(parentKey);
                    this.WalkingLive = this.WalkingLive.getChildByName(parentKey);
                }
                string nextKey = "";

                for (int i = 1; i < keyChain.Count(); i++)
                {
                    nextKey += keyChain[i];
                }
                return this.getSettingWidthDefault(nextKey, StoreValue, false);
            }
            else
            {
                this.WalkingLive.SetFlatConfig(name, StoreValue);
                return StoreValue;
            }
        }

        public String getConfigByKey(string key)
        {
            string[] keyChain = key.Split('.');

            string parentKey = keyChain[0]; 
            if (keyChain.Count() > 1)
            {
                
                if (!this.WalkingLive.ChildExists())
                {
                    return null;
                }
                this.WalkingLive = this.WalkingLive.getChildByName(parentKey);
                string nextKey = "";
                for (int i = 1; i < keyChain.Count(); i++)
                {
                    nextKey += keyChain[i];
                }
                return this.getConfigByKey(nextKey);
            }
            else
            {
                return this.WalkingLive.getContent();
            }
        }

        public PConfigContent getOriginConfigByKey(string key)
        {
            string[] keyChain = key.Split('.');

            string parentKey = keyChain[0];
            if (keyChain.Count() > 1)
            {

                if (!this.WalkingLive.ChildExists())
                {
                    return null;
                }
                this.WalkingLive = this.WalkingLive.getChildByName(parentKey);
                string nextKey = "";
                for (int i = 1; i < keyChain.Count(); i++)
                {
                    nextKey += keyChain[i];
                }
                return this.getOriginConfigByKey(nextKey);
            }
            else
            {
                return this.WalkingLive;
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
