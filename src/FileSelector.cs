using Projector.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projector
{
    class FileSelector
    {
        OpenFileDialog opener = new OpenFileDialog();

        ReflectionScript openScript;
        private string nameOfVar = "filename";
        

        public FileSelector()
        {
            this.opener.DefaultExt = "txt";
        }

        public void setFileName(string fileName)
        {
            this.opener.FileName = fileName;
        }

        public void OnOpenFilenameSelected(string pathVarName, ReflectionScript script)
        {
            this.nameOfVar = pathVarName;
            this.openScript = script;
        }


        public void setExtensions(string extensionsKommaSeperated)
        {
            string[] extensions = extensionsKommaSeperated.Split(',');
            string extSet = "";
            string add = "";
            foreach (string ext in extensions)
            {
                if (this.opener.DefaultExt == "txt")
                    this.opener.DefaultExt = ext;

                extSet += add + ext.ToUpper() + "|" + ext.ToLower();
                add = "|";
            }
            this.opener.Filter = extSet;
        }

        public String getLastFileName()
        {
            return this.opener.FileName;
        }

        public void showOpenFileDialog()
        {
            if (this.opener.ShowDialog() == DialogResult.OK)
            {
                if (this.openScript != null)
                {
                    this.openScript.createOrUpdateStringVar("&" + this.nameOfVar,this.opener.FileName);
                    RefScriptExecute exec = new RefScriptExecute(this.openScript, this);
                    exec.run();

                }
            }
        }

    }
}
