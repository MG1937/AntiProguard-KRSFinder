using KlazzRelationShipFinder.KRSFinder.Base;
using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class InvokeOpCodeHandler : IBaseHandler
    {
        /// <summary>
        /// 处理invoke操作码
        /// </summary>
        /// <param name="lineCode">NEED</param>
        /// <param name="smaliFileAnalyseModule">NONEED</param>
        /// <param name="tempRegister">NEED</param>
        /// <returns>null</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            string head = lineCode.Split(" ")[0].Trim();
            string data = lineCode.Replace(head, "").Trim();

            string method_name = Utils.getMethodName(data);
            string method_klazz = Utils.getKlazz(data);

            Method method = new Method();
            method.klazz = method_klazz;
            method.methodName = method_name;

            tempRegister.putRegister("result", new TempRegister(method));
            return null;
        }
    }
}
