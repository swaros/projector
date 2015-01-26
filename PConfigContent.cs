using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Projector
{
    [Serializable()]
    class PConfigContent : ISerializable
    {

        private String Name = "Unamed";
        private String FlatContent;
        private List<PConfigContent> Content;
        private PConfigContent parentNode;

        public PConfigContent()
        {

        }

        public PConfigContent(SerializationInfo info, StreamingContext ctxt)
        {
            this.Name = (string)info.GetValue("Name", typeof(string));
            this.FlatContent = (string)info.GetValue("FlatContent", typeof(string));
            this.Content = (List<PConfigContent>)info.GetValue("Childs", typeof(List<PConfigContent>));
            this.parentNode = (PConfigContent)info.GetValue("parent", typeof(PConfigContent));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Name", this.Name);
            info.AddValue("FlatContent", this.FlatContent);
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

        public void Update(string value)
        {
            this.FlatContent = value;
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



        public PConfigContent addGroupChild(String Name)
        {
            if (this.Content == null)
            {
                this.Content = new List<PConfigContent>();
            }
            if (this.getChildByName(Name) == null)
            {
                PConfigContent addConfig = new PConfigContent();
                this.Content.Add(addConfig);
                addConfig.SetFlatConfig(Name, null);
                return addConfig;
            }
            return null;
        }
    }
}
