using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{

    class MysqlKeys
    {
        public string Table;
        /*public bool nonUnique;
        public string keyName;
        public int seqInSequence;
        public string columnName;
        public string collation;
        public Int64 cardinality;
        public int subPart;
        public bool isPacked;
        public bool nullPossible;
        public string indexType;
        public string comment;
        */
        private string tableName;

        public bool initialized = false;


        private List<MysqlKeySet> keySet = new List<MysqlKeySet>();

        private List<MysqlKeySet> PrimaryKeySet = new List<MysqlKeySet>();

        public MysqlKeys()
        {
            

        }

        public void init(string TableName, MysqlHandler database)
        {
            this.tableName = TableName;
            this.readKeys(database);
            this.initialized = true;
        }

        public void readKeys( MysqlHandler database)
        {
            bool closeConnect = false;
            
            if (!database.isConnected())
            {
                closeConnect = true;
                database.connect();
            }
            assignValues( database.selectAsHash("SHOW INDEX FROM " + this.tableName));

            if (closeConnect) database.disConnect();
        }

        public List<MysqlKeySet> getPrimaryKeys()
        {
            return PrimaryKeySet;
        }

        public MysqlKeySet getKeyByFieldName(string name)
        {
            for (int i = 0; i < keySet.Count(); i++)
            {
                if (keySet[i].columnName == name)
                {
                    return keySet[i];
                }
            }
            return null;
        }


        public List<MysqlKeySet> getUniqueKeys()
        {
            List<MysqlKeySet> result = new List<MysqlKeySet>();
            for (int i = 0; i < keySet.Count(); i++)
            {
                if (!keySet[i].nonUnique)
                {
                    result.Add(keySet[i]);
                }
            }
            return result;
        }

        public bool columnIsPrimary(string fieldName)
        {
            MysqlKeySet tmp = getKeyByFieldName(fieldName);
            if (null == tmp) return false;
            else
            {
                return (tmp.keyName == "PRIMARY");
            }
        }

        public bool columnIsUnique(string fieldName)
        {
            MysqlKeySet tmp = getKeyByFieldName(fieldName);
            if (null == tmp) return false;
            else
            {
                return !tmp.nonUnique;
            }
        }

        private void assignValues(List<Hashtable> res)
        {
            keySet.Clear();
            PrimaryKeySet.Clear();
            for (int i = 0; i < res.Count; i++)
            {
                MysqlKeySet set = new MysqlKeySet();
                set.nonUnique =  res[i]["Non_unique"].ToString() != "0";
                set.keyName = res[i]["Key_name"].ToString();
                set.columnName = res[i]["Column_name"].ToString();
                set.indexType = res[i]["Index_type"].ToString();
                set.seqInSequence = int.Parse(res[i]["Seq_in_index"].ToString());


                if (res[i]["Collation"] != null) set.collation = res[i]["Collation"].ToString();
                if (res[i]["Cardinality"] != null) set.cardinality = int.Parse( res[i]["Cardinality"].ToString());
                if (res[i]["Sub_part"] != null) set.subPart = int.Parse(res[i]["Sub_part"].ToString());
                if (res[i]["Packed"] != null) set.isPacked = true; else set.isPacked = false;
                if (res[i]["Null"] != null && "YES"== res[i]["Null"].ToString()) set.nullPossible = true; else set.nullPossible = false;

                keySet.Add(set);

                if (set.keyName == "PRIMARY")
                {
                    PrimaryKeySet.Add(set);
                }

            }
        }

    }
}
