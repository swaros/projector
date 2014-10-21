using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;


namespace Projector
{
    class XmlSetup
    {
        private string fullFilename = null;

        private string filename = @"projector_default.xml";
        private string nodeName = "options";
        private Hashtable settings = new Hashtable();

        public string lastError = "";

        public Int64 count = 0;

        //private LinkedList<

        private string getFileName(){

            string filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + System.IO.Path.DirectorySeparatorChar  + this.filename;
            this.fullFilename = filename;
            return filename;
        }

        public void setFileName(string filename)
        {
            this.filename = filename;
            this.getFileName();
        }

        public Hashtable getHashMap()
        {
            return this.settings;
        }


        public void setFullFilename(string filename)
        {
            this.fullFilename = filename;
        }

        public bool removeSetting(string name)
        {
            if (settings.ContainsKey(name))
            {
                settings.Remove(name);
                return true;
            }
            return false;
        }


        public void addSetting(string name, string value){

            if (settings.ContainsKey(name))
            {
                settings[name] = value;
            }
            else
            {

                try
                {
                    settings.Add(name, value);
                    this.count++;
                }
                catch
                {

                }
            }
        }

        public string getValue(string keyName)
        {
            if (settings.ContainsKey(keyName))
                return this.settings[keyName].ToString();
            else
                return null;
            
        }
       

        public string getSetting(string name)
        {
            if (settings.ContainsKey(name)) return settings[name].ToString();
            else return null;
        }

        public XmlSetup copyValues(XmlSetup target)
        {
            foreach (DictionaryEntry de in this.settings)
            {
                string keyname = de.Key.ToString();                
                string value = de.Value.ToString();

                target.addSetting(keyname, value);
            }
            return target;
        }

        public void exportXml(string filename)
        {
            string origFilename = this.fullFilename;
            this.fullFilename = filename;
            saveXml();
            this.fullFilename = origFilename;
        }

        public void addAsXmlNode(XmlDocument doc, XmlNode catNode)
        {
            
            ICollection keyColl = settings.Keys;
            XmlNode myRoot, myNode;
            XmlAttribute myAttribute;
            foreach (DictionaryEntry de in this.settings)
            {
                
                myNode = doc.CreateElement(de.Key.ToString());
                myAttribute = doc.CreateAttribute("value");
                myAttribute.InnerText = de.Value.ToString();
                myNode.Attributes.Append(myAttribute);
                catNode.AppendChild(myNode);
                this.count++;
            }

        }


        public void saveXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode myRoot, myNode, catNode;
            XmlAttribute myAttribute;
            

            myRoot = doc.CreateElement("Projector");
            doc.AppendChild(myRoot);
            ICollection keyColl = settings.Keys;

            this.count = 0;
            catNode = doc.CreateElement(this.nodeName);
            //myAttribute = doc.CreateAttribute(de.Key.ToString());
            //myAttribute.InnerText = de.Value.ToString();
            //catNode.Attributes.Append(myAttribute);
            foreach (DictionaryEntry de in this.settings)
            {               
                myNode = doc.CreateElement(de.Key.ToString());
                myAttribute = doc.CreateAttribute("value");
                myAttribute.InnerText = de.Value.ToString();
                myNode.Attributes.Append(myAttribute);
                catNode.AppendChild(myNode);
                this.count++;
            }
            myRoot.AppendChild(catNode);

           
           
            try
            {
                doc.Save(this.fullFilename);
                
            }
            catch (Exception ex)
            {
                lastError = ex.Message;



                //MessageBox.Show("Error writing " + filename);
            }
        }

        public void loadXml()
        {
            if (File.Exists(this.fullFilename))
            {
                XmlTextReader textReader = new XmlTextReader(this.fullFilename);
                textReader.Read();
                string NodeKey = "";
                string NodeName = "";
                this.count = 0;
                settings.Clear();
                while (textReader.Read())
                {
                    textReader.MoveToElement();
                    XmlNodeType nType = textReader.NodeType;
                    if (textReader.Depth == 2)
                    {
                        if (textReader.AttributeCount > 0)
                        {
                            NodeKey = textReader.GetAttribute(0).ToString();
                            NodeName = textReader.Name.ToLower();
                            try
                            {
                                settings.Add(NodeName, NodeKey);
                                this.count++;
                            }
                            catch (IOException ex)
                            {
                                lastError = ex.Message;
                            }

                        }
                    }

                }
                textReader.Close();
            }
        }

    }
}
