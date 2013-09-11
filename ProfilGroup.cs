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

        Hashtable settings;

        public ProfilGroup()
        {
            InitializeComponent();
            XmlSetup pSetup = new XmlSetup();
            pSetup.setFileName("profileGroups.xml");
            pSetup.loadXml();

            settings = pSetup.getHashMap();

            foreach (DictionaryEntry de in settings)
            {
                //de.Key.ToString();                
                //de.Value.ToString();
                groupName.Items.Add(de.Key.ToString());
            }

        }

        private void selectExistsTables()
        {
            if (settings.ContainsKey(groupName.Text.ToLower()))
            {

                string[] profiles = settings[groupName.Text.ToLower()].ToString().Split('|');
                for (int i = 0; i < this.GroupedDatabases.Items.Count; i++)
                {
                    this.GroupedDatabases.Items[i].Checked = (profiles.Contains(this.GroupedDatabases.Items[i].Text));
                }

               
            }
        }


        private void groupName_TextChanged(object sender, EventArgs e)
        {
            selectExistsTables();
        }

        
    }
}
