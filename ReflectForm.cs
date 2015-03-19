using Projector.Net.Secured;
using Projector.Script;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Projector
{
    public partial class ReflectForm : Form
    {

        public string ScriptIdent = "";

        private ReflectionScript Script;

        private ReflectionScript onCloseScript;

        private ReflectionScript OnMessageScript;

        private ReflectionScript OnWebResultScript;

        private string webRequestName = "httpResponse";

        public ReflectForm()
        {
            InitializeComponent();
        }

        public void invoke (ReflectionScript script)
        {
            this.Script = script;
            if (this.Script.getErrorCount() == 0)
            {
                RefScriptExecute executer = new RefScriptExecute(this.Script,this);
                executer.run();
            }
        }

        public void Message(string name,string message)
        {
            if (this.OnMessageScript != null)
            {
                this.OnMessageScript.createOrUpdateStringVar("&" + name, message);
                RefScriptExecute executer = new RefScriptExecute(this.OnMessageScript, this);
                executer.run();
            }

            foreach (Control element in this.Controls)
            {
                if (element is PrConsole)
                {
                    PrConsole console = (PrConsole)element;
                    console.addMessage(message);
                }
            }

        }

        public void OnMessage(ReflectionScript script)
        {
            this.OnMessageScript = script;
        }

        public void OnWebResponse(string nameOfVar,ReflectionScript script)
        {
            this.webRequestName = nameOfVar;
            this.OnWebResultScript = script;
        }

        public void OnCloseForm(ReflectionScript script)
        {
            this.onCloseScript = script;
        }

        public void setLeft(int left)
        {
            this.Left = left;
        }

        public void setTop(int top)
        {
            this.Top = top;
        }

        public void setWidth(int w)
        {
            this.Width = w;
        }

        public void setHeight(int h)
        {
            this.Height = h;
        }

        public void CloseForm()
        {
            this.Close();
        }

        public void setCaption(string caption)
        {
            this.Text = caption;
        }

        public String getCaption()
        {
            return this.Text;
        }

        public void setEnabled(Boolean setit)
        {
            this.Enabled = setit;
        }

        private void closeRequestAction()
        {
            if (this.onCloseScript != null)
            {

                foreach (Control element in this.Controls)
                {
                    //this.onCloseScript.updateVarByObject()

                    string nameOfObject = element.Name;
                    string objectValue = element.Text;

                    this.onCloseScript.createOrUpdateStringVar("&" + nameOfObject + "." + "Text", objectValue);

                    if (this.onCloseScript.Parent != null)
                    {
                        this.onCloseScript.Parent.createOrUpdateStringVar("&" + this.ScriptIdent + "." + nameOfObject + "." + "Text", objectValue);
                    }

                }

                if (this.onCloseScript.getNotRuntimeErrorCount() == 0)
                {
                    RefScriptExecute executer = new RefScriptExecute(this.onCloseScript, this);
                    executer.run();
                }
            }
        }

        public void WebPostRequest(string username, string password, string callUrl)
        {
            this.sendHtToWeb(username, password,  callUrl);
        }


        private void sendHtToWeb(string username, string password, string callUrl)
        {
            if (httpWorker.IsBusy)
            {
                MessageBox.Show("There is an Request already Send and in Progress. A Webrequest is Limited by one per RequestForm.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string oldCap = this.getCaption();
            this.Enabled = false;
            this.setCaption("WAIT: Sending Content to Web...");
            NetHtAccess webCall = new NetHtAccess();
            
            webCall.setUri(callUrl);
            webCall.setUser(username, password);
            webCall.resetParameters();
            foreach (Control element in this.Controls)
            {
                Boolean sendThisAsString = true;
                //this.onCloseScript.updateVarByObject()
                if (element is LabelText)
                {
                    LabelText lt = (LabelText)element;
                    if (lt.doNotSend)
                    {
                        sendThisAsString = false;
                    }
                }

                if (element is ImageLoader)
                {
                    ImageLoader img = (ImageLoader)element;
                    sendThisAsString = false;
                    webCall.addOrReplaceParam(element.Name, img);
                }

                if (sendThisAsString)
                {
                    string nameOfObject = element.Name;
                    string objectValue = element.Text;
                    webCall.addOrReplaceParam(nameOfObject, objectValue);
                }

            }
            httpWorker.RunWorkerAsync(webCall);
            
            this.Enabled = true;
            this.setCaption(oldCap);    
        }


        private void ReflectForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            this.Visible = false;  
        }

        private void ReflectForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.closeRequestAction();
            Application.DoEvents();
            
        }

        private void httpWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            NetHtAccess webCall = (NetHtAccess)e.Argument;
            webCall.load();
            httpWorker.ReportProgress(1, webCall);
            
        }

        private void httpWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //NetHtAccess webCall = (NetHtAccess)e.Result;
            this.Enabled = true;
        }

        private void httpWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            NetHtAccess webCall = (NetHtAccess)e.UserState;
            string error = webCall.getLastError();
            if (error != "")
            {
                MessageBox.Show(error, "Error On Webrequest", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (this.OnWebResultScript != null)
                {
                    if (this.webRequestName != "")
                    {
                        this.OnWebResultScript.createOrUpdateStringVar("&" + this.webRequestName, webCall.getContent());
                    }
                    RefScriptExecute exec = new RefScriptExecute(this.OnWebResultScript, this);
                    exec.run();

                }
            }
            this.Enabled = true;
        }
    }
}
