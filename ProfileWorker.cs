using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{
    class ProfileWorker
    {
        /// <summary>
        /// contains all profiles after init
        /// </summary>
        private List<String> knownProfiles = new List<string>();

        private ReflectionScript iterateScr;
        private ReflectionScript iterateDoneScr;     

        public ProfileWorker()
        {
            this.init();
        }

        /// <summary>
        /// assign script that will be executed until interation
        /// </summary>
        /// <param name="scr">Reflectionscriot that will be executed on each iteration</param>
        public void OnIteration(ReflectionScript scr)
        {
            this.iterateScr = scr;
        }

        /// <summary>
        /// assign script that willbe executed is iteration is done
        /// </summary>
        /// <param name="scr">Reflection Script</param>
        public void OnDone(ReflectionScript scr)
        {
            this.iterateDoneScr = scr;
        }

        /// <summary>
        /// trigger to start the iteration
        /// </summary>
        public void startIterate()
        {
            foreach (string name in this.knownProfiles)
            {
                this.iterate(name);
            }
        }

        /// <summary>
        /// trigger to start iteration over all profiles in the give groupname
        /// </summary>
        /// <param name="groupName">Groupname for iteration</param>
        public void startIterateGroup(string groupName)
        {
            GroupProfilWorker worker = new GroupProfilWorker(new PConfig());
            List<string> grps = worker.getGroupMember(groupName);

            foreach (string groupname in grps)
            {                
                this.iterate(groupName);                
            }
        }
        
        /// <summary>
        /// returns a Profile by the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// starts the iteration. The current iteration name
        /// is assigned to the scriptvar &ITERATION.PROFIL
        /// </summary>
        /// <param name="str"></param>
        private void iterate(string str)
        {
            if (this.iterateScr != null)
            {
                this.iterateScr.createOrUpdateStringVar("&ITERATION.PROFIL", str);
                RefScriptExecute exec = new RefScriptExecute(this.iterateScr, this);
                exec.run();
            }
               
        }

        /// <summary>
        /// trigger if the iteration is Done.
        /// executes the assigned script if set
        /// </summary>
        private void iterateDone()
        {

            if (this.iterateDoneScr != null)
            {
                RefScriptExecute exec = new RefScriptExecute(this.iterateDoneScr, this);
                exec.run();
            }

        }

        /// <summary>
        /// initialize and get all profiles
        /// </summary>
        private void init()
        {
            this.knownProfiles.Clear();
            PConfig config = new PConfig();
            this.knownProfiles = config.getListWidthDefault(PConfig.KEY_PROFILS, new List<string>());            
        }
    }
}
