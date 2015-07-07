using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class DataContent
    {
        public MysqlHandler db1;
        public MysqlHandler db2;
        public List<string> Source;
        public bool massInsertQuerys = true;
    }
}
