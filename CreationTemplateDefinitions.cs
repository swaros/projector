using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    static class CreationTemplateDefinitions
    {
        private static List<CreationTemplate> creationDefintion = new List<CreationTemplate>();

       


        public static void addDefinition(string name, bool hasLenght, bool hasSperatedValues,bool isDate, bool isNumeric, bool isString, bool canCollate)
        {
            CreationTemplate crTemp = new CreationTemplate();

            crTemp.name = name;
            crTemp.hasLength = hasLenght;
            crTemp.hasSperatedValues = hasSperatedValues;
            crTemp.isDate = isDate;
            crTemp.isNumeric = isNumeric;
            crTemp.isString = isString;

            crTemp.canCollate = canCollate;
            CreationTemplateDefinitions.addDefinition(crTemp);
        }


        public static void addDefinition(CreationTemplate def)
        {
            creationDefintion.Add(def);
        }

                


        public static CreationTemplate getTemplateByName(string name)
        {
            name = name.ToUpper();
            if (CreationTemplateDefinitions.creationDefintion.Count < 1) initMysqlTypes();
            for (int i = 0; i < creationDefintion.Count; i++)
            {
                if (creationDefintion[i].name.ToUpper() == name) return creationDefintion[i];
            }

            return null;
        }

        private static void initMysqlTypes()
        {
            addDefinition("BIT", true, false, false, false, false, false);
            addDefinition("TINYINT", true, false, false, true, false, false);
            addDefinition("SMALLINT", true, false, false, true, false, false);
            addDefinition("MEDIUMINT", true, false, false, true, false, false);
            addDefinition("INT", true, false, false, true, false, false);
            addDefinition("INTEGER", true, false, false, true, false, false);
            addDefinition("BIGINT", true, false, false, true, false, false);
            addDefinition("REAL", true, false, false, true, false, false);
            addDefinition("DOUBLE", true, false, false, true, false, false);
            addDefinition("FLOAT", true, false, false, true, false, false);
            addDefinition("DECIMAL", true, false, false, true, false, false);
            addDefinition("NUMERIC", true, false, false, true, false, false);
            addDefinition("DATE", false, false, true, false, false, false);
            addDefinition("TIME", false, false, true, false, false, false);
            addDefinition("TIMESTAMP", false, false, true, false, false, false);
            addDefinition("DATETIME", false, false, true, false, false, false);
            addDefinition("YEAR", false, false, true, false, false, false);
            addDefinition("CHAR", true, false, false, false, false, false);
            addDefinition("VARCHAR", true, false, false, false, true, true);
            addDefinition("BINARY", true, false, false, false, false, false);
            addDefinition("VARBINARY", true, false, false, false, false, false);
            addDefinition("TINYBLOB", false, false, false, false, false, false);
            addDefinition("BLOB", false, false, false, false, false, false);
            addDefinition("MEDIUMBLOB", false, false, false, false, false, false);
            addDefinition("LONGBLOB", false, false, false, false, false, false);
            addDefinition("TINYTEXT", false, false, false, false, true, true);
            addDefinition("TEXT", false, false, false, false, true, true);
            addDefinition("MEDIUMTEXT", false, false, false, false, true, true);
            addDefinition("LONGTEXT", false, false, false, false, true, true);
            addDefinition("ENUM",false,true,false,false,true,true);
            addDefinition("SET", false, true, false, false, true, true);
            


        }


    }
}
