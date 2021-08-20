using KlazzRelationShipFinder.KRSFinder.Base;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

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
        /// 为指定类保存其成员
        /// </summary>
        /// <param name="var">欲添加的成员对象</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void saveVar(Var var) {
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

            foreach (Var v in temp_vars) {
                if (v.var_name.Equals(var_name)) {
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
