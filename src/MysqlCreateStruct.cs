using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class MysqlCreateStruct
    {
        public string name;
        public string dataType;
        public UInt64 length;
        public Boolean notNull;
        public string defaultValue;
        public string comments;
        public Boolean isPrimary;
        public Boolean autoIncrement;

        public Object refObject;

        CreationTemplate crTemplate = new CreationTemplate();

        public MysqlCreateStruct()
        {
            this.autoIncrement = false;
            this.comments = "";
            this.dataType = "VARCHAR";
            this.defaultValue = "";
            this.isPrimary = false;
            this.length = 255;
            this.name = "";
            this.notNull = false;

        }

        public string getSql()
        {
            string sql = "`" + this.name + "` ";
            CreationTemplate crTempl = CreationTemplateDefinitions.getTemplateByName(this.dataType);

            sql += this.dataType;
            if (null != crTempl)
            {
                if (crTempl.hasLength && this.length > 0)
                    sql += "(" + this.length + ") ";



            }
            sql += " ";
            if (this.defaultValue.Length > 0)
            {
                this.defaultValue.Replace("'", "");
                sql += "DEFAULT '" + this.defaultValue + "'";
            }

            return sql;
        }

    }
}
