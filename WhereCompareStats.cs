using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class WhereCompareStats
    {
        public const int Equals = 1;
        public const int NonEquals = 2;
        public const int GtEquals = 3;
        public const int LwEquals = 4;
        public const int Gt = 5;
        public const int Lw = 6;
        public const int Like = 7;
        public const int Subselect = 8;

        public static String getCompareString(QueryComposer qc, string fieldName)
        {
            string Statement = "";

            string compare = qc.getCompareValue(fieldName);
            if (null != compare)
            {
                switch (compare.ToUpper())
                {
                    case "LIKE":
                        Statement = fieldName + " " + compare + " '%" + qc.getWhereValue(fieldName).ToString().Replace("'", @"\'") + "%' ";
                        break;

                    case "IN":
                        Statement = fieldName + " " + compare + " (" + qc.getWhereValue(fieldName).ToString().Replace("'", @"\'") + ") ";
                        break;

                    case "SUBSELECT":
                        Statement = fieldName + " IN (" + qc.getWhereValue(fieldName) + ") ";
                        break;

                    default:
                        Statement = fieldName + " " + compare + " '" + qc.getWhereValue(fieldName).ToString().Replace("'", @"\'") + "' ";
                        break;
                }
            }
            return Statement;
        }



    }
}
