using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class PatternVarchar : PatternTypes
    {

        string[] strTypes = { "'" , "\"" };

        string currentIdent = "";

        public override void init()
        {
            this.isBlock = true;
            this.startident = "'";
            this.endIdent = "'";
            this.useIdentChecker = true;
            this.typeId = 2;
        }

        public override Boolean checkStart()
        {
            for (int i = 0; i < this.strTypes.Length; i++)
            {
                if (strTypes[i] == this.currentContent)
                {
                    this.currentIdent = strTypes[i];
                    return true;
                }
            }


            return false;
        }

        public override Boolean checkEnd()
        {
            return (this.currentIdent.Length > 0 && this.currentIdent == this.currentContent) ;            
        }
    }
}
