using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System.Text.RegularExpressions;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class MethodNameHandler : IBaseHandler
    {
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            //.method public A(III)LA/B/C;
            Regex reg = new Regex("(\\w+\\(.*\\))");
            if (lineCode.Contains("constructor"))
            {
                reg = new Regex("constructor\\s+(.+\\(.*\\))");
            }
            Match match = reg.Match(lineCode);
            return match.Groups[1].Value;
        }
    }
}
