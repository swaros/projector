using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Projector
{
    public class RefScrAutoStart
    {
        private string path;

        private string extension = "*.pscr";

        private Boolean AllScripts = false;

        private List<RefScrAutoScrContainer> scriptCollection = new List<RefScrAutoScrContainer>();

        public Boolean setPath(string path){

            if (System.IO.Directory.Exists(path))
            {
                this.path = path;
                this.parseFolder();
                return true;
            }

            return false;
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
            }

        }

    }
}
