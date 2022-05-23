using KlazzRelationShipFinder.KRSFinder.Base;
using KRS_Gui;
using System;

namespace KlazzRelationShipFinder.KRSFinder.LogPrinter
{
    class Log
    {
        public static void log(string log)
        {
            //Console.WriteLine("[" + TAG + "]" + ":" + log);
            Program.gui.AppendText(log);
        }

        public static void log(string TAG, string log)
        {
            //Console.WriteLine("[" + TAG + "]" + ":" + log);
            //Program.gui.AppendText("[" + TAG + "]" + ":" + log);
        }

        public static void show(int sum)
        {
            Program.gui.ShowText(sum + "/" + Config.totalFiles);
        }

        public static void show(string sum)
        {
            Program.gui.ShowText(sum + "/" + Config.totalFiles);
        }
    }
}
