using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class GotoOpCodeHandler : IBaseHandler
    {
        /// <summary>
        /// GOTO相关操作码处理
        /// </summary>
        /// <param name="lineCode">NEED</param>
        /// <param name="smaliFileAnalyseModule">NONEED</param>
        /// <param name="tempRegister">NONEED</param>
        /// <returns>GOTO_target</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            string head = lineCode.Split(" ")[0].Trim();
            lineCode = lineCode.Replace(head + " ", "").Trim();

            Regex reg = new Regex(":(.+)");
            return reg.Match(lineCode).Groups[1].Value.Trim();
        }
    }
}
