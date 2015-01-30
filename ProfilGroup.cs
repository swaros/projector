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
    public partial class ProfilGroup : Form
    {


        PConfig Config = new PConfig();

        List<string> currentGroups;

        Boolean userIsContoller = false;

        public ProfilGroup(List<string> existingGroup)
        {
            InitializeComponent();
            List<string> groups = Config.getListWidthDefault(PConfig.KEY_GROUPS_NAMES, existingGroup);

            this.currentGroups = groups;
            foreach (string groupLabel in groups)
            {
                groupName.Items.Add(groupLabel);
            }

        }

        private void selectExistsTables()
        {
            Profil dbProf = new Profil("default");
            ColorCalc cCalc = new ColorCalc();
            string groupId = groupName.Text.ToLower();
            if (this.currentGroups.Contains(groupId))
            {
                groupName.ForeColor = Color.DarkGreen;
                groupName.BackColor = Color.LightSeaGreen;
                List<string> members = Config.getListWidthDefault(PConfig.KEY_GROUPS_MEMBERS + "." + groupId, new List<string>());

                for (int i = 0; i < this.GroupedDatabases.Items.Count; i++)
                {
                    this.GroupedDatabases.Items[i].Checked = (members.Contains(this.GroupedDatabases.Items[i].Text));

                    dbProf.changeProfil(this.GroupedDatabases.Items[i].Text);
                    if (dbProf.getProperty("set_bgcolor") != null && dbProf.getProperty("set_bgcolor").Length > 2)
                    {
                        Color rowCol = Color.FromArgb(int.Parse(dbProf.getProperty("set_bgcolor")));
                        cCalc.setBaseColor(rowCol);
                        this.GroupedDatabases.Items[i].BackColor = cCalc.LightenBy(50);
                        this.GroupedDatabases.Items[i].ForeColor = cCalc.DarkenBy(50);
                    }


                }
                manipulateBtn.Image = Projector.Properties.Resources.delete_16;
                GroupedDatabases.Enabled = true;

            }
            else
            {
                groupName.ForeColor = Color.DarkRed;
                groupName.BackColor = Color.LightYellow;
                manipulateBtn.Image = Projector.Properties.Resources.add_16;
                for (int i = 0; i < this.GroupedDatabases.Items.Count; i++)
                {
                    this.GroupedDatabases.Items[i].Checked = false;
                }
                GroupedDatabases.Enabled = false;
            }

        }


        private void groupName_TextChanged(object sender, EventArgs e)
        {
            this.userIsContoller = false;
            this.selectExistsTables();
            this.userIsContoller = true;
        }

        private void GroupedDatabases_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (userIsContoller)
            {
                string profilName = e.Item.Text;
                string groupId = groupName.Text.ToLower();
                if (groupId.Length > 3)
                {

                    if (this.currentGroups.Contains(groupId))
                    {

                        List<string> members = Config.getListWidthDefault(PConfig.KEY_GROUPS_MEMBERS + "." + groupId, new List<string>());
                        if (e.Item.Checked)
                        {
                            if (!members.Contains(profilName))
                            {
                                members.Add(profilName);
                            }
                            else
                            {
                                e.Item.Checked = false;
                            }
                        }
                        else
                        {
                            if (members.Contains(profilName))
                            {
                                members.Remove(profilName);                                
                            }
                        }
                        Config.setList(PConfig.KEY_GROUPS_MEMBERS + "." + groupId, members);
                    }
                }
                else
                {
                    e.Item.Checked = false;
                }
            }

        }

        private void manipulateBtn_Click(object sender, EventArgs e)
        {
            string groupId = groupName.Text.ToLower();
            if (this.currentGroups.Contains(groupId))
            {
                // removing
                this.currentGroups.Remove(groupId);
                this.groupName.Items.Remove(groupId);
            }
            else
            {
                this.currentGroups.Add(groupId);
                this.groupName.Items.Add(groupId);
            }
            Config.setList(PConfig.KEY_GROUPS_NAMES,this.currentGroups);
            selectExistsTables();
        }


    }
}
