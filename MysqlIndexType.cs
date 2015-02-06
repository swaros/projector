using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class MysqlIndexType
    {
        private string name;
        private List<MysqlCreateStruct> refers = new List<MysqlCreateStruct>();

        public MysqlIndexType(string keyName)
        {
            this.name = keyName;
        }

        public string getName()
        {
            return this.name;
        }

        public void addRefererStruct(MysqlCreateStruct refStruct)
        {
            this.refers.Add(refStruct);
            
        }

        public String getFieldString()
        {
            String result = "";
            String add = "";
            for (int i = 0; i < refers.Count; i++)
            {
                result += add + "`" + refers[i].name + "`";
                add = ",";
            }
            return result;
        }


        public void clear()
        {
            this.refers.Clear();
        }

    }
}
