using KlazzRelationShipFinder.KRSFinder.Base;
using KlazzRelationShipFinder.KRSFinder.MessageSaver;
using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System;

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

            Method thiz = new Method();
            thiz.klazz = smaliFileAnalyseModule.klazz_name;
            thiz.methodName = smaliFileAnalyseModule.method;

            //if (method.klazz.IndexOf("java/lang/StringBuilder") != -1) return null;

            //20220517 增加对传入方法内参数的检查
            //invoke-virtual { v4, v0, v1, v2, v3}, Test2.method5:(IIII)V
            data = data.Substring(data.IndexOf("{") + 1);
            data = data.Substring(0, data.IndexOf("}"));
            if (data.IndexOf("..") != -1)
            {
                string data_temp = "";
                string[] tmp = data.Split("..");
                string pre = tmp[0].Substring(0, 1);
                int tmp_b = Convert.ToInt32(tmp[0].Trim().Substring(1));
                int tmp_e = Convert.ToInt32(tmp[1].Trim().Substring(1));
                for (; tmp_b < tmp_e; tmp_b++)
                {
                    data_temp += pre + tmp_b + ",";
                }
                data_temp += pre + tmp_e;
                data = data_temp;
            }
            if (data.Trim().Equals(""))
            {
                //invoke-static {} xxx
                thiz.addNullArgCallStack(method);
                Var noarg_var = new Var();
                noarg_var.isFuncArg = true;
                noarg_var.var_name = "-1";
                RelationSaver.saveCallStack(noarg_var, thiz);
                return null;
            }
            string[] regs = data.Split(",");
            int index = (head.Contains("static")) ? 0 : 1;

            if (index == regs.Length)
            {
                //invoke-virtual { p0 } xxx
                //即无参函数情况下,也应收集其为调用栈
                thiz.addNullArgCallStack(method);
                Var noarg_var = new Var();
                noarg_var.isFuncArg = true;
                noarg_var.var_name = "-1";
                RelationSaver.saveCallStack(noarg_var, thiz);
            }

            for (; index < regs.Length; index++)
            {
                //污点产生处
                object temp_value = tempRegister.getRegister(regs[index].Trim());
                if (temp_value is Var)
                {
                    if (((Var)temp_value).isFuncArg)
                    {
                        thiz.addCallStack((Var)temp_value, method, index - (head.Contains("static") ? 0 : 1));
                        RelationSaver.saveCallStack((Var)temp_value, thiz);

                        object pollute = tempRegister.getRegister("result");
                        if (pollute is Method)
                        {
                            //产生污点
                            ((Method)pollute).addPollute((Var)temp_value);
                            tempRegister.putRegister("result", new TempRegister(pollute));
                        }
                        continue;
                    }

                    ((Var)temp_value).addComment_dataFlowIn(method, smaliFileAnalyseModule.klazz_name, smaliFileAnalyseModule.method);
                    RelationSaver.saveVar((Var)temp_value);
                }
                else if (temp_value is Method)
                {
                    //检查是否携带污点
                    foreach (Var p in ((Method)temp_value).carryPollute)
                    {
                        thiz.addCallStack(p, method, index - (head.Contains("static") ? 0 : 1));
                        RelationSaver.saveCallStack(p, thiz);
                    }
                }
                else
                {
                    //寄存器不可识别时仍储存调用栈
                    thiz.addNullArgCallStack(method);
                    Var noarg_var = new Var();
                    noarg_var.isFuncArg = true;
                    noarg_var.var_name = "-1";
                    RelationSaver.saveCallStack(noarg_var, thiz);
                }
            }

            return null;
        }
    }
}
