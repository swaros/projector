using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Reflection;

namespace Projector
{
    class HighlighterMysql
    {
        TextParser tp = new TextParser();
        TimeUtil timer = new TimeUtil();

        private string definitionFile = "mysql_reserved.def";
        private string[] reservedWords;
        private bool doNotLoadAgain = false;

        public bool useTimer = true;
        public long maxRepeatTime = 10000000;


        private string lastCompiledSource = "";

        System.ComponentModel.BackgroundWorker parserThread = new System.ComponentModel.BackgroundWorker();

        private void initBgWorker(){
            parserThread.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);
            parserThread.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            parserThread.ProgressChanged +=
                new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        //ProgressChangedEventArgs
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }


        public void parse(RichTextBox sourceCode)
        {
            if (!doNotLoadAgain || reservedWords == null) loadWordsFromFile();



            if (!useTimer || timer.getDiff(maxRepeatTime))
            {
                if (sourceCode.Text != lastCompiledSource)
                {
                    parseIng(sourceCode);
                    timer.setTimer();
                    lastCompiledSource = sourceCode.Text;
                }
            }
        }

        public string[] getReservedWords()
        {
            if (!doNotLoadAgain) loadWordsFromFile();
            return reservedWords;
        }

        // load the defined reserved mysqlwords from file
        private void loadWordsFromFile()
        {
            string path = Environment.SpecialFolder.ApplicationData.ToString();

            FileInfo fi = new FileInfo(Assembly.GetEntryAssembly().Location);
            path = fi.DirectoryName;

            tp.FontSize = 8;
            tp.caseCheck = false;

            if (File.Exists(path + Path.DirectorySeparatorChar + definitionFile))
            {
                reservedWords = File.ReadAllLines(path + Path.DirectorySeparatorChar + definitionFile);

                for (int i = 0; i < reservedWords.Length; i++)
                {
                    tp.addKeyWord(reservedWords[i],1);
                }

            }
            tp.addKeyWord("(", 2);
            tp.addKeyWord(")", 2);
            tp.addKeyWord(",", 2);
            tp.addKeyWord("+", 2);
            tp.addKeyWord("-", 2);
            tp.addKeyWord("=", 2);
            tp.addKeyWord("!", 2);
            tp.addKeyWord("<", 2);
            tp.addKeyWord(">", 2);
            tp.addKeyWord(";", 2);
            doNotLoadAgain = true;
        }

        public void addTableName(string tableName)
        {
            tp.addKeyWord(tableName, 4);
        }

        public void addFieldName(string tableName)
        {
            tp.addKeyWord(tableName, 3);
        }

        private void parseIng(RichTextBox sourceCodeInput)
        {
            RichTextBox sourceCode = new RichTextBox();
            sourceCode.Text = sourceCodeInput.Text;

            int startpos = sourceCodeInput.SelectionStart;
            int endPos = sourceCodeInput.SelectionLength;
            tp.checkWords(sourceCode);
            tp.markTags(sourceCode, "\"", "\"", Color.DarkGreen, Color.LightGoldenrodYellow, 0);
            tp.markTags(sourceCode, "'", "'", Color.DarkGreen, Color.FromArgb(238, 245, 189), 0);
            tp.markTags(sourceCode, "/*", "*/", Color.Gray, Color.FromArgb(210, 210, 210), 0);
            tp.markTags(sourceCode, "`", "`", Color.Green, Color.Transparent, 0);
            

            sourceCodeInput.Rtf = sourceCode.Rtf;
            sourceCodeInput.SelectionStart = startpos;
            sourceCodeInput.SelectionLength = endPos;
        }

    }
}
