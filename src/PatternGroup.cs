using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    public class PatternGroup : PatternTypes
    {
        public override void init()
        {
            this.typeId = 1;
            this.startident = "(";
            this.endIdent = ")";
            this.isBlock = true;
            this.scanReverse = true;
        }

        public override Boolean checkStart()
        {
            throw new NotImplementedException();
        }

        public override Boolean checkEnd()
        {
            throw new NotImplementedException();
        }
    }
}
