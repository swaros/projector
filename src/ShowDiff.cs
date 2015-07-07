using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Projector
{
    public partial class ShowDiff : Form
    {
        public ShowDiff()
        {
            InitializeComponent();
           
        }

        private void leftText_TextChanged(object sender, EventArgs e)
        {
            
        }

        private bool isExists(string name, int start,string[] array)
        {
            for (int i = start; i < array.Length; i++)
            {
                if (array[i] == name) return true;
            }
            return false;
        }

        public void compareText()
        {
            Regex r = new Regex("([= \\t{}()<>:;,])");
         
            String[] tokens = r.Split(leftText.Text);
            String[] tokensRight = r.Split(rightText.Text);


            leftText.Text = "";
            rightText.Text = "";

            int dd = 0;
            foreach (string token in tokens)
            {
               string data=  token;

               leftText.SelectionColor = Color.Black;
               leftText.SelectionFont = new Font("Courier New", 10, FontStyle.Regular);

               if (dd < tokensRight.Length && token != tokensRight[dd])
               {
                   leftText.SelectionColor = Color.Red;
                   leftText.SelectionFont = new Font("Courier New", 10, FontStyle.Bold);
               }
               else
               {
                   leftText.SelectionColor = Color.Blue;
                   leftText.SelectionFont = new Font("Courier New", 10, FontStyle.Regular);
                   dd++;
               }

               leftText.SelectedText = token;
               
            }

            dd = 0;
            foreach (string token in tokensRight)
            {
                string data = token;

                rightText.SelectionColor = Color.Black;
                rightText.SelectionFont = new Font("Courier New", 10, FontStyle.Regular);

                if (dd < tokens.Length && token != tokens[dd])
                {
                    leftText.SelectionColor = Color.Red;
                    leftText.SelectionFont = new Font("Courier New", 10, FontStyle.Bold);
                }
                else
                {
                    rightText.SelectionColor = Color.Blue;
                    rightText.SelectionFont = new Font("Courier New", 10, FontStyle.Regular);
                    dd++;
                }

                rightText.SelectedText = token;

            }
            
        }

        private void rightText_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
