using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Projector.Script;

namespace Projector
{
    public partial class ProjectorForm : Form
    {
        /// <summary>
        /// show Page for Scripts in defined scriptfolder
        /// </summary>
        const int SCRIPT_BUTTON_MODE = 3;
        /// <summary>
        /// show Profil Workbench
        /// </summary>
        const int STYLE_BUTTON_MODE = 2;
        /// <summary>
        /// show Big Startbuttons 
        /// </summary>
        const int STYLE_BIG_QUICK_BUTTONS = 0;

        /// <summary>
        /// DEPRECATED
        /// </summary>
        const int STYLE_SMALL_QUICK_BUTTONS = 1;

        /// <summary>
        /// Current Profil
        /// </summary>
        private Profil profil = new Profil();
        /// <summary>
        /// if true, all unassigned Profils will be grouped in 
        /// an Group named "unassigned". only on Workbench mode
        /// </summary>
        private Boolean groupUnassigned = false;


        /// <summary>
        /// the current Mode for the Style
        /// </summary>
        private int buttonStyle = 0;

        /// <summary>
        /// max possible Styles for buttonmode
        /// </summary>
        private int maxStyle = 3;

        /// <summary>
        /// current loaded scriptfile (used by the quickrun button)
        /// </summary>
        private string scriptFile;

        /// <summary>
        /// setting for Scriptfolder View so only scripts that have an label will be desolayed
        /// </summary>
        private Boolean displayNamedScripstOnly = false;

        /// <summary>
        /// contains the name of the folder that contains the Reflector Scripts. also used for Scriptcommand EXEC
        /// </summary>
        private string mainScriptFolder;

        /// <summary>
        /// cache for all Groups. also used for saving
        /// </summary>
        private List<string> profilGroups = new List<string>();

        /// <summary>
        /// contains all selected Profiles on Workbench
        /// </summary>
        private Hashtable SelectedProfiles = new Hashtable();

        /// <summary>
        /// Handler for Reflection Scripts. manages which script can be displayed and how
        /// </summary>
        RefScrAutoStart ScriptAutoLoader = new RefScrAutoStart();

        /// <summary>
        /// default Style setting for any Child that don't have own settings.
        /// forms only.
        /// </summary>
        private static string mainFormStyle = StyleFormProps.STYLE_DEFAULT;

        /// <summary>
        /// default Style setting for any Child that don't have own settings.
        /// MDI only and there is only on and this one created by this form, so this setting 
        /// don't needed as static
        /// </summary>
        private string mainMDIStyle = StyleFormProps.STYLE_DEFAULT;

        /// <summary>
        /// Main Configuration Handler
        /// </summary>
        private PConfig Setup = new PConfig();

        /// <summary>
        /// Current backgroundimage for Workbench view.
        /// set null for non image
        /// </summary>
        Image bgImage;

        private int WorbenchBackGroundImage = 0;

        /// <summary>
        /// Main Projector Form.
        /// </summary>
        public ProjectorForm()
        {
            InitializeComponent();
            this.mainScriptFolder = Application.StartupPath;
            Setup.loadRuntimeConfig();
            bgImage = Projector.Properties.Resources.bg_07;
            //bgImage = Projector.Properties.Resources.bg_03;
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

                this.Setup.setValue("client.showgroups", mainSlitter.Panel1Collapsed);
                this.Setup.setValue("client.buttonstyle", this.buttonStyle);

                this.Setup.setValue("client.scriptpath", this.mainScriptFolder);
                this.Setup.setValue("client.namedscriptsonly", this.displayNamedScripstOnly);

                this.Setup.setValue("client.style.mainform", ProjectorForm.mainFormStyle);
                this.Setup.setValue("client.style.mainmdi", this.mainMDIStyle);

                this.Setup.setList("client.groups.names", this.profilGroups);
                this.Setup.setValue("client.maximized", this.WindowState == FormWindowState.Maximized);
                this.Setup.setValue("client.workbench.bgimage", this.WorbenchBackGroundImage);    
                
                
            }
            else
            {
                this.Left = this.Setup.getIntSettingWidthDefault("client.left", this.Left);
                this.Top = this.Setup.getIntSettingWidthDefault("client.top", this.Top);
                this.Width = this.Setup.getIntSettingWidthDefault("client.width", this.Width);
                this.Height = this.Setup.getIntSettingWidthDefault("client.height", this.Height);

                if (this.Top < -1) this.Top = 1;
                if (this.Left < -1) this.Left = 1;
                if (this.Width < 200) this.Width = 200;
                if (this.Height < 200) this.Height = 200;

                mainSlitter.Panel1Collapsed = this.Setup.getBooleanSettingWidthDefault("client.showgroups", mainSlitter.Panel1Collapsed);
                groupButtonsToolStripMenuItem.Checked = !mainSlitter.Panel1Collapsed;

                this.buttonStyle = this.Setup.getIntSettingWidthDefault("client.buttonstyle", this.buttonStyle);

                this.mainScriptFolder = this.Setup.getSettingWidthDefault("client.scriptpath", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
                this.displayNamedScripstOnly = this.Setup.getBooleanSettingWidthDefault("client.namedscriptsonly", this.displayNamedScripstOnly);
               

                this.mainMDIStyle = this.Setup.getSettingWidthDefault("client.style.mainmdi", this.mainMDIStyle);
                ProjectorForm.mainFormStyle = this.Setup.getSettingWidthDefault("client.style.mainform", ProjectorForm.mainFormStyle);

                this.profilGroups = this.Setup.getListWidthDefault("client.groups.names", this.profilGroups);

                bool maximized = this.Setup.getBooleanSettingWidthDefault("client.maximized", this.WindowState == FormWindowState.Maximized);

                this.WorbenchBackGroundImage = this.Setup.getIntSettingWidthDefault("client.workbench.bgimage", this.WorbenchBackGroundImage);
                this.setBgImage();
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

        public void HoldLayout()
        {
            flowLayout.Visible = false;
            flowLayout.SuspendLayout();
        }

        public void RedrawLayout()
        {
            this.flowLayout.ResumeLayout();
            this.flowLayout.Visible = true;
        }

        /// <summary>
        /// Returns the Main Defined Style for all
        /// Child Forms, that don't have own style settings
        /// </summary>
        /// <returns></returns>
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
            flowLayout.BackgroundImage = bgImage;
            this.RedrawLayout();
        }

        /// <summary>
        /// drawing method Workbench
        /// </summary>
        private void drawNewStyle()
        {
            this.HoldLayout();
            List<string> grp = this.getGroupMembers(chooseGroup.Text);

            flowLayout.BackColor = Color.FromArgb(60, 60, 60);
            flowLayout.Padding = new Padding(10);
            flowLayout.BackgroundImage = null;
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
            flowLayout.BackgroundImage = bgImage;
            this.RedrawLayout();
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

        /// <summary>
        /// Drawing method for Button Style
        /// </summary>
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
        /// Tick handler until reading the script folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptDawTimer_Tick(object sender, EventArgs e)
        {
            if (!this.ScriptAutoLoader.ImWorking())
            {
                flowLayout.Controls.Clear();
                this.ScriptDawTimer.Enabled = false;
                this.DrawScriptView();
            }
            else
            {
                if (flowLayout.Controls["SCRBTN"] == null)
                {
                    Button SCRBTN = new Button();
                    SCRBTN.Name = "SCRBTN";
                    SCRBTN.Image = Projector.Properties.Resources.folder_1;
                    SCRBTN.FlatStyle = FlatStyle.Flat;
                    SCRBTN.Width = flowLayout.Width - 60;
                    SCRBTN.Height = flowLayout.Height - 60;
                    //SCRBTN.BackColor = Color.White;
                    SCRBTN.Enabled = true;
                    SCRBTN.FlatAppearance.BorderColor = flowLayout.BackColor;
                    SCRBTN.TextImageRelation = TextImageRelation.ImageAboveText;
                    SCRBTN.Text = "Reading Script Fils in " + this.mainScriptFolder;
                    this.flowLayout.Controls.Add(SCRBTN);
                    SCRBTN.Tag = "A";
                }

                Button FlashBtn = (Button)flowLayout.Controls["SCRBTN"];
                if (FlashBtn.Tag.ToString() == "A")
                {
                    FlashBtn.Tag = "B";
                    FlashBtn.Image = Projector.Properties.Resources.folder_2;
                }
                else if (FlashBtn.Tag.ToString() == "B")
                {
                    FlashBtn.Tag = "C";
                    FlashBtn.Image = Projector.Properties.Resources.folder_3;
                }
                else if (FlashBtn.Tag.ToString() == "C")
                {
                    FlashBtn.Tag = "D";
                    FlashBtn.Image = Projector.Properties.Resources.folder_4;
                }
                else if (FlashBtn.Tag.ToString() == "D")
                {
                    FlashBtn.Tag = "E";
                    FlashBtn.Image = Projector.Properties.Resources.folder_3;
                }
                else if (FlashBtn.Tag.ToString() == "E")
                {
                    FlashBtn.Tag = "F";
                    FlashBtn.Image = Projector.Properties.Resources.folder_2;
                }
                else
                {
                    FlashBtn.Tag = "A";
                    FlashBtn.Image = Projector.Properties.Resources.folder_1;
                }

            }
        }

        /// <summary>
        /// Draws the Script workbench
        /// </summary>
        private void DrawScriptView()
        {
            List<RefScrAutoScrContainer> scripts = this.ScriptAutoLoader.getAllScripts();

            foreach (RefScrAutoScrContainer script in scripts)
            {
                ScriptStartButton tmpStartBtn = new ScriptStartButton(this);
                tmpStartBtn.setScript(script);
                flowLayout.Controls.Add(tmpStartBtn);
            }
        }


        /// <summary>
        /// Main Method to redaw all profiles an assigned Elements
        /// </summary>
        private void updateProfilSelector()
        {

            profilSelector.DropDownItems.Clear();
            flowLayout.BackgroundImage = null;
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
                    this.ScriptDawTimer.Enabled = true;

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

        /// <summary>
        /// Click handler for QuickButtons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dynBtnClick(object sender, System.EventArgs e)
        {
            Button sendBtn = (Button)sender;

            string showAtProperty = sendBtn.Text;

            if (showAtProperty != null)
            {
                this.startProfile(showAtProperty, sender, e);
            }

        }

        /// <summary>
        /// Clickhandler for ProfilButtons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void styleBtnClick(object sender, System.EventArgs e)
        {
            Button sendBtn = (Button)sender;

            string showAtProperty = sendBtn.Name;

            if (showAtProperty != null)
            {
                this.startProfile(showAtProperty, sender, e);
            }

        }

        /// <summary>
        /// Click Handler for Profil-List in the Toolbar
        /// </summary>
        /// <param name="who"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Wrapper for external access the Setup Form
        /// </summary>
        public void callSetup()
        {
            setupToolStripMenuItem_Click(null, null);
        }

        /// <summary>
        /// Click handler menu Setup.
        /// Creates and show the Setup Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupForm sForm = new setupForm(profil.getName());

            sForm.Text = profil.getName();
            sForm.nameOfProfil.Text = profil.getName();
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

                // if the name is changed, chage the profil
                if (sForm.nameOfProfil.Text != profil.getName())
                {
                    GroupProfilWorker worker = new GroupProfilWorker(this.Setup);
                    worker.renameProfil(profil.getName(), sForm.nameOfProfil.Text);

                }

                profil.saveSetup();
                updateProfilSelector();
            }
        }

        /// <summary>
        /// Checks a Connection by the given Profil
        /// </summary>
        /// <param name="testPro">The Profi that will be checked</param>
        /// <returns>Container that contains Test Result</returns>
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

        /// <summary>
        /// Handler for Menu Item Show Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Handler to start the MDI Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Handler to start Database Sync Tool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            copyDb cform = new copyDb();
            cform.Show();
        }

        /// <summary>
        /// Handler for clicking the Profil label that
        /// forces an reload of all profiles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showProfilLabel_Click(object sender, EventArgs e)
        {
            updateProfilSelector();
        }



        /// <summary>
        /// Event handler atfter Form was Created and Activated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectorForm_Shown(object sender, EventArgs e)
        {
            initApp();
            updateProfilSelector();
        }

        /// <summary>
        /// Handler by clicking Menu "Groups...".
        /// Creates and displays the Group Setup Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            p1.ShowDialog();
            drawGroupButtons();
            updateProfilSelector();

        }

        /// <summary>
        /// Handler for menuitem GroupQuery
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GroupQuery gq = new GroupQuery();
            gq.Show();

        }

        /// <summary>
        /// Handler for the Connection Testing ToolBar-Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Handler for Menu "Save Project"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backupProfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveProject.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Setup.saveConfigToFile(saveProject.FileName);

            }
        }

        /// <summary>
        /// Handler for menu "Open Project"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openProjectDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Setup.loadConfigFromFile(openProjectDlg.FileName);
                this.updateProfilSelector();
            }
        }

       
        /// <summary>
        /// handler for menu Item "Change Button Mode"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void switchButtonModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonStyle++;
            if (buttonStyle == 1) buttonStyle = 2;
            if (buttonStyle > maxStyle) buttonStyle = 0;
            updateStyleButtons();
            updateProfilSelector();
        }

        /// <summary>
        /// Handler for Group DropDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chooseGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateProfilSelector();
        }

        /// <summary>
        /// On Paint Event ...not used atm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flowLayout_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// handler for Toolbar Button "show Groups"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupButtonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainSlitter.Panel1Collapsed = !groupButtonsToolStripMenuItem.Checked;
            
            if (ProjectorForm.STYLE_BUTTON_MODE == buttonStyle)
            {
                this.flowLayout.Controls.Clear();
                this.drawNewStyle();

            }
        }

        /// <summary>
        /// Handler for script Run Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Handler for the menu "Edit Script"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptWriter newScript = new ScriptWriter(this);
            newScript.Show();
        }

        /// <summary>
        /// sets the backgroundimage for the worbench view
        /// depending on the index 
        /// </summary>
        private void setBgImage()
        {
            switch (this.WorbenchBackGroundImage){
                case 0:
                    bgImage = null;    
                    break;
                case 1:
                    bgImage = Projector.Properties.Resources.bg_02;
                    break;
                case 2:
                    bgImage = Projector.Properties.Resources.bg_03;
                    break;
                case 3:
                    bgImage = Projector.Properties.Resources.bg_04;
                    break;
                case 4:
                    bgImage = Projector.Properties.Resources.bg_05;
                    break;
                case 5:
                    bgImage = Projector.Properties.Resources.bg_06;
                    break;
                case 6:
                    bgImage = Projector.Properties.Resources.bg_07;
                    break;
            }
        }


        /// <summary>
        /// Handler for the menu "main Setup"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainSetup ms = new MainSetup();
            ms.ScriptPath.setPath(this.mainScriptFolder);
            ms.ScriptPath.setInfo("Default Script Folder");
            ms.displayNamedScript.Checked = this.displayNamedScripstOnly;
            ms.bgSelect.SelectedIndex = this.WorbenchBackGroundImage;
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
                this.WorbenchBackGroundImage = ms.bgSelect.SelectedIndex;
                this.setBgImage();
                updateProfilSelector();
            }
        }

        /// <summary>
        /// method to update the Buttonstate depending on the used ButtonStyle
        /// </summary>
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

        /// <summary>
        /// A Toolbar Button to switch the style was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void style_0_Click(object sender, EventArgs e)
        {
            this.buttonStyle = 0;
            this.updateStyleButtons();
        }
        /// <summary>
        /// A Toolbar Button to switch the style was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void style_1_Click(object sender, EventArgs e)
        {
            this.buttonStyle = 1;
            this.updateStyleButtons();
        }
        /// <summary>
        /// A Toolbar Button to switch the style was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void style_2_Click(object sender, EventArgs e)
        {
            this.buttonStyle = 2;
            this.updateStyleButtons();
        }
        /// <summary>
        /// A Toolbar Button to switch the style was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void style_3_Click(object sender, EventArgs e)
        {
            this.buttonStyle = 3;
            this.updateStyleButtons();
        }

        private void flowLayout_MouseDown(object sender, MouseEventArgs e)
        {

        }
        /// <summary>
        /// handle state of drag and Drop actions on the group Button panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flowLayoutControllPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Handles state od drag and Drop on the main Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flowLayout_DragEnter(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent("Projector.ProfilButton"))
            {
                ProfilButton pButton = (ProfilButton)e.Data.GetData("Projector.ProfilButton");
                
                e.Effect = DragDropEffects.Copy;
            }
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
            

            if (Control.ModifierKeys == Keys.Shift)
            {
                this.CloneProfil(pButton.profilName);
            }
            else
            {
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

        }

        /// <summary>
        /// Reorders the sortorder on Profils on workbench if they are grouped together
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dropped"></param>
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

        /// <summary>
        /// Handler for menuItem §remove selected Profiles"
        /// also riggered by pressing DEL on keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Backround Process for checking connections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Backround Process for checking connections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectionTest_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusResult state = (statusResult)e.UserState;
            if (buttonStyle == ProjectorForm.STYLE_BUTTON_MODE)
            {
                ProfilButton upBtn = this.getStoredPButton(state.usedProfil);
                if (upBtn != null)
                {

                    if (upBtn.Top + upBtn.Height > ClientSize.Height || upBtn.Top < 0)
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
        /// <summary>
        /// Backround Process for checking connections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectionTest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripButton3.Enabled = true;
        }

        /// <summary>
        /// Returns a new Profil and check the name.
        /// if the name allready used, a new one will be crated
        /// until it is a unique name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Profil addNewProfil(string name)
        {
            List<string> currentProfiles = this.Setup.getListWidthDefault(PConfig.KEY_PROFILS, new List<string>());
            string newName = name;
            int nr = 1;
            while (currentProfiles.Contains(newName))
            {
                nr++;
                newName = name + " v.(" + nr + ")";
            }

            Profil addedProfil = new Profil(newName);
            return addedProfil;

        }

        private void CloneProfil(string sourceProfilName)
        {
            /*
            List<string> currentProfiles = this.Setup.getListWidthDefault(PConfig.KEY_PROFILS, new List<string>());
            if (currentProfiles.Contains(sourceProfilName))
            {
                Profil source = new Profil(sourceProfilName);
                source.changeProfil(sourceProfilName);
                string[] sep = "v.,".Split(',');
                string[] stripAutoText = sourceProfilName.Split(sep,StringSplitOptions.None);
                string newName = stripAutoText[0];
                Profil added = addNewProfil(newName);
                
                
                added.setProperty(ProfilProps.DB_NAME, source.getProperty(ProfilProps.DB_NAME));
                added.setProperty(ProfilProps.DB_HOST, source.getProperty(ProfilProps.DB_HOST));
                added.setProperty(ProfilProps.DB_SCHEMA, source.getProperty(ProfilProps.DB_SCHEMA));
                added.setProperty(ProfilProps.DB_USERNAME, source.getProperty(ProfilProps.DB_USERNAME));
                added.setProperty(ProfilProps.MDI_STYLE, source.getProperty(ProfilProps.MDI_STYLE));
                added.setProperty(ProfilProps.STYLE_COLOR, source.getProperty(ProfilProps.STYLE_COLOR));
                added.setProperty(ProfilProps.WINDOW_STYLE, source.getProperty(ProfilProps.WINDOW_STYLE));
                added.setProperty(ProfilProps.MDI_STYLE, source.getProperty(ProfilProps.MDI_STYLE));

                added.changeProfil(newName);
                
                this.setCurrentProfile(newName);
                setupToolStripMenuItem_Click(null, null);
            }*/
        }
        /// <summary>
        /// Handler for Toolbar Button "Add New Profile"
        /// Adding an new Profil after a name was inserted.
        /// then the setup will be triggered
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private void addProfil_Click(object sender, EventArgs e)
        {
            UserTextInput userInput = new UserTextInput();
            userInput.textinfo.Text = "New Profile";
            userInput.groupBox.Text = "Set Name for the New Profile";
            if (userInput.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Profil addedProfil = this.addNewProfil(userInput.textinfo.Text);
                this.setCurrentProfile(addedProfil.getName());
                setupToolStripMenuItem_Click(sender, e);

            }

        }

        /// <summary>
        /// Handler fpr toolbar button to show the Grous Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enableGroupView_Click(object sender, EventArgs e)
        {
            groupButtonsToolStripMenuItem.Checked = !groupButtonsToolStripMenuItem.Checked;
            enableGroupView.Checked = groupButtonsToolStripMenuItem.Checked;
            groupButtonsToolStripMenuItem_Click(sender, e);
        }

       



    }
}
