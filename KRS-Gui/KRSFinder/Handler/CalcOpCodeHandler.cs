using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class CalcOpCodeHandler : IBaseHandler
    {
        /// <summary>
        /// 处理计算相关操作码
        /// </summary>
        /// <param name="lineCode">NEED</param>
        /// <param name="smaliFileAnalyseModule">NONEED</param>
        /// <param name="tempRegister">NEED</param>
        /// <returns>NULL</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            string head = lineCode.Split(" ")[0].Trim();
            string[] blocks = lineCode.Replace(head, "").Split(",");
            string register = blocks[0].Trim();
            tempRegister.removeRegister(register);
            return null;
        }
    }
}
