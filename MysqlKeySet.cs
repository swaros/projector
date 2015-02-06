using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class MysqlKeySet
    {
        public bool nonUnique;
        public string keyName;
        public int seqInSequence;
        public string columnName;
        public string collation;
        public Int64 cardinality;
        public int subPart;
        public bool isPacked;
        public bool nullPossible;
        public string indexType;

    }
}
