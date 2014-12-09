using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projector
{
    class ReflectNew : ReflectObjectInterface
    {
        public Object execute()
        {
            return null;
        }

        public Object getObject(ReflectionScriptDefines refObject, Object parent)
        {
            if (refObject.typeOfObject == "queryBrowser")
            {
                return this.getQueryBrowser(refObject,parent);
            }

            if (refObject.typeOfObject == "ReflectForm")
            {
                return this.getForm(refObject, parent);
            }

            if (refObject.typeOfObject == "TextBox")
            {
                return this.getTextBox(refObject, parent);
            }

            if (refObject.typeOfObject == "LabelText")
            {
                return this.getLabelText(refObject, parent);
            }

            if (refObject.typeOfObject == "LabelInteger")
            {
                return this.getLabelInt(refObject, parent);
            }


            if (refObject.typeOfObject == "ReflectButton")
            {
                return this.getButton(refObject, parent);
            }


            if (refObject.typeOfObject == "NumericUpDown")
            {
                return this.getNumeric(refObject, parent);
            }

            return null;
        }

        private ReflectButton getButton(ReflectionScriptDefines refObject, Object parent)
        {
            ReflectButton newTextBox = new ReflectButton();
            newTextBox.Name = refObject.name;

            if (parent is Form)
            {
                Form parForm = (Form)parent;
                parForm.Controls.Add(newTextBox);
            }

            return newTextBox;
        }



        private LabelText getLabelText(ReflectionScriptDefines refObject, Object parent)
        {
            LabelText newTextBox = new LabelText();
            newTextBox.Name = refObject.name;

            if (parent is Form)
            {
                Form parForm = (Form)parent;
                parForm.Controls.Add(newTextBox);
            }

            return newTextBox;
        }

        private LabelInteger getLabelInt(ReflectionScriptDefines refObject, Object parent)
        {
            LabelInteger newTextBox = new LabelInteger();
            newTextBox.Name = refObject.name;

            if (parent is Form)
            {
                Form parForm = (Form)parent;
                parForm.Controls.Add(newTextBox);
            }

            return newTextBox;
        }


        private TextBox getTextBox(ReflectionScriptDefines refObject, Object parent)
        {
            TextBox newTextBox = new TextBox();
            newTextBox.Name = refObject.name;

            if (parent is Form)
            {
                Form parForm = (Form)parent;
                parForm.Controls.Add(newTextBox);
            }

            return newTextBox;
        }

        private NumericUpDown getNumeric(ReflectionScriptDefines refObject, Object parent)
        {
            NumericUpDown newTextBox = new NumericUpDown();
            newTextBox.Name = refObject.name;

            if (parent is Form)
            {
                Form parForm = (Form)parent;
                parForm.Controls.Add(newTextBox);
            }

            return newTextBox;
        }


        private ReflectForm getForm(ReflectionScriptDefines refObject, Object parent)
        {
            ReflectForm newForm = new ReflectForm();
            newForm.ScriptIdent = refObject.name;

            if (parent is MdiForm)
            {
                MdiForm mdi = (MdiForm)parent;
                mdi.addSubWindow(newForm,refObject.name);
            }

            return newForm;
        }


        private queryBrowser getQueryBrowser(ReflectionScriptDefines refObject, Object parent)
        {
            queryBrowser browser = new queryBrowser();
            browser.Name = refObject.name;
            browser.ScriptIdent = refObject.name;

            if (parent is MdiForm)
            {

                MdiForm mdi = (MdiForm)parent;
                queryBrowser existingQb = (queryBrowser)mdi.getQueryForm(refObject.name);
                if (null == existingQb)
                {
                    mdi.addQueryWindow(browser);
                }
                else
                {
                    return existingQb;

                }
            }

            return browser;
        }
    }
}
