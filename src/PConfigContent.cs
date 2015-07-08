using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Projector.Crypt;

namespace Projector
{
    [Serializable()]
    class PConfigContent : ISerializable
    {

        private String Name = "Unamed";
        private String FlatContent;
        private List<PConfigContent> Content;
        private PConfigContent parentNode;

        public static string password;

        public PConfigContent()
        {
            
        }

        private string getDeCryptStringContent(string valueName,SerializationInfo info)
        {
            string str = (string)info.GetValue(valueName, typeof(string));
            if (PConfigContent.password != null && str != null && PConfigContent.password.Length >= PrCrypt.MIN_LENGTH && !PrCrypt.error)
            {
                str = PrCrypt.Decrypt(str, PConfigContent.password);
            }
            return str;
        }

        private string getCryptStringContent(string str)
        {            
            if (PConfigContent.password != null && str != null && PConfigContent.password.Length >= PrCrypt.MIN_LENGTH && !PrCrypt.error)
            {
                str = PrCrypt.Encrypt(str, PConfigContent.password);
            }
            return str;
        }

        public PConfigContent(SerializationInfo info, StreamingContext ctxt)
        {
            this.Name = this.getDeCryptStringContent("Name",info);
            this.FlatContent = this.getDeCryptStringContent("FlatContent", info); 
            this.Content = (List<PConfigContent>)info.GetValue("Childs", typeof(List<PConfigContent>));
            this.parentNode = (PConfigContent)info.GetValue("parent", typeof(PConfigContent));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Name", this.getCryptStringContent( this.Name));
            info.AddValue("FlatContent", this.getCryptStringContent( this.FlatContent) );
            info.AddValue("Childs", this.Content);
            info.AddValue("parent", this.parentNode, typeof(PConfigContent));
        }

        public void SetFlatConfig(String Name, String Content)
        {
            this.Name = Name;
            this.FlatContent = Content;
            
        }

        public Boolean ChildExists()
        {
            return this.Content != null && this.Content.Count > 0;
        }

        public Boolean ChildUsable()
        {
            return this.Content != null;
        }

        public void changeName(string name)
        {
            this.Name = name;
        }

        public void Update(string value)
        {
            this.FlatContent = value;
        }

        public void Update(List<string> values)
        {
            this.Update(values, true);
        }

        public void RemoveChild(string name)
        {
            
            if (this.ChildUsable())
            {
                 List<string> Cont =  this.getContentAsList();
                 if (Cont != null && Cont.Contains(name))
                 {
                     int index = this.getIndexByName(name);
                     this.Content.RemoveAt(index);
                 }
            }
        }


        public void Update(List<string> values, Boolean overWrite)
        {

            if (overWrite)
            {
                if (values == null)
                {
                    this.Content = null;
                    return;
                }
                this.Content = new List<PConfigContent>();
            }

            foreach (string val in values)
            {
                if (overWrite || this.getIndexByName(val) == -1 )
                    this.addGroupChild(val);               
            }
        }

        public List<String> getContentAsList()
        {
            if (this.ChildUsable())
            {
                List<String> result = new List<string>();
                foreach (PConfigContent cont in this.Content)
                {
                    result.Add(cont.Name);
                }
                return result;
            }
            return null;
        }


        public Boolean UpdateFlatChild(string childName, string value)
        {
            PConfigContent content = this.getChildByName(childName);
            if (content != null)
            {
               content.Update(value);
               return true;
            }
            return false;
        }

        public String getName()
        {
            return this.Name;
        }

        public String getContent()
        {
            return this.FlatContent;
        }

        public PConfigContent getChildByName(string name)
        {
            if (this.ChildUsable())
            {
                for (int i = 0; i < this.Content.Count; i++)
                {
                    if (this.Content[i].getName() == name)
                    {
                        return this.Content[i];
                    }
                }
            }
            return null;
        }

        public int getIndexByName(string name)
        {
            if (this.ChildUsable())
            {
                for (int i = 0; i < this.Content.Count; i++)
                {
                    if (this.Content[i].getName() == name)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public PConfigContent addOrGetGroupChild(String Name)
        {
            PConfigContent addConfig = this.getChildByName(Name);
            if (addConfig == null)
            {
                return this.addGroupChild(Name);
            }
            return addConfig;
        }

        public PConfigContent addGroupChild(String Name)
        {
            if (this.Content == null)
            {
                this.Content = new List<PConfigContent>();
            }
            if (this.getChildByName(Name) == null)
            {
                PConfigContent addConfig = new PConfigContent();
                addConfig.SetFlatConfig(Name, null);
                this.Content.Add(addConfig);                
                return addConfig;
            }
            return null;
        }
    }
}
