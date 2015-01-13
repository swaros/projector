using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projector
{
    public class CodeFormater
    {
        private RichTextBox content;

        public int TabCharCount = 4;

        public CodeFormater()
        {

        }

        public void setContent(RichTextBox cont)
        {
            this.content = cont;
        }

        private int calcTab(int current)
        {
            double part = current / this.TabCharCount;
            int tabCount = (int) Math.Ceiling(part);

            // adding one because we want to shift to the right
            tabCount++;

            int existingTabCount = tabCount * TabCharCount;
            int needed = existingTabCount - current;
            return needed;
            
        }

        private int getTabOffset()
        {
            int charPos = this.content.SelectionStart;
            string[] findlastArray = this.content.Text.Substring(0,charPos).Split('\n');

            string lastLine = findlastArray.Last();
            return calcTab(lastLine.Length);
        }

        private String getTab(int cnt)
        {
            String tab = "";
            for (int i = 0; i < cnt; i++)
            {
                tab += " ";
            }
            return tab;
        }

        public void tabKey()
        {
            string[] selectedLines = this.content.SelectedText.Split('\n');
            this.content.SelectedText = "";
            int tabOffset = this.getTabOffset();
            string add = "";
            for (int i = 0; i < selectedLines.Count(); i++)
            {
                this.content.SelectedText += add + this.getTab(tabOffset) + selectedLines[i];
                add = System.Environment.NewLine;
            }
        }

    }
}
