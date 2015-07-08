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
       
        public string selectedProfil = null;

        private PConfig Setup;
        List<string> existingProfiles;

        public ProfilSelect()
        {
            InitializeComponent();
            this.Setup = new PConfig();
            this.transLate();
            
            this.existingProfiles = this.Setup.getListWidthDefault(PConfig.KEY_PROFILS, new List<string>());
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
            if (!this.existingProfiles.Contains(profileName.Text))
            {
                Profil addProfil = new Profil(profileName.Text);

            }
           
        }

        private void refreshList()
        {
            listViewProfiles.Items.Clear();
            foreach (string profil in this.existingProfiles)
            {
                
                ListViewItem tmp = new ListViewItem(profil);
                tmp.SubItems.Add(profil);
                tmp.ImageIndex = 1;

                listViewProfiles.Items.Add(tmp);
            }
        }


        private void loadProfiles()
        {                        
            refreshList();
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
