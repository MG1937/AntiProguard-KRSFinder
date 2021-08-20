using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace KlazzRelationShipFinder.KRSFinder.Module.Smali
{
    class Utils
    {
        /// <summary>
        /// 从代码中提取class信息
        /// </summary>
        /// <param name="code">xxx LA/B/C;->xxx</param>
        /// <returns>A/B/C</returns>
        public static string getKlazz(string code) {
            Regex reg = new Regex("L(.*?);");
            return reg.Match(code).Groups[1].Value;
        }

        /// <summary>
        /// 从代码中提取成员名
        /// </summary>
        /// <param name="code">opcode LA/B/C;->xxx:LA/B/C;</param>
        /// <returns>xxx</returns>
        public static string getReferedVar(string code) {
            Regex reg = new Regex(";->(.+):");
            return reg.Match(code).Groups[1].Value;
        }

        /// <summary>
        /// 从代码中提取调用的方法名
        /// </summary>
        /// <param name="code">invoke LA/B/C;->a()V</param>
        /// <returns>a()</returns>
        public static string getMethodName(string code) {
            Regex reg = new Regex("->(.+\\(.+\\))");
            return reg.Match(code).Groups[1].Value;
        }
    }
}
