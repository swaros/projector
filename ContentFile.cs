using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace Projector
{
    class ContentFile
    {
        private string[] content;

        private const string REGEX_STRING = "\"([^\"]*)\"";

        private Hashtable Lines = new Hashtable();

        private string splitChars = ";";

        public void loadFile(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                content = System.IO.File.ReadAllLines(filename);
                this.parseLines();
            }
        }

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
                    string key = "_" + i + "_";
                    //rename.Add(key, str.Replace("\"", ""));
                    rename.Add(key, str);
                    code = code.Replace(str, key);
                }
                string[] cells = code.Split(this.splitChars.ToCharArray());
                List<string> row = new List<string>();
                for (int x = 0; x < cells.Length; x++)
                {
                    foreach (DictionaryEntry rp in rename)
                        cells[x] = cells[x].Replace(rp.Key.ToString(), rp.Value.ToString());

                    row.Add(cells[x]);

                }
                Lines.Add(i, row);

            }
        }


    }
}
