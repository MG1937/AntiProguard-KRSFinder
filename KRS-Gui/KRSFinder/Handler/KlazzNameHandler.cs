using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class KlazzNameHandler : IBaseHandler
    {
        /// <summary>
        /// 提取.class操作码包含的类名
        /// </summary>
        /// <param name="lineCode"></param>
        /// <returns>(string)当前的klazz名称</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            //.class public LA/B/C;
            string opCode = ".class";
            lineCode = lineCode.Replace(opCode, "").Trim();
            lineCode = lineCode.Substring(lineCode.IndexOf("L") + 1);
            lineCode = lineCode.Substring(0, lineCode.Length - 1);
            //return A/B/C
            return lineCode;
        }
    }
}
