using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{
    /// <summary>
    /// this Class get informations about stored procedures
    /// that already stored in the Database
    /// </summary>
    class MysqlProcedures
    {
        /// <summary>
        /// Query to Reads all stored procedures
        /// </summary>
        private const string readQuery = "SHOW PROCEDURE STATUS";

        /// <summary>
        /// Query to get more detailed informations about a specific procedure.
        /// name Placeholder [NAME] have to replaced by the name of the procedure
        /// </summary>
        private const string readProcStruct = "SHOW CREATE PROCEDURE [NAME]";

        /// <summary>
        /// position for the creation sql in the result
        /// </summary>
        private const int createCodeIndex = 2;

        /// <summary>
        /// Storage of alle Procedures
        /// </summary>
        private List<StoredProcedure> spList = new List<StoredProcedure>();        

        /// <summary>
        /// Flag is the initialreading is dome
        /// </summary>
        public bool isLoaded = false;

        /// <summary>
        /// count of stored procedures
        /// </summary>
        public int count = 0;

        /// <summary>
        /// The current reade position
        /// </summary>
        private int current = 0;

        /// <summary>
        /// Is true if some error occurs
        /// </summary>
        public bool isError = false;

        /// <summary>
        /// the errormessage from last occured error
        /// </summary>
        public string errorMessage = "";

        /// <summary>
        /// Gets the current selected Procedure
        /// </summary>
        /// <returns>Detailed informations about the current Stored Procedure</returns>
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

        /// <summary>
        /// resets the reader
        /// </summary>
        public void rewind()
        {
            current = 0;
        }

        /// <summary>
        /// Returns a stored Procedure by the Name
        /// </summary>
        /// <param name="name">Name of the Procedure</param>
        /// <returns></returns>
        public StoredProcedure getProcByName(string name)
        {
            for (int i = 0; i < spList.Count; i++)
            {
                if (spList[i].name == name) return spList[i];
            }
            return null;
        }


        /// <summary>
        /// Reads all Procedures and updates 
        /// the content
        /// </summary>
        /// <param name="database">Used database Connection</param>
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

        /// <summary>
        /// gets the creation Query for the given 
        /// Stored procedure
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="database"></param>
        private void getCreate(StoredProcedure proc, MysqlHandler database)
        {
            if (database.isConnected())
            {
                MySql.Data.MySqlClient.MySqlDataReader res = database.sql_select(MysqlProcedures.readProcStruct.Replace("[NAME]",proc.name));

                if (res != null)
                {
                    while (res.Read() && res.FieldCount > 0)
                    {
                        string getResult;
                        try
                        {
                            getResult = res.GetString(MysqlProcedures.createCodeIndex);
                        }
                        catch (System.Data.SqlTypes.SqlNullValueException nullEx)
                        {
                            getResult = null;
                        }
                        
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
