using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Xml;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Projector
{
    class AutoCompletion
    {
        private Hashtable wordList = new Hashtable();
        RichTextBox usedTextBox;

        public Boolean useWordBeforeAsGroup = true;

        private ListBox ownListBox;

        private string lastSelectedWord;
        private string wordBefore;  

        private Form parentForm;

        private int setSelStart = 0;
        private int setSelLength = 0;

        public AutoCompletion(RichTextBox useThis)
        {
            this.usedTextBox = useThis;
        }

        public void addWord(string word)
        {
            if (!wordList.ContainsKey(word))
            {
                wordList.Add(word, "default");
            }
        }

        public void addWord(string word,string group)
        {
            if (!wordList.ContainsKey(word))
            {
                wordList.Add(word, group);
            }
        }

        public void addWord(string[] word)
        {
            this.addWord(word, "default");
        }

        public void addWord(string[] word,string group)
        {
            for (int i = 0; i < word.Length; i++)
            {
                this.addWord(word[i],group);
            }
        }

        private string getlastSelectedWord()
        {

            int selPos = this.usedTextBox.SelectionStart;
            string leftTxt = this.usedTextBox.Text.Substring(0, selPos);            
            string[] allwords = Regex.Split(leftTxt, "[ \\n,=+-/*]");
            leftTxt = allwords[allwords.Length - 1];
            this.lastSelectedWord = leftTxt;
            if (allwords.Length > 1)
            {
                this.wordBefore = allwords[allwords.Length - 2];
            }
            else
            {
                this.wordBefore = null;
            }

            this.setSelStart = selPos - leftTxt.Length;
            this.setSelLength = leftTxt.Length;
            return leftTxt;
        }

        public int getSelectionStart()
        {
            return this.setSelStart;
        }


        public int getSelectionLength()
        {
            return this.setSelLength;
        }

        public void setSelection(KeyEventArgs keyEvent)
        {
            if (keyEvent.KeyCode == Keys.Space && keyEvent.Control)
            {
                this.setSelection();
            }
        }

        public void setSelection()
        {
            if (this.setSelStart > 0 && this.setSelLength > 0)
            {
                this.usedTextBox.SelectionStart = this.setSelStart;
                this.usedTextBox.SelectionLength = this.setSelLength;
            }
        }

        

        public List<string> getListFromPart()
        {
            string part = getlastSelectedWord();            
            return getListFromPart(part); 
        }

        public List<string> getListFromPart(string part)
        {
            List<string> result = new List<string>();
            List<string> defaultResult = new List<string>();
            foreach (DictionaryEntry de in this.wordList)
            {
                string keyname = de.Key.ToString().ToLower();
                string value = de.Value.ToString();
                if (!this.useWordBeforeAsGroup || value == "default" || value == this.wordBefore || value == keyname)
                {
                    if (keyname.Contains(part.ToLower()))
                    {
                        string subStr = keyname.Substring(0, part.Length);
                        if (subStr.ToLower() == part.ToLower()){
                            if (value == "default"){
                                defaultResult.Add(de.Key.ToString());
                            } else {
                                result.Add(de.Key.ToString());
                            }
                        }
                    }
                }
                
            }
            if (result.Count > 0)
                return result;

            return defaultResult;
        }

        // own autocomplete management
        public void assignListBox(ListBox outList)
        {
            if (this.ownListBox == null)
            {
                this.ownListBox = outList;
                this.ownListBox.SelectedIndexChanged += new System.EventHandler(listBoxEvent);
                this.hideList();
            }
        }

        private void listBoxEvent(object sender, System.EventArgs e)
        {             
            //ListBox VarCharAdd = (ListBox)sender;
            this.doAutoInsert(this.ownListBox.Text);
            this.hideList();
        
        }

        public void keypressHandler(KeyEventArgs e)
        {
            if (this.ownListBox != null && e.Control && e.KeyCode == Keys.Space)
            {
                List<string> codes = this.getListFromPart();
                this.ownListBox.Items.Clear();
                bool show = false;
                bool doReplaceNow = false;
                for (int i = 0; i < codes.Count; i++)
                {
                    this.ownListBox.Items.Add(codes[i]);
                    if (i > 1) show = true;
                    doReplaceNow = true;
                }
                if (show)
                {
                    Point pos = this.usedTextBox.GetPositionFromCharIndex(this.usedTextBox.SelectionStart);
                    this.ownListBox.Left = pos.X;
                    this.ownListBox.Top = pos.Y + this.usedTextBox.Font.Height + 5;
                    this.showList();
                    
                }
                else if (doReplaceNow)
                {
                    this.doAutoInsert( codes[0] );
                }
            }
        }

        private void doAutoInsert(string newText)
        {
            int insertPos = this.getSelectionStart();

            this.usedTextBox.Text = this.usedTextBox.Text.Remove(insertPos, this.getSelectionLength());
            this.usedTextBox.Text = this.usedTextBox.Text.Insert(insertPos, newText);            
            this.ownListBox.Text = "";
            this.usedTextBox.SelectionStart = insertPos + newText.Length;
        }

        private void hideList()
        {
            this.ownListBox.SendToBack();
            this.ownListBox.Visible = false;
        }

        private void showList()
        {
            this.ownListBox.BringToFront();
            this.ownListBox.Visible = true;
        }


        public void autoCreateList(Form toParent)
        {
            if (null == ownListBox)
            {
                ownListBox = new ListBox();
                ownListBox.BackColor = Color.LightYellow;
                ownListBox.Visible = true;
                ownListBox.Top = 10;
                ownListBox.Left = 10;

                ownListBox.BringToFront();
                parentForm = toParent;
                toParent.Controls.Add(ownListBox);
            }
        }

    }
}
