using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;

namespace Projector
{
    public class RefScrAutoStart
    {
        private string path;

        private string extension = "*.pscr";

        private Boolean AllScripts = false;

        private BackgroundWorker taskMan = new BackgroundWorker(); 

        private List<RefScrAutoScrContainer> scriptCollection = new List<RefScrAutoScrContainer>();

        public Boolean GotInfomation = false;


        public RefScrAutoStart()
        {
            taskMan.DoWork += taskMan_DoWork;
            taskMan.ProgressChanged += taskMan_ProgressChanged;
            taskMan.RunWorkerCompleted += taskMan_RunWorkerCompleted;
            taskMan.WorkerReportsProgress = true;
        }



        /// <summary>
        /// sets the path and starts the Parsing of the folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Boolean setPath(string path){

            if (System.IO.Directory.Exists(path))
            {
                this.path = path;
                this.parseFolder();
                return true;
            }

            return false;
        }

        private void taskMan_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RefScrAutoScrContainer addMe = (RefScrAutoScrContainer)e.UserState;
            this.scriptCollection.Add(addMe);            
        }

        private void taskMan_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] filePaths = (string[])e.Argument;
            foreach (string fileName in filePaths)
            {
                string content = System.IO.File.ReadAllText(fileName);

                RefScrAutoScrContainer addMe = new RefScrAutoScrContainer();
                addMe.Script = new ReflectionScript();
                addMe.Script.setCode(content);
                if (addMe.Script.getErrorCount() == 0)
                {
                    addMe.Label = addMe.Script.SetupStringValue(ReflectionScript.SETUP_LABEL);
                    addMe.Description = addMe.Script.SetupStringValue(ReflectionScript.SETUP_DESC);

                    if (this.AllScripts && addMe.Label == null)
                    {
                        addMe.Label = System.IO.Path.GetFileNameWithoutExtension(fileName);

                    }

                    if (addMe.Label != null)
                    {
                        //this.scriptCollection.Add(addMe);
                        taskMan.ReportProgress(1, addMe);
                    }
                }
            }
        }


        public Boolean ImWorking()
        {
            return this.taskMan.IsBusy;
        }

        private void taskMan_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GotInfomation = true;
        }

        public void showAll(Boolean setDisplay)
        {
            this.AllScripts = setDisplay;
        }

        public List<RefScrAutoScrContainer> getAllScripts()
        {
            return this.scriptCollection;
        }

        private void parseFolder()
        {
            string[] filePaths;

            try
            {
                filePaths = Directory.GetFiles(this.path, this.extension);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            this.scriptCollection.Clear();
            this.GotInfomation = false;
            if (this.taskMan.IsBusy)
            {
                return;
            }
            this.taskMan.RunWorkerAsync(filePaths);
            
            /*
            foreach (string fileName in filePaths)
            {
                string content = System.IO.File.ReadAllText(fileName);

                RefScrAutoScrContainer addMe = new RefScrAutoScrContainer();
                addMe.Script = new ReflectionScript();
                addMe.Script.setCode(content);
                if (addMe.Script.getErrorCount() == 0)
                {
                    addMe.Label = addMe.Script.SetupStringValue(ReflectionScript.SETUP_LABEL);
                    addMe.Description = addMe.Script.SetupStringValue(ReflectionScript.SETUP_DESC);

                    if (this.AllScripts && addMe.Label == null)
                    {
                        addMe.Label = System.IO.Path.GetFileNameWithoutExtension(fileName);
                        
                    }

                    if (addMe.Label != null)
                    {
                        this.scriptCollection.Add(addMe);
                    }
                }
            }*/

        }

    }
}
