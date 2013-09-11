using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Projector
{
    public class Profil
    {
        private string name = null;
        XmlSetup setup = new XmlSetup();

        public Profil(string profilName)
        {
            this.changeProfil(profilName);
        }

        public void changeProfil(string newName)
        {
            this.name = newName;
            this.setup.setFileName(this.name+"_config.xml");
            this.loadProfileSettings();
        }

        public String getName()
        {
            return this.name;
        }

        public void addToXmlNode(XmlDocument doc, XmlNode catNode)
        {
            this.setup.addAsXmlNode(doc, catNode);
        }


        public void getValuesFromProfil(Profil source)
        {
            this.setup = source.setup.copyValues(this.setup);            
        }

        public string getProperty(string name)
        {
            return this.setup.getSetting(name);
        }

        public void setProperty(string name, string value)
        {
            this.setup.addSetting(name,value);
        }

        public void exportXml(string filename)
        {
            this.setup.exportXml(filename);
        }

        public void saveSetup()
        {
            this.setup.saveXml();
        }

        public void loadProfileSettings()
        {
            this.setup.loadXml(); 
        }

    }
}
