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
    public partial class CreateStructForm : Form
    {

        MysqlCreateStructContainer CreateStructCont = new MysqlCreateStructContainer();
        HighlighterMysql highLight = new HighlighterMysql();

        bool clickRowEnabled = false;

        public CreateStructForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            MysqlCreateStruct addStruct = new MysqlCreateStruct();
            addStruct.autoIncrement = colAutoInc.Checked;
            addStruct.name = colName.Text;
            addStruct.defaultValue = colDefault.Text;
            addStruct.dataType = colType.Text;
            addStruct.isPrimary = colPrim.Checked;
            addStruct.length = (ulong) colLength.Value;
            addStruct.notNull = colNotNull.Checked;
            addStruct.comments = "";

            ListViewItem addLvItem = new ListViewItem();
            addStruct.refObject = addLvItem;

            addLvItem.Tag = addStruct;

            if (CreateStructCont.addStruct(addStruct))
            {

                listView1.Items.Add(addLvItem);
                updateLvItem(addLvItem);
                updateSql();
            }
            else
            {
                MessageBox.Show("Fieldname Allready in use");
            }

        }

        private void updateFormByListItem(ListViewItem lvItem)
        {
                MysqlCreateStruct mStr = (MysqlCreateStruct)lvItem.Tag;
                colAutoInc.Checked = mStr.autoIncrement;
                colName.Text = mStr.name;
                colDefault.Text = mStr.defaultValue;
                colType.Text = mStr.dataType;
                colPrim.Checked = mStr.isPrimary;
                colLength.Value = mStr.length;
                colNotNull.Checked = mStr.notNull;
                
        }

        private void updateLvItem(ListViewItem lvItem)
        {
            MysqlCreateStruct mStr = (MysqlCreateStruct)lvItem.Tag;


            if (lvItem.SubItems.Count < 6)
            {
                lvItem.SubItems.Clear();
                lvItem.SubItems.Add(mStr.dataType);
                lvItem.SubItems.Add(mStr.length.ToString());
                lvItem.SubItems.Add(mStr.defaultValue);
                lvItem.SubItems.Add("");
                lvItem.SubItems.Add(mStr.comments);
            }
            else
            {
                lvItem.SubItems[1].Text = mStr.dataType;
                lvItem.SubItems[2].Text = mStr.length.ToString();
                lvItem.SubItems[3].Text = mStr.defaultValue;
                lvItem.SubItems[4].Text = "";
                lvItem.SubItems[5].Text = mStr.comments;
            }
            lvItem.Text = mStr.name;
            if (mStr.isPrimary) lvItem.BackColor = Color.LightGreen;
            else lvItem.BackColor = Color.Transparent;

        }
       
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                updateFormByListItem(listView1.SelectedItems[0]);

                if (clickRowEnabled && indexList.SelectedItems.Count > 0)
                {
                    string existingFields = indexList.SelectedItems[0].SubItems[1].Text;
                    if (existingFields == "")
                    {
                        indexList.SelectedItems[0].SubItems[1].Text = listView1.SelectedItems[0].Text;
                        List<string> fields = new List<string>();
                        fields.Add(listView1.SelectedItems[0].Text);
                        CreateStructCont.addIndex(indexList.SelectedItems[0].Text, fields);
                    }
                    else
                    {
                        indexList.SelectedItems[0].SubItems[1].Text += "," + listView1.SelectedItems[0].Text;
                        CreateStructCont.addFieldToIndex(indexList.SelectedItems[0].Text, listView1.SelectedItems[0].Text);
                    }
                    updateSql();
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                MysqlCreateStruct addStruct = (MysqlCreateStruct)listView1.SelectedItems[0].Tag;
                addStruct.autoIncrement = colAutoInc.Checked;
                addStruct.name = colName.Text;
                addStruct.defaultValue = colDefault.Text;
                addStruct.dataType = colType.Text;
                addStruct.isPrimary = colPrim.Checked;
                addStruct.length = (ulong)colLength.Value;
                addStruct.notNull = colNotNull.Checked;
                addStruct.comments = "";
                updateLvItem (listView1.SelectedItems[0]);
            }
            updateSql();
        }


        private void updateSql()
        {
            if (tableName.Text.Length > 0)
            {
                CreateStructCont.setTablename(tableName.Text);
                CreateStructCont.setEngine(tableType.Text);
                sqlBox.Text = CreateStructCont.getCreationSql();
                highLight.parse(sqlBox);
            }
        }

        private void tableName_TextChanged(object sender, EventArgs e)
        {
            tabControl1.Enabled = (tableName.Text.Length > 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListViewItem addItm = new ListViewItem();

            addItm.Text = keynameBox.Text;
            addItm.SubItems.Add("");
            addItm.SubItems.Add(keyTypeBox.Text);

            indexList.Items.Add(addItm);
            

        }

        private void indexList_SelectedIndexChanged(object sender, EventArgs e)
        {
            rowClickBtn.Enabled = (indexList.SelectedItems.Count > 0);
            clickRowEnabled = false;
            updateClickRowStatus();
        }

        private void updateClickRowStatus()
        {
            indexList.Enabled = !clickRowEnabled;
            if (clickRowEnabled)
            {
                rowClickBtn.Image = Properties.Resources.arrow_up_24;
                rowClickBtn.Text = "Now click on the Rows to add the fields. Click me again to stop ";
                this.Cursor = Cursors.Hand;
            }
            else
            {
                rowClickBtn.Image = Properties.Resources.arrow_back_24;
                rowClickBtn.Text = "Select Index to Add Fields";
                this.Cursor = Cursors.Default;
            }
        }

        private void rowClickBtn_Click(object sender, EventArgs e)
        {
            clickRowEnabled = !clickRowEnabled;
            updateClickRowStatus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (indexList.SelectedItems.Count > 0)
                indexList.SelectedItems[0].Remove();
        }
    }
}
