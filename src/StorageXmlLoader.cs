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
    class StorageXmlLoader
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
        /// true after loading a xml
        /// </summary>
        private bool loaded = false;

        /// <summary>
        /// Maps recursiv the xml node to pconfig content
        /// </summary>
        /// <param name="xNode">current xml node</param>
        /// <param name="cNode">curren pConfigContent</param>
        private void mapNodeToConfig(XmlNode xNode, PConfigContent cNode)
        {
            // only valid if we have 2 attributes
            if (xNode != null && xNode.Attributes != null && xNode.Attributes.Count == 2)
            {
                XmlAttribute nameAttr = xNode.Attributes[this.nameKey];
                XmlAttribute valueAttr = xNode.Attributes[this.contentKey];
                if (nameAttr != null)
                {
                    string content = nameAttr.InnerText;
                    cNode.changeName(content);
                }

                if (valueAttr != null)
                {
                    string content = valueAttr.InnerText;
                    if (content != "")
                    {
                        cNode.Update(content);
                    }
                    
                }
                if (xNode.HasChildNodes)
                {
                    for (int i = 0; i < xNode.ChildNodes.Count; i++)
                    {                        
                        PConfigContent subNode = cNode.addGroupChild(xNode.ChildNodes[i].Name);
                        mapNodeToConfig(xNode.ChildNodes[i], subNode);
                    }
                }
            }
        }

        /// <summary>
        /// try to read xml content and map these to
        /// pconfig properties
        /// </summary>
        /// <returns>null on mapping error</returns>
        public PConfigContent tryToReadConfig()
        {
            PConfigContent loadedConf = new PConfigContent();
            if (this.loaded)
            {
                XmlNode first = this.doc.FirstChild;
                this.mapNodeToConfig(first, loadedConf);
                return loadedConf;
            }
            return null;
        }

        /// <summary>
        /// Loads xml file
        /// </summary>
        /// <param name="filename">file path</param>
        /// <returns>false if not exists or parsing error</returns>
        public bool load(string filename)
        {
            if (File.Exists(filename))
            {
                string content = System.IO.File.ReadAllText(filename);
                try
                {
                    this.doc.LoadXml(content);
                }
                catch (Exception ex)
                {
                    lastError = ex.Message;
                    return false;
                }
                this.loaded = true;
                return true;

            }
            return false;
        }

    }
}
