using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{
    public abstract class PatternTypes
    {
        public int typeId;

        /* the conten as string*/
        public string content;

        /* isblock is a flag for elements with an start and a endelement, so
         this pattern hase no content itself. for example an subselect starts with a '(' and ends with ')' */
        public Boolean isBlock = false;

        /* the identifier that starts the sequence. for example: "(" */
        public string startident;

        /* the identifier that ends the sequence. for example: ")" */
        public string endIdent;

        /* if this true, so this pattern use the checkStart and checkEnd methods to find sequences. */
        public Boolean useIdentChecker = false;

        /* if set so find the in reverse style */

        public Boolean scanReverse = false;

        HashSet<PatternTypes> subPatterns = new HashSet<PatternTypes>();


        public string currentContent;

        public PatternTypes()
        {
            this.init();
            this.checkValues();
        }

        public abstract void init();

        public abstract Boolean checkStart();

        public abstract Boolean checkEnd();


        public void checkValues()
        {
            if (this.isBlock && (null == startident || null == endIdent)) throw new Exception("Block Patterns needed start- and end idents");
            if (0 == typeId) throw new Exception("Type is not set");
        }

    }
}
