using KlazzRelationShipFinder.KRSFinder.Base;
using KRS_Gui;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlazzRelationShipFinder.KRSFinder.LogPrinter
{
    class Log
    {
        public static void log(string TAG,string log) {
            //Console.WriteLine("[" + TAG + "]" + ":" + log);
            //Program.gui.AppendText(log);
        }

        public static void show(int sum) 
        {
            Program.gui.AppendText(sum + "/" + Config.totalFiles);
        }

        public static void show(string sum)
        {
            Program.gui.AppendText(sum + "/" + Config.totalFiles);
        }
    }
}
