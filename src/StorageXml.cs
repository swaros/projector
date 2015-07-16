using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;


namespace Projector.Storage
{
    class StorageXml
    {
        /// <summary>
        /// the base XmlDocument
        /// </summary>
        private XmlDocument doc = new XmlDocument();

        /// <summary>
        /// the name of the content attribute
        /// </summary>
        private string contentKey = "cnt";

        /// <summary>
        /// the name of the name attribute 
        /// </summary>
        private string nameKey = "name";

        /// <summary>
        /// last error message (saving error)
        /// </summary>
        public string lastError = "";

        /// <summary>
        /// add an attribute to the XmlNode
        /// </summary>
        /// <param name="node">the used XmlNode</param>
        /// <param name="key">Name of the Attribute</param>
        /// <param name="value">Content of the Attribute</param>
        private void addAttribute(XmlNode node, string key, string value)
        {
            XmlAttribute nodeAttr = this.doc.CreateAttribute(key);
            nodeAttr.InnerText = value;
            node.Attributes.Append(nodeAttr);
        }

        /// <summary>
        /// returns just chas that can be used as an xml node.
        /// if no chas in there it returns 'cfg' as name
        /// </summary>
        /// <param name="Input">a string</param>
        /// <returns>string with chars only</returns>
        private string getValidXmlName(string Input)
        {
            Regex rgx = new Regex("[^a-zA-Z]");
            string str = rgx.Replace(Input, "");

            if (str != "")
            {
                return str;
            }
            return "cfg";
        }

        /// <summary>
        /// creates an node by the given Configuration
        /// </summary>
        /// <param name="config">Config node</param>
        /// <returns>created xml Node</returns>
        private XmlNode createNodeFromConfig(PConfigContent config)
        {
            string name = config.getName();
            if (name == "")
            {
                return null;
            }
            XmlNode newNode = this.doc.CreateElement(this.getValidXmlName(name));
            this.addAttribute(newNode, this.contentKey, config.getContent());
            this.addAttribute(newNode, this.nameKey, name);

            if (config.ChildExists())
            {
                List<string> childs = config.getContentAsList();
                if (childs != null)
                {
                    for (int i = 0; i < childs.Count; i++)
                    {
                        PConfigContent addConf = config.getChildByName(childs[i]);
                        XmlNode addNode = this.createNodeFromConfig(addConf);
                        if (addNode != null)
                        {
                            newNode.AppendChild(addNode);
                        }
                        
                    }
                }
            }

            return newNode;
        }

        /// <summary>
        /// add a Configuration and fills
        /// the xml coument with content from this
        /// </summary>
        /// <param name="config"></param>
        public void addNodeByConfig(PConfigContent config)
        {
            XmlNode node = this.createNodeFromConfig(config);
            if (node != null)
            {
                this.doc.AppendChild(node);
            }
        }

        /// <summary>
        /// save the xml file
        /// </summary>
        /// <param name="filename"></param>
        public void save(string filename)
        {
            try
            {
                doc.Save(filename);

            }
            catch (Exception ex)
            {
                lastError = ex.Message;
            }
        }


    }
}
