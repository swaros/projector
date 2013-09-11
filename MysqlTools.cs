using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;


namespace Projector
{
    class MysqlTools
    {

        private Int64 copyRowLimit = 10000;
        public bool showProgress = true;
        public bool addTablename = true;
        public bool useMassInsertQuerys = true;
        prozessMsg msg = new prozessMsg();
        public void copyTable(MysqlHandler connection, string sourceTable, string targetTable)
        {
            bool disconnecting = false;
            string sql = "CREATE TABLE " + targetTable + " SELECT * FROM " + sourceTable;
            if (!connection.isConnected())
            {
                disconnecting = true;
                connection.connect();
            }

            connection.sql_update(sql);

            if (disconnecting) connection.disConnect();


        }

        public static statusResult checkConnection(Profil testProfil)
        {
            statusResult result = new statusResult();
            MysqlHandler TestConnect = new MysqlHandler(testProfil);
            TestConnect.connect();

            if (TestConnect.lastSqlErrorMessage.Length > 0)
            {
                result.message = TestConnect.lastSqlErrorMessage;
                result.status = false;
                result.StatusKey = 0;
                return result;
            }
            else
            {
                TestConnect.disConnect();
                result.message = Projector.Properties.Resources.MysqlConnectionTestSucces; ;
                result.status = true;
                result.StatusKey = 1;
                return result;


            }

        }

        public String alterTableFromSource(MysqlHandler connection, string sourceTable, MysqlHandler targetConnection)
        {
            return alterTableFromSource(connection, sourceTable, targetConnection, false);
        }

        public String alterTableFromSource(MysqlHandler connection, string sourceTable, MysqlHandler targetConnection,Boolean addOnly)
        {
            bool disconnecting = false;
            bool targetDisconnecting = false;

            if (!connection.isConnected())
            {
                disconnecting = true;
                connection.connect();
            }

            if (!targetConnection.isConnected())
            {
                targetDisconnecting = true;
                targetConnection.connect();
            }
            string createSql = connection.getTableCreationString(sourceTable);
            List<MysqlStruct> sourceStruct = connection.getAllFieldsStruct(sourceTable);
            List<MysqlStruct> targetStruct = targetConnection.getAllFieldsStruct(sourceTable);

            String alterSql = getAlterTableSql(sourceStruct, targetStruct,addOnly);
            if (alterSql.Length > 0) alterSql = "ALTER TABLE `" + sourceTable + "` " + alterSql;
            if (disconnecting) connection.disConnect();
            if (targetDisconnecting) targetConnection.disConnect();

            targetConnection.resetTableList();
            return alterSql;
        }

        private String getAlterTableSql(List<MysqlStruct> sourceStruct, List<MysqlStruct> targetStruct)
        {
            return this.getAlterTableSql(sourceStruct, targetStruct, false);
        }

        private String getAlterTableSql(List<MysqlStruct> sourceStruct, List<MysqlStruct> targetStruct,Boolean addOnly)
        {
            //String alterSql = "ALTER TABLE `"++"`";
            String alterSql = "";
            String add = "";
            String keysAdd = "";
            for (int i = 0; i < sourceStruct.Count; i++)
            {
                // comparing on filedname
                String fieldName = sourceStruct[i].name;
                bool foundField = false;
                bool isEqual = false;
                for (int p = 0; p < targetStruct.Count; p++)
                {
                    if (targetStruct[p].name == fieldName)
                    {
                        foundField = true;
                        // compare struct
                        if (!addOnly)
                        {
                            isEqual = sourceStruct[i].isEqualTo(targetStruct[p]);

                            if (!isEqual)
                            {
                                String modify = sourceStruct[i].getStructFieldModifier();
                                alterSql += add + "MODIFY " + modify + Environment.NewLine;
                                add = ",";
                                if (sourceStruct[i].Key == "PRI" && targetStruct[p].Key != "PRI") keysAdd = ", ADD PRIMARY KEY (`" + fieldName + "`)";
                            }
                        }
                    }


                }
                if (!foundField)
                {
                    String addField = sourceStruct[i].getStructFieldModifier();
                    alterSql += add + "ADD " + addField;
                    add = ",";
                    if (sourceStruct[i].Key == "PRI") keysAdd = ", ADD PRIMARY KEY (`" + fieldName + "`)";
                }
            }


            return alterSql + keysAdd;
        }


        public bool copyCreationTable(MysqlHandler connection, string sourceTable, MysqlHandler targetConnection)
        {
            bool disconnecting = false;
            bool targetDisconnecting = false;

            if (!connection.isConnected())
            {
                disconnecting = true;
                connection.connect();
            }

            if (!targetConnection.isConnected())
            {
                targetDisconnecting = true;
                targetConnection.connect();
            }
            string createSql = connection.getTableCreationString(sourceTable);
            if (createSql != "")
            {
                targetConnection.sql_update(createSql);

            }
            if (disconnecting) connection.disConnect();
            if (targetDisconnecting) targetConnection.disConnect();

            targetConnection.resetTableList();
            return (targetConnection.tableExists(sourceTable));
        }

        public void dropTable(MysqlHandler connection, string tableName, bool backup)
        {
            bool disconnecting = false;

            if (!connection.isConnected())
            {
                disconnecting = true;
                connection.connect();
            }

            if (backup)
            {
                dropTable(connection, tableName + "_backup", false);
                copyTable(connection, tableName, tableName + "_backup");

            }

            string sql = "DROP TABLE " + tableName;
            connection.sql_update(sql);

            if (disconnecting) connection.disConnect();
        }

        public List<String> getCopyTableStatements(MysqlHandler connection, string sourceTable, MysqlHandler targetConnection)
        {
            bool disconnecting = false;
            bool targetDisconnecting = false;
            List<String> querys = new List<string>();

            if (!connection.isConnected())
            {
                disconnecting = true;
                connection.connect();
            }

            if (!targetConnection.isConnected())
            {
                targetDisconnecting = true;
                targetConnection.connect();
            }

            targetConnection.lastSqlErrorMessage = "";

            querys.Add("DROP TABLE IF EXISTS " + sourceTable);
            querys.Add(connection.getTableCreationString(sourceTable));

            copyRowLimit = 500;

            Int64 writes = 0;
            Int64 start = 0;
            writes = getCopyStatements(connection, sourceTable, start, targetConnection, querys, useMassInsertQuerys);
            start = writes;
            while (writes != 0)
            {

                if (showProgress)
                {
                    msg.msg.Text = "Block ...." + start + " @ " + sourceTable;
                    msg.Refresh();
                }

                writes = getCopyStatements(connection, sourceTable, start, targetConnection, querys, useMassInsertQuerys);
                start += writes;
            }
            copyRowLimit = 10000;

            if (disconnecting) connection.disConnect();
            if (targetDisconnecting) targetConnection.disConnect(); 

            return querys;


        }


        public void copyTable(MysqlHandler connection, string sourceTable, MysqlHandler targetConnection)
        {
            bool disconnecting = false;
            bool targetDisconnecting = false;

            if (!connection.isConnected())
            {
                disconnecting = true;
                connection.connect();
            }

            if (!targetConnection.isConnected())
            {
                targetDisconnecting = true;
                targetConnection.connect();
            }

            targetConnection.lastSqlErrorMessage = "";

            if (!targetConnection.tableExists(sourceTable))
            {
                string createSql = connection.getTableCreationString(sourceTable);
                targetConnection.sql_update(createSql);
                if (targetConnection.lastSqlErrorMessage.Length > 0)
                {
                    MessageBox.Show(createSql + "\n\n" + targetConnection.lastSqlErrorMessage);
                    targetConnection.lastSqlErrorMessage = "";
                }
                else
                {
                    copyRows(connection, sourceTable, targetConnection);
                }
            }



            if (disconnecting) connection.disConnect();
            if (targetDisconnecting) targetConnection.disConnect();

            targetConnection.resetTableList();

        }


        public string getMysqlDump(MysqlHandler connection, string sourceTable)
        {
            bool disconnecting = false;
            if (!connection.isConnected())
            {
                disconnecting = true;
                connection.connect();
            }
            string ResultStr = connection.getTableCreationString(sourceTable);

            ResultStr += "\n\n";

            List<string> SqlArr = getSqlDumpRows(connection, sourceTable);

            for (int i = 0; i < SqlArr.Count; i++)
            {
                ResultStr += SqlArr[i];
            }

            if (disconnecting) connection.disConnect();
            return ResultStr;
        }







        //******************  PRIVATE *******************

        private void copyRows(MysqlHandler connection, string sourceTable, MysqlHandler targetConnection)
        {

            if (showProgress)
            {
                msg.msg.Text = "Start .... " + sourceTable;
                msg.Show();
                msg.Refresh();
            }
            Int64 writes = 0;
            Int64 start = 0;
            writes = copyValues(connection, sourceTable, start, targetConnection);
            start = writes;
            while (writes != 0)
            {

                if (showProgress)
                {
                    msg.msg.Text = "Block ...." + start + " @ " + sourceTable;
                    msg.Refresh();
                }

                writes = copyValues(connection, sourceTable, start, targetConnection);
                start += writes;
            }

            if (showProgress)
            {
                msg.Close();
            }
        }

        public Int64 getCopyStatements(MysqlHandler connection, string sourceTable, Int64 start, MysqlHandler targetConnection, List<string> queryList, bool asFullInsert)
        {

            Int64 count = 0;
            Int64 refresher = 0;
            List<MysqlStruct> Struct = connection.getAllFieldsStruct(sourceTable);
            
            string addToField = sourceTable + ".";

            if (showProgress && msg.Visible) msg.progressBar.Maximum = (int)this.copyRowLimit + 1;

            if (connection.isConnected())
            {
                MySql.Data.MySqlClient.MySqlDataReader Reader =
                    connection.sql_select("SELECT * FROM " + sourceTable + " LIMIT " + start + "," + this.copyRowLimit);
                if (Reader != null)
                {

                    //if (Reader.RecordsAffected==-1) return 0;
                    bool startRow = true;
                    string fullInsert = "INSERT INTO " + sourceTable;
                    string valuesExport = "";
                    string valuesExportAdd = "";
                    string fields = "";
                    string fieldsAdd = "(";
                    string valuesSeperator = "";
                    while (Reader.Read())
                    {
                        count++;
                        refresher++;
                        string insertSql = "INSERT INTO " + sourceTable + " SET ";

                        

                        string add = "";
                        valuesExportAdd = valuesSeperator + "(";
                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            
                            if (refresher > 1000)
                            {
                                if (showProgress && msg.Visible)
                                {
                                    msg.msg.Text = "get copy statement for Table " + sourceTable + " block: " + start + "/" + count;
                                    msg.progressBar.Value = (int)count;
                                    msg.progressBar.Refresh();
                                    msg.Refresh();
                                    msg.Focus();
                                }
                                refresher = 0;
                            }


                            System.Type fType = Reader.GetFieldType(i);
                            Object chk = null;
                            try
                            {
                                chk = Reader.GetValue(i);
                            }
                            catch (Exception)
                            {

                                //throw;
                            }

                            if (asFullInsert)
                            {
                                if (startRow)
                                {
                                    fields += fieldsAdd + "`" + Struct[i].name + "`";
                                    fieldsAdd = ",";
                                }
                                
                            }

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
                                        case "UInt64":
                                            Int64 val = Reader.GetInt64(i);
                                            insertSql += add + addToField + Struct[i].name + " = " + val;
                                            if (asFullInsert) valuesExport += valuesExportAdd + val;
                                            break;
                                        case "Single":
                                        case "Float":
                                            float fVal = Reader.GetFloat(i);
                                            insertSql += add + addToField + Struct[i].name + " = '" + fVal.ToString().Replace(',', '.') + "'";
                                            if (asFullInsert) valuesExport += valuesExportAdd + "'" + fVal.ToString().Replace(',', '.') + "'";
                                            break;

                                        case "Double":
                                            double dVal = Reader.GetDouble(i);
                                            insertSql += add + addToField + Struct[i].name + " = '" + dVal.ToString().Replace(',', '.') + "'";
                                            if (asFullInsert) valuesExport += valuesExportAdd + "'" + dVal.ToString().Replace(',', '.') + "'";
                                            break;
                                        case "DateTime":
                                            DateTime dt = Reader.GetDateTime(i);
                                            String dtValue = zeroVal(dt.Year) + "-" + zeroVal(dt.Month) + "-" + zeroVal(dt.Day) + " " + dt.TimeOfDay;
                                            insertSql += add + addToField + Struct[i].name + " = '" + dtValue + "'";
                                            if (asFullInsert) valuesExport += valuesExportAdd + "'" + dtValue + "'";
                                            break;
                                        case "Boolean":
                                            bool bval = Reader.GetBoolean(i);
                                            if (bval) insertSql += add + addToField + Struct[i].name + " = 1";
                                            else insertSql += add + addToField + Struct[i].name + " = 0";
                                            if (asFullInsert)
                                            {
                                                if (bval) valuesExport += valuesExportAdd + "1";
                                                else valuesExport += valuesExportAdd + "0";
                                            }
                                            break;
                                        default:

                                            string stringVal = Reader.GetString(i);

                                            /*
                                            if (stringVal.Contains(@"\"))
                                            {
                                                stringVal.Replace(@"\", @"\\");
                                            }
                                            */
                                            string sval = stringVal.Replace("'", @"\'");
                                            sval = sval.Replace("‘", "?");
                                            insertSql += add + addToField + Struct[i].name + " = '" + sval + "'";
                                            if (asFullInsert) valuesExport += valuesExportAdd + "'" + sval + "'";
                                            break;
                                    }

                                    add = ",";
                                    valuesExportAdd = ",";
                                }
                                else
                                {
                                    valuesExport += valuesExportAdd + "NULL";
                                }


                            }
                            else
                            {
                                valuesExport += valuesExportAdd + "NULL";
                            }
                            
                            valuesSeperator = ",";
                        }
                      
                        valuesExport += ")";
                        startRow = false;
                        if (!asFullInsert) queryList.Add(insertSql);
                        
                    }
                    if (asFullInsert && count > 0)
                    {
                        string exportSql = fullInsert + fields + ") VALUES " + valuesExport + ";";
                        queryList.Add(exportSql);
                    }
                    Reader.Close();
                }

            }
            return count;
        }

        public Int64 copyValues(MysqlHandler connection, string sourceTable, Int64 start, MysqlHandler targetConnection)
        {

            Int64 count = 0;
            Int64 refresher = 0;
            List<MysqlStruct> Struct = connection.getAllFieldsStruct(sourceTable);

            string addToField = sourceTable + ".";

            if (showProgress && msg.Visible) msg.progressBar.Maximum = (int)this.copyRowLimit + 1;

            if (connection.isConnected())
            {
                MySql.Data.MySqlClient.MySqlDataReader Reader =
                    connection.sql_select("SELECT * FROM " + sourceTable + " LIMIT " + start + "," + this.copyRowLimit);
                if (Reader != null)
                {

                    //if (Reader.RecordsAffected==-1) return 0;

                    while (Reader.Read())
                    {
                        count++;
                        refresher++;
                        string insertSql = "INSERT INTO " + sourceTable + " SET ";
                        string add = "";
                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (refresher > 1000)
                            {
                                if (showProgress && msg.Visible)
                                {
                                    msg.msg.Text = "Copying Table " + sourceTable + " block: " + start + "/" + count;
                                    msg.progressBar.Value = (int)count;
                                    msg.progressBar.Refresh();
                                    msg.Refresh();
                                    msg.Focus();
                                }
                                refresher = 0;
                            }


                            System.Type fType = Reader.GetFieldType(i);
                            Object chk = null;
                            try
                            {
                                chk = Reader.GetValue(i);
                            }
                            catch (Exception)
                            {

                                //throw;
                            }
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
                                        case "UInt64":
                                            Int64 val = Reader.GetInt64(i);
                                            insertSql += add + addToField + Struct[i].name + " = " + val;
                                            break;
                                        case "Single":
                                        case "Float":
                                            float fVal = Reader.GetFloat(i);
                                            insertSql += add + addToField + Struct[i].name + " = '" + fVal.ToString().Replace(',', '.') + "'";
                                            break;

                                        case "Double":
                                            double dVal = Reader.GetDouble(i);
                                            insertSql += add + addToField + Struct[i].name + " = '" + dVal.ToString().Replace(',', '.') + "'";
                                            break;
                                        case "DateTime":
                                            DateTime dt = Reader.GetDateTime(i);
                                            String dtValue = zeroVal(dt.Year) + "-" + zeroVal(dt.Month) + "-" + zeroVal(dt.Day) + " " + dt.TimeOfDay;
                                            insertSql += add + addToField + Struct[i].name + " = '" + dtValue + "'";
                                            break;
                                        case "Boolean":
                                            bool bval = Reader.GetBoolean(i);
                                            if (bval) insertSql += add + addToField + Struct[i].name + " = 1";
                                            else insertSql += add + addToField + Struct[i].name + " = 0";
                                            break;
                                        default:
                                            string sval = Reader.GetString(i).Replace("'", @"\'");

                                            insertSql += add + addToField + Struct[i].name + " = '" + sval + "'";
                                            break;
                                    }

                                    add = ",";
                                }
                            }
                        }
                        //returnValue += insertSql + ";\n";
                        targetConnection.sql_update(insertSql);

                    }
                    Reader.Close();
                }

            }
            return count;
        }

        private string zeroVal(int val)
        {
            if (val < 10) return "0" + val;
            else return "" + val;
        }

        public static string zeroValue(int val)
        {
            if (val < 10) return "0" + val;
            else return "" + val;
        }

        private List<string> getSqlDumpRows(MysqlHandler connection, string sourceTable)
        {
            List<string> Res = new List<string>();
            List<string> RTemp = new List<string>();
            if (showProgress)
            {
                msg.msg.Text = "Start .... " + sourceTable;
                msg.Show();
                msg.Refresh();
            }
            Int64 writes = 0;
            Int64 start = 0;
            RTemp = getTableExport(connection, sourceTable, start);
            Res.AddRange(RTemp);
            start = writes;
            while (writes != 0)
            {

                if (showProgress)
                {
                    msg.msg.Text = "Read Block ...." + start + " @ " + sourceTable;
                    msg.Refresh();
                }

                RTemp = getTableExport(connection, sourceTable, start);
                Res.AddRange(RTemp);
                start += writes;
            }

            if (showProgress)
            {
                msg.Close();
            }
            return Res;
        }

        private List<string> getTableExport(MysqlHandler connection, string sourceTable, Int64 start)
        {
            List<string> Data = new List<string>();
            if (connection.isConnected())
            {
                MySql.Data.MySqlClient.MySqlDataReader Reader =
                    connection.sql_select("SELECT * FROM " + sourceTable + " LIMIT " + start + "," + this.copyRowLimit);
                if (Reader != null)
                {
                    while (Reader.Read())
                    {
                        string result = getInsertSql(sourceTable, Reader);
                        if (result != "") Data.Add(result + ";\n");
                    }
                }
            }
            return Data;
        }

        public string getInsertSql(string sourceTable, MySql.Data.MySqlClient.MySqlDataReader Reader)
        {

            string insertSql = "";
            string add = "";

            for (int i = 0; i < Reader.FieldCount; i++)
            {

                if (i == 0) insertSql = "INSERT INTO " + sourceTable + " SET ";

                System.Type fType = Reader.GetFieldType(i);
                Object chk = null;
                try
                {
                    chk = Reader.GetValue(i);
                }
                catch (Exception)
                {

                    //throw;
                }
                if (chk != null)
                {
                    int hash = chk.GetHashCode();
                    string dd = chk.ToString();
                    string addTableString = ""; // if option addTablename set her was set the tablename to adding idendtifier
                    if (this.addTablename) addTableString = sourceTable + ".";
                    if (chk != null && dd != "")
                    {
                        switch (fType.Name)
                        {
                            case "Int32":
                            case "UInt32":
                            case "Int64":
                            case "UInt64":
                                Int64 val = Reader.GetInt64(i);
                                insertSql += add + addTableString + Reader.GetName(i) + " = " + val;
                                break;
                            case "Single":
                            case "Float":
                                float fVal = Reader.GetFloat(i);
                                insertSql += add + addTableString + Reader.GetName(i) + " = '" + fVal.ToString().Replace(',', '.') + "'";
                                break;

                            case "Double":
                                double dVal = Reader.GetDouble(i);
                                insertSql += add + addTableString + Reader.GetName(i) + " = '" + dVal.ToString().Replace(',', '.') + "'";
                                break;
                            case "DateTime":
                                DateTime dt = Reader.GetDateTime(i);
                                String dtValue = zeroVal(dt.Year) + "-" + zeroVal(dt.Month) + "-" + zeroVal(dt.Day) + " " + dt.TimeOfDay;
                                insertSql += add + addTableString + Reader.GetName(i) + " = '" + dtValue + "'";
                                break;
                            case "Boolean":
                                bool bval = Reader.GetBoolean(i);
                                if (bval) insertSql += add + Reader.GetName(i) + " = 1";
                                else insertSql += add + addTableString + Reader.GetName(i) + " = 0";
                                break;
                            default:
                                string sval = Reader.GetString(i).Replace("'", @"\'");

                                insertSql += add + addTableString + Reader.GetName(i) + " = '" + sval + "'";
                                break;
                        }

                        add = ",";
                    }
                }
            }
            return insertSql;
        }


        public static string getInserts(string sourceTable, MySql.Data.MySqlClient.MySqlDataReader Reader)
        {

            string insertSql = "";
            string add = "";

            string addTableString = ""; // if option addTablename set her was set the tablename to adding idendtifier
            addTableString = sourceTable + ".";

            for (int i = 0; i < Reader.FieldCount; i++)
            {

                if (i == 0) insertSql = "INSERT INTO " + sourceTable + " SET ";

                System.Type fType = Reader.GetFieldType(i);
                Object chk = null;
                try
                {
                    chk = Reader.GetValue(i);
                }
                catch (Exception)
                {

                    //throw;
                }
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
                            case "UInt64":
                                Int64 val = Reader.GetInt64(i);
                                insertSql += add + addTableString + Reader.GetName(i) + " = " + val;
                                break;
                            case "Single":
                            case "Float":
                                float fVal = Reader.GetFloat(i);
                                insertSql += add + addTableString + Reader.GetName(i) + " = '" + fVal.ToString().Replace(',', '.') + "'";
                                break;

                            case "Double":
                                double dVal = Reader.GetDouble(i);
                                insertSql += add + addTableString + Reader.GetName(i) + " = '" + dVal.ToString().Replace(',', '.') + "'";
                                break;
                            case "DateTime":
                                DateTime dt = Reader.GetDateTime(i);
                                String dtValue = zeroValue(dt.Year) + "-" + zeroValue(dt.Month) + "-" + zeroValue(dt.Day) + " " + dt.TimeOfDay;
                                insertSql += add + addTableString + Reader.GetName(i) + " = '" + dtValue + "'";
                                break;
                            case "Boolean":
                                bool bval = Reader.GetBoolean(i);
                                if (bval) insertSql += add + Reader.GetName(i) + " = 1";
                                else insertSql += add + addTableString + Reader.GetName(i) + " = 0";
                                break;
                            default:
                                string sval = Reader.GetString(i).Replace("'", @"\'");

                                insertSql += add + addTableString + Reader.GetName(i) + " = '" + sval + "'";
                                break;
                        }

                        add = ",";
                    }
                }
            }
            return insertSql;
        }

        public static void getMaskedSql(string sql, MysqlStruct currentStruct)
        {
            ArrayList items = new ArrayList();
            String filename = "mysqldef.dat";
            if (File.Exists(filename))
            {

                string lineval = "";
                StreamReader filereader = new StreamReader(filename);
                while ((lineval = filereader.ReadLine()) != null)
                {
                    items.Add(lineval);
                }
                filereader.Close();
                //items.Sort();
            }
        }
    }
}
