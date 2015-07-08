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
    class TableToForm
    {

        

        private string TableName;
        private MysqlHandler db;
      
        List<MysqlStruct> structInfo = new List<MysqlStruct>();
        int addedTop = 20;


        List<TextBox> SearchVarchar = new List<TextBox>();
        List<Label> SearchLabels = new List<Label>();
        List<ComboBox> SearchEnums = new List<ComboBox>();
        List<DateTimePicker> serachDateTime = new List<DateTimePicker>();
        List<NumericUpDown> SearchInts = new List<NumericUpDown>();

        List<String> preValues = new List<string>();

        private QueryComposer maskQuery;
   

        public TableToForm(string tableName, MysqlHandler DataBase)
        {
            this.db = DataBase;
            this.TableName = tableName;
            if (!DataBase.isConnected()) throw new Exception("Database not connected. TableToForm needs an connected MysqlHandler");

            maskQuery = new QueryComposer(this.TableName);

        }

        public TableToForm(string tableName, MysqlHandler DataBase, List<string> values)
        {
            this.db = DataBase;
            this.TableName = tableName;
            this.preValues = values;
            if (!DataBase.isConnected()) throw new Exception("Database not connected. TableToForm needs an connected MysqlHandler");

            maskQuery = new QueryComposer(this.TableName);

        }
      
        public QueryComposer getComposer()
        {
            return maskQuery;
        }


        public void show(Control CtrlElement)
        {
            this.show(CtrlElement.Controls);
        }

        public void show(Control.ControlCollection maskBox)
        {
            maskBox.Clear();
            this.buildForm(maskBox,true);
        }


        private void buildForm(Control.ControlCollection maskBox, bool newRead)
        {
            if (newRead)
            {
                structInfo.Clear();
                structInfo = this.db.getAllFieldsStruct(this.TableName);
            }

            for (int i = 0; i < structInfo.Count; i++)
            {

       

                int elementHeight = 23;
                Label infoStr = new Label();
                infoStr.Text = structInfo[i].name + " [" + structInfo[i].realType + "]";
                
                //infoStr.Text = structInfo[i].name;
                infoStr.Left = 10;
                infoStr.Top = addedTop;
                infoStr.AutoSize = true;
                              
              //  infoStr.Visible = true;

                SearchLabels.Add(infoStr);
                maskBox.Add(infoStr);

                string setValue = "";
                if (preValues.Count > i)
                {
                    setValue = preValues[i]; 
                }

                switch (structInfo[i].realType)
                {
                    case "varchar":
                        TextBox tmpBox = new TextBox();
                        tmpBox.Left = 250;
                        tmpBox.Top = addedTop;
                        //if (structInfo[i].len < 20) tmpBox.Width = structInfo[i].len * 10;
                        //else tmpBox.Width = 200;
                        tmpBox.Width = 200;
                        tmpBox.MaxLength = structInfo[i].len;
                        tmpBox.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        tmpBox.TextChanged += new System.EventHandler(dynInput);
                        tmpBox.Text = setValue;
                        maskBox.Add(tmpBox);

                        
                        break;
                    case "text":
                        TextBox tmpBox2 = new TextBox();
                        tmpBox2.Left = 250;
                        tmpBox2.Top = addedTop;
                        tmpBox2.Width = 200;
                        tmpBox2.Height = 100;
                        tmpBox2.Multiline = true;

                        elementHeight = 110;

                        tmpBox2.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        tmpBox2.TextChanged += new System.EventHandler(dynInput);
                        tmpBox2.Text = setValue;
                        maskBox.Add(tmpBox2);

                        
                        break;
                    case "int":
                    case "tinyint":
                    case "bigint":
                    case "mediumint":
                    case "smallint":
                        NumericUpDown tmpNumber = new NumericUpDown();
                        tmpNumber.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        tmpNumber.Left = 250;
                        tmpNumber.Top = addedTop;
                        tmpNumber.Minimum = structInfo[i].minIntVal;
                        tmpNumber.Maximum = structInfo[i].maxIntval;
                        tmpNumber.ValueChanged += new System.EventHandler(dynNumeric);

                        if (setValue != "")
                        {
                            try
                            {
                                tmpNumber.Value = Int64.Parse(setValue);
                            }
                            catch (Exception)
                            {


                            }
                        }

                        maskBox.Add(tmpNumber);

                        
                        break;

                    case "timestamp":
                    case "datetime":
                        MaskedTextBox tmpTimePicker = new MaskedTextBox();
                        tmpTimePicker.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        tmpTimePicker.Left = 250;
                        tmpTimePicker.Top = addedTop;

                        tmpTimePicker.Mask = "0000-00-00 00:00:00";
                        tmpTimePicker.Width = 110;

                        tmpTimePicker.TextChanged += new System.EventHandler(dynMaskInput);
                        tmpTimePicker.Text = setValue;
                        maskBox.Add(tmpTimePicker);

                        
                        break;

                    case "enum":
                        ComboBox tmpEnum = new ComboBox();
                        tmpEnum.Top = addedTop;
                        tmpEnum.Left = 250;
                        tmpEnum.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        if (structInfo[i].enums != null)
                        {
                            for (int b = 0; b < structInfo[i].enums.Length; b++)
                            {
                                tmpEnum.Items.Add(structInfo[i].enums[b]);
                            }
                        }
                        tmpEnum.TextChanged += new System.EventHandler(dynComboBoxInput);
                        tmpEnum.Text = setValue;
                        maskBox.Add(tmpEnum);

                       
                        break;
                    default:
                        TextBox tmpBox3 = new TextBox();
                        tmpBox3.Left = 250;
                        tmpBox3.Top = addedTop;
                        //if (structInfo[i].len < 20) tmpBox.Width = structInfo[i].len * 10;
                        //else tmpBox.Width = 200;
                        tmpBox3.Width = 200;
                        tmpBox3.MaxLength = structInfo[i].len;
                        tmpBox3.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        tmpBox3.TextChanged += new System.EventHandler(dynInput);
                        tmpBox3.Text = setValue;
                        maskBox.Add(tmpBox3);

                       
                        break;
                }
                

                // finaly calc new height
                addedTop += elementHeight;


            }

            // adding controlls

            addedTop += 10;
            /*
            Button fireSQL = new Button();
            fireSQL.Top = addedTop;
            fireSQL.Left = 300;
            fireSQL.Text = "Start";
            fireSQL.Height = 46;
            fireSQL.Width = 96;
            fireSQL.ImageAlign = ContentAlignment.TopCenter;
            fireSQL.TextAlign = ContentAlignment.BottomCenter;
            fireSQL.Image = Projector.Properties.Resources.view_refresh;
            
            fireSQL.Click += new System.EventHandler(dynRunQuery);
            maskBox.Add(fireSQL);
            */
        }

       

        // dynamic events
        private void dynInput(object sender, System.EventArgs e)
        {
            TextBox VarCharAdd = (TextBox)sender;
            String name = VarCharAdd.Name.Replace("DYNAMIC_PROJ_", "");            
            maskQuery.addSetValue (name, VarCharAdd.Text);            
            
            


            VarCharAdd.BackColor = Color.LightGreen;
        }

        private void dynMaskInput(object sender, System.EventArgs e)
        {
            MaskedTextBox VarCharAdd = (MaskedTextBox)sender;
            String name = VarCharAdd.Name.Replace("DYNAMIC_PROJ_", "");

            maskQuery.addSetValue(name, VarCharAdd.Text);            
           
            VarCharAdd.BackColor = Color.LightGreen;
        }


       

        private void dynComboBoxInput(object sender, System.EventArgs e)
        {
            ComboBox VarCharAdd = (ComboBox)sender;
            String name = VarCharAdd.Name.Replace("DYNAMIC_PROJ_", "");
            
            VarCharAdd.BackColor = Color.LightSteelBlue;

            maskQuery.addSetValue(name, VarCharAdd.Text);
            if (VarCharAdd.Text == "") maskQuery.removeWhere(name);
            
            VarCharAdd.BackColor = Color.LightGreen;
        }

        private void dynNumeric(object sender, System.EventArgs e)
        {
            NumericUpDown VarCharAdd = (NumericUpDown)sender;
            String name = VarCharAdd.Name.Replace("DYNAMIC_PROJ_", "");
            
            maskQuery.addSetValue(name, VarCharAdd.Value.ToString());
            
            VarCharAdd.BackColor = Color.LightGreen;
        }

        /*
        private void dynWhereCompare(object sender, System.EventArgs e)
        {
            ComboBox VarCharAdd = (ComboBox)sender;
            String name = VarCharAdd.Name.Replace("DYNAMIC_PROJ_", "");

            //maskQuery.addSetValue(name, VarCharAdd.Value.ToString());
            maskQuery.addSetValueComp(name,VarCharAdd.Text);
            
            VarCharAdd.BackColor = Color.LightGreen;
        }
         */
        /*
        private void dynRunQuery(object sender, System.EventArgs e)
        {
           
        }
        */

    }
}
