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
        /// <summary>
        /// Name of this Node
        /// </summary>
        private String Name = "Unamed";

        /// <summary>
        /// simple content stored as string
        /// </summary>
        private String FlatContent;

        /// <summary>
        /// List of subcontents
        /// </summary>
        private List<PConfigContent> Content;
        /// <summary>
        /// the parent node
        /// </summary>
        private PConfigContent parentNode;

        public static string password;

        public PConfigContent()
        {
            
        }

        /// <summary>
        /// returns decypted flat content by using already known password
        /// 
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="info"></param>
        /// <returns>decrypted string</returns>
        private string getDeCryptStringContent(string valueName,SerializationInfo info)
        {
            string str = (string)info.GetValue(valueName, typeof(string));
            if (PConfigContent.password != null && str != null && PConfigContent.password.Length >= PrCrypt.MIN_LENGTH && !PrCrypt.error)
            {
                str = PrCrypt.Decrypt(str, PConfigContent.password);
            }
            return str;
        }

        /// <summary>
        /// returns the encrypted string. for enyryption
        /// the password must be already set
        /// </summary>
        /// <param name="str">the string that should be encrypted</param>
        /// <returns></returns>
        private string getCryptStringContent(string str)
        {            
            if (PConfigContent.password != null && str != null && PConfigContent.password.Length >= PrCrypt.MIN_LENGTH && !PrCrypt.error)
            {
                str = PrCrypt.Encrypt(str, PConfigContent.password);
            }
            return str;
        }

        /// <summary>
        /// serialisation constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public PConfigContent(SerializationInfo info, StreamingContext ctxt)
        {
            this.Name = this.getDeCryptStringContent("Name",info);
            this.FlatContent = this.getDeCryptStringContent("FlatContent", info); 
            this.Content = (List<PConfigContent>)info.GetValue("Childs", typeof(List<PConfigContent>));
            this.parentNode = (PConfigContent)info.GetValue("parent", typeof(PConfigContent));
        }

        /// <summary>
        /// Serialisation Object getter
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Name", this.getCryptStringContent( this.Name));
            info.AddValue("FlatContent", this.getCryptStringContent( this.FlatContent) );
            info.AddValue("Childs", this.Content);
            info.AddValue("parent", this.parentNode, typeof(PConfigContent));
        }

        /// <summary>
        /// sets the simple content stored as string
        /// and rename this node
        /// </summary>
        /// <param name="Name">New Name od this node </param>
        /// <param name="Content">the new Flat Content</param>
        public void SetFlatConfig(String Name, String Content)
        {
            this.Name = Name;
            this.FlatContent = Content;
            
        }

        /// <summary>
        /// checks if some childs existing
        /// </summary>
        /// <returns>true if childs readalbe</returns>
        public Boolean ChildExists()
        {
            return this.Content != null && this.Content.Count > 0;
        }

        /// <summary>
        /// checks if childs usable.
        /// this not the same as ChildExists because
        /// this method do not checks if childs already exists
        /// </summary>
        /// <returns>true if child initialized</returns>
        public Boolean ChildUsable()
        {
            return this.Content != null;
        }

        /// <summary>
        /// change the name of this node
        /// </summary>
        /// <param name="name"></param>
        public void changeName(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Update the Flat Content
        /// </summary>
        /// <param name="value">string value</param>
        public void Update(string value)
        {
            this.FlatContent = value;
        }

        /// <summary>
        /// Updates the Flat Content with an List of strings
        /// </summary>
        /// <param name="values">List of strings</param>
        public void Update(List<string> values)
        {
            this.Update(values, true);
        }

        /// <summary>
        /// remove a named child if exists
        /// </summary>
        /// <param name="name">the name of the child that should be Removed</param>
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

        /// <summary>
        /// Updates a chunk of strings
        /// </summary>
        /// <param name="values">List of stings</param>
        /// <param name="overWrite">if true the content will replaced</param>
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

        /// <summary>
        /// returns the content as list of strings
        /// independend from sontent.       
        /// </summary>
        /// <returns>List of strings builded from names</returns>
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

        /// <summary>
        /// Updates an existing child just
        /// with an string value
        /// </summary>
        /// <param name="childName">the name of the existing child</param>
        /// <param name="value">the value as string</param>
        /// <returns>if false if the child not exists</returns>
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

        /// <summary>
        /// returns the name of the node
        /// </summary>
        /// <returns></returns>
        public String getName()
        {
            return this.Name;
        }

        /// <summary>
        /// returns the string content
        /// </summary>
        /// <returns>content as string</returns>
        public String getContent()
        {
            return this.FlatContent;
        }

        /// <summary>
        /// get the child by name
        /// </summary>
        /// <param name="name">name of the existing child</param>
        /// <returns>return the child or null if not exists</returns>
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

        /// <summary>
        /// gets the index of child by name
        /// </summary>
        /// <param name="name">Name of the existing child</param>
        /// <returns>the position in Content Hash</returns>
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

        /// <summary>
        /// gets the group by name or add en empty group
        /// if not exists
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>the existing or new created Group</returns>
        public PConfigContent addOrGetGroupChild(String Name)
        {
            PConfigContent addConfig = this.getChildByName(Name);
            if (addConfig == null)
            {
                return this.addGroupChild(Name);
            }
            return addConfig;
        }

        /// <summary>
        /// adds an empty child group if this group not already exists.        
        /// </summary>
        /// <param name="Name">the name of the childgroup</param>
        /// <returns>returns null if the group already exists</returns>
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
