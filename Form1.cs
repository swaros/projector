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

        private Profil profil = new Profil();
        List<Button> ProfilBtn = new List<Button>();

        private Boolean groupUnassigned = false;
        private bool showGroups = false;

        private int buttonStyle = 0;
        private int maxStyle = 3;

        private string scriptFile;

        private Boolean displayNamedScripstOnly = false;
        private string mainScriptFolder;

        private List<string> profilGroups = new List<string>();

        private Hashtable SelectedProfiles = new Hashtable();

        RefScrAutoStart ScriptAutoLoader = new RefScrAutoStart();

        private static string mainFormStyle = StyleFormProps.STYLE_DEFAULT;
        private string mainMDIStyle = StyleFormProps.STYLE_DEFAULT;

        private PConfig Setup = new PConfig();

        public ProjectorForm()
        {
            InitializeComponent();
            this.mainScriptFolder = Application.StartupPath;
            Setup.loadRuntimeConfig();
            updateProfilSelector();


        }

        /// <summary>
        /// add an selected Proflibutton to to index 
        /// so this forms knows whats buttons are selected
        /// </summary>
        /// <param name="btn">The button the is selected</param>
        public void setSelected(ProfilButton btn)
        {
            if (!this.SelectedProfiles.ContainsKey(btn.profilName))
            {
                this.SelectedProfiles.Add(btn.profilName, btn);
            }
        }

        /// <summary>
        /// removes buttons form the index of selected profilbuttons
        /// </summary>
        /// <param name="btn">Profilbutton that are unselected</param>
        public void setUnSelected(ProfilButton btn)
        {
            if (this.SelectedProfiles.ContainsKey(btn.profilName))
            {
                this.SelectedProfiles.Remove(btn.profilName);
            }
        }

        /// <summary>
        /// renames a group.
        /// </summary>
        /// <param name="old">old name of the Group (must exists)</param>
        /// <param name="newstr">new name of the Group (must NOT exists)</param>
        /// <returns>retunrs true if remaning was suceesfully</returns>
        public Boolean renameGroup(string old, string newstr)
        {
            GroupProfilWorker worker = new GroupProfilWorker(this.Setup);
            return worker.renameGroup(old, newstr);

        }

        /// <summary>
        /// selects an Profil, so it will be the current one
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// reading or updating default Settings for the Client.
        /// 
        /// </summary>
        /// <param name="forSaving">if true the values are stored, on false some dafault values be used </param>
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

                this.Setup.setValue("client.style.mainform", ProjectorForm.mainFormStyle);
                this.Setup.setValue("client.style.mainmdi", this.mainMDIStyle);

                this.Setup.setList("client.groups.names", this.profilGroups);
                this.Setup.setValue("client.maximized", this.WindowState == FormWindowState.Maximized);
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

                this.mainMDIStyle = this.Setup.getSettingWidthDefault("client.style.mainmdi", this.mainMDIStyle);
                ProjectorForm.mainFormStyle = this.Setup.getSettingWidthDefault("client.style.mainform", ProjectorForm.mainFormStyle);

                this.profilGroups = this.Setup.getListWidthDefault("client.groups.names", this.profilGroups);

                bool maximized = this.Setup.getBooleanSettingWidthDefault("client.maximized", this.WindowState == FormWindowState.Maximized);

                if (maximized && this.WindowState != FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }

        }

        /// <summary>
        /// initialize the Whole app. Load configs
        /// and updates the view
        /// </summary>
        private void initApp()
        {

            this.copySettings(false);
            updateStyleButtons();
            drawGroupButtons();

        }

        /// <summary>
        /// Default Form Close Handling.
        /// Saves the current Configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.copySettings(true);
            this.Setup.saveRuntimeConfig();
        }

        /// <summary>
        /// Updates all Buttons in the left Group panles with 
        /// the current Style, depending on the current ButtonStyle
        /// </summary>
        /// <param name="template"></param>
        private void updateGroupButonStyle(Button template)
        {
            for (int i = 0; i < flowLayoutControllPanel.Controls.Count; i++)
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

        public static string getMainFormStyle()
        {
            return ProjectorForm.mainFormStyle;
        }

        /// <summary>
        /// Render the groupbuttons on the left GroupButton Panel.
        /// </summary>
        private void drawGroupButtons()
        {

            flowLayoutControllPanel.Controls.Clear();
            List<String> groups = this.Setup.getListWidthDefault(PConfig.KEY_GROUPS_NAMES, new List<string>());

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

            foreach (string grpLabel in groups)
            {
                chooseGroup.Items.Add(grpLabel);
                Button tmpBtn = new Button();
                tmpBtn.Text = grpLabel;
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

        /// <summary>
        /// Click Trigger for generated Group Buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectGroup(object sender, System.EventArgs e)
        {
            Button tmpBtn = (Button)sender;
            chooseGroup.SelectedIndex = (int)tmpBtn.Tag;

        }

        /// <summary>
        /// For styling the whole Form.
        /// copy the color values to all childs
        /// </summary>
        /// <param name="fore"></param>
        /// <param name="back"></param>
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

        /// <summary>
        /// Join a Profil to an existing Group.
        /// </summary>
        /// <param name="groupname">Name of the Group</param>
        /// <param name="profil">Name of the Profil</param>
        public void joinToGroup(string groupname, string profil)
        {
            GroupProfilWorker grp = new GroupProfilWorker(this.Setup);
            if (grp.joinToExistingGoup(groupname, profil))
            {
                //drawNewStyleButtons(groupname, false);
                if (this.flowLayout.Controls.ContainsKey(profil))
                {
                    this.flowLayout.Controls.RemoveByKey(profil);
                }
            }
        }

        /// <summary>
        /// Join two Profils to an new Group
        /// </summary>
        /// <param name="nameOne">Name of the first Profil</param>
        /// <param name="nameTwo">Name of the second Profil</param>
        public void joinToNewGoup(string nameOne, string nameTwo)
        {
            GroupProfilWorker grp = new GroupProfilWorker(this.Setup);
            if (grp.joinToNewGoup(nameOne, nameTwo))
            {
                drawNewStyleButtons();
            }

        }

        /// <summary>
        /// get an existing ProfilGroupBox in the
        /// current ControlContainer by name
        /// </summary>
        /// <param name="name">Name of the ProfilButton (equal to the Name of the Profil)</param>
        /// <returns></returns>
        private ProfilGroupBox getStoredGrp(string name)
        {
            if (this.flowLayout.Controls.ContainsKey(name))
            {
                if (this.flowLayout.Controls[name] is ProfilGroupBox)
                {
                    return (ProfilGroupBox)this.flowLayout.Controls[name];
                }
            }
            return null;
        }

        /// <summary>
        /// get an existing ProfilButton in the
        /// current ControlContainer by name
        /// </summary>
        /// <param name="name">Name of the ProfilButton (equal to the Name of the Profil)</param>
        /// <returns></returns>
        private ProfilButton getStoredPButton(string name)
        {
            foreach (Control chk in this.flowLayout.Controls)
            {
                if (chk is ProfilButton)
                {
                    
                    ProfilButton pBn =  (ProfilButton)chk;
                    if (pBn.getName() == name)
                    {
                        return pBn;
                    }

                }
            }
            return null;
        }


        /// <summary>
        /// Drawing of the ProfilButtons. Default with redrwaing option and 
        /// wihout limittaion of groups
        /// </summary>
        private void drawNewStyleButtons()
        {
            this.drawNewStyleButtons(null, true);
        }



        /// <summary>
        /// Draws ProfilButtobs.
        /// </summary>
        /// <param name="show">Name of the group that should be shown only, or null for all Profiles</param>
        /// <param name="redraw">Full update (deletes all and redcreate it)</param>
        private void drawNewStyleButtons(string show, Boolean redraw)
        {
            if (redraw)
            {
                this.flowLayout.Controls.Clear();
            }
            List<string> groups = this.Setup.getListWidthDefault(PConfig.KEY_GROUPS_NAMES, new List<string>());
            List<string> assigned = new List<string>();
            foreach (string grpName in groups)
            {
                ProfilGroupBox grpBtnSt = this.getStoredGrp(grpName);
                ProfilGroupBox grpBtn;
                if (redraw)
                {
                    grpBtn = new ProfilGroupBox();
                    grpBtn.Name = grpName;
                    grpBtn.setText(grpName);
                    grpBtn.setParent(this);
                }
                else
                {
                    grpBtn = this.getStoredGrp(grpName);
                }
                if (grpBtn != null)
                {
                    for (int i = 1; i < grpBtn.grpFlow.Controls.Count; i++)
                    {
                        grpBtn.grpFlow.Controls.RemoveAt(i);
                    }
                    List<string> childs = this.Setup.getListWidthDefault(PConfig.KEY_GROUPS_MEMBERS + "." + grpName, new List<string>());

                    foreach (string childName in childs)
                    {
                        assigned.Add(childName);
                        profil.changeProfil(childName);


                        ProfilButton tmpPBtn = new ProfilButton(this);
                        tmpPBtn.Name = childName;
                        tmpPBtn.setName(childName);
                        tmpPBtn.setDescription(profil.getProperty("db_host"));
                        tmpPBtn.assignedGroup = grpName;
                        string colortext = profil.getProperty("set_bgcolor");
                        if (colortext != null && colortext.Length > 0)
                        {

                            tmpPBtn.setColor(Color.FromArgb(int.Parse(colortext)));
                            tmpPBtn.Margin = new Padding(15);


                        }
                        tmpPBtn.StartBtn.Click += new System.EventHandler(styleBtnClick);
                        grpBtn.grpFlow.Controls.Add(tmpPBtn);
                    }
                    if (show != null && show == grpName)
                    {
                        grpBtn.expand();
                    }
                    if (redraw)
                        flowLayout.Controls.Add(grpBtn);



                }
            }
            if (redraw)
            {
                List<string> all = this.Setup.getListWidthDefault(PConfig.KEY_PROFILS, new List<string>());
                ProfilGroupBox grpBtnUnassigned = new ProfilGroupBox();
                grpBtnUnassigned.setText("Unassigend");
                grpBtnUnassigned.setParent(this);

                Boolean unusedExists = false;
                foreach (string unused in all)
                {

                    if (!assigned.Contains(unused))
                    {

                        profil.changeProfil(unused);


                        ProfilButton tmpPBtn = new ProfilButton(this);
                        tmpPBtn.setName(unused);
                        tmpPBtn.setDescription(profil.getProperty("db_host"));
                        string colortext = profil.getProperty("set_bgcolor");
                        if (colortext != null && colortext.Length > 0)
                        {

                            tmpPBtn.setColor(Color.FromArgb(int.Parse(colortext)));
                            tmpPBtn.Margin = new Padding(15);



                        }
                        tmpPBtn.Name = unused;
                        tmpPBtn.StartBtn.Click += new System.EventHandler(styleBtnClick);
                        //flowLayout.Controls.Add(tmpPBtn);
                        if (groupUnassigned)
                        {
                            grpBtnUnassigned.grpFlow.Controls.Add(tmpPBtn);
                            unusedExists = true;
                        }
                        else
                        {
                            flowLayout.Controls.Add(tmpPBtn);
                        }

                    }

                }
                if (unusedExists)
                    flowLayout.Controls.Add(grpBtnUnassigned);
            }
        }

        /// <summary>
        /// drawing method for Buttonstyle 3
        /// </summary>
        private void drawNewStyle()
        {

            List<string> grp = this.getGroupMembers(chooseGroup.Text);

            flowLayout.BackColor = Color.FromArgb(60, 60, 60);
            flowLayout.Padding = new Padding(10);

            this.setMainColors(Color.White, Color.FromArgb(80, 80, 80));
            if (mainSlitter.Panel1Collapsed)
            {
                drawNewStyleButtons();
                return;
            }


            Profil itProf = new Profil();
            foreach (String proName in grp)
            {

                if (proName != null)
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
            Button tmpBtn = new Button();
            tmpBtn.FlatStyle = FlatStyle.Flat;
            tmpBtn.ForeColor = Color.LightGray;
            tmpBtn.BackColor = Color.FromArgb(50, 50, 50);
            updateGroupButonStyle(tmpBtn);
        }

        /// <summary>
        /// resets styling to the default windows style
        /// </summary>
        private void resetDefaultWindowsStyle()
        {
            toolStrip1.Visible = true;
            flowLayout.BackColor = SystemColors.Control;
            this.setMainColors(SystemColors.ControlText, SystemColors.Control);

            Button tmpBtnTemplate = new Button();
            updateGroupButonStyle(tmpBtnTemplate);
        }


        private void DrawQuickButtonStyle()
        {
            this.resetDefaultWindowsStyle();
            flowLayout.Controls.Clear();
            List<string> grp = this.getGroupMembers(chooseGroup.Text);

            foreach (string proName in grp)
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


                tmpBtn.Image = Projector.Properties.Resources.db;
                tmpBtn.ImageAlign = ContentAlignment.TopCenter;
                tmpBtn.TextAlign = ContentAlignment.BottomCenter;

                flowLayout.Controls.Add(tmpBtn);

            }

            Button syncBtn = new Button();
            syncBtn.Text = "Sync Database";
            syncBtn.Click += new System.EventHandler(syncDbBtnClick);
            syncBtn.Width = 150;
            syncBtn.Height = 60;
            syncBtn.Image = Projector.Properties.Resources.arrow_back_32;


            syncBtn.ImageAlign = ContentAlignment.TopCenter;
            syncBtn.TextAlign = ContentAlignment.BottomCenter;

            flowLayout.Controls.Add(syncBtn);


            showProfilLabel.Text = profil.getName();


        }


        /// <summary>
        /// Returns all Profiles or all members of an group
        /// </summary>
        /// <param name="name">name of the group or [ALL] for all Profiles</param>
        /// <returns></returns>
        private List<string> getGroupMembers(string name)
        {
            if (name != "[ALL]")
            {
                return this.Setup.getListWidthDefault(PConfig.KEY_GROUPS_MEMBERS + "." + name, new List<string>());
            }
            else
            {
                return this.Setup.getListWidthDefault(PConfig.KEY_PROFILS, new List<string>());
            }

        }

        /// <summary>
        /// returns all Groupmembers as hashtable
        /// </summary>
        /// <returns>an Hashtable with all Profiles of given Group or all if Name is [ALL]</returns>
        private Hashtable getGroups()
        {
            Hashtable groups = new Hashtable();

            if (chooseGroup.Text != "[ALL]")
            {
                List<string> pGroups = this.Setup.getListWidthDefault(PConfig.KEY_GROUPS_MEMBERS + "." + chooseGroup.Text, new List<string>());
                foreach (string pGrp in pGroups)
                {
                    groups.Add(pGrp, pGrp);
                }

            }
            else
            {
                /*
                List<string> pGroups = this.Setup.getListWidthDefault(PConfig.KEY_PROFILS, new List<string>());
                foreach (string pGrp in pGroups)
                {
                    groups.Add(pGrp, pGrp);
                }
                 */
            }
            return groups;
        }

        /// <summary>
        /// Main Method to redaw all profiles an assigned Elements
        /// </summary>
        private void updateProfilSelector()
        {

            profilSelector.DropDownItems.Clear();
            flowLayout.Controls.Clear();
            this.SelectedProfiles.Clear();

            if (ProjectorForm.SCRIPT_BUTTON_MODE == buttonStyle)
            {
                //toolStrip1.Visible = false;
                mainSlitter.Panel1Collapsed = true;
                this.setMainColors(Color.Black, Color.DarkGray);
                flowLayout.BackColor = Color.LightGray;


                this.ScriptAutoLoader.showAll(!this.displayNamedScripstOnly);
                if (this.ScriptAutoLoader.setPath(this.mainScriptFolder))
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

            if (ProjectorForm.STYLE_BIG_QUICK_BUTTONS == buttonStyle)
            {
                this.DrawQuickButtonStyle();
                return;
            }

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

        /// <summary>
        /// Updates the Form elements depending on the choosen Prifil
        /// </summary>
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

        /// <summary>
        /// Clickhandler for dynamic createad Buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void syncDbBtnClick(object sender, System.EventArgs e)
        {
            Button sendBtn = (Button)sender;
            toolStripButton2_Click(sender, e);
        }

        /// <summary>
        /// Public handler to change the current Profil
        /// </summary>
        /// <param name="name"></param>
        public void setCurrentProfile(string name)
        {
            profil.changeProfil(name);
            updateSelectedProfil();
            ProfilInfo.Text = name + " [" + profil.getProperty("db_username") + '@' + profil.getProperty("db_schema") + "]";
        }

        /// <summary>
        /// updates the Drowpdown for choosing the Groups
        /// </summary>
        /// <param name="name"></param>
        public void setCurrentGroup(string name)
        {
            if (this.chooseGroup.Items.Contains(name))
            {
                this.chooseGroup.Text = name;
            }
        }

        /// <summary>
        /// Handler to Creates an new MIDI Window and show it,
        /// 
        /// </summary>
        /// <param name="name">name of the Profil that will be used</param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            setupForm sForm = new setupForm(profil.getName());

            sForm.Text = profil.getName();

            sForm.host.Text = profil.getProperty("db_host");
            sForm.username.Text = profil.getProperty("db_username");
            sForm.password.Text = profil.getProperty("db_password");
            sForm.schema.Text = profil.getProperty("db_schema");
            sForm.diffviewsetup.Text = profil.getProperty("diff_command");
            sForm.diffviewparam.Text = profil.getProperty("diff_param");
            sForm.constrain_setup.Checked = (profil.getProperty("foreign_key_check") == "1");
            sForm.styleComboBox.Text = profil.getProperty(ProfilProps.WINDOW_STYLE);
            sForm.mdiStyleComboBox.Text = profil.getProperty(ProfilProps.MDI_STYLE);

            string colortext = profil.getProperty("set_bgcolor");
            if (colortext != null && colortext.Length > 0)
            {
                sForm.colorText.Text = colortext;
                sForm.colorText.BackColor = Color.FromArgb(int.Parse(colortext));
            }

            if (sForm.ShowDialog() == DialogResult.OK)
            {
                profil.setProperty(ProfilProps.DB_HOST, sForm.host.Text);
                profil.setProperty(ProfilProps.DB_USERNAME, sForm.username.Text);
                profil.setProperty(ProfilProps.DB_PASSWORD, sForm.password.Text);
                profil.setProperty(ProfilProps.DB_SCHEMA, sForm.schema.Text);
                profil.setProperty(ProfilProps.STYLE_COLOR, sForm.colorText.Text);
                profil.setProperty(ProfilProps.DIFF_CMD, sForm.diffviewsetup.Text);
                profil.setProperty(ProfilProps.DIFF_PARAM, sForm.diffviewparam.Text);
                profil.setProperty(ProfilProps.WINDOW_STYLE, sForm.styleComboBox.Text);
                profil.setProperty(ProfilProps.MDI_STYLE, sForm.mdiStyleComboBox.Text);
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
            MysqlHandler TestConnect = new MysqlHandler(testPro,2000);

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
                result.usedProfil = testPro.getName();
                return result;
            }
            else
            {
                TestConnect.disConnect();
                result.message = Projector.Properties.Resources.MysqlConnectionTestSucces; ;
                result.status = true;
                result.StatusKey = 1;
                result.usedProfil = testPro.getName();
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
            if (profil.getProperty(ProfilProps.STYLE_COLOR) != null && profil.getProperty(ProfilProps.STYLE_COLOR).Length > 2)
            {
                sensor.BackColor = Color.FromArgb(int.Parse(profil.getProperty(ProfilProps.STYLE_COLOR)));
            }
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
            qb.sensorProfil.getValuesFromProfil(profil);
            qb.listTables();
            qb.Show();
        }

        private void mDIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MdiForm mdif = new MdiForm();
            if (mdif.currentProfil == null) mdif.currentProfil = new Profil(profil.getName());

            mdif.currentProfil.getValuesFromProfil(profil);
            mdif.Text = " [" + profil.getProperty(ProfilProps.DB_USERNAME) + '@' + profil.getProperty(ProfilProps.DB_HOST) + "/" + profil.getProperty(ProfilProps.DB_SCHEMA) + "]";

            if (mdif.currentProfil.getProperty(ProfilProps.STYLE_COLOR) != null && mdif.currentProfil.getProperty(ProfilProps.STYLE_COLOR).Length > 2)
            {
                
                 StyleFormProps profilStyle = new StyleFormProps();
                 profilStyle.MainColor = Color.FromArgb(int.Parse(mdif.currentProfil.getProperty(ProfilProps.STYLE_COLOR)));
                 if (mdif.currentProfil.getProperty(ProfilProps.MDI_STYLE) == null)
                 {
                     profilStyle.composeStyle(this.mainMDIStyle);
                 }
                 else
                 {
                     profilStyle.composeStyle(mdif.currentProfil.getProperty(ProfilProps.MDI_STYLE));                    
                 }
                 mdif.setStyle(profilStyle);

            }

            mdif.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            copyDb cform = new copyDb();
            cform.Show();
        }

        private void showProfilLabel_Click(object sender, EventArgs e)
        {
            updateProfilSelector();
        }

        private void exportCurrentProfilToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
            drawGroupButtons();
            updateProfilSelector();

        }

        private void groupQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GroupQuery gq = new GroupQuery();

            gq.Show();

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            toolStripButton3.Enabled = false;
            if (connectionTest.IsBusy)
            {
                connectionTest.CancelAsync();
                toolStripButton3.Enabled = true;
            }
            else
            {
                List<string> allMembers = this.getGroupMembers(this.chooseGroup.Text);
                connectionTest.RunWorkerAsync(allMembers);
            }
            
        }

        private void backupProfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveProject.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Setup.saveConfigToFile(saveProject.FileName);

            }
        }

        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openProjectDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Setup.loadConfigFromFile(openProjectDlg.FileName);
                this.updateProfilSelector();
            }
        }

        private void importPartsOfProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void groupedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showGroups = groupedToolStripMenuItem.Checked;
            updateProfilSelector();
        }

        private void switchButtonModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonStyle++;
            if (buttonStyle == 1) buttonStyle = 2;
            if (buttonStyle > maxStyle) buttonStyle = 0;
            updateStyleButtons();
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
            this.showGroups = groupedToolStripMenuItem.Checked;
            if (ProjectorForm.STYLE_BUTTON_MODE == buttonStyle)
            {
                this.flowLayout.Controls.Clear();
                this.drawNewStyle();

            }
        }

        private void scriptRunButton_Click(object sender, EventArgs e)
        {

            if (this.openScript.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.scriptFile = this.openScript.FileName;
            }


            if (this.scriptFile != null)
            {
                string execScript = System.IO.File.ReadAllText(this.scriptFile);
                ReflectionScript script = new ReflectionScript();
                script.setCode(execScript);
                RefScriptExecute executer = new RefScriptExecute(script, this);
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

            List<string> styles = StyleFormProps.getAllStyles();
            foreach (string style in styles)
            {
                ms.MainFormStyle.Items.Add(style);
                ms.MainMDIStyle.Items.Add(style);
            }

            ms.MainMDIStyle.Text = this.mainMDIStyle;
            ms.MainFormStyle.Text = ProjectorForm.mainFormStyle;

            if (ms.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.mainScriptFolder = ms.ScriptPath.getPath();
                this.displayNamedScripstOnly = ms.displayNamedScript.Checked;
                ProjectorForm.mainFormStyle = ms.MainFormStyle.Text;
                this.mainMDIStyle = ms.MainMDIStyle.Text;
                updateProfilSelector();
            }
        }

        private void updateStyleButtons()
        {
            style_0.Checked = false;
          
            style_2.Checked = false;
            style_3.Checked = false;
            switch (this.buttonStyle)
            {
                case 0:
                    style_0.Checked = true;
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

        private void flowLayout_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void flowLayoutControllPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void flowLayout_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Projector.ProfilButton"))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// drop stuff over the empty space from flowlayout
        /// </summary>
        /// <param name="sender">object thats send the message</param>
        /// <param name="e">event args</param>
        private void flowLayout_DragDrop(object sender, DragEventArgs e)
        {
            //string movedOut = e.Data.GetData(DataFormats.Text).ToString();
            ProfilButton pButton = (ProfilButton)e.Data.GetData("Projector.ProfilButton");
            if (pButton.assignedGroup != null)
            {
                GroupProfilWorker worker = new GroupProfilWorker(this.Setup);
                if (worker.removeFromGroup(pButton.assignedGroup, pButton.profilName))
                {
                    pButton.assignedGroup = null;
                    this.drawNewStyleButtons(pButton.assignedGroup, true);
                }
            }

        }


        public void reOrderButtonsInGroup(ProfilButton target, ProfilButton dropped)
        {
            GroupProfilWorker worker = new GroupProfilWorker(this.Setup);
            if (target.assignedGroup != null && dropped.assignedGroup != null && target.assignedGroup == dropped.assignedGroup)
            {
                int newPos = worker.setPositionInGroup(dropped.assignedGroup, dropped.profilName, target.profilName);
                if (flowLayout.Controls[target.assignedGroup] != null)
                {
                    if (flowLayout.Controls[target.assignedGroup] is ProfilGroupBox)
                    {
                        ProfilGroupBox currentGroup = (ProfilGroupBox)flowLayout.Controls[target.assignedGroup];
                        currentGroup.setProfileNewPosition(dropped, newPos);
                    }
                }
            }


        }

        private void removeSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Setup.removeEmptyChilds = true;
            int cnt = this.SelectedProfiles.Count;
            if (cnt > 0)
            {
                if (MessageBox.Show("removing " + cnt + " profiles ? This Action can't be revertet. Maybe you should save this Project first as Backup", "Confirm Deletes", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    GroupProfilWorker worker = new GroupProfilWorker(this.Setup);
                    foreach (DictionaryEntry pDic in this.SelectedProfiles)
                    {
                        string profilName = pDic.Key.ToString();
                        ProfilButton btn = (ProfilButton)pDic.Value;
                        if (btn.assignedGroup != null)
                        {
                            worker.removeFromGroup(btn.assignedGroup, profilName);
                        }
                        this.Setup.RemoveChild(PConfig.KEY_PROFILS, profilName);
                    }
                    this.Setup.removeEmptyChilds = false;
                    this.SelectedProfiles.Clear();
                    this.updateProfilSelector();
                }
            }
        }

        private void CconnectionTest_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> profiles = (List<string>) e.Argument;
            Profil testProfil = new Profil();
            foreach (string prof in profiles)
            {
                testProfil.changeProfil(prof);
                statusResult state = this.checkConnection(testProfil);
                connectionTest.ReportProgress(1, state);
                
            }
        }

        private void connectionTest_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusResult state = (statusResult)e.UserState;
            if (buttonStyle == ProjectorForm.STYLE_BUTTON_MODE)
            {
                ProfilButton upBtn = this.getStoredPButton(state.usedProfil);
                if (upBtn != null)
                {

                    if (upBtn.Top + upBtn.Height > ClientSize.Height || upBtn.Top + upBtn.Height < 0)
                    {
                        flowLayout.ScrollControlIntoView(upBtn);
                    }

                    if (state.status)
                    {
                        upBtn.setColor(Color.Green);
                    }
                    else
                    {
                        upBtn.setColor(Color.Red);
                    }
                    upBtn.flush();
                    upBtn.setDescription( state.message );
                }
            }
        }

        private void connectionTest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripButton3.Enabled = true;
        }



    }
}
