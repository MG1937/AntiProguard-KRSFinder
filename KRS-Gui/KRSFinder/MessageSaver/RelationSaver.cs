using KlazzRelationShipFinder.KRSFinder.Base;
using KRS_Gui.KRSFinder.MessageSaver;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KlazzRelationShipFinder.KRSFinder.MessageSaver
{
    /// <summary>
    /// 储存各个类,变量间的关系
    /// 注意,该类可能会在多线程的情况下储存类信息
    /// 应当注意线程锁的设计
    /// </summary>
    public class RelationSaver
    {
        /// <summary>
        /// 全局对象,用以储存各个类中成员的关系
        /// Key:class_name
        /// </summary>
        public static Dictionary<string, List<Var>> relations = new Dictionary<string, List<Var>>();

        /// <summary>
        /// 储存调用栈
        /// Key:class_name
        /// </summary>
        public static Dictionary<string, List<Method>> methodCallStack = new Dictionary<string, List<Method>>();

        public static string convertRelationToJson()
        {
            if (!SaverConfig.saveMembership) return "{}";
            JObject Json = new JObject();

            foreach (string klazz in relations.Keys)
            {
                JObject data = new JObject();
                foreach (Var var in relations[klazz])
                {
                    data.Add(var.var_name, new JArray(var.comments));
                }
                Json.Add(klazz, data);
            }
            return Json.ToString();
        }

        public static string convertCallStackToJson()
        {
            if (!SaverConfig.saveDataFlow) return "{}";
            JObject Json = new JObject();
            foreach (string klazz in methodCallStack.Keys)
            {
                JObject ms = new JObject();
                foreach (Method method in methodCallStack[klazz])
                {
                    string methodName = method.methodName;
                    JObject stacks = new JObject();
                    foreach (string px in method.flow.Keys)
                    {
                        stacks.Add(px, new JArray(method.flow[px]));
                    }
                    ms.Add(methodName, stacks);
                }
                Json.Add(klazz, ms);
            }
            return Json.ToString();
        }

        public static void Clear()
        {
            relations.Clear();
            methodCallStack.Clear();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void saveCallStack(Var var, Method method)
        {
            if (!SaverConfig.saveDataFlow) return;
            string klazz = method.klazz;
            List<Method> temp_ms = methodCallStack.GetValueOrDefault(klazz, null);
            if (temp_ms == null)
            {
                methodCallStack[klazz] = new List<Method> { (Method)method.Clone() };
                method.Clear();
                return;
            }

            foreach (Method m in temp_ms)
            {
                if (m.methodName.Equals(method.methodName))
                {
                    m.addCallStacks(var, method);
                    method.Clear();
                    return;
                }
            }

            temp_ms.Add((Method)method.Clone());
            method.Clear();
        }

        /// <summary>
        /// 为指定类保存其成员
        /// </summary>
        /// <param name="var">欲添加的成员对象</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void saveVar(Var var)
        {
            if (!SaverConfig.saveMembership) return;
            if (var.isFuncArg) return;
            string klazz_name = var.klazz;
            List<Var> temp_vars = relations.GetValueOrDefault(klazz_name, null);

            if (temp_vars == null)
            {   //若指定类无成员,直接添加成员对象
                relations[klazz_name] = new List<Var> { (Var)var.Clone() };
                //清理原成员对象的comments列表
                var.clearComments();
                return;
            }

            //欲添加的成员名
            string var_name = var.var_name;

            foreach (Var v in temp_vars)
            {
                if (v.var_name.Equals(var_name))
                {
                    //Console.WriteLine(var_name);
                    //若目标成员集中已有欲添加的成员
                    //则直接向目标成员集中追加欲添加成员的数据
                    v.addComments(var.comments);
                    //清理成员对象的comments列表
                    var.clearComments();
                    return;
                }
            }

            //若在目标成员集中没有找到与欲添加成员相同的成员名
            temp_vars.Add((Var)var.Clone());
            //清理成员对象的comments列表
            var.clearComments();
        }
    }
}
