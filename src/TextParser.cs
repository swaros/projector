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
//using Majestic12;

namespace Projector
{
    class TextParser
    {
        String[] set_keywords = new String[5000];
        int[] set_tag_colors = new int[5000];
        int set_keys = 0;
        //private Hashtable commentTags = new Hashtable();
        private bool saveAsCommentArray = false;
        public bool checkOnComment = true;

        private Point[] commentArray = new Point[2000];
        private int maxPoints = 2000;
        private int currentPoint = 0;

        //private int currentMarkupPosition = 0;

        private bool addReturn = true;

        public int FontSize = 12;
        public bool caseCheck = true;

        public void isComment()
        {
            saveAsCommentArray = true;
            checkOnComment = false;
        }

        public void ísNotComment()
        {
            saveAsCommentArray = false;
            checkOnComment = true;
        }

        private void storeCommentArray(int start, int end)
        {
            if (currentPoint < maxPoints)
            {
                commentArray[currentPoint].X = start;
                commentArray[currentPoint].Y = end;
                currentPoint++;
            }
        }

        private bool isInComment(int pos)
        {
            for (int i = 0; i < currentPoint; i++)
            {
                if (pos >= commentArray[i].X)
                {
                    if (pos <= commentArray[i].Y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /*
        public void addCommentTag(string name, syntaxTags tags)
        {
            commentTags.Add(name, tags);
        }



        private bool isComment(int pos)
        {

            return true;
        }
         */
        //-------------------------------------------------------------------------------------------------------------------------

        public void checkSource(RichTextBox m_rtb)
        {

            Regex r = new Regex(".*\\((.*)\\)");
            Match m = r.Match(m_rtb.Text);
            if (m.Success)
            {
                string[] arguments = m.Groups[1].Value.Split(',');
                for (int i = 0; i < arguments.Length; i++)
                {
                    Console.WriteLine("Argument " + (i + 1) + " = " + arguments[i]);
                }
            }


            /*
            Regex r = new Regex("\\n");
            
            String[] lines = r.Split(m_rtb.Text);
            m_rtb.Clear();

            foreach (string l in lines)
            {
                parseCode(m_rtb, l);
            }*/
        }

        public void parseCode(RichTextBox m_rtb, string line)
        {
            Regex r = new Regex("{}");

            String[] tokens = r.Split(line);
            foreach (string token in tokens)
            {
                string sLine = token;
            }
        }


        public void addKeyWord(string name, int colorKey)
        {
            if (set_keys < 5000)
            {
                set_keywords[set_keys] = name;
                set_tag_colors[set_keys] = colorKey;
                set_keys++;
            }
        }

        public void checkWords(RichTextBox m_rtb)
        {
            //this.currentMarkupPosition = m_rtb.SelectionCharOffset;
            Regex r = new Regex("\\n");
            String[] lines = r.Split(m_rtb.Text);
            
            int startpos = m_rtb.SelectionStart;
            int endPos = m_rtb.SelectionLength;
            m_rtb.Clear();

            for (int i = 0; i < lines.Length; i++)
            {
                this.addReturn = (i < lines.Length - 1);
                ParseLine(m_rtb, lines[i]);
            }


           //m_rtb.SelectionCharOffset = this.currentMarkupPosition;
            m_rtb.SelectionStart = startpos;
            m_rtb.SelectionLength = endPos;
        }



        public void ParseLine(RichTextBox m_rtb, string line)
        {
            Regex r = new Regex("([= \\t{}()<>:;+-/*,'])");

            String[] tokens = r.Split(line);
            foreach (string token in tokens)
            {
                m_rtb.SelectionColor = Color.Black;
                m_rtb.SelectionFont = new Font("Courier New", FontSize, FontStyle.Regular);

                for (int i = 0; i < set_keys; i++)
                {
                    if  ((!caseCheck && (set_keywords[i].ToLower() == token.ToLower() || @"/" + set_keywords[i].ToLower() == token.ToLower()))
                    ||  (caseCheck && (set_keywords[i] == token || @"/" + set_keywords[i] == token)))
                    {
                        // Apply alternative color and font to highlight keyword.
                        if (i < set_tag_colors.Length)
                        {
                            if (set_tag_colors[i] == 1)
                            {
                                m_rtb.SelectionColor = Color.Blue;
                                m_rtb.SelectionFont = new Font("Courier New", FontSize, FontStyle.Bold);
                            }
                            else if (set_tag_colors[i] == 2)
                            {
                                m_rtb.SelectionColor = Color.Magenta;
                                m_rtb.SelectionFont = new Font("Courier New", FontSize, FontStyle.Bold);
                                //m_rtb.SelectionBackColor = Color.MistyRose;
                            }
                            else if (set_tag_colors[i] == 3)
                            {
                                m_rtb.SelectionColor = Color.Brown;
                                //m_rtb.SelectionBackColor = Color.AntiqueWhite;
                            }
                            else if (set_tag_colors[i] == 4)
                            {
                                m_rtb.SelectionColor = Color.Navy;
                                m_rtb.SelectionFont = new Font("Courier New", FontSize, FontStyle.Bold);
                                //m_rtb.SelectionBackColor = Color.Moccasin;
                            }
                            else if (set_tag_colors[i] == 5)
                            {
                                m_rtb.SelectionColor = Color.DarkOliveGreen;
                                //m_rtb.SelectionBackColor = Color.White;
                            }

                            else if (set_tag_colors[i] == 6)
                            {
                                m_rtb.SelectionColor = Color.Crimson;
                                //m_rtb.SelectionBackColor = Color.White;
                            }

                        }
                        else m_rtb.SelectionColor = Color.Blue;

                        
                        break;
                    }
                }
                m_rtb.SelectedText = token;

            }
            if (addReturn) m_rtb.SelectedText = System.Environment.NewLine;
        }


        //--------------------------------------------------------------------------------------------------------

        public string findSourceOptions(RichTextBox source, string what, string delimiter1, string delimiter2)
        {
            int foundpos = source.Find(what, 0, source.Text.Length, RichTextBoxFinds.MatchCase);



            if (foundpos > 0)
            {
                int maxSearchRight = source.Text.Length;

                string substr = source.Text.Substring(foundpos + what.Length, maxSearchRight - (foundpos + what.Length));
                int startPos = substr.IndexOf(delimiter1);
                int lastPos = substr.IndexOf(delimiter2);

                if (startPos > 0 && lastPos > 0)
                {
                    string foundstring = substr.Substring(startPos + delimiter1.Length, lastPos - (startPos + delimiter2.Length));
                    return foundstring;
                }


            }
            return "NONRESULT";
        }

        public int markTags(RichTextBox source, string delimiter1, Color fontColor, Color bgColor, int start)
        {
            return markTags(source, delimiter1, delimiter1, fontColor, bgColor, start, true);
        }

        public int markTags(RichTextBox source, string delimiter1, string delimiter2, Color fontColor, Color bgColor, int start)
        {
            return markTags(source, delimiter1, delimiter2, fontColor, bgColor, start, true);
        }

        public int markTags(RichTextBox source, string delimiter1, string delimiter2, Color fontColor, Color bgColor, int start, bool reverse)
        {
            if (source.Text.Length < 2) return -1;
            int foundpos = source.Find(delimiter1, start, source.Text.Length, RichTextBoxFinds.MatchCase);

            if (foundpos > -1 && foundpos > start)
            {
                int maxrounds = 0;
                // check on foundpos in commentstring

                while (checkOnComment && isInComment(foundpos) && foundpos > -1)
                {
                    foundpos = source.Find(delimiter1, foundpos + 1, source.Text.Length, RichTextBoxFinds.MatchCase);
                    maxrounds++;

                    if (maxrounds > 500)
                    {
                        MessageBox.Show("endless ???");
                        return -1;
                    }
                }
                if (foundpos == -1) return -1;

                int found_right = source.Find(delimiter2, foundpos + delimiter1.Length, source.Text.Length, RichTextBoxFinds.MatchCase);
                if (found_right > -1 && found_right > start)
                {
                    if (!checkOnComment || (checkOnComment && !isInComment(found_right) && !isInComment(foundpos)))
                    {
                        source.SelectionStart = foundpos;
                        source.SelectionLength = found_right - foundpos + delimiter1.Length;
                        if (fontColor != Color.Transparent) source.SelectionColor = fontColor;
                        source.SelectionBackColor = bgColor;
                        //source.SelectionFont = new Font("Courier New", 12, FontStyle.Regular);
                    }

                    // market text save as komment
                    if (saveAsCommentArray)
                    {
                        storeCommentArray(foundpos, found_right);
                    }

                    if (reverse)
                    {
                        int res = found_right;
                        int lastFound = -1;
                        while (res > 0 && lastFound != res)
                        {
                            res = markTags(source, delimiter1, delimiter2, fontColor, bgColor, res + 1, true);
                            lastFound = res;

                        }
                    }

                    else return found_right;
                }
            }
            return -1;

        }



        public string markString(RichTextBox source, string what, string delimiter1, string delimiter2, Color fontColor, Color bgColor)
        {
            int foundpos = source.Find(what, 0, source.Text.Length, RichTextBoxFinds.MatchCase);



            if (foundpos > 0)
            {
                int maxSearchRight = source.Text.Length;

                string substr = source.Text.Substring(foundpos + what.Length, maxSearchRight - (foundpos + what.Length));
                int startPos = substr.IndexOf(delimiter1);
                int lastPos = substr.IndexOf(delimiter2);

                if (startPos > 0 && lastPos > 0)
                {
                    string foundstring = substr.Substring(startPos + delimiter1.Length, lastPos - (startPos + delimiter2.Length));
                    return foundstring;
                }


            }
            return "NONRESULT";
        }

        public string markString(RichTextBox source, string what, string delimiter1, string delimiter2, Color fontColor, Color bgColor, int start)
        {
            int foundpos = source.Find(what, start, source.Text.Length, RichTextBoxFinds.MatchCase);



            if (foundpos > 0)
            {
                int maxSearchRight = source.Text.Length;

                string substr = source.Text.Substring(foundpos + what.Length, maxSearchRight - (foundpos + what.Length));
                int startPos = substr.IndexOf(delimiter1);
                int lastPos = substr.IndexOf(delimiter2);

                if (startPos > 0 && lastPos > 0)
                {
                    string foundstring = substr.Substring(startPos + delimiter1.Length, lastPos - (startPos + delimiter2.Length));
                    return foundstring;
                }


            }
            return "NONRESULT";
        }


    }
}
