using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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

        public List<RefScrAutoScrContainer> getAllScripts()
        {
            return this.scriptCollection;
        }

        private void parseFolder()
        {
            string[] filePaths = Directory.GetFiles(this.path, this.extension,  SearchOption.AllDirectories);
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
