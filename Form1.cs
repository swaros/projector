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
    public partial class ProjectorForm : Form
    {

        const int SCRIPT_BUTTON_MODE = 3;
        const int STYLE_BUTTON_MODE = 2;
        const int STYLE_BIG_QUICK_BUTTONS = 0;
        const int STYLE_SMALL_QUICK_BUTTONS = 1;

        private Profil profil = new Profil("default");
        List<Button> ProfilBtn = new List<Button>();

        private bool showGroups = false;

        private int buttonStyle = 0;
        private int maxStyle = 3;

        private string scriptFile;

        private Boolean displayNamedScripstOnly = false;
        private string mainScriptFolder;

        private List<string> profilGroups = new List<string>();

        RefScrAutoStart ScriptAutoLoader = new RefScrAutoStart();

        PConfig Setup = new PConfig();

        public ProjectorForm()
        {
            InitializeComponent();
            this.mainScriptFolder = Application.StartupPath;
            Setup.loadRuntimeConfig();
            updateProfilSelector();

            

            //this.BackColor = SystemColors.Control;
           
            
        }


        private void profilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProfilSelect pro = new ProfilSelect();
            if (pro.ShowDialog() == DialogResult.OK)
            {
                if (pro.selectedProfil != null) profil.changeProfil(pro.selectedProfil);
                ProfilInfo.Text = pro.selectedProfil + " [" + profil.getProperty("db_username") + '@' + profil.getProperty("db_schema") + "]";               
                updateProfilSelector();
            }
        }

        private void copySettings(Boolean forSaving)
        {
            if (forSaving)
            {
                this.Setup.setValue("client.left", this.Left);
                this.Setup.setValue("client.top", this.Top);
                this.Setup.setValue("client.width", this.Width);
                this.Setup.setValue("client.height", this.Height);

                this.Setup.setValue("client.showgroups", this.showGroups);

                this.Setup.setValue("client.showgroups", mainSlitter.Panel1Collapsed);                
                this.Setup.setValue("client.buttonstyle", this.buttonStyle);

                this.Setup.setValue("client.scriptpath", this.mainScriptFolder);
                this.Setup.setValue("client.namedscriptsonly", this.displayNamedScripstOnly);

                this.Setup.setList ("client.groups.names", this.profilGroups);
            }
            else
            {
                this.Left = this.Setup.getIntSettingWidthDefault("client.left", this.Left);
                this.Top = this.Setup.getIntSettingWidthDefault("client.top", this.Top);
                this.Width = this.Setup.getIntSettingWidthDefault("client.width", this.Width);
                this.Height = this.Setup.getIntSettingWidthDefault("client.height", this.Height);

                this.showGroups = this.Setup.getBooleanSettingWidthDefault("client.showgroups", this.showGroups);

                mainSlitter.Panel1Collapsed = this.Setup.getBooleanSettingWidthDefault("client.showgroups", mainSlitter.Panel1Collapsed);
                groupButtonsToolStripMenuItem.Checked = !mainSlitter.Panel1Collapsed;

                this.buttonStyle = this.Setup.getIntSettingWidthDefault("client.buttonstyle", this.buttonStyle);

                this.mainScriptFolder = this.Setup.getSettingWidthDefault("client.scriptpath", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
                this.displayNamedScripstOnly = this.Setup.getBooleanSettingWidthDefault("client.namedscriptsonly", this.displayNamedScripstOnly);
                groupedToolStripMenuItem.Checked = this.showGroups;

                this.profilGroups = this.Setup.getListWidthDefault("client.groups.names", this.profilGroups);
            }
            
        }


        private void initApp()
        {
            /*
            XmlSetup tmpSetup = new XmlSetup();
            tmpSetup.setFileName("Projector_config.xml");
            tmpSetup.loadXml();
            string check;
             */

            this.copySettings(false);
            updateStyleButtons();

            /*
            check = tmpSetup.getSetting("client_left");
            if (check != null && check != "" && int.Parse(check) > 0) this.Left = int.Parse(check);

            check = tmpSetup.getSetting("client_top");
            if (check != null && check != "" && int.Parse(check) > 0) this.Top = int.Parse(check);

            check = tmpSetup.getSetting("client_w");
            if (check != null && check != "" && int.Parse(check) > 100) this.Width = int.Parse(check);

            check = tmpSetup.getSetting("client_h");
            if (check != null && check != "" && int.Parse(check) > 100) this.Height = int.Parse(check);

            check = tmpSetup.getSetting("showgroup");
            if (check != null && check != "") this.showGroups = (int.Parse(check) == 1);

            check = tmpSetup.getSetting("showgroupbuttons");
            if (check != null && check != "") groupButtonsToolStripMenuItem.Checked = (int.Parse(check) == 1);
            mainSlitter.Panel1Collapsed = !groupButtonsToolStripMenuItem.Checked;

            check = tmpSetup.getSetting("buttonstyle");
            if (check != null && check != "") this.buttonStyle = int.Parse(check);

            check = tmpSetup.getSetting("scriptpath");
            if (check != null && check != "" && System.IO.Directory.Exists(check)) this.mainScriptFolder = check;

            check = tmpSetup.getSetting("namedscriptsonly");
            if (check != null && check != "") this.displayNamedScripstOnly = (int.Parse(check) == 1);

            groupedToolStripMenuItem.Checked = this.showGroups;
            */
            /*
            tmpSetup.addSetting("client_left", leftPosition.ToString());
            tmpSetup.addSetting("client_top", topPosition.ToString());
            tmpSetup.addSetting("client_w", w.ToString());
            tmpSetup.addSetting("client_h", h.ToString());
             * 
             *  tmpSetup.addSetting("buttonStyle", buttonStyle.ToString());
            if (showGroups) tmpSetup.addSetting("showGroup", "1");
            else tmpSetup.addSetting("showGroup", "0");
            */
            drawGroupButtons();

        }

        private void ProjectorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            int leftPosition = this.Left;
            int topPosition = this.Top;
            int w = this.Width;
            int h = this.Height;

            XmlSetup tmpSetup = new XmlSetup();
            tmpSetup.setFileName("Projector_config.xml");

            tmpSetup.addSetting("client_left", leftPosition.ToString());
            tmpSetup.addSetting("client_top", topPosition.ToString());
            tmpSetup.addSetting("client_w", w.ToString());
            tmpSetup.addSetting("client_h", h.ToString());
            tmpSetup.addSetting("buttonstyle", buttonStyle.ToString());
            if (showGroups) tmpSetup.addSetting("showgroup", "1");
            else tmpSetup.addSetting("showgroup", "0");

            if (this.displayNamedScripstOnly) tmpSetup.addSetting("namedscriptsonly", "1");
            else tmpSetup.addSetting("namedscriptsonly", "1");

            if (groupButtonsToolStripMenuItem.Checked) tmpSetup.addSetting("showgroupbuttons", "1");
            else tmpSetup.addSetting("showgroupbuttons", "0");

            tmpSetup.addSetting("scriptpath", this.mainScriptFolder);

            tmpSetup.saveXml();
            */

            this.copySettings(true);

            this.Setup.saveRuntimeConfig();
        }


        private void updateGroupButonStyle(Button template)
        {
            for (int i = 0; i < flowLayoutControllPanel.Controls.Count;i++ )
            {
                Object tmpBtn = flowLayoutControllPanel.Controls[i];
                if (tmpBtn is Button)
                {
                    Button btn = (Button)tmpBtn;
                    btn.FlatStyle = template.FlatStyle;
                    btn.ForeColor = template.ForeColor;
                    btn.BackColor = template.BackColor;
                }
            }
        }

        private void drawGroupButtons()
        {

            flowLayoutControllPanel.Controls.Clear();

            XmlSetup pSetup = new XmlSetup();
            pSetup.setFileName("profileGroups.xml");
            pSetup.loadXml();


            Hashtable settings = pSetup.getHashMap();
            chooseGroup.Items.Clear();
            chooseGroup.Items.Add("[ALL]");

            Button tmpBtnx = new Button();
            tmpBtnx.Text = "ALL";
            tmpBtnx.Click += new System.EventHandler(btnSelectGroup);
            tmpBtnx.Width = 150;
            tmpBtnx.Height = 40;
            tmpBtnx.Tag = 0;

            tmpBtnx.FlatStyle = FlatStyle.Standard;
            
            tmpBtnx.Image = Projector.Properties.Resources.layer_group_add;
            tmpBtnx.ImageAlign = ContentAlignment.MiddleLeft;
            tmpBtnx.TextAlign = ContentAlignment.MiddleRight;
            flowLayoutControllPanel.Controls.Add(tmpBtnx);

            int select = 1;

            foreach (DictionaryEntry de in settings)
            {
                chooseGroup.Items.Add(de.Key.ToString());
                Button tmpBtn = new Button();
                tmpBtn.Text = de.Key.ToString();
                tmpBtn.Click += new System.EventHandler(btnSelectGroup);
                tmpBtn.Width = 150;
                tmpBtn.Height = 40;
                tmpBtn.FlatStyle = FlatStyle.Standard;
                tmpBtn.Tag = select;
                tmpBtn.Image = Projector.Properties.Resources.layer_group_add;
                tmpBtn.ImageAlign = ContentAlignment.MiddleLeft;
                tmpBtn.TextAlign = ContentAlignment.MiddleRight;
                
                flowLayoutControllPanel.Controls.Add(tmpBtn);
                select++;
            }
            chooseGroup.SelectedIndex = 0;
        }

        private void btnSelectGroup(object sender, System.EventArgs e)
        {
            Button tmpBtn = (Button)sender;
            chooseGroup.SelectedIndex = (int)tmpBtn.Tag;

        }

        private void setMainColors(Color fore, Color back)
        {
            this.BackColor = back;
            this.ForeColor = fore;

            toolStrip1.BackColor = back;
            toolStrip1.ForeColor = fore;

            toolStripContainer1.BackColor = back;
            toolStripContainer1.ForeColor = fore;

            toolStripContainer1.TopToolStripPanel.BackColor = back;


            menuStrip1.BackColor = back;
            menuStrip1.ForeColor = fore;

            statusStrip1.BackColor = back;
            statusStrip1.ForeColor = fore;

            chooseGroup.BackColor = back;
            chooseGroup.ForeColor = fore;
        }

        private void drawNewStyle()
        {
            Hashtable grp = this.getGroups();
            XmlSetup tmpSetup = new XmlSetup();
            tmpSetup.setFileName("Projector_profiles.xml");
            //tmpSetup.setFileName("Projector_global.xml");
            tmpSetup.loadXml();
            
            flowLayout.BackColor = Color.FromArgb(60, 60, 60);
            flowLayout.Padding = new Padding(10);

            this.setMainColors(Color.White, Color.FromArgb(80, 80, 80));

            
            for (Int64 i = 0; i < tmpSetup.count; i++)
            {
                string keyname = "profil_" + (i + 1);
                string proName = tmpSetup.getValue(keyname);

                if (proName != null)
                {
                    if (grp.Count == 0 || grp.ContainsKey(proName))
                    {
                        profil.changeProfil(proName);
                        profilSelector.DropDownItems.Add(proName, Projector.Properties.Resources.folder_closed_16, ProfilSelectExitClick);

                        ProfilButton tmpPBtn = new ProfilButton(this);
                        tmpPBtn.setName(proName);
                        tmpPBtn.setDescription(profil.getProperty("db_host"));
                        string colortext = profil.getProperty("set_bgcolor");
                        if (colortext != null && colortext.Length > 0)
                        {

                            tmpPBtn.setColor(Color.FromArgb(int.Parse(colortext)));
                            tmpPBtn.Margin = new Padding(15);
                            

                        }
                        tmpPBtn.StartBtn.Click += new System.EventHandler(styleBtnClick);
                        flowLayout.Controls.Add(tmpPBtn);
                    }
                }
            }
            Button tmpBtn = new Button();
            tmpBtn.FlatStyle = FlatStyle.Flat;
            tmpBtn.ForeColor = Color.LightGray;
            tmpBtn.BackColor = Color.FromArgb(50,50,50 );
            updateGroupButonStyle(tmpBtn);
        }

        private Hashtable getGroups()
        {
            Hashtable groups = new Hashtable();
            XmlSetup pSetup = new XmlSetup();
            pSetup.setFileName("profileGroups.xml");
            pSetup.loadXml();

            Hashtable settings = pSetup.getHashMap();

            if (chooseGroup.Text != "[ALL]")
            {
                if (pSetup.getHashMap().Contains(chooseGroup.Text))
                {
                    string[] currGrp = pSetup.getHashMap()[chooseGroup.Text].ToString().Split('|');
                    for (int z = 0; z < currGrp.Length; z++)
                    {
                        groups.Add(currGrp[z], currGrp[z]);
                    }

                }
            }
            return groups;
        }

        private void updateProfilSelector()
        {
            XmlSetup tmpSetup = new XmlSetup();
            tmpSetup.setFileName("Projector_profiles.xml");
            //tmpSetup.setFileName("Projector_global.xml");
            tmpSetup.loadXml();
            profilSelector.DropDownItems.Clear();

            /*
            Panel panel1 = new Panel();
            panel1.Location = new Point(56, 72);
            panel1.Size = new Size(264, 152);
            
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(panel1);
            */
           


            flowLayout.Controls.Clear();
            

            if (ProjectorForm.SCRIPT_BUTTON_MODE == buttonStyle)
            {
                toolStrip1.Visible = false;
                mainSlitter.Panel1Collapsed = true;
                this.setMainColors(Color.Black, Color.DarkGray);
                flowLayout.BackColor = Color.LightGray;


                this.ScriptAutoLoader.showAll(!this.displayNamedScripstOnly);
                if (this.ScriptAutoLoader.setPath( this.mainScriptFolder ))
                {
                    List<RefScrAutoScrContainer> scripts = this.ScriptAutoLoader.getAllScripts();

                    foreach (RefScrAutoScrContainer script in scripts)
                    {
                        ScriptStartButton tmpStartBtn = new ScriptStartButton(this);
                        tmpStartBtn.setScript(script);
                        flowLayout.Controls.Add(tmpStartBtn);                        
                    }

                }

                return;
            }
            mainSlitter.Panel1Collapsed = !groupButtonsToolStripMenuItem.Checked;


            if (ProjectorForm.STYLE_BUTTON_MODE == buttonStyle)
            {
                this.drawNewStyle();
                return;
            }

         
            toolStrip1.Visible = true;
            flowLayout.BackColor = SystemColors.Control;
            this.setMainColors(SystemColors.ControlText, SystemColors.Control);

            Button tmpBtnTemplate = new Button();
            updateGroupButonStyle(tmpBtnTemplate);

            Hashtable assignGrp = new Hashtable();
            Hashtable currentView = new Hashtable();
            if (showGroups || chooseGroup.Text != "[ALL]")
            {

                XmlSetup pSetup = new XmlSetup();
                pSetup.setFileName("profileGroups.xml");
                pSetup.loadXml();

                Hashtable settings = pSetup.getHashMap();

                if (chooseGroup.Text != "[ALL]")
                {
                    if (pSetup.getHashMap().Contains(chooseGroup.Text))
                    {
                        string[] currGrp = pSetup.getHashMap()[chooseGroup.Text].ToString().Split('|');
                        for (int z = 0; z < currGrp.Length; z++)
                        {
                            currentView.Add(currGrp[z], currGrp[z]);
                        }
                            
                    }
                }

                if (showGroups)
                {
                    foreach (DictionaryEntry de in settings)
                    {
                        //de.Key.ToString();                
                        //de.Value.ToString();
                        //groupSelectBox.Items.Add(de.Key.ToString());

                        GroupBox grpBox = new GroupBox();
                        grpBox.Name = de.Key.ToString();
                        grpBox.Text = de.Key.ToString();
                        grpBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                        grpBox.AutoSize = true;


                        FlowLayoutPanel flowInside = new FlowLayoutPanel();
                        flowInside.Dock = DockStyle.Fill;
                        flowInside.AutoSize = true;
                        flowInside.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                        grpBox.Controls.Add(flowInside);

                        flowLayout.Controls.Add(grpBox);

                        string[] grpProfiles = de.Value.ToString().Split('|');

                        for (int z = 0; z < grpProfiles.Length; z++)
                        {
                            string prName = grpProfiles[z];
                            if (assignGrp != null && !assignGrp.Contains(prName)) assignGrp.Add(prName, flowInside);
                        }
                    }


                }


            }

            for (Int64 i = 0; i < tmpSetup.count; i++)
            {
                string keyname = "profil_" + (i + 1);
                string proName = tmpSetup.getValue(keyname);

                if (proName != null)
                {
                    profil.changeProfil(proName);
                    profilSelector.DropDownItems.Add(proName, Projector.Properties.Resources.folder_closed_16, ProfilSelectExitClick);

                    Button tmpBtn = new Button();

                    string colortext = profil.getProperty("set_bgcolor");
                    if (colortext != null && colortext.Length > 0)
                    {

                        tmpBtn.BackColor = Color.FromArgb(int.Parse(colortext));
                    }


                    tmpBtn.Text = proName;
                    tmpBtn.Click += new System.EventHandler(dynBtnClick);
                    tmpBtn.Width = 150;
                    tmpBtn.Height = 80;

                    if (buttonStyle == 1)
                    {
                        tmpBtn.Height = 25;
                        //tmpBtn.FlatStyle = FlatStyle.System;
                    }

                    if (buttonStyle == 0)
                    {
                        tmpBtn.Image = Projector.Properties.Resources.db;
                        tmpBtn.ImageAlign = ContentAlignment.TopCenter;
                        tmpBtn.TextAlign = ContentAlignment.BottomCenter;
                    }


                    if (buttonStyle == 2)
                    {
                        tmpBtn.Image = Projector.Properties.Resources.db;
                        tmpBtn.ImageAlign = ContentAlignment.TopLeft;
                        tmpBtn.TextAlign = ContentAlignment.TopRight;
                        tmpBtn.Height = 55;

                    }

                    if (chooseGroup.Text != "[ALL]" && chooseGroup.Text != "")
                    {
                        if (currentView.ContainsValue(proName)) ProfilBtn.Add(tmpBtn);
                    }
                    else
                    {
                        ProfilBtn.Add(tmpBtn);
                    }


                    if (showGroups)
                    {
                        if (assignGrp.Contains(proName))
                        {
                            FlowLayoutPanel ctrl = (FlowLayoutPanel)assignGrp[proName];
                            ctrl.Controls.Add(tmpBtn);
                        }
                    }
                    else
                    {
                        if (chooseGroup.Text != "[ALL]" && chooseGroup.Text != "")
                        {
                            if (currentView.ContainsValue(proName)) flowLayout.Controls.Add(tmpBtn);
                        }
                        else
                        {
                            flowLayout.Controls.Add(tmpBtn);
                        }
                        
                    }

                }
                else
                {
                    //missing profile in flow
                    MessageBox.Show("Missing profile " + keyname + ".", "Config error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (buttonStyle == 0)
            {
                Button syncBtn = new Button();
                syncBtn.Text = "Sync Database";
                syncBtn.Click += new System.EventHandler(syncDbBtnClick);
                syncBtn.Width = 150;
                syncBtn.Height = 60;
                syncBtn.Image = Projector.Properties.Resources.arrow_back_32;


                syncBtn.ImageAlign = ContentAlignment.TopCenter;
                syncBtn.TextAlign = ContentAlignment.BottomCenter;

                flowLayout.Controls.Add(syncBtn);
            }

            showProfilLabel.Text = profil.getName();


        }

        private void updateButtonsState()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < ProfilBtn.Count; i++)
            {
                string showAtProperty = ProfilBtn[i].Text;
                profil.changeProfil(showAtProperty);

               

                statusResult result = new statusResult();
                result = checkConnection(profil);

                names.Add(showAtProperty);

                ProfilBtn[i].Image = Projector.Properties.Resources.database_check;
                ProfilBtn[i].Update();
                if (result.status)
                {
                    ProfilBtn[i].Image = Projector.Properties.Resources.dbplus;


                    MysqlHandler TestConnect = new MysqlHandler(profil);
                    TestConnect.connect();
                    MySql.Data.MySqlClient.MySqlDataReader data = TestConnect.sql_select("SELECT version()");
                    data.Read();
                    ProfilBtn[i].Text += " " + data.GetString(0);
                    TestConnect.disConnect();

                }
                else
                {
                    ProfilBtn[i].Image = Projector.Properties.Resources.database4_2;
                    ProfilBtn[i].ForeColor = Color.Red;
                }

                ProfilBtn[i].Update();

            }
            MessageBox.Show("Close this message to return to the Original Display State. Now you can see the mysql Server Version");
            for (int i = 0; i < ProfilBtn.Count; i++)
            {
                ProfilBtn[i].Text = names[i];
            }

        }


        private void updateSelectedProfil()
        {
            showProfilLabel.Text = profil.getName();
            ProfilInfo.Text = profil.getName() + " [" + profil.getProperty("db_username") + '@' + profil.getProperty("db_schema") + "]";
            this.Text = "Profil: " + profil.getName();
            mDIToolStripMenuItem.Text = "Launch " + profil.getName() + "...";


            string showAtProperty = profil.getName();
            profil.changeProfil(showAtProperty);

            

            statusResult result = new statusResult();
            result = checkConnection(profil);


            if (result.status)
            {
                connectionState.ForeColor = Color.DarkGreen;

                MysqlHandler TestConnect = new MysqlHandler(profil);
                TestConnect.connect();
                MySql.Data.MySqlClient.MySqlDataReader data = TestConnect.sql_select("SELECT version()");
                data.Read();
                connectionState.Text = "Connection Test OK ... Version " + data.GetString(0);
                TestConnect.disConnect();

            }
            else
            {
                connectionState.ForeColor = Color.Red;
                connectionState.Text = "NO CONNECTION";
            }

        }

        private void syncDbBtnClick(object sender, System.EventArgs e)
        {
            Button sendBtn = (Button)sender;
            toolStripButton2_Click(sender, e);
        }

        public void setCurrentProfile(string name)
        {
            profil.changeProfil(name);
            updateSelectedProfil();
            ProfilInfo.Text = name + " [" + profil.getProperty("db_username") + '@' + profil.getProperty("db_schema") + "]";
        }

        private void startProfile(string name, Object sender, System.EventArgs e)
        {
            profil.changeProfil(name);

            statusResult result = new statusResult();
            result = checkConnection(profil);

            if (result.status)
            {

                profil.changeProfil(name);
                updateSelectedProfil();
                ProfilInfo.Text = name + " [" + profil.getProperty("db_username") + '@' + profil.getProperty("db_schema") + "]";
                mDIToolStripMenuItem_Click(sender, e);
            }
            else
            {
                MessageBox.Show(this, result.message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dynBtnClick(object sender, System.EventArgs e)
        {
            Button sendBtn = (Button)sender;

            string showAtProperty = sendBtn.Text;

            if (showAtProperty != null)
            {
                this.startProfile(showAtProperty, sender, e);
            }

        }

        private void styleBtnClick(object sender, System.EventArgs e)
        {
            Button sendBtn = (Button)sender;

            string showAtProperty = sendBtn.Name;

            if (showAtProperty != null)
            {
                this.startProfile(showAtProperty, sender, e);
            }

        }

        private void ProfilSelectExitClick(object who, EventArgs e)
        {
            System.Windows.Forms.ToolStripDropDownItem check = (System.Windows.Forms.ToolStripDropDownItem)who;
            string showAtProperty = check.Text;

            if (showAtProperty != null)
            {
                profil.changeProfil(showAtProperty);
                updateSelectedProfil();

                //mDIToolStripMenuItem_Click(who, e);
            }

        }

        public void callSetup()
        {
            setupToolStripMenuItem_Click(null, null);
        }

        private void setupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupForm sForm = new setupForm();

            sForm.Text = profil.getName();

            sForm.host.Text = profil.getProperty("db_host");
            sForm.username.Text = profil.getProperty("db_username");
            sForm.password.Text = profil.getProperty("db_password");
            sForm.schema.Text = profil.getProperty("db_schema");
            sForm.diffviewsetup.Text = profil.getProperty("diff_command");
            sForm.diffviewparam.Text = profil.getProperty("diff_param");
            sForm.constrain_setup.Checked = (profil.getProperty("foreign_key_check") == "1");

            string colortext = profil.getProperty("set_bgcolor");
            if (colortext != null && colortext.Length > 0)
            {
                sForm.colorText.Text = colortext;
                sForm.colorText.BackColor = Color.FromArgb(int.Parse(colortext));
            }

            if (sForm.ShowDialog() == DialogResult.OK)
            {
                profil.setProperty("db_host", sForm.host.Text);
                profil.setProperty("db_username", sForm.username.Text);
                profil.setProperty("db_password", sForm.password.Text);
                profil.setProperty("db_schema", sForm.schema.Text);
                profil.setProperty("set_bgcolor", sForm.colorText.Text);
                profil.setProperty("diff_command", sForm.diffviewsetup.Text);
                profil.setProperty("diff_param", sForm.diffviewparam.Text);
                if (sForm.constrain_setup.Checked)
                    profil.setProperty("foreign_key_check", "1");
                else
                    profil.setProperty("foreign_key_check", "0");
                profil.saveSetup();
                updateProfilSelector();
            }
        }


        private statusResult checkConnection(Profil testPro)
        {
            statusResult result = new statusResult();
            MysqlHandler TestConnect = new MysqlHandler(testPro);

            try
            {
                TestConnect.connect();
            }
            catch (Exception)
            {

                //throw;
            }

            if (TestConnect.lastSqlErrorMessage.Length > 0)
            {
                result.message = TestConnect.lastSqlErrorMessage;
                result.status = false;
                result.StatusKey = 0;
                return result;
            }
            else
            {
                TestConnect.disConnect();
                result.message = Projector.Properties.Resources.MysqlConnectionTestSucces; ;
                result.status = true;
                result.StatusKey = 1;
                return result;


            }

        }


        private void databaseWatchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void startWatcher_Click(object sender, EventArgs e)
        {
            databaseWatchToolStripMenuItem_Click(sender, e);
        }

        private void sensorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MysqlSensor sensor = new MysqlSensor();
            if (profil.getProperty("set_bgcolor") != null && profil.getProperty("set_bgcolor").Length > 2)
            {
                sensor.BackColor = Color.FromArgb(int.Parse(profil.getProperty("set_bgcolor")));
            }
            //sensor.sensorProfil = profil;
            sensor.sensorProfil.getValuesFromProfil(profil);
            sensor.getStartupData();
            sensor.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            sensorToolStripMenuItem_Click(sender, e);

        }

        private void querysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            queryBrowser qb = new queryBrowser();
            if (profil.getProperty("set_bgcolor") != null && profil.getProperty("set_bgcolor").Length > 2)
            {
                qb.BackColor = Color.FromArgb(int.Parse(profil.getProperty("set_bgcolor")));
            }
            //watcher.profil = profil;
            //qb.sensorProfil = profil;
            qb.sensorProfil.getValuesFromProfil(profil);
            qb.listTables();
            qb.Show();
        }

        private void mDIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MdiForm mdif = new MdiForm();
            //mdif.currentProfil = profil;

            

            if (mdif.currentProfil == null) mdif.currentProfil = new Profil("midi");

           

            mdif.currentProfil.getValuesFromProfil(profil);
            mdif.Text = " [" + profil.getProperty("db_username") + '@' + profil.getProperty("db_host") + "/" + profil.getProperty("db_schema") + "]";

            if (mdif.currentProfil.getProperty("set_bgcolor") != null && mdif.currentProfil.getProperty("set_bgcolor").Length > 2)
            {
                StyleFormProps profilStyle = new StyleFormProps();
                profilStyle.MainColor = Color.FromArgb(int.Parse(mdif.currentProfil.getProperty("set_bgcolor")));
                profilStyle.composeFlatLight();
                mdif.setStyle(profilStyle);

            }

            mdif.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            copyDb cform = new copyDb();
            XmlSetup tmpSetup = new XmlSetup();
            tmpSetup.setFileName("Projector_profiles.xml");
            tmpSetup.loadXml();
            profilSelector.DropDownItems.Clear();

            cform.sourceSelect.Items.Clear();
            cform.targetSelect.Items.Clear();

            for (Int64 i = 0; i < tmpSetup.count; i++)
            {
                string keyname = "profil_" + (i + 1);
                string proName = tmpSetup.getValue(keyname);

                if (proName != null)
                {
                    // profilSelector.DropDownItems.Add(proName, Projector.Properties.Resources.folder_closed_16, ProfilSelectExitClick);

                    cform.sourceSelect.Items.Add(proName);
                    cform.targetSelect.Items.Add(proName);

                }
            }



            cform.Show();
        }

        private void showProfilLabel_Click(object sender, EventArgs e)
        {
            updateProfilSelector();
        }

        private void exportCurrentProfilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (exportProfileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                profil.exportXml(exportProfileDlg.FileName);
            }
        }



        private void ProjectorForm_Shown(object sender, EventArgs e)
        {
            initApp();
            updateProfilSelector();
        }

        private void groupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProfilGroup p1 = new ProfilGroup(this.profilGroups);

            List<String> profiles = this.Setup.getListWidthDefault(PConfig.KEY_PROFILS, new List<string>());

            if (profiles != null)
            {
                foreach (string profilName in profiles)
                {
                    p1.GroupedDatabases.Items.Add(profilName);
                }
            }

            /*
            XmlSetup tmpSetup = new XmlSetup();
            tmpSetup.setFileName("Projector_profiles.xml");

            tmpSetup.loadXml();

            for (Int64 i = 0; i < tmpSetup.count; i++)
            {
                string keyname = "profil_" + (i + 1);
                string proName = tmpSetup.getValue(keyname);

                if (proName != null)
                {


                    ListViewItem addItem = new ListViewItem();
                    addItem.Text = proName;
                    p1.GroupedDatabases.Items.Add(addItem);
                }
            }
            XmlSetup pSetup = new XmlSetup();
            pSetup.setFileName("profileGroups.xml");
            pSetup.loadXml();
            */
            // 


            if (p1.ShowDialog() == System.Windows.Forms.DialogResult.OK && p1.groupName.Text.Length > 1)
            {

                /*
                p1.groupName.Text = p1.groupName.Text.Replace(" ", "_");
                p1.groupName.Text = p1.groupName.Text.Replace("-", "_");
                p1.groupName.Text = p1.groupName.Text.Replace("/", "_");
                p1.groupName.Text = p1.groupName.Text.Replace(@"\", "_");
                p1.groupName.Text = p1.groupName.Text.Replace(":", "_");

                string profilNames = "";
                string split = "";
                for (int b = 0; b < p1.GroupedDatabases.Items.Count; b++)
                {



                    if (p1.GroupedDatabases.Items[b].Checked)
                    {

                        profilNames += split + p1.GroupedDatabases.Items[b].Text;
                        split = "|";
                    }

                }

                pSetup.addSetting(p1.groupName.Text, profilNames);

                pSetup.saveXml();
                 */
                drawGroupButtons();
                updateProfilSelector();

            }

        }

        private void groupQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GroupQuery gq = new GroupQuery();

            gq.Show();

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            toolStripButton3.Enabled = false;
            updateButtonsState();
            toolStripButton3.Enabled = true;
        }

        private void backupProfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveProject.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ConfigProfil exporter = new ConfigProfil();
                exporter.ExportToXml(saveProject.FileName);

            }
        }

        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openProjectDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ConfigProfil exporter = new ConfigProfil();
                exporter.importFromXml(openProjectDlg.FileName);
                initApp();
                updateProfilSelector();
            }
        }

        private void importPartsOfProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openProjectDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ConfigProfil exporter = new ConfigProfil();
                List<string> profilListing = exporter.getImportableProfiles(openProjectDlg.FileName);

                if (profilListing != null && profilListing.Count > 0)
                {
                    ImportSettings setImport = new ImportSettings();

                    for (int i = 0; i < profilListing.Count; i++)
                    {
                        setImport.ImportListView.Items.Add(profilListing[i]);
                    }

                    if (setImport.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {

                    }

                }

            }
        }

        private void groupedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showGroups = groupedToolStripMenuItem.Checked;
            updateProfilSelector();
        }

        private void switchButtonModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonStyle++;
            if (buttonStyle > maxStyle) buttonStyle = 0;
            updateProfilSelector();
        }

        private void chooseGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateProfilSelector();
        }

        private void flowLayout_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupButtonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainSlitter.Panel1Collapsed = !groupButtonsToolStripMenuItem.Checked;
        }

        private void scriptRunButton_Click(object sender, EventArgs e)
        {
            if (this.scriptFile == null)
            {
                if (this.openScript.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.scriptFile = this.openScript.FileName;
                }
            }

            if (this.scriptFile != null)
            {
                string execScript = System.IO.File.ReadAllText(this.scriptFile);
                ReflectionScript script = new ReflectionScript();
                script.setCode(execScript);
                RefScriptExecute executer = new RefScriptExecute(script,this);
                executer.run();
            }
        }

        private void editScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptWriter newScript = new ScriptWriter(this);
            newScript.Show();
        }

        private void mainSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainSetup ms = new MainSetup();
            ms.ScriptPath.setPath(this.mainScriptFolder); 
            ms.ScriptPath.setInfo("Default Script Folder");
            ms.displayNamedScript.Checked = this.displayNamedScripstOnly;
            if (ms.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.mainScriptFolder = ms.ScriptPath.getPath();
                this.displayNamedScripstOnly = ms.displayNamedScript.Checked;

                updateProfilSelector();
            }
        }

        private void updateStyleButtons()
        {
            style_0.Checked = false;
            style_1.Checked = false;
            style_2.Checked = false;
            style_3.Checked = false;
            switch (this.buttonStyle)
            {
                case 0:
                    style_0.Checked = true;
                    break;
                case 1:
                    style_1.Checked = true;
                    break;
                case 2:
                    style_2.Checked = true;
                    break;
                case 3:
                    style_3.Checked = true;
                    break;
            }
            this.updateProfilSelector();
        }

        private void style_0_Click(object sender, EventArgs e)
        {
            this.buttonStyle = 0;
            this.updateStyleButtons();
        }

        private void style_1_Click(object sender, EventArgs e)
        {
            this.buttonStyle = 1;
            this.updateStyleButtons();
        }

        private void style_2_Click(object sender, EventArgs e)
        {
            this.buttonStyle = 2;
            this.updateStyleButtons();
        }

        private void style_3_Click(object sender, EventArgs e)
        {
            this.buttonStyle = 3;
            this.updateStyleButtons();
        }




    }
}
