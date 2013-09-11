using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Projector
{
    public partial class CretaeInsertSelect : Form
    {

        private bool isInit = false;
        private bool disConnect = false;
        List<MysqlStruct> sourceStruct;
        List<MysqlStruct> targetStruct;
        private MysqlHandler db;
        private string tableName;

        Hashtable Tables = new Hashtable();

        public CretaeInsertSelect()
        {
            InitializeComponent();
            //button2.Visible = false;
        }


        public void setTables(Profil useProfil, string source)
        {
            this.db = new MysqlHandler(useProfil);
            init(db, source);
        }


        private void init(MysqlHandler db, string source)
        {
            this.tableName = source;

            if (!db.isConnected())
            {
                disConnect = true;
                db.connect();
            }

            sourceStruct = db.getAllFieldsStruct(source);


            isInit = true;
            redrawComponents();

        }


        private void redrawComponents()
        {
            if (this.isInit)
            {
                Source.Items.Clear();
                target.Nodes.Clear();

                for (int i = 0; i < sourceStruct.Count; i++)
                {
                    ListViewItem lwLeft = new ListViewItem();
                    lwLeft.Text = sourceStruct[i].name;
                    lwLeft.SubItems.Add(sourceStruct[i].realType);

                    if (sourceStruct[i].Key != "")
                    {
                        if (sourceStruct[i].Key != "PRI")
                        {
                            lwLeft.ImageIndex = 2;
                            
                        }
                        else
                        {
                            lwLeft.ImageIndex = 3;
                            
                        }
                    }

                    Source.Items.Add(lwLeft);
                }


                List<string> tables = this.db.getTableList();
                for (int p = 0; p < tables.Count; p++)
                {

                    TreeNode tn = new TreeNode();
                    tn.Text = tables[p];
                    tn.SelectedImageIndex = 1;
                    tn.ImageIndex = 1;
                 

                    TreeNode subTn = new TreeNode();
                    subTn.Text = "...reading";
                    subTn.ImageIndex = 0;
                    subTn.SelectedImageIndex = 0;
                    tn.Nodes.Add(subTn);

                    target.Nodes.Add(tn);
                }
            }

        }

        private void target_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.LastNode.Text == "...reading")
            {
                e.Node.Nodes.Clear();

                targetStruct = db.getAllFieldsStruct(e.Node.Text);
                for (int i = 0; i < targetStruct.Count; i++)
                {
                    TreeNode subTn = new TreeNode();
                    subTn.Text = targetStruct[i].name;
                    subTn.ToolTipText = targetStruct[i].realType;

                    if (targetStruct[i].Key != "")
                    {
                        if (targetStruct[i].Key != "PRI")
                        {
                            subTn.ImageIndex = 2;
                            subTn.SelectedImageIndex = 2;
                        }
                        else
                        {
                            subTn.ImageIndex = 3;
                            subTn.SelectedImageIndex = 3;
                        }
                    }

                    e.Node.Nodes.Add(subTn);
                }

            }
        }

        private void addToCompare(int group)
        {
            if (Source.SelectedItems.Count > 0)
            {

                if (target.SelectedNode != null && target.SelectedNode.Level == 1 && !listView1.Items.ContainsKey(Source.SelectedItems[0].Text + group))
                {
                    ListViewItem lwAdd = new ListViewItem();
                    lwAdd.Text = Source.SelectedItems[0].Text;
                    lwAdd.Name = Source.SelectedItems[0].Text + group;
                    lwAdd.SubItems.Add( target.SelectedNode.Parent.Text + "." +  target.SelectedNode.Text);
                    if (group >= 0 && group < listView1.Groups.Count) lwAdd.Group = listView1.Groups[group];
                    listView1.Items.Add(lwAdd);

                    if (!Tables.Contains(target.SelectedNode.Parent.Text))
                    {
                        Tables.Add(target.SelectedNode.Parent.Text, "added");
                    }

                }
                else
                {
                    MessageBox.Show("Please Select a Target in the Right field Tree","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addToCompare(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //addToCompare(1);
            if (target.SelectedNode != null && target.SelectedNode.Level > 0)
            {
                UserTextInput inpForm = new UserTextInput();
                inpForm.textinfo.Text = "";
                if (inpForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ListViewItem lwAdd = new ListViewItem();
                    lwAdd.Text = target.SelectedNode.Text;
                    lwAdd.Name = target.SelectedNode.Text + 1;
                    lwAdd.SubItems.Add(inpForm.textinfo.Text);

                    lwAdd.Group = listView1.Groups[1];
                    listView1.Items.Add(lwAdd);
                }

            }
        }


        public string buildSql()
        {
            InsertSelect sqlBuild = new InsertSelect(this.tableName, this.Tables);
            List<string> wheres = new List<string>();

            // where field compares
            for (int i = 0; i < listView1.Groups[1].Items.Count; i++)
            {

                wheres.Add(listView1.Groups[1].Items[i].Text + " = " + listView1.Groups[1].Items[i].SubItems[1].Text);
            }
            
            

            sqlBuild.addWhereList(wheres);

            for (int i = 0; i < listView1.Groups[0].Items.Count; i++)
            {
                sqlBuild.copyField(listView1.Groups[0].Items[i].Text, listView1.Groups[0].Items[i].SubItems[1].Text);
            }

            for (int i = 0; i < listView1.Groups[2].Items.Count; i++)
            {
                sqlBuild.copyField(listView1.Groups[2].Items[i].Text, listView1.Groups[2].Items[i].SubItems[1].Text);
            }
            return sqlBuild.getStatement();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void autoAssign()
        {
            for (int i = 0; i < Source.SelectedItems.Count; i++)
            {
                for (int p = 0; p < target.SelectedNode.Nodes.Count; p++)
                {
                    if (Source.SelectedItems[i].Text == target.SelectedNode.Nodes[p].Text && !listView1.Items.ContainsKey(Source.SelectedItems[i].Text + "0"))
                    {
                        ListViewItem lwAdd = new ListViewItem();
                        lwAdd.Text = Source.SelectedItems[i].Text;
                        lwAdd.Name = Source.SelectedItems[i].Text+"0";
                        lwAdd.SubItems.Add(target.SelectedNode.Text + "." + target.SelectedNode.Nodes[p].Text);
                        lwAdd.Group = listView1.Groups[0];
                        listView1.Items.Add(lwAdd);

                        if (!Tables.Contains(target.SelectedNode.Text))
                        {
                            Tables.Add(target.SelectedNode.Text, "added");
                        }
                    }
                }
            }
        }


        private void Source_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Source.SelectedItems != null && Source.SelectedItems.Count > 1 && target.SelectedNode != null)
            {

                if (target.SelectedNode.IsExpanded && target.SelectedNode.Level == 0)
                {
                    // trying to autoinsert
                    autoAssign();
                    

                }

            }
        }

        private void target_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (Source.SelectedItems != null && Source.SelectedItems.Count > 1 && target.SelectedNode != null)
            {

                if (target.SelectedNode.IsExpanded && target.SelectedNode.Level == 0)
                {
                    // trying to autoinsert
                    autoAssign();


                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Source.SelectedItems.Count > 0)
            {
                UserTextInput inpForm = new UserTextInput();
                inpForm.textinfo.Text = "";
                if (inpForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ListViewItem lwAdd = new ListViewItem();
                    lwAdd.Text = Source.SelectedItems[0].Text;
                    lwAdd.Name = Source.SelectedItems[0].Text + 2;
                    if (Source.SelectedItems[0].SubItems[1].Text == "varchar" || Source.SelectedItems[0].SubItems[1].Text == "text")
                        lwAdd.SubItems.Add("'" + inpForm.textinfo.Text + "'");
                    else
                        lwAdd.SubItems.Add(inpForm.textinfo.Text);

                    lwAdd.Group = listView1.Groups[2];
                    listView1.Items.Add(lwAdd);
                }

            }
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listView1.SelectedItems.Count > 0)
            {
                for (int i = listView1.SelectedItems.Count - 1; i >= 0; i--)
                {
                    listView1.SelectedItems[i].Remove();
                }
            }
        }

    }
}
