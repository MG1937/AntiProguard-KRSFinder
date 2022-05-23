using KlazzRelationShipFinder.KRSFinder.Base;
using KlazzRelationShipFinder.KRSFinder.MessageSaver;
using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class PutOpCodeHandler : IBaseHandler
    {
        /// <summary>
        /// 分析put操作码
        /// </summary>
        /// <param name="lineCode">NEED!</param>
        /// <param name="method">NEED!</param>
        /// <param name="tempRegister">NEED!</param>
        /// <returns>null</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            string head = lineCode.Split(" ")[0].Trim();

            string[] blocks = lineCode.Replace(head, "").Trim().Split(",");
            if (head.Contains("aput"))
            {
                //aput vx,vy,vz
                //aput的目标寄存器应该被废弃
                string register = blocks[1].Trim();
                tempRegister.removeRegister(register);
                return null;
            }
            else if (head.Contains("sput"))
            {
                //sput vx, field_id
                /**
                 * GaborPaller DalvikOpcodes
                 * Puts vx into a static field.
                 */
                string register = blocks[0].Trim();
                object reg_data = tempRegister.getRegister(register);
                if (reg_data == null) return null;
                if (!(reg_data is Var) && !(reg_data is string)) return null;

                string data = blocks[1];
                string klazz = Utils.getKlazz(data);
                string var_name = Utils.getReferedVar(data);

                Var var = new Var();
                var.var_name = var_name;
                var.klazz = klazz;
                var.addComment_setValue(reg_data, smaliFileAnalyseModule.klazz_name, smaliFileAnalyseModule.method);
                RelationSaver.saveVar(var);
                return null;
            }
            else if (head.Contains("iput"))
            {
                //iput vx,vy, field_id
                /**
                 * GaborPaller DalvikOpcodes
                 * Puts vx into an instance field. The instance is referenced by vy.
                 */
                string register = blocks[0].Trim();
                object reg_data = tempRegister.getRegister(register);
                if (reg_data == null) return null;
                //20220517 新增对方法赋值的支持
                if (!(reg_data is Var) && !(reg_data is Method) && !(reg_data is string)) return null;

                string data = blocks[2];
                string klazz = Utils.getKlazz(data);
                string var_name = Utils.getReferedVar(data);

                Var var = new Var();
                var.var_name = var_name;
                var.klazz = klazz;
                if (reg_data is Var && ((Var)reg_data).isFuncArg)
                {
                    //被覆盖寄存器为方法参数
                    tempRegister.removeRegister(register);
                    tempRegister.putRegister(register, new TempRegister(var));
                    return null;
                }
                var.addComment_setValue(reg_data, smaliFileAnalyseModule.klazz_name, smaliFileAnalyseModule.method);
                RelationSaver.saveVar(var);
                return null;
            }

            return null;
        }
    }
}
