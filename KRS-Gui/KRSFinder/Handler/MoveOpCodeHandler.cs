using KlazzRelationShipFinder.KRSFinder.Base;
using KlazzRelationShipFinder.KRSFinder.MessageSaver;
using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System;

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
            //注意!move也可以有赋值行为!
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
                tempRegister.removeRegister(register);
                tempRegister.putRegister(register, new TempRegister(value));
                return null;
            }
            else if (head.Contains("exception"))
            {
                //move-exception vx
                string register = blocks[0].Trim();
                tempRegister.removeRegister(register);
                return null;
            }
            //move vx,vy
            string vx = blocks[0].Trim();
            string vy = blocks[1].Trim();

            value = tempRegister.getRegister(vy);

            //判断方法参数
            //local vx = px时,vx作为被px污染的参数继续传播
            //px = local vx时,认为px已经被普通局部变量污染,传播中止.
            if (vy.Substring(0, 1).Equals("p"))
            {
                if ((!smaliFileAnalyseModule.isStatic && vy.Equals("p0")) || smaliFileAnalyseModule.polluteFuncArgReg.Contains(vy))
                {; }
                else if (value == null)
                {
                    //px寄存器未被占用时,可作为方法参数
                    Var arg_p = new Var();
                    arg_p.isFuncArg = true;
                    int index = Convert.ToInt32(vy.Substring(1)) + ((smaliFileAnalyseModule.isStatic) ? 1 : 0);
                    arg_p.var_name = index + "";
                    tempRegister.putRegister(vy, new TempRegister(arg_p));
                }
                else
                {
                    //px寄存器被占用,且明确为非方法参数时标记被污染寄存器
                    if ((value is Var && !((Var)value).isFuncArg) || !(value is Var))
                    {
                        smaliFileAnalyseModule.polluteFuncArgReg.Add(vy);
                    }
                }
            }

            object temp_value = tempRegister.getRegister(vx);

            if (value == null)
            {
                if (!(temp_value is Var))
                {
                    //move vx,vy时vy为空,且vx非Var时则没有必要储存vx
                    tempRegister.removeRegister(vx);
                    smaliFileAnalyseModule.polluteFuncArgReg.Add(vx);
                }
                return null;
            }

            tempRegister.removeRegister(vy);


            //若vx数据类型为成员对象
            if (temp_value is Var)
            {
                if (value is Var && ((Var)value).isFuncArg)
                {
                    //若vx处为方法参数,则标记污染
                    smaliFileAnalyseModule.polluteFuncArgReg.Add(vx);
                    tempRegister.removeRegister(vx);
                    //方法参数已被覆盖
                    tempRegister.putRegister(vx, new TempRegister(value));
                    return null;
                }
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
