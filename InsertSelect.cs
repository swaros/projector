using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{
    class InsertSelect
    {
        private string insertFromTable;
        private string insertToTable;

        Hashtable fieldMappings = new Hashtable();
        Hashtable tables;
        List<string> whereHash;

        public InsertSelect(string fromTable, string toTable)
        {
            this.insertFromTable = fromTable;
            this.insertToTable = toTable;
        }

        public InsertSelect(string toTable, Hashtable Tables)
        {
            this.insertFromTable = "";
            this.insertToTable = toTable;

            this.tables = Tables;
        }


        public void addtableList(Hashtable Tables)
        {
            this.tables = Tables;
        }

        public void addWhereList(List<string> Wheres)
        {
            this.whereHash = Wheres;
        }

        public void copyField(string source, string target)
        {
            if (fieldMappings.Contains(source))
            {
                fieldMappings[source] = target;
            }
            else
            {
                fieldMappings.Add(source, target);
            }
        }

        public string getStatement()
        {
            if (this.fieldMappings.Count > 0)
            {
                string sql = "INSERT INTO " + this.insertToTable + " (";
                string sqlPost = "";
                string add = "";
                foreach (DictionaryEntry de in this.fieldMappings)
                {
                    string keyname = de.Key.ToString();
                    string value = de.Value.ToString();

                    sql += add + keyname;
                    sqlPost += add + value;
                    add = ",";
                }
                if (this.tables == null)
                    sql += ") SELECT " + sqlPost + " FROM " + this.insertFromTable;
                else
                {
                    sql += ") SELECT " + sqlPost + " FROM " + this.insertFromTable;
                    add = "";
                    foreach (DictionaryEntry tabs in this.tables)
                    {
                        sql += add + tabs.Key.ToString();
                        add = ",";
                    }
                }

                if (this.whereHash != null)
                {
                    add = " WHERE ";
                    for (int i = 0; i < this.whereHash.Count; i++ )
                    {
                        sql += add + this.whereHash[i];
                        add = " AND ";
                    }
                }

                return sql;
            }
            else
            {
                return null;
            }
        }
    }
}
