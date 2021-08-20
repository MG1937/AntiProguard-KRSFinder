using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    interface IBaseHandler
    {
        /// <summary>
        /// 处理单行Smali代码
        /// </summary>
        /// <param name="lineCode">单行Smali代码</param>
        /// <returns>返回处理结果</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null);
    }
}
