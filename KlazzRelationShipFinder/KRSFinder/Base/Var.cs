using System;
using System.Collections.Generic;
using System.Text;

namespace KlazzRelationShipFinder.KRSFinder.Base
{
    /// <summary>
    /// 模拟成员对象,储存数据
    /// </summary>
    public class Var : ICloneable
    {
        /// <summary>
        /// 成员名
        /// </summary>
        public string var_name { set; get; }

        /// <summary>
        /// 成员的数据类型:TODO
        /// </summary>
        //public string var_type { set; get; }

        /// <summary>
        /// 当前成员隶属类
        /// </summary>
        public string klazz { set; get; }

        /// <summary>
        /// 关于成员的其他注释
        /// </summary>
        public List<string> comments = new List<string>();

        public void clearComments() {
            comments.Clear();
        }

        public void addComment(string comment) {
            if (!this.comments.Contains(comment))
            {
                this.comments.Add(comment);
            }
        }

        /// <summary>
        /// 在某方法中被赋予某值
        /// </summary>
        /// <param name="var">当前成员赋值对象</param>
        /// <param name="klazz">赋值方法隶属类</param>
        /// <param name="methodName">赋值操作隶属方法</param>
        public void addComment_setValueByVar(Var var,string klazz,string methodName) {
            string comment = null;
            if (klazz.Equals(this.klazz))
            {
                comment = "曾在自身类的" + methodName + "方法中被赋予";
            }
            else 
            {
                comment = "曾在" + klazz + "#" + methodName + "方法中被赋予";
            }

            if (var.klazz.Equals(this.klazz))
            {
                comment += "自身类的" + var.var_name + "成员";
            }
            else 
            {
                comment += var.klazz + "的" + var.var_name + "成员";
            }
            addComment(comment);
        }

        /// <summary>
        /// 在某方法中被赋予某方法的返回数据
        /// </summary>
        /// <param name="method"></param>
        /// <param name="klazz">赋值方法隶属类</param>
        /// <param name="methodName">赋值方法</param>
        public void addComment_setValueByMethod(Method method, string klazz, string methodName) {
            string comment = null;
            if (klazz.Equals(this.klazz))
            {
                comment = "曾在自身类的" + methodName + "方法中";
            }
            else 
            {
                comment = "曾在" + klazz + "#" + methodName + "方法中";
            }

            if (method.klazz.Equals(this.klazz))
            {
                comment += "被赋予自身类的" + method.methodName + "方法的返回数据";
            }
            else 
            {
                comment += "被赋予" + method.klazz + "#" + method.methodName + "方法的返回数据";
            }
            addComment(comment);
        }

        /// <summary>
        /// 在某方法中被赋予某常量
        /// </summary>
        /// <param name="conststr"></param>
        /// <param name="klazz">进行赋值操作时的所在类</param>
        /// <param name="methodName">所在类的具体方法</param>
        public void addComment_setValueByConstStr(string conststr,string klazz, string methodName) {
            string comment = null;
            if (klazz.Equals(this.klazz))
            {
                comment = "曾在自身类的" + methodName + "方法中被赋予常量" + conststr;
            }
            else 
            {
                comment = "曾在" + klazz + "#" + methodName + "方法中被赋予常量" + conststr;
            }
            addComment(comment);
        }

        /// <summary>
        /// 成员对象在某个方法中被设置为局部变量
        /// </summary>
        /// <param name="local_var_name">被设置的变量名</param>
        /// <param name="klazz">方法隶属类</param>
        /// <param name="methodName">方法名</param>
        public void addComment_beSetLocalName(string local_var_name, string klazz, string methodName) {
            string comment = null;
            if (klazz == this.klazz)
            {
                comment = "曾在自身类的" + methodName + "方法中被命名为'" + local_var_name + "'的局部变量";
            }
            else 
            {
                comment = "曾在" + klazz + "#" + methodName + "方法中被命名为'" + local_var_name + "'的局部变量";
            }
            addComment(comment);
        }

        public void addComment_setValue(object data,string klazz, string methodName) {
            if (data is Var)
            {
                addComment_setValueByVar((Var)data, klazz, methodName);
            }
            else if (data is string)
            {
                addComment_setValueByConstStr((string)data, klazz, methodName);
            }
            else if (data is Method) 
            {
                addComment_setValueByMethod((Method)data, klazz, methodName);
            }
        }

        public void addComments(List<string> comments) {
            foreach (string c in comments) {
                if (!this.comments.Contains(c)) {
                    this.comments.Add(c);
                }
            }
        }

        public override bool Equals(object t) {
            try
            {
                Var temp = (Var)t;
                if (klazz == temp.klazz && temp.var_name == var_name && temp.comments.Count == comments.Count)
                {
                    for (int i = 0; i < temp.comments.Count; i++)
                    {
                        if (!temp.comments[i].Equals(comments[i]))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception) {
                return false;
            }
            
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            Var v = (Var)MemberwiseClone();
            v.comments = new List<string>(this.comments);
            return v;
        }
    }
}
