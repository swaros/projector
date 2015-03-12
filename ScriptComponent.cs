using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Projector.Script;

namespace Projector
{
    public partial class ScriptComponent : UserControl
    {

        private ReflectNewWidget widget;

        private RichBox code;

        public ScriptComponent()
        {
            InitializeComponent();
        }

        public void assignWidget(ReflectNewWidget widget)
        {
            this.widget = widget;
            this.NameOfTool.Text = this.widget.Name;
            if (this.widget.Icon != null)
            {
                this.ToolImage.Image = this.widget.Icon;
            }
        }

        public void assignScript(RichBox script)
        {
            this.code = script;
        }

        private String composeCodeByPlaceHolder()
        {
            if (this.widget != null)
            {
                string[] parts = this.widget.CodeInsert.Split(' ');
            
                string code = "";
                UserTextInput input = new UserTextInput();
                input.groupBox.Text = "Insert name of";
                foreach (string pstr in parts)
                {
                    if (pstr.Length > 0 && pstr[0] == '!')
                    {
                        input.textinfo.Text = pstr.Substring(1);
                        if (input.ShowDialog() == DialogResult.OK)
                        {
                            code += input.textinfo.Text.Replace(" ","_") + " ";
                        }
                        else
                        {
                            code += pstr.Replace("!","") + " ";
                        }
                    }
                    else
                    {
                        code += pstr + " ";
                    }
                }
                return code;
            }
            return "";

        }

        private void ToolImage_DoubleClick(object sender, EventArgs e)
        {
            if (this.code != null)
            {
                this.code.SelectedText = this.composeCodeByPlaceHolder();
            }
        }

        
    }
}
