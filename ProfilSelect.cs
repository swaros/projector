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
    public partial class ProfilSelect : Form
    {
        XmlSetup profiles = new XmlSetup();
        public string selectedProfil = null;

        public ProfilSelect()
        {
            InitializeComponent();
            profiles.setFileName("Projector_profiles.xml");
            this.transLate();
            this.loadProfiles();
        }

        private void transLate()
        {
            
            this.newProflie.Text = Projector.Properties.Resources.profilNew;
            this.groupBox1.Text = Projector.Properties.Resources.profilGroup;
            this.okbtn.Text = Projector.Properties.Resources.btnOk;
            this.cancelbtn.Text = Projector.Properties.Resources.btnCancel;
            this.profil_label.Text = Projector.Properties.Resources.defaultTxt;
            
        }

        private void addProfile()
        {
            string keyName = "profil_" + (profiles.count + 1);
            if (profileName.Text.Length > 1)
            {
                profiles.addSetting(keyName, profileName.Text);
                profiles.saveXml();
                if (profiles.lastError.Length > 0)
                {
                    MessageBox.Show("Error: " + profiles.lastError, Projector.Properties.Resources.ProfileErrorOnSaveMessage, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    profiles.loadXml();
                    profiles.lastError = "";
                }
                refreshList(profiles);
            }
        }

        private void refreshList(XmlSetup profiles)
        {
            listViewProfiles.Items.Clear();
            for (Int64 i = 0; i < profiles.count; i++)
            {
                string keyname = "profil_" + (i + 1);
                string proName = profiles.getValue(keyname);

                ListViewItem tmp = new ListViewItem(proName);
                tmp.SubItems.Add(keyname);
                tmp.ImageIndex = 1;

                if (proName != null) listViewProfiles.Items.Add(tmp);
            }
        }


        private void loadProfiles()
        {
            
            profiles.loadXml();
            refreshList(profiles);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.addProfile();
        }

        private void listViewProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewProfiles.SelectedItems.Count > 0)
                selectedProfil = listViewProfiles.SelectedItems[0].Text;
            this.profil_label.Text = selectedProfil;
        }

        private void listViewProfiles_DoubleClick(object sender, EventArgs e)
        {
            if (listViewProfiles.SelectedItems.Count > 0)
                selectedProfil = listViewProfiles.SelectedItems[0].Text;
            this.profil_label.Text = selectedProfil;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        
    }
}
