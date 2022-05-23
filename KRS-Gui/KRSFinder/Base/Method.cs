using KRS_Gui.KRSFinder.MessageSaver;
using System;
using System.Collections.Generic;

namespace KlazzRelationShipFinder.KRSFinder.Base
{
    public class Method
    {
        /// <summary>
        /// 当前方法的隶属类
        /// </summary>
        public string klazz { set; get; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string methodName { set; get; }

        /// <summary>
        /// 参数流入栈
        /// Key:px
        /// </summary>
        public Dictionary<string, List<string>> flow = new Dictionary<string, List<string>>();

        /// <summary>
        /// 携带污点列表,污点由invoke处产生
        /// </summary>
        public List<Var> carryPollute = new List<Var>();

        public void addPollute(Var var)
        {
            if (var.isFuncArg && !carryPollute.Contains(var))
            {
                carryPollute.Add(var);
            }
        }

        public override bool Equals(object obj)
        {
            try
            {
                Method m = (Method)obj;
                if (m.klazz.Equals(this.klazz) && m.methodName.Equals(this.methodName))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 添加调用栈
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="flowIn"></param>
        /// <param name="indexOfFlowIn">流入目标函数的参数下标</param>
        public void addCallStack(Var arg, Method flowIn, int indexOfFlowIn)
        {
            if (!SaverConfig.saveDataFlow) return;
            //此处传入的名为arg的Var对象,是为了记录作为污染源的方法参数.
            //flowIn作为污染源最终流入的目标方法
            //indexOfFlowIn为污染源再次作为方法参数流入目标方法的指定下标
            if (!arg.isFuncArg) return;
            string arg_p = arg.var_name;
            string flow_str = flowIn.klazz + ";->" + flowIn.methodName + ";->" + indexOfFlowIn;
            List<string> stack = flow.ContainsKey(arg_p) ? flow[arg_p] : new List<string>();
            if (!stack.Contains(flow_str))
            {
                stack.Add(flow_str);
            }
            flow[arg_p] = stack;
        }

        public void addNullArgCallStack(Method flowIn)
        {
            if (!SaverConfig.saveDataFlow) return;
            string flow_str = flowIn.klazz + ";->" + flowIn.methodName + ";->-1";
            List<string> stack = flow.ContainsKey("-1") ? flow["-1"] : new List<string>();
            if (!stack.Contains(flow_str))
            {
                stack.Add(flow_str);
            }
            flow["-1"] = stack;
        }

        public void addCallStacks(Var arg, Method flowIn)
        {
            if (!SaverConfig.saveDataFlow) return;
            if (!arg.isFuncArg) return;
            string arg_p = arg.var_name;
            List<string> flowin_stacks = flowIn.flow.GetValueOrDefault(arg_p, new List<string>());
            List<string> stacks = flow.GetValueOrDefault(arg_p, null);
            if (stacks == null)
            {
                stacks = new List<string>(flowIn.flow.GetValueOrDefault(arg_p, new List<string>()));
                flow[arg_p] = stacks;
                return;
            }
            foreach (string str in flowin_stacks)
            {
                if (!stacks.Contains(str))
                {
                    stacks.Add(str);
                }
            }
            flow[arg_p] = stacks;
        }

        public void Clear()
        {
            flow.Clear();
        }

        public object Clone()
        {
            Method m = (Method)MemberwiseClone();
            m.flow = new Dictionary<string, List<string>>(flow);
            return m;
        }
    }
}
