using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace Projector
{
    class JoinTable
    {
        public string selfTableName;
        public string refrenceTableName;
        private Hashtable onEqualJoins = new Hashtable();

        public JoinTable(string reference, string joinOnTable)
        {
            this.refrenceTableName = reference;
            this.selfTableName = joinOnTable;
        }

        public void addEqualJoinOn(string ownFieldname, string otherFieldname)
        {
            this.onEqualJoins.Add(ownFieldname, otherFieldname);
        }


        public string getJoinOnStatement()
        {
            string joinSql = " ( ";
            string add = "";
            
            foreach (DictionaryEntry de in this.onEqualJoins)
            {
                joinSql += add + selfTableName + "." + de.Key.ToString() + " = " + this.refrenceTableName + "." + de.Value.ToString();
                add = " AND ";
            }

            joinSql += " ) ";
            return joinSql;
        }

    }
}
