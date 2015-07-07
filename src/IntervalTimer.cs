using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Projector.Script;

namespace Projector
{
    public partial class IntervalTimer : UserControl
    {

        private ReflectionScript onTimerScr;

        public IntervalTimer()
        {
            InitializeComponent();
        }

        public void OnTick(ReflectionScript script)
        {
            this.onTimerScr = script;
        }

        public void SetInterval(int milliseconds)
        {
            this.timer1.Interval = milliseconds;
        }

        public void Start()
        {
            this.timer1.Enabled = true;
        }

        public void Stop()
        {
            this.timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.onTimerScr != null)
            {
                RefScriptExecute exec = new RefScriptExecute(this.onTimerScr, this);
                exec.run();
            }
        }

    }
}
