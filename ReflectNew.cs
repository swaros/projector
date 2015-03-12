﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Projector.Data;
using Projector.Script;

namespace Projector
{
    class ReflectNew : ReflectObjectInterface
    {

        private Boolean useObjectStorage = true;

        private Hashtable obReferences = new Hashtable();        

        public Object execute()
        {
            return null;
        }

        public void removeMe(Form refObject)
        {
            if (this.obReferences.ContainsKey(refObject.Name))
            {                
                this.obReferences.Remove(refObject.Name);                
            }

        }

        public List<ReflectNewWidget> getWidgets()
        {
            List<ReflectNewWidget> widgets = new List<ReflectNewWidget>();

            ReflectNewWidget rForm = new ReflectNewWidget();
            rForm.Name = "Form";
            rForm.Icon = Projector.Properties.Resources.computer_16;
            rForm.CodeInsert = "NEW ReflectForm !MyForm";
            widgets.Add(rForm);

            ReflectNewWidget rList = new ReflectNewWidget();
            rList.Name = "List";
            rList.Icon = Projector.Properties.Resources.ui_list_box_blue;
            rList.CodeInsert = "NEW ReflectList !MyReflectList";
            widgets.Add(rList);


            ReflectNewWidget rBt = new ReflectNewWidget();
            rBt.Name = "Button";
            rBt.Icon = Projector.Properties.Resources.sqlresult;
            rBt.CodeInsert = "NEW ReflectButton !MyButton";
            widgets.Add(rBt);

            ReflectNewWidget rBt2 = new ReflectNewWidget();
            rBt2.Name = "Input Text";
            rBt2.Icon = Projector.Properties.Resources.tag;
            rBt2.CodeInsert = "NEW LabelText !MyTextBox";
            widgets.Add(rBt2);

            ReflectNewWidget rBt3 = new ReflectNewWidget();
            rBt3.Name = "Input Number";
            rBt3.Icon = Projector.Properties.Resources.tag;
            rBt3.CodeInsert = "NEW LabelInteger !MyNumberBox";
            widgets.Add(rBt3);


            ReflectNewWidget rBt4 = new ReflectNewWidget();
            rBt4.Name = "Date";
            rBt4.Icon = Projector.Properties.Resources.tag;
            rBt4.CodeInsert = "NEW DateSelect !MyDateBox";
            widgets.Add(rBt4);

            ReflectNewWidget rBt5 = new ReflectNewWidget();
            rBt5.Name = "Group Query";
            rBt5.Icon = Projector.Properties.Resources.database_link;
            rBt5.CodeInsert = "NEW GroupQuery !MyGroupQuery";
            widgets.Add(rBt5);

            ReflectNewWidget rBt6 = new ReflectNewWidget();
            rBt6.Name = "DB";
            rBt6.Icon = Projector.Properties.Resources.database;
            rBt6.CodeInsert = "NEW ReflectionDatabase !MyDb";
            widgets.Add(rBt6);

            ReflectNewWidget rBt7 = new ReflectNewWidget();
            rBt7.Name = "DB Browser";
            rBt7.Icon = Projector.Properties.Resources.database_table;
            rBt7.CodeInsert = "NEW queryBrowser !MyDbBrowser";
            widgets.Add(rBt7);

            ReflectNewWidget rBt8 = new ReflectNewWidget();
            rBt8.Name = "String Worker";
            rBt8.Icon = Projector.Properties.Resources.edit_replace;
            rBt8.CodeInsert = "NEW RString !MyStringWorker";
            widgets.Add(rBt8);

            ReflectNewWidget rBt9 = new ReflectNewWidget();
            rBt9.Name = "ProfileWorker";
            rBt9.Icon = Projector.Properties.Resources.emblem_people;
            rBt9.CodeInsert = "NEW ProfileWorker !MyProfilWorker";
            widgets.Add(rBt9);

            ReflectNewWidget rBt10 = new ReflectNewWidget();
            rBt10.Name = "ListView";
            rBt10.Icon = Projector.Properties.Resources.tag;
            rBt10.CodeInsert = "NEW ListView !MyListView";
            widgets.Add(rBt10);

            ReflectNewWidget rBt11 = new ReflectNewWidget();
            rBt11.Name = "Result List";
            rBt11.Icon = Projector.Properties.Resources.tag;
            rBt11.CodeInsert = "NEW ResultList !MyResult";
            widgets.Add(rBt11);

            return widgets;
        }

        public Hashtable getStoredObjects()
        {
            return this.obReferences;
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

            if (refObject.typeOfObject == "ProfileWorker")
            {
                return this.getProfilWorker(refObject, parent);
            }

            if (refObject.typeOfObject == "Profil")
            {
                return this.getProfil(refObject, parent);
            }

            if (refObject.typeOfObject == "ReflectionDatabase")
            {
                return this.getDatabase(refObject, parent);
            }

            if (refObject.typeOfObject == "RString")
            {
                return this.getRString(refObject, parent);
            }

            if (refObject.typeOfObject == "PrConsole")
            {
                return this.getConsole(refObject, parent);
            }

            if (refObject.typeOfObject == "ResultList")
            {
                return this.getResultList(refObject, parent);
            }
            return null;
        }

        private ReflectionDatabase getDatabase(ReflectionScriptDefines refObject, Object parent)
        {
            return new ReflectionDatabase();
        }

        private RString getRString(ReflectionScriptDefines refObject, Object parent)
        {
            return new RString();
        }

        private Profil getProfil(ReflectionScriptDefines refObject, Object parent)
        {
            return new Profil();
        }

        private ProfileWorker getProfilWorker(ReflectionScriptDefines refObject, Object parent)
        {
            return new ProfileWorker();
        }

        private ResultList getResultList(ReflectionScriptDefines refObject, Object parent)
        {
            return new ResultList();
        }
      

        /// <summary>
        /// add an Controll to a parent Object depending of the type of parent
        /// </summary>
        /// <param name="addThis">The Controll that will be added</param>
        /// <param name="refObject">Current ReflectionScript Object</param>
        /// <param name="parent">Parent Object</param>
        /// <returns>The object that is added</returns>
        private Object NewControlObject(Control addThis, ReflectionScriptDefines refObject, Object parent)
        {
            addThis.Name = refObject.name;
            if (parent is Form)
            {
                Form parForm = (Form)parent;
                parForm.Controls.Add(addThis);
            }
            return addThis;
        }

        private PrConsole getConsole(ReflectionScriptDefines refObject, Object parent)
        {
            PrConsole newTextBox = new PrConsole();
            newTextBox = (PrConsole)NewControlObject(newTextBox, refObject, parent);
            return newTextBox;
        }


        private ReflectButton getButton(ReflectionScriptDefines refObject, Object parent)
        {
            ReflectButton newTextBox = new ReflectButton();
            newTextBox = (ReflectButton)NewControlObject(newTextBox, refObject, parent);
            return newTextBox;
        }

        private LabelText getLabelText(ReflectionScriptDefines refObject, Object parent)
        {
            LabelText newTextBox = new LabelText();
            newTextBox = (LabelText)NewControlObject(newTextBox, refObject, parent);
            return newTextBox;
        }

        private LabelInteger getLabelInt(ReflectionScriptDefines refObject, Object parent)
        {
            LabelInteger newTextBox = new LabelInteger();
            newTextBox = (LabelInteger)NewControlObject(newTextBox, refObject, parent);
            return newTextBox;
        }


        private TextBox getTextBox(ReflectionScriptDefines refObject, Object parent)
        {
            TextBox newTextBox = new TextBox();
            newTextBox = (TextBox)NewControlObject(newTextBox, refObject, parent);
            return newTextBox;
        }

        private ReflectList getListView(ReflectionScriptDefines refObject, Object parent)
        {
            ReflectList newTextBox = new ReflectList();
            newTextBox = (ReflectList)NewControlObject(newTextBox, refObject, parent);
            return newTextBox;
        }


        private ListView getOriginListView(ReflectionScriptDefines refObject, Object parent)
        {
            ListView resultView = new ListView();
            resultView.Name = refObject.name;

            resultView.View = View.Details;                        
            resultView.FullRowSelect = true;
            resultView.GridLines = true;

            return resultView;
        }

        private NumericUpDown getNumeric(ReflectionScriptDefines refObject, Object parent)
        {
            NumericUpDown newTextBox = new NumericUpDown();
            newTextBox = (NumericUpDown)NewControlObject(newTextBox, refObject, parent);
            return newTextBox;
        }

        private DateSelect getDatePick(ReflectionScriptDefines refObject, Object parent)
        {
            DateSelect newTextBox = new DateSelect();
            newTextBox = (DateSelect)NewControlObject(newTextBox, refObject, parent);
            return newTextBox;
        }

        private ReflectForm getForm(ReflectionScriptDefines refObject, Object parent)
        {

            ReflectForm newForm;

            newForm = (ReflectForm)this.getExistingObject(refObject.name);
            string objToken = refObject.name;
            if (newForm == null)
            {
                newForm = new ReflectForm();
            }
            newForm.ScriptIdent = refObject.name;
            newForm.Name = objToken;
            if (parent is MdiForm)
            {
                MdiForm mdi = (MdiForm)parent;
                mdi.addSubWindow(newForm,refObject.name);
            }
            else if (parent is Form)
            {
                this.showRForm(newForm, refObject.name);
            }
            else if (parent is ReflectionScript)
            {
                ReflectionScript parScr = (ReflectionScript)parent;
                if (parScr.SetupBoolValue(ReflectionScript.SETUP_PREVIEW))
                {
                    this.showRForm(newForm, refObject.name);
                }
            }
            else
            {
                this.showRForm(newForm, refObject.name);
            }
            this.addObject(refObject.name, newForm);
            return newForm;
        }

        private void showRForm(ReflectForm newForm, string name)
        {
            try
            {
                newForm.Show();
            }
            catch (ObjectDisposedException ex)
            {
                newForm = new ReflectForm();
                newForm.ScriptIdent = name;
                newForm.Name = name;
                newForm.Show();
                this.addObject(name, newForm, true);
            }
        }


        private GroupQuery getGroupQuery(ReflectionScriptDefines refObject, Object parent)
        {

            if (this.obReferences.ContainsKey(refObject.name))
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
                        this.obReferences.Remove(refObject.name);
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

            if (!this.obReferences.ContainsKey(refObject.name))
            {
                this.obReferences.Add(refObject.name, grQuery);
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

        private void addObject(string name, Object obj)
        {
            this.addObject(name, obj, false);
        }

        private void addObject(string name,Object obj, Boolean ForceUpdate)
        {
            if (this.useObjectStorage)
            {
                if (ForceUpdate && this.obReferences.ContainsKey(name))
                {
                    this.obReferences.Remove(name);
                }

                if (!this.obReferences.ContainsKey(name))
                {
                    this.obReferences.Add(name, obj);
                }
            }
        }

        private Object getExistingObject(string name)
        {
            if (this.obReferences.ContainsKey(name))
            {
                return this.obReferences[name];
            }
            return null;
        }
    }
}
