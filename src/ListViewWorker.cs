using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections;

namespace Projector
{
    class ListViewWorker
    {
        const int AUTOSIZE_CONTENT = 0;
        const int AUTOSIZE_HEADER = 1;

        public bool replacePointToComma = true;
        public bool maskInteger = false;

        public int columnAutoSizeMode = 0;
        public void copyListView(ListView source, ListView target)
        {
            copyListView(source,target,1, 1);
        }

       

        public void mergeListView(string matchOn, ListView source, ListView target)
        {
            // add not existing columns and find the matching index
            Stopwatch watch = new Stopwatch();
            int sourceMatchIndex = -1;
            int targetMatchIndex = -1;
            List<int> addColumns = new List<int>();
            List<string> knownCols = new List<string>();
            source.View = View.Details;

            Hashtable sourceIndex = new Hashtable();
            Hashtable targetIndex = new Hashtable();

            for (int a = 0; a < target.Columns.Count; a++)
            {
                knownCols.Add(target.Columns[a].Text);
            }

            for (int a = 0; a < source.Columns.Count; a++)
            {
                if (!knownCols.Contains(source.Columns[a].Text))
                {
                    target.Columns.Add(source.Columns[a].Text);
                    if (source.Columns[a].Text != matchOn)
                    {
                        addColumns.Add(a);
                    }
                } else
                {
                    if (source.Columns[a].Text == matchOn)
                    {
                        sourceMatchIndex = a;
                    }
                }
                
            }
            if (sourceMatchIndex > -1)
            {
                for (int a = 0; a < target.Columns.Count; a++)
                {
                    if (target.Columns[a].Text == matchOn)
                    {
                        targetMatchIndex = a;
                    }
                }

                for (int i = 0; i < target.Items.Count; i++)
                {
                    string sVal = target.Items[i].SubItems[targetMatchIndex].Text;
                    if (!targetIndex.ContainsKey(sVal))
                    {
                        targetIndex.Add(sVal, i);
                    }
                }



                watch.Start();
                if (targetMatchIndex > -1)
                {
                    int updateCnt = 0;
                    for (int i = 0; i < source.Items.Count; i++)
                    {
                        string sVal = source.Items[i].SubItems[sourceMatchIndex].Text;
                        if (targetIndex.ContainsKey(sVal))
                        {
                            int hitPos = (int)targetIndex[sVal];
                            foreach (int add in addColumns)
                            {
                                target.Items[hitPos].SubItems.Add(source.Items[i].SubItems[add].Text);

                            }
                            
                            updateCnt++;
                            if (updateCnt == 20)
                            {
                                target.TopItem = target.Items[hitPos];
                            }
                        }
                        /*
                        for (int r = 0; r < target.Items.Count; r++)
                        {
                            string tVal = target.Items[r].SubItems[targetMatchIndex].Text;
                            if (tVal == sVal)
                            {
                                updateCnt++;
                                if (updateCnt == 20)
                                {
                                    target.TopItem = target.Items[r];
                                }
                                
                                
                                foreach (int add in addColumns)
                                {
                                    target.Items[r].SubItems.Add(source.Items[i].SubItems[add].Text);
                                    
                                }
                            }
                        }*/
                        if (updateCnt > 20)
                        {
                            updateCnt = 0;
                        }
                        if (watch.ElapsedMilliseconds > 500)
                        {
                            target.Update();
                            Application.DoEvents();
                            watch.Restart();
                        }
                    }
                }
            }
            
        } 

        public void fillListView(string matchOn, ListView source, ListView target)
        {
            // add not existing columns and find the matching index
            Stopwatch watch = new Stopwatch();
            int sourceMatchIndex = -1;
            int targetMatchIndex = -1;
            List<int> addColumns = new List<int>();
            List<string> knownCols = new List<string>();
            source.View = View.Details;

            Hashtable sourceIndex = new Hashtable();
            Hashtable targetIndex = new Hashtable();

            for (int a = 0; a < target.Columns.Count; a++)
            {
                knownCols.Add(target.Columns[a].Text);
            }

            for (int a = 0; a < source.Columns.Count; a++)
            {
                if (!knownCols.Contains(source.Columns[a].Text))
                {
                    target.Columns.Add(source.Columns[a].Text);
                    if (source.Columns[a].Text != matchOn)
                    {
                        addColumns.Add(a);
                    }
                } else
                {
                    if (source.Columns[a].Text == matchOn)
                    {
                        sourceMatchIndex = a;
                    }
                }
                
            }
            if (sourceMatchIndex > -1)
            {
                for (int a = 0; a < target.Columns.Count; a++)
                {
                    if (target.Columns[a].Text == matchOn)
                    {
                        targetMatchIndex = a;
                    }
                }

                for (int i = 0; i < target.Items.Count; i++)
                {
                    string sVal = target.Items[i].SubItems[targetMatchIndex].Text;
                    if (targetIndex.ContainsKey(sVal))
                    {
                        List<int> posList = (List<int>)targetIndex[sVal];
                        posList.Add(i);
                        targetIndex[sVal] = posList;
                    }
                    else
                    {
                        List<int> posList = new List<int>();
                        posList.Add(i);
                        targetIndex.Add(sVal, posList);
                    }
                    
                }



                watch.Start();
                if (targetMatchIndex > -1)
                {
                    int updateCnt = 0;
                    for (int i = 0; i < source.Items.Count; i++)
                    {
                        string sVal = source.Items[i].SubItems[sourceMatchIndex].Text;
                        if (targetIndex.ContainsKey(sVal))
                        {
                            List<int> posList = (List<int>)targetIndex[sVal];
                            foreach (int hitPos in posList)
                            {                               
                                foreach (int add in addColumns)
                                {
                                    target.Items[hitPos].SubItems.Add(source.Items[i].SubItems[add].Text);

                                }

                                updateCnt++;
                                if (updateCnt == 20)
                                {
                                    target.TopItem = target.Items[hitPos];
                                }
                            }
                        }

                        if (updateCnt > 20)
                        {
                            updateCnt = 0;
                        }
                        if (watch.ElapsedMilliseconds > 500)
                        {
                            target.Update();
                            Application.DoEvents();
                            watch.Restart();
                        }
                    }
                }
            }
            
        } 


        public void copyListView(ListView source, ListView target,int imageIndex, int selectImageIndex)
        {
            target.Items.Clear();
            target.Columns.Clear();

            for (int a = 0; a < source.Columns.Count; a++)
            {
                target.Columns.Add(source.Columns[a].Text);
            }

            for (int i = 0; i < source.Items.Count; i++)
            {
                ListViewItem tmpItem = new ListViewItem(source.Items[i].Text);

                tmpItem.ImageIndex = imageIndex;
                tmpItem.StateImageIndex = selectImageIndex;
                tmpItem.Name = source.Items[i].Name;

                for (int t = 0; t < source.Items[i].SubItems.Count; t++)
                {
                    if (t > 0)
                    {
                        ListViewItem.ListViewSubItem addTmp = new ListViewItem.ListViewSubItem();
                        addTmp.Text = source.Items[i].SubItems[t].Text;
                        addTmp.Name = source.Items[i].SubItems[t].Name;
                        tmpItem.SubItems.Add(addTmp);
                    }
                    else tmpItem.Text = source.Items[i].SubItems[t].Text;
                }
                target.Items.Add(tmpItem);
            }
        }

        public void addListView(ListView source, ListView target, int imageIndex, int selectImageIndex)
        {
            

            for (int i = 0; i < source.Items.Count; i++)
            {
                ListViewItem tmpItem = new ListViewItem(source.Items[i].Text);

                tmpItem.ImageIndex = imageIndex;
                tmpItem.StateImageIndex = selectImageIndex;
                tmpItem.Name = source.Items[i].Name;

                for (int t = 0; t < source.Items[i].SubItems.Count && t < target.Items[i].SubItems.Count; t++)
                {
                    if (t > 0)
                    {
                        ListViewItem.ListViewSubItem addTmp = new ListViewItem.ListViewSubItem();
                        addTmp.Text = source.Items[i].SubItems[t].Text;
                        addTmp.Name = source.Items[i].SubItems[t].Name;
                        tmpItem.SubItems.Add(addTmp);
                    }
                    else tmpItem.Text = source.Items[i].SubItems[t].Text;
                }
                target.Items.Add(tmpItem);
            }
        }

        private string getGroupName(string tableName)
        {

            string[] prSplit = tableName.Split('_');

            if (prSplit[0] != "" && prSplit[0].Length < 6) return prSplit[0];

            string next = prSplit[0];

            if (next == "")
            {
                for (int i = 1; i < prSplit.Length; i++)
                {
                    if (next == "") next = prSplit[i];
                }
            }

            if (next == "") next = "No Label";

            Regex splitter = new Regex(@"(?<!^)(?=[A-Z])");
            string[] words = splitter.Split(next);
            return words[0];
        }

        public void buildGroups(ListView inview)
        {
            for (int i = 0; i < inview.Items.Count; i++ )
            {
                string groupName = getGroupName( inview.Items[i].Text);
                if (groupName != null)
                {
                    
                    ListViewGroup lwg = new ListViewGroup(groupName, groupName);
                    
                    if (!inview.Groups.Contains(lwg))
                    {
                        inview.Groups.Add(lwg);
                    }

                    inview.Items[i].Group = inview.Groups[groupName];
                }
            }
        }

        public string exportCsv(ListView inview)
        {
            return exportCsv(inview, true);
        }

        public string exportCsv(ListView inview, Boolean header)
        {
            string result = "";
            
            string endLn = System.Environment.NewLine;
            string sep = ";";
            string add = "";
            if (header)
            {
                add = "";
                for (int i = 0; i < inview.Columns.Count; i++)
                {
                    string value = inview.Columns[i].Text;
                    if (replacePointToComma) value = value.Replace('.', ',');
                    result += add + "\"" + value + "\"";
                    add = sep;
                }
                result += endLn;
            }

            for (int i = 0; i < inview.Items.Count; i++)
            {
                add = "";
                for (int p = 0; p < inview.Items[i].SubItems.Count; p++)
                {
                    string value = inview.Items[i].SubItems[p].Text;
                    if (replacePointToComma) value = value.Replace('.', ',');
                    result += add + "\"" + value + "\"";
                    add = sep;
                }
                result += endLn;
            }

            return result;
        }

        public void autoSizeColumns(ListView listView1)
        {
            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                switch (columnAutoSizeMode)
                {
                    case 0:
                        listView1.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);                       
                        break;

                    case 1:
                        listView1.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                        break;

                    default:
                        listView1.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.None);
                        break;

                }
            }
        }


        public void autoSizeColumns(ListView listView1,ColumnHeaderAutoResizeStyle style)
        {
            for (int i = 0; i < listView1.Columns.Count; i++)
            {               
                listView1.Columns[i].AutoResize(style);                        
            }
        }

        public String getEntry(ListView listView1, string columName, int rowNumber)
        {
            if (listView1.Items.Count <= rowNumber)
            {
                return "";
            }

            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                string fieldname = listView1.Columns[i].Text;
                string[] part = fieldname.Split(' ');

                string field = part[0];
                if (field == columName)
                {
                    string result = listView1.Items[rowNumber].SubItems[i].Text;
                    return result;
                }
            }

            return "";
        }

        public ListView setRowColors(ListView source, Color aRowColor, Color bRowColor)
        {
            return this.setRowColors(source, aRowColor, bRowColor, source.ForeColor);
        }

        public ListView setRowColors(ListView source, Color aRowColor, Color bRowColor, Color TextColor)
        {
            int c = 0;
            for (int t = 0; t < source.Items.Count; t++)
            {
                c++;
                if (c > 1) c = 0;
                if (c==0) source.Items[t].BackColor = aRowColor;
                else source.Items[t].BackColor = bRowColor;

                source.Items[t].ForeColor = TextColor;
            }
            return source;
        }    

        public ListView setImageIndex(ListView source,int imageIndex, int selectImageIndex)
        {
            for (int t = 0; t < source.Items.Count; t++)
            {
                source.Items[t].ImageIndex = imageIndex;
                source.Items[t].StateImageIndex = selectImageIndex;
            }
            return source;
        }

        public ListView search(string search, ListView inview, int start)
        {
            bool lastState = inview.Enabled;
            inview.Enabled = false;
            for (int i = start; i < inview.Items.Count; i++)
            {
                for (int t = 0; t < inview.Items[i].SubItems.Count; t++)
                {
                    if (inview.Items[i].SubItems[t].Text.Equals(search))
                    {
                        inview.Items[i].Selected = true;
                        //if (inview.Items[i]!=null) inview.TopItem = inview.Items[i];
                        inview.Update();
                    }
                }
            }
            inview.Enabled = lastState;
            return inview;
        }

        public ListView searchAndmark(string search, ListView inview, int start, int imageIndex, int noFoundImageIndex)
        {
            bool lastState = inview.Enabled;
            inview.Enabled = false;
            for (int i = start; i < inview.Items.Count; i++)
            {
                inview.Items[i].ImageIndex = noFoundImageIndex;
                for (int t = 0; t < inview.Items[i].SubItems.Count; t++)
                {
                    if (inview.Items[i].SubItems[t].Text.Equals(search))
                    {
                        inview.Items[i].Selected = true;
                        inview.Items[i].ImageIndex = imageIndex;
                        //if (inview.Items[i]!=null) inview.TopItem = inview.Items[i];
                        //inview.Update();
                    }

                }
            }
            inview.Enabled = lastState;
            return inview;
        }


        public ListView searchAndmark(string search, ListView inview, int start,Color mark)
        {
            bool lastState = inview.Enabled;
            inview.Enabled = false;
            
            for (int i = start; i < inview.Items.Count; i++)
            {
                //inview.Items[i].BackColor = Color.Transparent;
                for (int t = 0; t < inview.Items[i].SubItems.Count; t++)
                {
                    if (inview.Items[i].SubItems[t].Text.Equals(search))
                    {
                        //inview.Items[i].Selected = true;
                        //inview.Items[i].ImageIndex = imageIndex;
                        inview.Items[i].SubItems[t].BackColor = mark;
                        
                        //if (inview.Items[i]!=null) inview.TopItem = inview.Items[i];
                        //inview.Update();
                    }

                }
            }
            inview.Enabled = lastState;
            return inview;
        }

        public ListView searchAndSelect(string search, ListView inview, int start)
        {
            bool lastState = inview.Enabled;
            inview.Enabled = false;

            for (int i = start; i < inview.Items.Count; i++)
            {

                for (int t = 0; t < inview.Items[i].SubItems.Count; t++)
                {
                    if (inview.Items[i].SubItems[t].Text.Equals(search))
                    {
                        inview.Items[i].Selected = true;
                    }   
                }
            }
            inview.Enabled = lastState;
            return inview;
        }


        public int quickFind(string searchString, ListView sourcetabs)
        {
            return quickFind(searchString, sourcetabs, false);
        }

        public int quickFind(string searchString, ListView sourcetabs,bool selectit)
        {
            bool topItemIsSet = false;
            int firstSelect = 0;
            for (int i = 0; i < sourcetabs.Items.Count; i++)
            {
                int searchLen = searchString.Length;
                if (sourcetabs.Items[i].Text.Length >= searchLen)
                {
                    string leftStr = sourcetabs.Items[i].Text.Substring(0, searchLen);
                    if (leftStr == searchString)
                    {
                        if (selectit) sourcetabs.Items[i].Selected = true;
                        if (!topItemIsSet)
                        {
                            sourcetabs.TopItem = sourcetabs.Items[i];
                            firstSelect = i;
                        }
                        topItemIsSet = true;
                        if (!selectit) return i;
                    }
                    else
                    {
                        if (selectit) sourcetabs.Items[i].Selected = false;
                    }
                }
                else
                {
                    if (selectit) sourcetabs.Items[i].Selected = false;
                }
            }
            return firstSelect;
        }


        public int searchAndmark(string search, ListView inview, int start, int imageIndex, int noFoundImageIndex, int max, bool endOnFound, bool foundit)
        {
            bool lastState = inview.Enabled;
            inview.Enabled = false;
            int pm = 0;
            int pp = 0;
            foundit = false;
            for (int i = start; i < inview.Items.Count; i++)
            {
                pm = i;
                pp++;
                if (pp >= max)
                {
                    inview.Enabled = lastState;
                    return pm;
                }

                inview.Items[i].ImageIndex = noFoundImageIndex;
                for (int t = 0; t < inview.Items[i].SubItems.Count; t++)
                {
                    if (inview.Items[i].SubItems[t].Text.Equals(search))
                    {
                        inview.Items[i].Selected = true;
                        inview.Items[i].ImageIndex = imageIndex;
                        //if (inview.Items[i]!=null) inview.TopItem = inview.Items[i];
                        //inview.Update();
                        foundit = true;
                        if (endOnFound)
                        {
                            inview.Enabled = lastState;

                            return pm;
                        }

                    }

                }
            }
            inview.Enabled = lastState;
            return pm;
        }
    }
}
