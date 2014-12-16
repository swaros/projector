using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Projector
{
    class ReflectNew : ReflectObjectInterface
    {

        private static Hashtable obReferences = new Hashtable();

        public Object execute()
        {
            return null;
        }

        public static void removeMe(Form refObject)
        {
            if (ReflectNew.obReferences.ContainsKey(refObject.Name))
            {                
                ReflectNew.obReferences.Remove(refObject.Name);                
            }

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


            if (refObject.typeOfObject == "DateSelect")
            {
                return this.getDatePick(refObject, parent);
            }

            if (refObject.typeOfObject == "GroupQuery")
            {
                return this.getGroupQuery(refObject, parent);
            }

            if (refObject.typeOfObject == "ReflectList")
            {
                return this.getListView(refObject, parent);
            }

            if (refObject.typeOfObject == "ListView")
            {
                return this.getOriginListView(refObject, parent);
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

        private ReflectList getListView(ReflectionScriptDefines refObject, Object parent)
        {
            ReflectList newTextBox = new ReflectList();
            newTextBox.Name = refObject.name;

            if (parent is Form)
            {
                Form parForm = (Form)parent;
                parForm.Controls.Add(newTextBox);
            }

            return newTextBox;
        }


        private ListView getOriginListView(ReflectionScriptDefines refObject, Object parent)
        {
            ListView resultView = new ListView();
            resultView.Name = refObject.name;

            /*
            if (parent is Form)
            {
                Form parForm = (Form)parent;
                parForm.Controls.Add(newTextBox);
            }
            */

            resultView.View = View.Details;                        
            resultView.FullRowSelect = true;
            resultView.GridLines = true;

            return resultView;
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

        private DateSelect getDatePick(ReflectionScriptDefines refObject, Object parent)
        {
            DateSelect newTextBox = new DateSelect();
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
            else if (parent is Form)
            {
                newForm.Show();
            }

            return newForm;
        }

        private GroupQuery getGroupQuery(ReflectionScriptDefines refObject, Object parent)
        {

            if (ReflectNew.obReferences.ContainsKey(refObject.name))
            {
                Object hValue = obReferences[refObject.name];
                if(hValue is GroupQuery){
                    GroupQuery exists = (GroupQuery)hValue;
                    
                    try
                    {
                        //exists.Show();
                        //exists.BringToFront();
                        //exists.Show();  
                        exists.imHere = true;
                        return exists;    
                    }
                    catch (ObjectDisposedException disposed)
                    {
                        ReflectNew.obReferences.Remove(refObject.name);
                    }
                    
                }
            }

            GroupQuery grQuery = new GroupQuery();
            grQuery.Name = refObject.name;
            
            if (parent is MdiForm)
            {
                MdiForm mdi = (MdiForm)parent;
                mdi.addSubWindow(grQuery, refObject.name);
            }
            //grQuery.Show();

            if (!ReflectNew.obReferences.ContainsKey(refObject.name))
            {
                ReflectNew.obReferences.Add(refObject.name, grQuery);
            }
            return grQuery;
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
