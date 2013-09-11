using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;

namespace Projector
{
    class ConfigProfil
    {
        public string lastError;


        private XmlDocument xmlWork = new XmlDocument();
        

        public void importFromXml(string filename)
        {
            
            if (File.Exists(filename))
            {                                                
                xmlWork.LoadXml(File.ReadAllText(filename));
                for (int i = 0; i < xmlWork.ChildNodes.Count; i++)
                {
                    getSettings(xmlWork.ChildNodes[i]);
                }                                
            }

        }

        public List<string> getImportableProfiles(string filename)
        {

            List<string> profileListing = new List<string>();
            if (File.Exists(filename))
            {
                xmlWork.LoadXml(File.ReadAllText(filename));
                for (int i = 0; i < xmlWork.ChildNodes.Count; i++)
                {
                    getProfileList(xmlWork.ChildNodes[i],profileListing);
                }

            }
            return profileListing;

        }


        private void getProfileList(XmlNode node, List<string> profileListing)
        {
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                XmlNode currentNode = node.ChildNodes[i];

                switch (currentNode.Name)
                {
                    case "ProfileListing":
                        //profiles = exportNode(node.ChildNodes[i], @"Projector_profiles.xml");
                        readProfileNames(node.ChildNodes[i],profileListing);
                        break;                    
                }

            }
        }

        private void readProfileNames(XmlNode node, List<string> profileListing)
        {
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                XmlNode currentNode = node.ChildNodes[i];

                string value = currentNode.Attributes[0].Value;
                string name = currentNode.Name;
                profileListing.Add(value);
            }
        }


        private void getSettings(XmlNode node)
        {
            XmlSetup profiles = new XmlSetup();
            for (int i = 0; i < node.ChildNodes.Count;i++ )
            {
                XmlNode currentNode = node.ChildNodes[i];

                switch (currentNode.Name)
                {
                    case "ProfileListing":                        
                        profiles = exportNode(node.ChildNodes[i], @"Projector_profiles.xml");
                        break;
                    case "Config":
                        exportNode(node.ChildNodes[i], @"Projector_config.xml");
                        break;
                    case "Groups":
                        exportNode(node.ChildNodes[i], @"profileGroups.xml");
                        break;
                    case "Profiles":
                        for (int p = 0; p < currentNode.ChildNodes.Count; p++)
                        {
                            string fileName = currentNode.ChildNodes[p].Name;
                            exportDBNode(currentNode.ChildNodes[p], fileName, profiles);
                        }
                            
                        break;

                }

            }
        }

        private XmlSetup exportNode(XmlNode node,string filename)
        {
            XmlSetup saveXml = new XmlSetup();
            saveXml.setFileName(filename);
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                XmlNode currentNode = node.ChildNodes[i];

                string value = currentNode.Attributes[0].Value;
                string name = currentNode.Name;

                saveXml.addSetting(name, value);
                
                

            }
            saveXml.saveXml();
            return saveXml;
        }

        private void exportDBNode(XmlNode node,string settingName, XmlSetup profilSetup)
        {
            XmlSetup saveXml = new XmlSetup();
            string fileName = profilSetup.getSetting(settingName) + "_config.xml";
            if (null != fileName){
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    XmlNode currentNode = node.ChildNodes[i];

                    string value = currentNode.Attributes[0].Value;
                    string name = currentNode.Name;

                    saveXml.addSetting(name, value);

                }
            }
            saveXml.setFileName(fileName);
            saveXml.saveXml();
            
        }

        public void ExportToXml(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode myRoot, profiles, profilList, globalNodes, catNode,groupNode;
            XmlAttribute myAttribute;

            XmlSetup mainSetup = new XmlSetup();
            mainSetup.setFileName("Projector_profiles.xml");
            mainSetup.loadXml();

            XmlSetup globalSetup = new XmlSetup();
            globalSetup.setFileName("Projector_config.xml");
            globalSetup.loadXml();

            XmlSetup groupSetup = new XmlSetup();
            groupSetup.setFileName("profileGroups.xml");
            groupSetup.loadXml();


            Profil settingProfil = new Profil("init");

            myRoot = doc.CreateElement("Projector");

            profiles = doc.CreateElement("Profiles");
            profilList = doc.CreateElement("ProfileListing");
            globalNodes = doc.CreateElement("Config");
            groupNode = doc.CreateElement("Groups");

            //global settings
            globalSetup.addAsXmlNode(doc, globalNodes);
            myRoot.AppendChild(globalNodes);

            //list of profiles
            mainSetup.addAsXmlNode(doc, profilList);
            myRoot.AppendChild(profilList);

            // groups
            groupSetup.addAsXmlNode(doc, groupNode);
            myRoot.AppendChild(groupNode);

            // profiles properties
            for (Int64 i = 0; i < mainSetup.count; i++)
            {
                string keyname = "profil_" + (i + 1);
                string proName = mainSetup.getValue(keyname);
                catNode = doc.CreateElement(keyname);
                settingProfil.changeProfil(proName);
                settingProfil.addToXmlNode(doc, catNode);
                profiles.AppendChild(catNode);
            }

            myRoot.AppendChild(profiles);

            // last assign
            doc.AppendChild(myRoot);
            try
            {
                doc.Save(fileName);

            }
            catch (Exception ex)
            {
                lastError = ex.Message;

            }

        }
    }
}
