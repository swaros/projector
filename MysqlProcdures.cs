using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{
    class MysqlProcedures
    {
        private const string readQuery = "SHOW PROCEDURE STATUS";
        private const string readProcStruct = "SHOW CREATE PROCEDURE [NAME]";
        private List<StoredProcedure> spList = new List<StoredProcedure>();
        //private Hashtable storedProcedures = new Hashtable();

        public bool isLoaded = false;
        public int count = 0;
        private int current = 0;

        public bool isError = false;
        public string errorMessage = "";

        /**
         * get next value of stored procedures
         */
        public StoredProcedure get()
        {
            if (current<count) {
                StoredProcedure tmp = spList[current];
                current++;

                return tmp;
            } else {
                return null;
            }
        }

        public void rewind()
        {
            current = 0;
        }

        public StoredProcedure getProcByName(string name)
        {
            for (int i = 0; i < spList.Count; i++)
            {
                if (spList[i].name == name) return spList[i];
            }
            return null;
        }


        /**
         * get the procedures index
         */
        public void getProcedures(MysqlHandler database)
        {
            isError = false;
            errorMessage = "";
            bool closeConnection = false;
            if (!database.isConnected())
            {
                closeConnection = true;
                database.connect();
            }

            if (database.isConnected())
            {
               // MySql.Data.MySqlClient.MySqlDataReader data = database.sql_select(MysqlProcedures.readQuery);
                List<Hashtable> data = database.selectAsHash(MysqlProcedures.readQuery);

                if (database.lastSqlErrorMessage != "")
                {
                    isError = true;
                    errorMessage = database.lastSqlErrorMessage;
                    return;
                }

                spList.Clear();
                count = 0;
                current = 0;
                isLoaded = false;
                for (int i = 0; i < data.Count; i++)
                {
                    Hashtable tmpData = data[i];
                    StoredProcedure tmpStore = new StoredProcedure();
                    count++;
                    isLoaded = true;
                    if (tmpData["Db"] != null) tmpStore.db = tmpData["Db"].ToString();
                    if (tmpData["Name"] != null) tmpStore.name = tmpData["Name"].ToString();
                    if (tmpData["Type"] != null) tmpStore.type = tmpData["Type"].ToString();
                    if (tmpData["Definer"] != null) tmpStore.definer = tmpData["Definer"].ToString();
                    if (tmpData["Modified"] != null) tmpStore.modified = tmpData["Modified"].ToString();
                    if (tmpData["Created"] != null) tmpStore.created = tmpData["Created"].ToString();
                    if (tmpData["Security_type"] != null) tmpStore.securityType = tmpData["Security_type"].ToString();
                    if (tmpData["Comment"] != null) tmpStore.created = tmpData["Comment"].ToString();
                    getCreate(tmpStore, database);
                    spList.Add(tmpStore);
                }


            }
            else
            {
                throw new Exception("MysqlProcedures: can not connect to Database");
            }

            if (closeConnection) database.disConnect();
        }

        private void getCreate(StoredProcedure proc, MysqlHandler database)
        {
            if (database.isConnected())
            {
                MySql.Data.MySqlClient.MySqlDataReader res = database.sql_select(MysqlProcedures.readProcStruct.Replace("[NAME]",proc.name));

                if (res != null)
                {
                    while (res.Read() && res.Depth > 0)
                    {
                        string getResult = res.GetString(2);
                        if (null != getResult)
                        {
                            proc.created = getResult;
                            res.Close();
                        }
                        return;
                    }
                    res.Close();
                }

            }
            else
            {
                throw new Exception("Database must be connected");
            }
        }

    }
}
