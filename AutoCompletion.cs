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

        private int setSelStart = 0;
        private int setSelLength = 0;

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

        private string getlastSelectedWord(RichTextBox textBox1)
        {
            int selPos = textBox1.SelectionStart;
            string leftTxt = textBox1.Text.Substring(0, selPos);            
            string[] allwords = Regex.Split(leftTxt, "[ ,=+-/*]");
            leftTxt = allwords[allwords.Length - 1];
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

        public void setSelection(RichTextBox inputBox)
        {
            if (this.setSelStart > 0 && this.setSelLength > 0)
            {
                inputBox.SelectionStart = this.setSelStart;
                inputBox.SelectionLength = this.setSelLength;
            }
        }

        public List<string> getListFromPart(RichTextBox inputBox)
        {
            string part = getlastSelectedWord(inputBox);
            //inputBox.SelectionStart = this.setSelStart;
            //inputBox.SelectionLength = this.setSelLength;
            return getListFromPart(part); 
        }

        public List<string> getListFromPart(string part)
        {
            List<string> result = new List<string>();
            foreach (DictionaryEntry de in this.wordList)
            {
                string keyname = de.Key.ToString().ToLower();
                string value = de.Value.ToString();

                if (keyname.Contains(part.ToLower()))
                {
                    string subStr = keyname.Substring(0, part.Length);
                    if (subStr.ToLower() == part.ToLower()) result.Add(de.Key.ToString());
                }
                
            }
            return result;
        }


    }
}
