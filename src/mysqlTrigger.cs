using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class mysqlTrigger
    {
        public string Name;
        public string Timing;
        public string Event;
        public string sql;
        public string Table;
        

        public string getCreateStatement()
        {
            /*
            return "DELIMITER |\n\n"+
            "CREATE TRIGGER " + this.Name + " " + this.Timing + " " + this.Event + " ON " + this.Table + "\n FOR EACH ROW " +
            " "+sql+
            ";\n|\n\nDELIMITER ;";*/

            return "CREATE TRIGGER " + this.Name + " " + this.Timing + " " + this.Event + " ON " + this.Table + " FOR EACH ROW \n" + sql;

        }
    }
}
