using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Projector
{
    class MysqlStruct
    {
        /// <summary>
        /// The Name of the Table
        /// </summary>
        public string tableName;
        /// <summary>
        /// The Name of the Field
        /// </summary>
        public string name;
        /// <summary>
        /// The Type of the Field
        /// </summary>
        public string type;
        /// <summary>
        /// if this set to NO the value can not be Null
        /// </summary>
        public string Null;
        public string Key;
        public string Default;
        public string Extra;
        public string[] enums;
        public Int64 minIntVal = 0;
        public UInt64 maxIntval = 255;

        public string param = "";
        public string typePrev = "";
        public string realType = "";
        public int len = 0;

        public void parseType()
        {
            if (this.type != null)
            {
                
                switch (this.type)
                {
                    case "timestamp":
                    case "text":
                    case "datetime":
                        this.realType = this.type;
                        break;
                    default:
                        string[] hash = this.type.Split(' ');

                        this.typePrev = hash[0];

                        if (hash.Length > 1)
                        {
                            this.param = hash[1];
                        }

                        char[] delimiters = new char[] { '(', ')' };
                        string[] parts = typePrev.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                        

                        if (parts.Length == 2)
                        {
                            realType = parts[0];
                            switch (realType)
                            {
                                case "enum": case "set":
                                    this.enums = parts[1].Replace("'","").Split(',');
                                    break;

                                case "tinyint":
                                case "smallint":
                                case "int":
                                case "bigint":
                                case "mediumint":
                                    len = int.Parse(parts[1]);
                                    
                                    break;
                            }
                        }

                        if (parts.Length == 0)
                        {
                            this.realType = this.type;
                        }

                        break;
                }

                setMinmaxOnInt();
            }
        }

        public void setMinmaxOnInt()
        {
            this.minIntVal = 0;
            switch (this.realType)
            {
                case "tinyint":
                    this.maxIntval = 255;
                    break;
                case "smallint":
                    this.maxIntval = 32767;
                    break;
                case "mediumint":
                    this.maxIntval = 16777215;
                    break;

                case "int":
                    this.maxIntval = 4294967295;
                    break;

                case "bigint":
                    this.maxIntval = 18446744073709551615;
                    break;
            }

            if (this.type.Contains("unsigned"))
            {
                this.maxIntval = this.maxIntval / 2;
                this.minIntVal = this.minIntVal - (Int64)this.maxIntval; 
            }
        }

        public bool isEqualTo(MysqlStruct compareStruct)
        {
            return (compareStruct.getStructInfo() == this.getStructInfo());
        }

        public string getDiffAlter(MysqlStruct compareStruct)
        {
            string info = "";

            if (this.type != null && this.type != compareStruct.type)
                info += "";

            return info;
        }


        public string getStructFieldModifier()
        {
            string info = "COLUMN `" + this.name + "` ";

            if (this.type != null) info += this.type;
            //if (this.len > 0) info += "(" + this.len + ")";

            if (this.Null != null && this.Null == "NO") info += " NOT NULL ";
            //if (this.Key != null) info += "Key: " + this.Key + "\n";
            if (this.Default != null && this.Default.Length > 0) info += " DEFAULT '" + this.Default + "' ";
            if (this.Extra != null) info += " " + this.Extra.ToUpper() + " ";

            /*
            if (this.param != "") info += "param: " + this.param + "\n";
            if (this.typePrev != "") info += "P type: " + this.typePrev + "\n";
            if (this.realType != "") info += "real Type: " + this.realType + "\n";
            */

            return info;
        }

        public string getStructInfo()
        {
            string info = "";
            if (this.name != null) info += "name: " + this.name + "\n";
            if (this.type != null) info += "Type: " + this.type + "\n";
            if (this.Null != null) info += "Null: " + this.Null + "\n";
            if (this.Key != null) info += "Key: " + this.Key + "\n";
            if (this.Default != null) info += "Default: " + this.Default + "\n";
            if (this.Extra != null) info += "Extra: " + this.Extra + "\n";

            if (this.param != "") info += "param: " + this.param + "\n";
            if (this.typePrev != "") info += "P type: " + this.typePrev + "\n";
            if (this.realType != "") info += "real Type: " + this.realType + "\n";
            if (this.len > 0) info += "len: " + this.len + "\n";

            return info;
        }
    }
}
