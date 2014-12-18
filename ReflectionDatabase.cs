using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{
    class ReflectionDatabase
    {
        public string name;

        private ProfileWorker pWorker = new ProfileWorker();

        private string iterationVaribaleName;
        private string iterationVaribaleValue;
        private string errorVariableName;

        private ReflectionScript onIterationScr;
        private ReflectionScript onErrorScr;

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
                            this.iteration(dict.Key.ToString(), val);
                        }
                    }
                }
                mysql.disConnect();
            }
            else
            {
                this.scrMysqError(" inavalid profil: " + profilName);
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

        private void scrMysqError(string message)
        {
            if (this.onErrorScr != null)
            {
                this.onErrorScr.createOrUpdateStringVar("&" + this.errorVariableName, message);
                RefScriptExecute exec = new RefScriptExecute(this.onErrorScr, this);
                exec.run();
            }
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

    }
}
