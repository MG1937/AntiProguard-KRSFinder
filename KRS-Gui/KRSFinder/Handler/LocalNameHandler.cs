using KlazzRelationShipFinder.KRSFinder.Base;
using KlazzRelationShipFinder.KRSFinder.MessageSaver;
using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class LocalNameHandler : IBaseHandler
    {
        /// <summary>
        /// 处理.local操作码
        /// </summary>
        /// <param name="lineCode">NEED</param>
        /// <param name="smaliFileAnalyseModule">NEED</param>
        /// <param name="tempRegister">NEED</param>
        /// <returns>null</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            //.local vx,"var_name":LXXX;
            string[] blocks = lineCode.Replace(".local", "").Split(",");
            string register = blocks[0].Trim();
            string local_var_name = blocks[1].Split(":")[0].Trim().Replace("\"","");

            //获取成员对象
            object var = tempRegister.getRegister(register);
            if (!(var is Var)) return null;

            //储存成员对象
            ((Var)var).addComment_beSetLocalName(local_var_name, smaliFileAnalyseModule.klazz_name, smaliFileAnalyseModule.method);
            RelationSaver.saveVar((Var)var);
            return null;
        }
    }
}
