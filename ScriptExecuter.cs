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
    public partial class ScriptExecuter : Form
    {
        public Profil usedProfile;
        private MysqlHandler Connection;
        
        public ScriptExecuter()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Connection = new MysqlHandler(usedProfile);

            Connection.connect();
            progress.Maximum = queryList.SelectedItems.Count;
            progress.Value = 0;
            for (int i = queryList.SelectedItems.Count-1; i >=0; i--)
            {
                state.Text = queryList.SelectedItems[i].SubItems[1].Text;
                Connection.sql_update(queryList.SelectedItems[i].SubItems[1].Text);
                if (Connection.lastSqlErrorMessage != "")
                {
                    if (MessageBox.Show(this, Connection.lastSqlErrorMessage + "\n Abort ? ", "SQL Error", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
                    {
                        break;

                    }
                }
                queryList.SelectedItems[i].Remove();

                progress.Value = progress.Maximum - i;
                Application.DoEvents();
            }


            Connection.disConnect();
        }

        public String createSqlExport()
        {
            String tmpBox = "";
            tmpBox = "/***********************************************/" + System.Environment.NewLine;
            tmpBox += "/* ======== Export dump from Projector ======= */" + System.Environment.NewLine;
            tmpBox += "/*                                             */" + System.Environment.NewLine;
            tmpBox += "/***********************************************/" + System.Environment.NewLine;
            tmpBox += "\n";
            for (int i = queryList.Items.Count - 1; i >= 0; i--)
            {
                tmpBox += queryList.Items[i].SubItems[1].Text + ";" + System.Environment.NewLine;
            }

            tmpBox += "/***************** END ***********************/" + System.Environment.NewLine;
            return tmpBox;
        }


        private void saveBTN_Click(object sender, EventArgs e)
        {
            if (writeSql.ShowDialog() == DialogResult.OK)
            {
                String tmpBox = "";
                tmpBox = "/***********************************************/" + System.Environment.NewLine;
                tmpBox += "/* ======== Export dump from Projector ======= */" + System.Environment.NewLine;
                tmpBox += "/*                                             */" + System.Environment.NewLine;
                tmpBox += "/***********************************************/" + System.Environment.NewLine;
                tmpBox += "\n";
                for (int i = queryList.Items.Count - 1; i >= 0; i--)
                {
                    tmpBox += queryList.Items[i].SubItems[1].Text + ";" + System.Environment.NewLine; 
                }

                tmpBox += "/***************** END ***********************/" + System.Environment.NewLine;

                System.IO.File.WriteAllText(writeSql.FileName, tmpBox);
                
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            GroupQuery gq = new GroupQuery();
            gq.queryListing.Items.Clear();
            string add = "";
            for (int i = queryList.SelectedItems.Count - 1; i >= 0; i--)
            {
                //gq.queryListing.Items.Add(queryList.SelectedItems[i].SubItems[1].Text);
                /*
                GroupQuery gq = new GroupQuery();
                gq.richTextBox1.Text = queryList.SelectedItems[i].SubItems[1].Text;
                gq.ShowDialog();
                 */
                gq.richTextBox1.Text += add + queryList.SelectedItems[i].SubItems[1].Text;
                add = ";" + System.Environment.NewLine;
            }
            gq.ShowDialog();
        }
    }
}
