using KlazzRelationShipFinder.KRSFinder.Base;
using KlazzRelationShipFinder.KRSFinder.Handler;
using KlazzRelationShipFinder.KRSFinder.MessageSaver;
using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System.Text.RegularExpressions;

namespace KRS_Gui.KRSFinder.Handler
{
    class FieldOpCodeHandler : IBaseHandler
    {
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            if (!lineCode.Contains("=")) return null;
            string value = lineCode.Split("=")[1].Trim();
            if (!value.StartsWith("\""))
            {
                return null;
            }

            Regex regex = new Regex("(\\w+):L");
            string var_name = regex.Match(lineCode).Groups[1].Value;
            Var var = new Var();
            var.klazz = smaliFileAnalyseModule.klazz_name;
            var.var_name = var_name;
            var.addComment_setStaticField(value);

            RelationSaver.saveVar(var);
            return null;
        }
    }
}
