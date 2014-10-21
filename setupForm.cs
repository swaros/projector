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
    public partial class setupForm : Form
    {
        public setupForm()
        {
            InitializeComponent();
            label1.Text = Projector.Properties.Resources.mysqlUserName;
            label2.Text = Projector.Properties.Resources.mysqlPassword;
            label3.Text = Projector.Properties.Resources.mysqlHost;
            label4.Text = Projector.Properties.Resources.mysqlSchema;

            okbtn.Text = Projector.Properties.Resources.btnOk;
            cancelbtn.Text = Projector.Properties.Resources.btnCancel;
            groupBox1.Text = Projector.Properties.Resources.mysqlSettings;
            tabPage1.Text = Projector.Properties.Resources.mysqlSettings;
            btnChk.Text = Projector.Properties.Resources.MysqlCheckConnetion;
            checkInput();

        }

        private void checkConnection(Profil testProfil)
        {
            MysqlHandler TestConnect = new MysqlHandler(testProfil);

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
                errorLabel.Text = TestConnect.lastSqlErrorMessage;
            }
            else
            {
                errorLabel.Text = Projector.Properties.Resources.MysqlConnectionTestSucces;
                TestConnect.disConnect();

            }

        }

        private bool checkInput()
        {
            if (username.Text.Length == 0 || password.Text.Length == 0 || schema.Text.Length == 0 || host.Text.Length == 0)
            {
                errorLabel.Text = Projector.Properties.Resources.ProfileCheckMessage;
                btnChk.Enabled = false;
                return false;
            }
            else
            {
                btnChk.Enabled = true;
                errorLabel.Text = "";
                return true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                Profil testProfil = new Profil("test");
                testProfil.setProperty("db_password",password.Text);
                testProfil.setProperty("db_username", username.Text);
                testProfil.setProperty("db_host", host.Text);
                testProfil.setProperty("db_schema", schema.Text);
                checkConnection(testProfil);

                /*
                 * username.Text = tmpSetup.getValue("db_username");
                password.Text = tmpSetup.getValue("db_password");
                host.Text = tmpSetup.getValue("db_host");
                schema.Text = tmpSetup.getValue("db_schema");
                 */
            }
        }

        private void setupForm_Load(object sender, EventArgs e)
        {

        }

        private void username_TextChanged(object sender, EventArgs e)
        {
            checkInput();
        }

        private void password_TextChanged(object sender, EventArgs e)
        {
            checkInput();
        }

        private void host_TextChanged(object sender, EventArgs e)
        {
            checkInput();
        }

        private void schema_TextChanged(object sender, EventArgs e)
        {
            checkInput();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                colorText.Text = colorDialog1.Color.ToArgb().ToString();

                colorText.BackColor = Color.FromArgb(int.Parse(colorText.Text));
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (openProfilXml.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                XmlSetup tmpSetup = new XmlSetup();
                tmpSetup.setFullFilename(openProfilXml.FileName);
                tmpSetup.loadXml();

                username.Text = tmpSetup.getValue("db_username");
                password.Text = tmpSetup.getValue("db_password");
                host.Text = tmpSetup.getValue("db_host");
                schema.Text = tmpSetup.getValue("db_schema");
                
            }
        }


    }
}
