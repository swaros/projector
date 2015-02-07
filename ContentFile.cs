using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace Projector
{
    /// <summary>
    /// get the Content of a Textfile and seperates this
    /// by line and an delimiter
    /// </summary>
    class ContentFile
    {
        /// <summary>
        /// the content of the file
        /// </summary>
        private string[] content;

        /// <summary>
        /// flag if file is loaded
        /// </summary>
        private Boolean fileLoaded = false;
        /// <summary>
        /// regex to exclude strings
        /// </summary>
        private const string REGEX_STRING = "\"([^\"]*)\"";

        /// <summary>
        /// All Errrors that occured
        /// </summary>
        private List<string> errorMessages = new List<string>();

        /// <summary>
        /// count of errors
        /// </summary>
        public int errorCount = 0;

        /// <summary>
        /// count of lines
        /// </summary>
        public int lineCount = 0;

        /// <summary>
        /// count of maximal read cells
        /// </summary>
        public int maxCellCount = 0;

        /// <summary>
        /// content seperated by lines and splitchars
        /// </summary>
        private Hashtable Lines = new Hashtable();

        /// <summary>
        /// the default delimiter char
        /// </summary>
        private string splitChars = ";";

        /// <summary>
        /// Loads the File
        /// </summary>
        /// <param name="filename">full filename</param>
        public void loadFile(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                try
                {
                    content = System.IO.File.ReadAllLines(filename);
                    this.fileLoaded = true;
                   
                }
                catch (Exception ex)
                {
                    this.errorMessages.Add(ex.Message);
                    this.errorCount++;
                    return;
                }

                this.parseLines();
            }
            else
            {
                this.errorCount++;
                this.errorMessages.Add("File Not Found " + filename);
            }
        }

        /// <summary>
        /// returns hashtable with seperated text
        /// </summary>
        /// <returns>strings in Hashtable. key is line Number, value the row as List of string </returns>
        public Hashtable getContent()
        {
            return this.Lines;
        }

        /// <summary>
        /// returns a row from index
        /// </summary>
        /// <param name="index">number of row</param>
        /// <returns>returns the row</returns>
        public List<string> getContentAtIndex(int index)
        {
            if (index < this.Lines.Count)
            {
                return (List<string>)this.Lines[index];
            }
            this.errorMessages.Add("Index "+ index + " is out of Range. Max " + this.Lines.Count);
            this.errorCount++;
            return null;
        }

        /// <summary>
        /// returns if lie is loaded
        /// </summary>
        /// <returns></returns>
        public Boolean fileIsLoaded()
        {
            return this.fileLoaded;
        }

        /// <summary>
        /// get all Errormessages
        /// </summary>
        /// <returns></returns>
        public List<string> getErrors()
        {
            return this.errorMessages;
        }


        /// <summary>
        /// Parse the loaded content
        /// and build internal Storage
        /// </summary>
        private void parseLines()
        {
            
            for (int i = 0; i < content.Count(); i++)
            {
                Hashtable rename = new Hashtable();
                string code = content[i];
                MatchCollection match = Regex.Matches(code, Projector.ContentFile.REGEX_STRING);
                
                for (int x = 0; x < match.Count; x++)
                {
                    string str = match[x].Value;
                    string key = "STR" + i + "T" + x+"T";
                    //str = str.Replace("\"", "");
                    rename.Add(key, str);
                    code = code.Replace(str, key);
                }
                string[] cells = code.Split(this.splitChars.ToCharArray());
                List<string> row = new List<string>();
                for (int x = 0; x < cells.Length; x++)
                {
                    foreach (DictionaryEntry rp in rename)
                    {
                        cells[x] = cells[x].Replace(rp.Key.ToString(), rp.Value.ToString());
                    }
                        

                    row.Add(cells[x]);

                }
                if (row.Count > this.maxCellCount)
                    this.maxCellCount = row.Count;
                this.Lines.Add(i, row);
                this.lineCount++;
            }

        }


    }
}
