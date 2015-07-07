using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projector
{
    public partial class DiffSensor : UserControl
    {

        private Double lastValue = 0;
        private Boolean diffMode = true;
        private Boolean GrowMode = false;
        private Boolean valueAdded = false;

        private string nextLabel = "";

        public DiffSensor()
        {
            InitializeComponent();
            this.sensorPanel.lineColor = Color.LightYellow;
            this.sensorPanel.BackColor = Color.DarkGreen;
            this.sensorPanel.Label = "Sensor";
            this.sensorPanel.showValues = true;
        }

        public void setEnabled(Boolean onoff)
        {
            this.Enabled = onoff;
        }

        public void setVisible(Boolean onoff)
        {
            this.Visible = onoff;
        }

        public void setLeft(int left)
        {
            this.Left = left;
        }

        public void setTop(int top)
        {
            this.Top = top;
        }

        public void setValue(int value)
        {
            this.setDValue(value);
        }

        public void setNextLabel(string label)
        {
            this.nextLabel = label;
        }

        public void Clear()
        {
            this.sensorPanel.Clear();
            this.reDraw();
        }

        private void setDValue(Double value)
        {
            double submitted = value;
            if (this.diffMode)
            {
                if (!this.valueAdded)
                {
                    this.lastValue = value;
                    this.valueAdded = true;
                    return;
                }
                else
                {
                    value = value - this.lastValue;
                }
            }
            if (this.GrowMode)
            {
                this.lastValue = submitted;
            }
            this.valueAdded = true;
            if (this.nextLabel != "")
            {
                this.sensorPanel.addValue(value, this.nextLabel);
                this.sensorPanel.useLabels = true;
                this.nextLabel = "";
            }
            else
            {
                this.sensorPanel.addValue(value);
            }
            
        }

        public void setValueAsString(string valueAsString)
        {
            Double value;
            try
            {
                value = Double.Parse(valueAsString);
            }
            catch (Exception)
            {
                return;
            }
            this.setDValue(value);
        }

        public void setDiffMode(Boolean onOff)
        {
            this.diffMode = onOff;
        }

        public void setGrowMode(Boolean onOff)
        {
            this.GrowMode = onOff;
        }

        public void setWidth(int val)
        {
            this.Width = val;
        }

        public void setHeight(int val)
        {
            this.Height = val;
        }

        public void reDraw()
        {
            this.sensorPanel.Update();
            this.sensorPanel.Invalidate();
        }
    }
}
