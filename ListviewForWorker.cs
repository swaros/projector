using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projector
{
    class ListviewForWorker
    {
        public ListView listView = null;
        public String sql = null;
        public String errorMessage = null;

        public ListviewForWorker(ListView lv,String sql)
        {
            this.listView = lv;
            this.sql = sql;
        }
    }
}
