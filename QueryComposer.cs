using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{
    class QueryComposer
    {
        // publics

        public const int SELECT = 1;
        public const int UPDATE = 2;
        public const int INSERT = 3;

        public const int ORDER_DESC = 2;
        public const int ORDER = 1;


        public string TableName = null;
        public bool useLimit = true;

        // privates

        private Hashtable whereStates = new Hashtable();
        private Hashtable whereCompares = new Hashtable();
        private Hashtable joinTables = new Hashtable();
        private Hashtable setValues = new Hashtable();
        private Hashtable orderByField = new Hashtable();
        private Hashtable usedFields = new Hashtable();
        private Boolean orderDesc = false;
        private int whereCount = 0;

        private List<JoinTable> leftJoins = new List<JoinTable>();

        private string FieldSelects = "*";
        private Int64 startLimit = 0;
        private Int64 limitCount = 100;

        
      

        public QueryComposer(string tableName)
        {
            this.TableName = tableName;
        }


        public bool addOrderField(string OrderBy)
        {
            if (!this.orderByField.ContainsKey(OrderBy))
            {
                this.orderByField.Add(OrderBy, OrderBy);
                return true;
            }
            return false;
        }

        public Boolean addUsedFieldNames(string fieldName)
        {
            if (!this.usedFields.ContainsKey(fieldName))
            {
                this.usedFields.Add(fieldName, fieldName);
                this.buildFieldSring();
                return true;
            }

            return false;
        }

        private void buildFieldSring()
        {
            if (this.usedFields.Count > 0)
            {

                string fieldStr = "";
                string add = "";
                foreach (DictionaryEntry de in this.usedFields)
                {
                    fieldStr += add + "`" + de.Key.ToString() + "`";
                    add = ", ";
                }
                this.FieldSelects = fieldStr;
            }
        }

        public void autoHandleSingleOrder(string ord)
        {
            if (this.orderByField.ContainsKey(ord))
            {
                this.orderDesc = !orderDesc;
            }
            else
            {
                this.removeAllOrder();
                this.addOrderField(ord);
            }
        }


        public bool removeOrderField(string Orderby)
        {
            if (!this.orderByField.ContainsKey(Orderby))
            {
                this.orderByField.Remove(Orderby);
                return true;
            }
            return false;
        }

        public void removeAllOrder()
        {
            this.orderByField.Clear();
        }

        //------------ private tools ------------

        private string composeOrder()
        {
            this.buildFieldSring();
            string orderStr = "";
            if (this.orderByField.Count > 0)
            {
                string add = "";

                foreach (DictionaryEntry de in this.orderByField)
                {
                   
                    //where += add + de.Key.ToString() + getCompareValue(de.Key.ToString()) + " '" + de.Value.ToString().Replace("'",@"\'") + "' ";
                    if (leftJoins != null)
                        orderStr += add + this.TableName + "." + de.Key.ToString();
                    else
                        orderStr += add + de.Key.ToString();
                    add = ",";
                }
                orderStr = " ORDER BY " + orderStr;
                if (orderDesc) orderStr += " DESC ";
            }
            return orderStr;
        }


        private string composeAndWhere()
        {
            string where = "";
            bool adding = false;
            if (whereCount > 0)
            {
                string add = "";
                where = " WHERE ";
               
                foreach (DictionaryEntry de in this.whereStates)
                {
                    adding = true;
                    //where += add + de.Key.ToString() + getCompareValue(de.Key.ToString()) + " '" + de.Value.ToString().Replace("'",@"\'") + "' ";
                    if (leftJoins.Count > 0)
                        where += add + this.TableName + "." + WhereCompareStats.getCompareString(this, de.Key.ToString());
                    else
                        where += add + WhereCompareStats.getCompareString(this, de.Key.ToString());

                    add = " AND ";
                }
            }
            if (!adding) return "";
            return where;
        }

        private string getLeftJoin()
        {           
            string res = "";
            List<string> checkExistTables = new List<string>();
            if (leftJoins.Count > 0)
            {
                string tab = "", compare = "", addTab = "", addComp = "";
                for (int i = 0; i < leftJoins.Count; i++)
                {
                    if (checkExistTables.Count < 1 || checkExistTables.Contains(leftJoins[i].selfTableName) == false)
                    {
                        tab += addTab + leftJoins[i].selfTableName;
                        addTab = ", ";
                        checkExistTables.Add(leftJoins[i].selfTableName);
                    }
                    compare += addComp + leftJoins[i].getJoinOnStatement();

                    addComp = " AND ";
                }

                res += " LEFT JOIN ( " + tab + ") ON (" + compare + ") ";
            }
            return res;
        }

        public void setResultField(string name)
        {
            this.FieldSelects = name;
        }

        public bool ifAllfieldsSelected()
        {
            return (this.FieldSelects == "*");
        }

        public void addJoinTable(string tableName,string myFieldName,string refFieldName)
        {
            JoinTable addleftJoins = new JoinTable(this.TableName, tableName);
            addleftJoins.addEqualJoinOn(refFieldName, myFieldName);
            leftJoins.Add(addleftJoins);
           
            
        }

        public void addJoinTable(MysqlStruct mstruct, MysqlStruct mstructRight)
        {
            JoinTable addleftJoins = new JoinTable(mstruct.tableName, mstructRight.tableName);
            addleftJoins.addEqualJoinOn(mstruct.name, mstruct.name);
            leftJoins.Add(addleftJoins);
        }

        public void resetJoinTable()
        {
            leftJoins = new List<JoinTable>();
        }

        public string selectUnion(List<string> tables)
        {
            string sql = "";
            string saveTableName = this.TableName;
            string add ="";
            foreach (string tmpCurrTable in tables){
                this.TableName = tmpCurrTable;

                sql += add + "(" + this.getSelect() + ")";
                add = System.Environment.NewLine + "UNION";
            }

            this.TableName = saveTableName;
            return sql;

        }


        /** 
         * Returns select query
         */
        public string getSelect()
        {
            if (TableName != null)
            {
                string sql = "SELECT ";
                sql += FieldSelects;
                sql += " FROM ";
                sql += TableName;

                if (leftJoins.Count > 0) sql += this.getLeftJoin();


                sql += composeAndWhere();
                sql += this.composeOrder();
                if (useLimit)
                {
                    sql += " LIMIT " + startLimit + "," + limitCount;
                }
                return sql;
            }
            else
            {
                throw new Exception("TableName must defined");
                
            }
            
        }

        public string getInsert()
        {
            
            if (TableName != null && getSetStatement() != null)
            {
                string sql = "INSERT INTO " + TableName + " SET " + getSetStatement();
                


                return sql;
            }

            return null;
        }


        // set start for limit
        public void limitStart(Int64 start)
        {
            this.startLimit = start;
        }

        //set range for limit
        public void setLimitRange(Int64 limit)
        {
            this.limitCount = limit;
        }

        //increase start 
        public void nextLimit()
        {
            this.startLimit += this.limitCount;
        }

        //decrease start 
        public void prevLimit()
        {
            if (this.startLimit > this.limitCount)
                this.startLimit -= this.limitCount;
            else
                this.startLimit = 0;
        }

       


        public void addWhere(string name, string value)
        {

            if (whereStates.ContainsKey(name))
            {
                whereStates[name] = value;
            }
            else
            {

                try
                {
                    whereStates.Add(name, value);
                    this.whereCount++;
                }
                catch
                {

                }
            }
        }

        public string getWhereValue(string keyName)
        {            
            return this.whereStates[keyName].ToString();

        }


        public string getSetting(string name)
        {
            if (whereStates.ContainsKey(name)) return whereStates[name].ToString();
            else return null;
        }

        public void removeWhere(string name)
        {
            if (whereStates.ContainsKey(name))
            {
                whereStates.Remove(name);
            }
        }

        public void addWhereComp(string name, string value)
        {

            if (whereCompares.ContainsKey(name))
            {
                whereCompares[name] = value;
            }
            else
            {

                try
                {
                    whereCompares.Add(name, value);                    
                }
                catch
                {

                }
            }
        }


        public String getWhereCompare(String name)
        {
            if (whereCompares.ContainsKey(name))
            {
                return whereCompares[name].ToString();
            }
            else
            {
                return null;
            }
        }


        // adds a value to database/table/field
        public void addSetValue(string keyName, string value)
        {
            if (setValues.ContainsKey(keyName))
            {
                setValues[keyName] = value;
            }
            else
            {
                setValues.Add(keyName, value);
            }
        }

        // get a set value
        public string getSetValue(string keyName)
        {
            if (setValues.ContainsKey(keyName))
            {
                return setValues[keyName].ToString();
            }
            else
            {
                return null;
            }
        }
        // remove value. return true if found en resettet or false if not exists
        public bool resetSetValue(string keyName, string value)
        {
            if (setValues.ContainsKey(keyName))
            {
                setValues.Remove(keyName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string getSetStatement()
        {

            if (setValues.Count < 1) return null;

            string setStat = "";
            string add = "";

            foreach (DictionaryEntry de in this.setValues)
            {

                setStat += add + "`" + de.Key.ToString() + "` = '" + de.Value.ToString().Replace("'", @"\'") + "' ";
                add = ", ";
            }

            return setStat;
        }


        public string getCompareValue(string keyName)
        {
            //return this.whereStates[keyName].ToString();
            string compare = getCompareSetting(keyName);
            if (compare == null) return "=";
            else return compare;
        }


        public string getCompareSetting(string name)
        {
            if (whereCompares.ContainsKey(name)) return whereCompares[name].ToString();
            else return null;
        }

        public static String addSemikolon(string sql){
            if (sql.Length > 0)
            {
                string lastChar = sql.Substring(sql.Length - 1);
                string secondLast = sql.Substring(sql.Length - 2);
                string thirdLast = sql.Substring(sql.Length - 3);
                if (lastChar != ";" && secondLast != "*/" && secondLast != ";" + System.Environment.NewLine && thirdLast != "*/" + System.Environment.NewLine) sql += ";";
                
            }
            return sql;
        }

    }
}
