using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{
    class ProfileWorker
    {

        //Profil profil = new Profil("default");
        List<String> knownProfiles = new List<string>();

        ReflectionScript iterateScr;
        ReflectionScript iterateDoneScr;     

        public ProfileWorker()
        {
            this.init();
        }

        public void OnIteration(ReflectionScript scr)
        {
            this.iterateScr = scr;
        }

        public void OnDone(ReflectionScript scr)
        {
            this.iterateDoneScr = scr;
        }

        public void startIterate()
        {
            foreach (string name in this.knownProfiles)
            {
                this.iterate(name);
            }
        }

        public void startIterateGroup(string groupName)
        {

            XmlSetup pSetup = new XmlSetup();
            pSetup.setFileName("profileGroups.xml");
            pSetup.loadXml();
            Hashtable settings = pSetup.getHashMap();


            if (pSetup.getHashMap().Contains(groupName))
            {
                string[] currGrp = pSetup.getHashMap()[groupName].ToString().Split('|');
                for (int z = 0; z < currGrp.Length; z++)
                {
                    this.iterate(currGrp[z]);
                }

            }
        }
        

        public Profil getProfilbyName(string name)
        {
            if (knownProfiles.Contains(name))
            {
                Profil tmpPro = new Profil(name);
                tmpPro.changeProfil(name);
                return tmpPro;
            }

            return null;
        }

        private void iterate(string str)
        {
            if (this.iterateScr != null)
            {
                this.iterateScr.createOrUpdateStringVar("&ITERATION.PROFIL", str);
                RefScriptExecute exec = new RefScriptExecute(this.iterateScr, this);
                exec.run();
            }
               
        }


        private void iterateDone()
        {

            if (this.iterateDoneScr != null)
            {
                RefScriptExecute exec = new RefScriptExecute(this.iterateDoneScr, this);
                exec.run();
            }

        }

        private void init()
        {
            this.knownProfiles.Clear();
            XmlSetup tmpSetup = new XmlSetup();
            
            tmpSetup.setFileName("Projector_profiles.xml");         
            tmpSetup.loadXml();
            for (Int64 i = 0; i < tmpSetup.count; i++)
            {
                string keyname = "profil_" + (i + 1);
                string proName = tmpSetup.getValue(keyname);

                if (proName != null)
                {
                    knownProfiles.Add(proName);                                     
                }
            }
        }
    }
}
