using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{
    class MysqlExplain
    {

        public const int KEY_LEVEL_BEST = 1;
        public const int KEY_LEVEL_WORST = 0;
        public const int KEY_LEVEL_GOOD = 2;
        public const int KEY_LEVEL_MEDIUM = 3;
        public const int KEY_LEVEL_NEED_OPTMIZE = 4;

        private QueryComposer composer;
        private MysqlHandler db;

        private List<Hashtable> Data;
              
        private int TableCounts = 0;

        private int currentExplain = 0;

        private Hashtable Explains = new Hashtable();

        private String sql;

        private int keyOptimizedLevel = 0;

        List<string> tables = new List<string>();

        public struct ExplainResult
        {
            public bool isNull;
            public int id;
            public String Table;
            public String SelectType;
            public String Type;
            public String PossibleKeys;
            public String Key;
            public int Keylen;
            public String Refer;
            public int Rows;
            public String Extras;

        }


        public MysqlExplain(QueryComposer comp, MysqlHandler connection)
        {
            this.composer = comp;
            this.db = connection;
            this.explainSelect();
            this.analyse();
        }

        public MysqlExplain(String sqlStr, MysqlHandler connection)
        {
            this.composer = null;
            this.sql = sqlStr;
            this.db = connection;
            this.explainSelect();
            this.analyse();
        }

        private String getExplainValue(Hashtable dat,string key)
        {
            if (dat[key] == null) return null;
            else return dat[key].ToString();

        }


        private int getExplainValueAsInt(Hashtable dat, string key)
        {
            if (dat[key] == null) return -1;
            else return int.Parse( dat[key].ToString());

        }

        /**
         * for iterating over explains
         */ 
        public ExplainResult getNextExplainResult()
        {
            if (currentExplain <= this.Explains.Count)
            {
                ExplainResult res = (ExplainResult)this.Explains[this.tables[currentExplain]];
                currentExplain++;
                return res;
            }
            else
            {
                ExplainResult resNull = new ExplainResult();
                resNull.isNull = true;
                return resNull;
            }
        }

        public void rewind()
        {
            currentExplain = 0;
        }

        public String getExplainReadableInfo()
        {
            String Message = "";

            for (int i = 0; i < tables.Count; i++)
            {
                ExplainResult res = (ExplainResult)this.Explains[this.tables[i]];
                if (!res.isNull && res.Table != null){
                    Message += res.Table + System.Environment.NewLine;
                    Message += "Used Key:[" + res.Key + "]" + System.Environment.NewLine;
                    Message += "Type:[" + res.Type + "] possible keys:[" + res.PossibleKeys + "] rows in index:[" + res.Rows + "]" + System.Environment.NewLine;
                    Message += System.Environment.NewLine;
                    if (res.Type == "const" || res.Type == "system")
                    {
                        Message += " BEST PERFORMACE ";
                        if (tables.Count > 1)
                        {
                            Message += " for this table .. check other tables keys.";
                        }
                        Message += System.Environment.NewLine;
                    }

                    if (res.Type == "eq_ref")
                    {
                        if (tables.Count > 1)
                        {
                            if (i != 0)
                            {
                                Message += "one row hit possible so this Key have a good perfomance for this query.";
                            }
                            else
                            {
                                Message += "one row hit possible but try to optimize the query. First table best Type ist const or system";
                            }
                        }
                    }

                    if (res.Type == "ref")
                    {
                        Message += "FOR MULTIPLE ROWS BEST PERFORMANCE. ELSEWHERE QUERY SHOULD BE OPTIMIZED." + System.Environment.NewLine;

                        if (res.Key == "PRIMARY")
                        {
                            Message += "The key is a primary key but this query use only the left part of all keys so this query ist not the best case (for looking at one row exactly) and should be optimized.";                            

                        }
                        else
                        {
                            Message += "The key is not a unique index and not a primary key so we have more the one as result.";
                        }
                        Message += System.Environment.NewLine + "NOTICE: if you need more then one hit, so we have the best solution with this type of key";
                    }

                    if (res.Type == "index_subquery")
                    {
                        Message += " QUERY SHOULD BE OPTIMIZED." + System.Environment.NewLine;
                        Message += "on your select with using IN the query have no hit over a unique Index." + System.Environment.NewLine;
                        Message += "for a better Performance try to optimze the subselect with (if possible) using a unique index .";
                    }

                    if (res.Type == "unique_subquery")
                    {
                        Message += " GOOD PERFORMACE FOR SUBSELECT." + System.Environment.NewLine;
                        Message += "Subquery have a unique hit so we for the subquery a good choice." + System.Environment.NewLine;
                        
                    }

                    if (res.Type == "ALL")
                    {
                        Message += " WORST CASE." + System.Environment.NewLine;
                        Message += "This Query results in a full tablescan for " + res.Table + ".";

                        if (tables.Count > 1)
                        {
                            Message += "Try an other Method to join the Tables.";
                            if (res.PossibleKeys != null && res.PossibleKeys != "")
                            {
                                Message += "look at possible keys for this Table " + res.PossibleKeys + System.Environment.NewLine;
                            }
                        }
                    }
                    Message += System.Environment.NewLine;
                    Message += "--------------------------";
                    Message += System.Environment.NewLine;
                }
                
            }


            return Message;
        }

        public String getNotification(){
            String notification = "";
            for (int i = 0; i < this.tables.Count; i++)
            {
                string current = this.tables[i];
                ExplainResult result = (ExplainResult)this.Explains[current];
                if (result.Extras != null)
                {
                    notification += result.Extras + ";";
                }
                
            }
            return notification;
        }


        public bool useKeys()
        {
            bool use = false;
            bool noReSet = false;
            for (int i = 0; i < this.tables.Count; i++)
            {
                string current = this.tables[i];
                ExplainResult result = (ExplainResult)this.Explains[current];
                if (result.Key != null)
                {
                    if (!noReSet) use = true;
                }
                else
                {
                    if (use == true)
                    {
                        use = false;
                        noReSet = true;
                    }
                }
            }
            return use;
        }

        private void analyse()
        {
            if (this.Data != null)
            {
                this.TableCounts = this.Data.Count;

                for (int i = 0; i < this.Data.Count; i++)
                {
                    ExplainResult result = new ExplainResult();
                    result.id = getExplainValueAsInt(this.Data[i],"id");
                    result.Table = getExplainValue(this.Data[i], "table");
                    result.Type = getExplainValue(this.Data[i], "type");
                    result.PossibleKeys = getExplainValue(this.Data[i], "possible_keys");
                    result.Key = getExplainValue(this.Data[i], "key");
                    result.Keylen = getExplainValueAsInt(this.Data[i], "key_len");
                    result.Refer = getExplainValue(this.Data[i], "ref");
                    result.Rows = getExplainValueAsInt(this.Data[i], "rows");
                    result.Extras = getExplainValue(this.Data[i], "Extra");
                    result.isNull = false;

                    String key_name = i.ToString();

                    if (result.Table != null)
                    {
                        key_name += result.Table;
                    }

                    if (this.Explains.Contains(key_name))
                        this.Explains[key_name] = result;
                    else
                        this.Explains.Add(key_name, result);
                    this.tables.Add(key_name);
                }
            }
        }

        private void explainSelect()
        {
            bool disconnect = false;

            string sql = this.sql;
            if (sql == null && this.composer != null) sql = this.composer.getSelect();
            sql = "EXPLAIN " + sql;
            if (!this.db.isConnected())
            {
                disconnect = true;
                this.db.connect();
            }


            this.Data = db.selectAsHash(sql);

            if (disconnect)
            {
                db.disConnect();
            }

        }
    }
}
