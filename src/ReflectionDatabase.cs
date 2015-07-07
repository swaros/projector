using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Projector.Script
{
    public class ReflectionDatabase
    {
        public string name;

        private ProfileWorker pWorker = new ProfileWorker();

        private string iterationVaribaleName;
        private string iterationVaribaleValue;
        private string errorVariableName;

        private ReflectionScript onIterationScr;
        private ReflectionScript onRowIterationScr;
        private ReflectionScript onErrorScr;
        private ReflectionScript onDoneScr;
        private ReflectionScript onEmptyScr;

        public List<Hashtable> lastResult;

        private BackgroundWorker worker = new BackgroundWorker();

        public ReflectionDatabase() {
            this.initWorker();
        }   

        private void initWorker()
        {
            worker.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);
            worker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            worker.ProgressChanged +=
                new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

            worker.WorkerReportsProgress = true;
        }


        public void startAsync(string profilName, string query)
        {
            Profil usedProfil = pWorker.getProfilbyName(profilName);
            if (usedProfil != null)
            {                
                DatabaseAsyncParam data = new DatabaseAsyncParam();
                data.UsedProfil = usedProfil;
                data.FieldName = this.iterationVaribaleName;
                data.ValueName = this.iterationVaribaleValue;
                data.Query = query;
                worker.RunWorkerAsync(data);
            }
             else
            {
                this.scrMysqError(" invalid profil: " + profilName);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            doneDb();
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DatabaseAsyncParam Param = (DatabaseAsyncParam) e.Argument;

            MysqlHandler mysql = new MysqlHandler(Param.UsedProfil);

            mysql.connect();

            List<Hashtable> hashResult = mysql.selectAsHash(Param.Query);
            List<string> errors = mysql.getErrorMessages();
            if (errors.Count > 0)
            {
                foreach (string errMsg in errors)
                {
                    //this.scrMysqError(errMsg);
                    worker.ReportProgress(0,errMsg);
                }
            }
            else
            {
                this.lastResult = hashResult;
                foreach (Hashtable row in hashResult)
                {
                    foreach (DictionaryEntry dict in row)
                    {
                        string val;
                        if (dict.Value == null)
                        {
                            val = "";
                        }
                        else
                        {
                            val = dict.Value.ToString();

                        }
                        //this.iteration(dict.Key.ToString(), val);
                        worker.ReportProgress(1, new string[]{dict.Key.ToString(), val});
                    }
                }
            }
            mysql.disConnect();
        }

        public ListView getListViewFromDb(string profilName, string query)
        {
            ListView resultView = new ListView();
            Profil usedProfil = pWorker.getProfilbyName(profilName);
            if (usedProfil != null)
            {
                MysqlHandler mysql = new MysqlHandler(usedProfil);

                mysql.connect();
                MySql.Data.MySqlClient.MySqlDataReader dbResult;
                dbResult = mysql.sql_select(query);
                if (dbResult != null && dbResult.HasRows)
                {
                    mysql.sql_data2ListView(dbResult, resultView);
                }

                List<string> errors = mysql.getErrorMessages();
                if (errors.Count > 0)
                {
                    foreach (string errMsg in errors)
                    {
                        this.scrMysqError(errMsg);
                    }
                }
                mysql.disConnect();
                doneDb();
            }
            else
            {
                this.scrMysqError(" invalid profil: " + profilName);
            }
            return resultView;
        }

        
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                if (e.ProgressPercentage == 0)
                {
                    string msg = (string) e.UserState;
                    this.scrMysqError(msg);
                }

                if (e.ProgressPercentage == 1)
                {
                    string[] obData = (string[])e.UserState;
                    this.iteration(obData[0], obData[1]);
                }

            }
        }

        public void sendQuery(string profilName, string query)
        {
            Profil usedProfil = pWorker.getProfilbyName(profilName);
            if (usedProfil != null)
            {
                MysqlHandler mysql = new MysqlHandler(usedProfil);

                mysql.connect();

                mysql.sql_select(query);
                List<string> errors = mysql.getErrorMessages();
                if (errors.Count > 0)
                {
                    foreach (string errMsg in errors)
                    {
                        this.scrMysqError(errMsg);
                    }
                }
                mysql.disConnect();
                doneDb();
            }
            else
            {
                this.scrMysqError(" invalid profil: " + profilName);
            }

        }


        public void startReading(string profilName, string query)
        {
            Profil usedProfil = pWorker.getProfilbyName(profilName);
            if (usedProfil != null)
            {
                MysqlHandler mysql = new MysqlHandler(usedProfil);

                mysql.connect();

                List<Hashtable> hashResult = mysql.selectAsHash(query);
                List<string> errors = mysql.getErrorMessages();
                if (errors.Count > 0)
                {
                    foreach (string errMsg in errors)
                    {
                        this.scrMysqError(errMsg);
                    }
                }
                else
                {
                    if (hashResult.Count > 0)
                    {
                        foreach (Hashtable row in hashResult)
                        {
                            this.rowIteration(row);
                            foreach (DictionaryEntry dict in row)
                            {

                                string val;
                                if (dict.Value == null)
                                {
                                    val = "";
                                }
                                else
                                {
                                    val = dict.Value.ToString();

                                }
                                this.iteration(dict.Key.ToString(), val);
                            }
                        }
                    }
                    else
                    {
                        this.onEmpty();
                    }
                }
                mysql.disConnect();
                doneDb();
            }
            else
            {
                this.scrMysqError(" invalid profil: " + profilName);
            }

        }


        private void iteration(string fieldname, string value)
        {
            if (this.onIterationScr != null)
            {
                this.onIterationScr.createOrUpdateStringVar("&" + this.iterationVaribaleName, fieldname);
                this.onIterationScr.createOrUpdateStringVar("&" + this.iterationVaribaleValue, value);
                RefScriptExecute exec = new RefScriptExecute(this.onIterationScr, this);
                exec.run();
            }
        }

        private void rowIteration(Hashtable hashResult)
        {
            if (onRowIterationScr != null)
            {
                int number = 0;
                foreach (DictionaryEntry row in hashResult)
                {
                    
                    string headName = row.Key.ToString();
                    string val = "";
                    if (row.Value != null)
                         val = row.Value.ToString();

                    this.onRowIterationScr.createOrUpdateStringVar("&" + headName, val);
                    number++;
                    this.onRowIterationScr.createOrUpdateStringVar("&ROW." + number, val);
                }
                RefScriptExecute exec = new RefScriptExecute(this.onRowIterationScr, this);
                exec.run();
            }
        }


        private void scrMysqError(string message)
        {
            if (this.onErrorScr != null)
            {
                this.onErrorScr.createOrUpdateStringVar("&" + this.errorVariableName, message);
                RefScriptExecute exec = new RefScriptExecute(this.onErrorScr, this);
                exec.run();
            }
            
        }

        private void doneDb()
        {
            if (this.onDoneScr != null)
            {

                RefScriptExecute exec = new RefScriptExecute(this.onDoneScr, this);
                exec.run();
            }
        }

        private void onEmpty()
        {
            if (this.onEmptyScr != null)
            {

                RefScriptExecute exec = new RefScriptExecute(this.onEmptyScr, this);
                exec.run();
            }
        }

        public void OnEmptyResult(ReflectionScript scr)
        {
            this.onEmptyScr = scr;
        }


        public void OnRow(ReflectionScript scr)
        {
            this.onRowIterationScr = scr;
        }

        public void OnIteration(String iterationName, string iterationValue, ReflectionScript scr)
        {
            this.iterationVaribaleName = iterationName;
            this.iterationVaribaleValue = iterationValue;
            this.onIterationScr = scr;
        }

        public void OnError(String messagevar, ReflectionScript scr)
        {
            this.errorVariableName = messagevar;
            this.onErrorScr = scr;
        }

        public void OnDone(ReflectionScript scr)
        {
            this.onDoneScr = scr;
        }

    }
}
