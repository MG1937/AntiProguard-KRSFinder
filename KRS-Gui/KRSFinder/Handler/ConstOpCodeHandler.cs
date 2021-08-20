using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class ConstOpCodeHandler : IBaseHandler
    {
        /// <summary>
        /// 处理const相关操作码
        /// </summary>
        /// <param name="lineCode">NEED</param>
        /// <param name="smaliFileAnalyseModule">NONEED</param>
        /// <param name="tempRegister">NEED</param>
        /// <returns>NULL</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            string head = lineCode.Split(" ")[0].Trim();
            string[] blocks = lineCode.Replace(head, "").Split(",");
            if (head.Equals("const-string"))
            {
                //const-string vx,"str"
                //若操作码设置string类型的常量,则储存相应寄存器
                string register = blocks[0].Trim();
                string conststr = blocks[1].Trim();

                tempRegister.putRegister(register, new TempRegister(conststr));
                return null;
            }
            else 
            {
                //若储存非string类型的常量,则删除目标寄存器
                string register = blocks[0].Trim();
                tempRegister.removeRegister(register);
                return null;
            }
        }
    }
}
