using System;
using System.Collections.Generic;
using System.Text;

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

        public override bool Equals(object obj)
        {
            try {
                Method m = (Method)obj;
                if (m.klazz.Equals(this.klazz) && m.methodName.Equals(this.methodName)) {
                    return true;
                }
            }
            catch (Exception) {
                return false;
            }
            return false;
        }
    }
}
