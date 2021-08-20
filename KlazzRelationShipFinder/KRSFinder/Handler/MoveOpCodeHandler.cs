using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using KlazzRelationShipFinder.KRSFinder.Base;
using System;
using System.Collections.Generic;
using System.Text;
using KlazzRelationShipFinder.KRSFinder.MessageSaver;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class MoveOpCodeHandler : IBaseHandler
    {
        /// <summary>
        /// 处理move操作码
        /// </summary>
        /// <param name="lineCode">NEED</param>
        /// <param name="smaliFileAnalyseModule">NEED</param>
        /// <param name="tempRegister">NEED</param>
        /// <returns>null</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            //注意1move也可以有赋值行为!
            string head = lineCode.Split(" ")[0];
            string[] blocks = lineCode.Replace(head, "").Trim().Split(",");
            object value;
            //move-result vx
            if (head.Contains("result"))
            {
                string register = blocks[0].Trim();
                value = tempRegister.getRegister("result");
                if (value == null) return null;
                tempRegister.removeRegister("result");
                tempRegister.putRegister(register, new TempRegister(value));
                return null;
            }
            else if (head.Contains("exception")) {
                //move-exception vx
                string register = blocks[0].Trim();
                tempRegister.removeRegister(register);
                return null;
            }
            //move vx,vy
            string vx = blocks[0].Trim();
            string vy = blocks[1].Trim();

            object temp_value = tempRegister.getRegister(vx);

            value = tempRegister.getRegister(vy);
            if (value == null) {
                if(!(temp_value is Var)) tempRegister.removeRegister(vx);
                return null; }

            tempRegister.removeRegister(vy);


            //若vx数据类型为成员对象
            if (temp_value is Var)
            {
                ((Var)temp_value).addComment_setValue(value, smaliFileAnalyseModule.klazz_name, smaliFileAnalyseModule.method);
                RelationSaver.saveVar((Var)temp_value);
            }
            else 
            {
                tempRegister.putRegister(vx, new TempRegister(value));
            }

            return null;
        }
    }
}
