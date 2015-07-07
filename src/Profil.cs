using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Projector
{
    public class Profil
    {
        public const int MODE_OLD_XML = 0;
        public const int MODE_MIXED = 1;
        public const int MODE_PCONFIG = 2;

        private string name = null;
        XmlSetup setup = new XmlSetup();
        PConfig PSetup = new PConfig();

        private const string nameSpace = "client.profiles.";
        private string pConfigPath;

        private int mode = 2;

        public Profil()
        {
            this.name = null;
        }


        public Profil(string profilName)
        {
            this.changeProfil(profilName);
        }

        private Boolean isPmode()
        {
            return (this.mode == Profil.MODE_MIXED || this.mode == Profil.MODE_PCONFIG);
        }

        private Boolean isXmlMode()
        {
            return (this.mode == Profil.MODE_MIXED || this.mode == Profil.MODE_OLD_XML);
        }

        public void changeProfil(string newName)
        {
            
            this.name = newName;
            if (this.isXmlMode())
            {
                this.setup.setFileName(this.name + "_config.xml");
                this.loadProfileSettings();
            }

            if (this.isPmode())
            {
                this.pConfigPath = Profil.nameSpace + this.name;
            }

        }

        public String getName()
        {
            return this.name;
        }

        public void addToXmlNode(XmlDocument doc, XmlNode catNode)
        {
            this.setup.addAsXmlNode(doc, catNode);
        }

        private void checkProfil()
        {
            if (this.name == null)
            {
                throw new Exception("Profil not selected");
            }
        }

        public void getValuesFromProfil(Profil source)
        {
            this.checkProfil();
            if (this.isXmlMode())
            {
                this.setup = source.setup.copyValues(this.setup);
            }

            if (this.isPmode())
            {
                this.name = source.getName();
            }

        }

        private string getPModePath(string name)
        {
            this.checkProfil();
            return Profil.nameSpace + this.name + "." + name;
        }

        private string getPModeProperty(string name)
        {
            this.checkProfil();
            return this.PSetup.getSettingWidthDefault(this.getPModePath(name), null);
        }

        public string getProperty(string name)
        {
            this.checkProfil();
            if (this.mode == Profil.MODE_MIXED)
            {
                this.PSetup.getSettingWidthDefault(this.getPModePath(name), this.setup.getSetting(name));
            }


            if (isXmlMode())
                return this.setup.getSetting(name);

            if (isPmode())
            {
                return this.getPModeProperty(name);
            }
            return "";
        }

        public void setProperty(string name, string value)
        {
            this.checkProfil();
            if (isXmlMode())
                this.setup.addSetting(name,value);
            if (isPmode())
                this.PSetup.setValue(this.getPModePath(name), value);    
        }

        public void exportXml(string filename)
        {
            this.checkProfil();
            this.setup.exportXml(filename);
        }

        public void saveSetup()
        {
            this.checkProfil();
            if (isXmlMode())
                this.setup.saveXml();
            if (isPmode())
                this.PSetup.saveRuntimeConfig();    
        }

        public void loadProfileSettings()
        {
            this.checkProfil();
            if (isXmlMode())
                this.setup.loadXml();

           
            
        }

    }
}
