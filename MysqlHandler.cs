using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
//using MySql.Data.MySqlClient;


namespace Projector
{
    class MysqlHandler
    {
        MySql.Data.MySqlClient.MySqlConnection connection;
        private string myConnectionString = "";
        Profil currentProfil = null;
        string userName = null;
        string userPassword = null;
        string host = null;
        string table = null;
        string lastSql = "";

        List<string> tableList = null;
        List<MysqlStruct> structList = null;
        List<string> errorMessages = new List<string>();

        private string versionInfo = "NONE";

        public string lastSqlErrorMessage = "";
        public int lastErrorCode = 0; 
        public bool validConnectionSetup = false;
        public bool listViewColumAddRowInfo = false;
        public int listViewAddImageIndex = 1;

        private Int64 allowedpacketSize = -1;

        bool connected = false;
        /**
         * init database handler by seperate
         * information
         * no auto connect
         * 
         **/ 
       /* public MysqlHandler(string username,string password,string host, string table)
        {
            this.userName = username;
            this.userPassword = password;
            this.host = host;
            this.table = table;
            getConnStr();
            this.connection = new MySql.Data.MySqlClient.MySqlConnection();
            
        }*/

        /**
         * init databasehandler with information from profile
         */ 
        public MysqlHandler(Profil dbProfil)
        {
            this.currentProfil = dbProfil;
            this.userName =dbProfil.getProperty("db_username");
            this.userPassword = dbProfil.getProperty("db_password");
            this.host = dbProfil.getProperty("db_host");
            this.table =  dbProfil.getProperty("db_schema");
            getConnStr();
            this.connection = new MySql.Data.MySqlClient.MySqlConnection();
            
        }

        /**
         * buld the connectionstring for mysql/NET
         * if all needed information exits
         */ 
        public string getConnStr()
        {
            if (this.host != null && this.userName != null && this.userPassword != null && this.table != null &&
                this.host.Length > 0 && this.userName.Length > 0 && this.userPassword.Length > 0 && this.table.Length > 0)
            {
                myConnectionString = "server=" + this.host +
                       ";uid=" + this.userName +
                       ";pwd=" + this.userPassword +
                       ";database=" + this.table +
                       ";Treat Tiny As Boolean = false;charset=utf8";

                if (this.currentProfil.getProperty("foreign_key_check") == "1")
                {

                }

                this.validConnectionSetup = true;
                return myConnectionString;
            }
            else
            {
                myConnectionString = null;
                this.validConnectionSetup = false;
                return null;
            }
        }

        /**
         * connect the database.
         * if allready connected then
         * first the connection will be disconnected
         * and reconnected
         */ 
        public void connect()
        {

            if (this.isConnected())
            {
                this.disConnect();
            }

            if (myConnectionString != null)
            {
                try
                {
                    this.connection.ConnectionString = myConnectionString;
                    this.connection.Open();
                    
                    this.connected = true;

                    if (this.currentProfil.getProperty("foreign_key_check") == "1")
                    {
                        MySql.Data.MySqlClient.MySqlDataReader md = this.sql_select("SET FOREIGN_KEY_CHECKS = 0");
                        md.Close();
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    this.lastSqlErrorMessage = ex.Message;
                    this.lastErrorCode = ex.ErrorCode;
                    this.connected = false;
                }
            }
            else
            {
                this.lastSqlErrorMessage = Projector.Properties.Resources.ErrorMysqlNoProperties;
            }
        }

        /**
         * close connection
         */ 
        public void disConnect()
        {
            this.connection.Close();
            this.connected = false;
        }
        // ------------ transactions--------------------

        public void beginTransaction()
        {
            this.sql_select("SET autocommit=0");
            this.sql_select("START TRANSACTION");
        }

        public void rollBack()
        {
            
            this.sql_select("ROLLBACK");
            this.sql_select("SET autocommit=1");
        }

        public void commit()
        {

            this.sql_select("COMMIT");
            this.sql_select("SET autocommit=1");
        }

        // ------ states --------------------------------

        public bool isConnected()
        {
            return this.connected;
        }

        // ----- sql statements -------------------------

        /**
         * reads first row and returns the conte as string list
         */ 
        public List<string> selectFirstAsList(string sql)
        {
            List<string> Data = new List<string>();
            MySql.Data.MySqlClient.MySqlDataReader reader = this.sql_select(sql);
            if (reader!=null)
            {
                while (reader.Read())
                {
                    string dataStr = reader.GetString(0);
                    Data.Add(dataStr);
                }
                reader.Close();
            }            
            return Data;
        }

        /**
         * returns the mysql version
         * by asking mysql
         */ 
        public string getMysqlVersion()
        {
            if (!this.isConnected()) this.connect();
            return this.connection.ServerVersion.ToString();
            /*
            if (this.versionInfo == "NONE" && this.isConnected())
            {
                MySql.Data.MySqlClient.MySqlDataReader data = this.sql_select("SELECT version()");
                data.Read();
                this.versionInfo = data.GetString(0);
            }
            return this.versionInfo;
             */
        }

        /**
         * returns mysql result as hashtable
         */ 
        public List<Hashtable> selectAsHash(string sql)
        {
            
            List<Hashtable> Data = new List<Hashtable>();             
            MySql.Data.MySqlClient.MySqlDataReader reader = this.sql_select(sql);
            if (reader != null)
            {
                
                while (reader.Read())
                {
                    Hashtable result = new Hashtable();        
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string dataStr = null;
                        try
                        {
                            dataStr = reader.GetString(i);
                        }
                        catch (Exception)
                        {
                            
                            
                        }
                        string column = reader.GetName(i);
                        result.Add(column, dataStr);
                    }
                    Data.Add(result);    
                }
                
                reader.Close();
            }

            return Data;
        }

        /**
         * returns the content from one named field (by fieldname)
         * as String list
         */ 
        public List<string> selectAsList(string sql, string fieldname)
        {
            List<string> Data = new List<string>();
            MySql.Data.MySqlClient.MySqlDataReader reader = this.sql_select(sql);
            if (reader != null)
            {
                while (reader.Read())
                {
                    string dataStr = reader.GetString(reader.GetOrdinal(fieldname));
                    Data.Add(dataStr);
                }
                reader.Close();
            }

            return Data;
        }

        /**
         * mapping method
         */ 
        public MySql.Data.MySqlClient.MySqlDataReader sql_select(string sql)
        {
            if (this.connected) return this.sql_select(sql, this.connection);
            else
            {
                this.lastSqlErrorMessage = Projector.Properties.Resources.ErrorMysqlNotConnected;
                return null;
            }
        }

        /**
         * fire aql query and try to read result.
         * using MySqlDataReader as result...
         */
        public MySql.Data.MySqlClient.MySqlDataReader sql_select(string sql, MySql.Data.MySqlClient.MySqlConnection use_connect)
        {
            if (sql.Length > 1)
            {
                if (this.connected)
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
                    MySql.Data.MySqlClient.MySqlDataReader myData;
                    this.lastSqlErrorMessage = "";
    
                    cmd.Connection = use_connect;



                    try
                    {
                        cmd.CommandText = sql;
                        myData = cmd.ExecuteReader();
                        this.lastSql = sql;
                        return myData;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        this.lastErrorCode = ex.ErrorCode;
                        this.lastSqlErrorMessage = "Error " + ex.Number + " has occurred: " + ex.Message;
                        errorMessages.Add(this.lastSqlErrorMessage);
                        return null;
                    }
                    catch (ArgumentOutOfRangeException arex)
                    {
                        
                        this.lastSqlErrorMessage = "Argument Exception has occurred: " + arex.Message + " " + arex.ParamName;
                        errorMessages.Add(arex.Message);
                        return null;
                    }
                }
                else
                {
                    this.lastSqlErrorMessage = Projector.Properties.Resources.ErrorMysqlNotConnected;
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /**
         * mapped
         */ 
        public Int64 getMaxAllowPacketSize()
        {
            return getMaxAllowPacketSize(false);
        }

        /**
         * returns the maximum of possible query size
         */
        public Int64 getMaxAllowPacketSize(bool forceRead)
        {
            bool disConnecting = false;
            if (this.allowedpacketSize == -1 || forceRead)
            {

                if (!this.isConnected())
                {
                    this.connect();
                    disConnecting = true;
                }

                string sql = "select @@max_allowed_packet";
                MySql.Data.MySqlClient.MySqlDataReader myData = this.sql_select(sql);
                Int64 size = -1;
                if (myData != null)
                {
                    while (myData.Read())
                    {
                        size = myData.GetInt64(0);

                    }
                    myData.Close();
                }
                if (disConnecting) this.disConnect();
                this.allowedpacketSize = size;
                return size;
            }
            else
            {
                return this.allowedpacketSize;
            }
            
        }


        public Int64 sql_update(string sql)
        {
            if (this.connected) return this.sql_update(sql, this.connection);
            else
            {
                this.lastSqlErrorMessage = Projector.Properties.Resources.ErrorMysqlNotConnected;
                return 0;
            }
        }

        public Int64 sql_update(string sql, MySql.Data.MySqlClient.MySqlConnection use_connect)
        {
            if (this.connected)
            {
                Int64 ret = 0;
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();

                cmd.Connection = use_connect;
                try
                {
                    cmd.CommandText = sql;
                    
                    ret = cmd.ExecuteNonQuery();
                    
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    this.lastErrorCode = ex.ErrorCode;
                    this.lastSqlErrorMessage = "UpdateError " + ex.Number + " has occurred: " + ex.Message;
                    return 0;
                }
                return ret;
            }
            else
            {
                this.lastSqlErrorMessage = Projector.Properties.Resources.ErrorMysqlNotConnected;
                return 0;
            }
        }


        public Int64 sql_procedureUpdate(string sql)
        {
            if (this.connected) return this.sql_procedureUpdate(sql, this.connection);
            else
            {
                this.lastSqlErrorMessage = Projector.Properties.Resources.ErrorMysqlNotConnected;
                return 0;
            }
        }


        public Int64 sql_procedureUpdate(string sql, MySql.Data.MySqlClient.MySqlConnection use_connect)
        {
            if (this.connected)
            {
                Int64 ret = 0;
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();

                cmd.Connection = use_connect;
                try
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    ret = cmd.ExecuteNonQuery();

                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    this.lastErrorCode = ex.ErrorCode;
                    this.lastSqlErrorMessage = "UpdateError " + ex.Number + " has occurred: " + ex.Message;
                    return 0;
                }
                return ret;
            }
            else
            {
                this.lastSqlErrorMessage = Projector.Properties.Resources.ErrorMysqlNotConnected;
                return 0;
            }
        }


        // ------------  tools -----------------------------------

        


        public void resetTableList()
        {
            if (tableList!=null) tableList.Clear();
        }

        private void readTableList()
        {
            readTableList(true);
        }

        public List<string> getTableList()
        {
            readTableList();
            return tableList;
        }

        private void readTableList(Boolean reRead)
        {

            
            if (tableList==null || tableList.Count<1)
            {
                tableList = new List<string>();
                string sql = "SHOW TABLES";
                string result = "";
                if (this.connected)
                {
                    MySql.Data.MySqlClient.MySqlDataReader res = sql_select(sql);
                    if (res !=null)
                    {
                        while (res.Read())
                        {
                            result = res.GetString(0);
                            tableList.Add(result);
                        }
                        res.Close();
                    }
                }
            }
            
        }

        public string getReaderAsString(string field, MySql.Data.MySqlClient.MySqlDataReader res)
        {            
            try
            {
                return res.GetString(res.GetOrdinal(field));
            }
            catch (Exception)
            {
                return "";
            }
        }

        public bool structIsEqual(List<MysqlStruct> source, List<MysqlStruct> target)
        {            
            if (source.Count!=target.Count) return  false;
            for (int i = 0; i < source.Count; i++)
            {
                 if (!source[i].isEqualTo(target[i])) return false;
            }
            return true;
        }


        public List<MysqlStruct> getAllFieldsStruct(string tableName)
        {
            List<MysqlStruct> AllStructs = new List<MysqlStruct>();
            getTableStruct(tableName);

            for (int i = 0; i < structList.Count; i++)
            {
                if (structList[i].tableName == tableName)
                {
                    AllStructs.Add(structList[i]);
                }
            }
            return AllStructs;
        }

        private bool structInfoExists(string tableName,string fieldname)
        {
            if (structList == null || structList.Count < 1)
            {
                structList = new List<MysqlStruct>();
                return false;
            }
            for (int i = 0; i < structList.Count; i++) 
                if (structList[i].tableName == tableName && structList[i].name == fieldname) return true;
            return false;
        }

        public MysqlStruct getStruct(string tableName, string field)
        {
            if (structList == null || structList.Count < 1)
            {
                structList = new List<MysqlStruct>();

            }
            for (int i = 0; i < structList.Count; i++) 
                 if (structList[i].tableName == tableName && structList[i].name== field ) return structList[i];

            getTableStruct(tableName);

            for (int i = 0; i < structList.Count; i++)
                if (structList[i].tableName == tableName && structList[i].name == field) return structList[i];

            return null;
            
        }


        public void getTableStruct(string tableName)
        {

            
            if (structList == null || structList.Count < 1)
            {
                structList = new List<MysqlStruct>();
                
            }

            string sql = "SHOW COLUMNS FROM "+ tableName;

            if (!this.connected)
            {
                this.connect();
            }
            
            if (this.connected)
            {
                MySql.Data.MySqlClient.MySqlDataReader res = sql_select(sql);
                if (res != null)
                {
                    while (res.Read())
                    {
                        MysqlStruct result = new MysqlStruct();
                        result.tableName = tableName;
                        result.name = getReaderAsString("Field",res);
                        result.type = getReaderAsString("Type",res);
                        result.Null = getReaderAsString("Null", res);
                        result.Key = getReaderAsString("Key", res);
                        result.Default = getReaderAsString("Default", res);
                        result.Extra = getReaderAsString("Extra", res);
                        result.parseType();
                        if (!structInfoExists(tableName, result.name)) structList.Add(result);
                    }

                    res.Close();
                }
            }
            

        }

        public bool tableExists(string tableName)
        {
            readTableList();
            return (tableList.Contains(tableName));
        }

        public string getTableCreationString(string tableName)
        {
            string sql = "SHOW CREATE TABLE " + tableName;
            string result = "";
            if (this.connected)
            {
                MySql.Data.MySqlClient.MySqlDataReader res = sql_select(sql);
                if (res!=null && res.HasRows)
                {
                    while (res.Read()){
                        result = res.GetString(1);
                    }
                    res.Close();
                }
            }
            return result;

        }

        public void sql_data2ListView(MySql.Data.MySqlClient.MySqlDataReader myData, ListView toList)
        {

            if (this.connected) this.sql_data2ListView(myData, toList, (toList.Columns.Count < 1));
            else this.lastSqlErrorMessage = Projector.Properties.Resources.ErrorMysqlNotConnected;
            
        }


        private string zeroVal(int val)
        {
            if (val < 10) return "0" + val;
            else return "" + val;
        }

        public string getMysqlValue(MySql.Data.MySqlClient.MySqlDataReader Reader, int FieldNumber)
        {

            if (Reader == null) return null;
            if (Reader.FieldCount <= FieldNumber) return null;

            System.Type fType = Reader.GetFieldType(FieldNumber);
            Object chk = null;
            int i = FieldNumber;
            try
            {
                chk = Reader.GetValue(i);
            }
            catch (Exception)
            {

                //throw;
            }
            
            string returnValue = "null";

            if (chk != null)
            {
                int hash = chk.GetHashCode();
                string dd = chk.ToString();
                if (chk != null && dd != "")
                {
                    switch (fType.Name)
                    {
                        case "Int32":
                        case "UInt32":
                        case "Int64":
                        case "Boolean": 
                        case "UInt64":
                            Int64 val = Reader.GetInt64(i);
                            returnValue = "" + val;
                            break;
                        case "Single":
                        case "Float":
                            float fVal = Reader.GetFloat(i);
                            returnValue =  fVal.ToString().Replace(',', '.') ;
                            break;

                        case "Double":
                            double dVal = Reader.GetDouble(i);
                            returnValue = dVal.ToString().Replace(',', '.') + "";
                            break;
                        case "DateTime":
                            DateTime dt = Reader.GetDateTime(i);
                            String dtValue = zeroVal(dt.Year) + "-" + zeroVal(dt.Month) + "-" + zeroVal(dt.Day) + " " + dt.TimeOfDay;
                            returnValue = dtValue;
                            break;
                        default:
                            string sval = Reader.GetString(i).Replace("'", @"\'");

                            returnValue = sval;
                            break;
                    }

                    
                }
            }
            return returnValue;
        }

        public List<string> getErrorMessages()
        {
            return errorMessages;
        }

        public void resetErrorMessages()
        {
            errorMessages.Clear();
        }

        public string fieldInfo(MySql.Data.MySqlClient.MySqlDataReader Reader, int FieldNumber)
        {
            if (Reader == null) return null;
            if (Reader.FieldCount <= FieldNumber) return null;

            System.Type fType = Reader.GetFieldType(FieldNumber);
            Object chk = null;
            int i = FieldNumber;
            try
            {
                chk = Reader.GetValue(i);
            }
            catch (Exception)
            {

                //throw;
            }
            
            string returnValue = "null";

            if (chk != null)
            {
                int hash = chk.GetHashCode();
                string dd = chk.ToString();
                if (chk != null && dd != "")
                {
                }
            }
            return returnValue;
        }

        /**
         * Fill the items and comlums from a exists Listview (toList) width a result from
         * MysqlDataReader
         */
        public void sql_data2ListView(MySql.Data.MySqlClient.MySqlDataReader myData, ListView toList, bool reDrawColums)
        {
            if (this.connected && myData!=null && myData.HasRows)
            {
                
                int fieldCounts = myData.FieldCount;
                toList.Items.Clear();
                if (reDrawColums)
                {

                    toList.Columns.Clear();
                    for (int i = 0; i < fieldCounts; i++)
                    {

                        String fieldKeyInfo = fieldInfo(myData,i);

                        String fname = myData.GetDataTypeName(i);                                           
                        
                        String fFullname = myData.GetName(i);
                        ColumnHeader tmp = new ColumnHeader();
                        tmp.Text = fFullname;
                        tmp.Name = fFullname;
                        if (this.listViewColumAddRowInfo) {                            
                            tmp.Text += " (" + fname + ")";
                            
                            
                        }
                        
                        
                        toList.Columns.Add(tmp);
                    }
                }
                while (myData.Read())
                {
                    ListViewItem item = new ListViewItem();                    
                    String fname = myData.GetDataTypeName(0);

                    try
                    {
                        item.Text = myData.GetString(0);
                        item.Name = myData.GetName(0);
                    }
                    catch (Exception)
                    {
                        item.Text = "null";
                    }

                    for (int i = 0; i < fieldCounts; i++)
                    {
                        string add;

                        add = "?";

                        ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();
                        add = getMysqlValue(myData, i);

                        subItem.Text = add;
                        subItem.Name = myData.GetName(i);

                        if (i > 0) item.SubItems.Add(subItem);
                        else item.Text = add;
                    }
                    toList.Items.Add(item);


                }
                if (myData!=null) myData.Close();
            }
            else
            {
                this.lastSqlErrorMessage = Projector.Properties.Resources.ErrorMysqlNotConnected;
            }
        }

       

    }
}
