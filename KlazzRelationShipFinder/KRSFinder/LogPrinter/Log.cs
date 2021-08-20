using System;
using System.Collections.Generic;
using System.Text;

namespace KlazzRelationShipFinder.KRSFinder.LogPrinter
{
    class Log
    {
        public static void log(string TAG,string log) {
            Console.WriteLine("[" + TAG + "]" + ":" + log);
        }
    }
}
