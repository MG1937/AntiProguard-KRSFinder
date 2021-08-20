using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class SourceNameHandler : IBaseHandler
    {
        /// <summary>
        /// 获取.source操作码的原文件名
        /// 若获取失败或当前文件名可能被ProGuard保护则返回null
        /// </summary>
        /// <param name="lineCode"></param>
        /// <returns>(string)当前的sourceName,e.g:SourceName</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            //.source "Example.java"
            if (lineCode == null) return null;

            string opCode = ".source";
            lineCode = lineCode.Trim();

            try
            {
                lineCode = lineCode.Replace(opCode, "");
                //Example.java
                string sourceName = lineCode.Substring(lineCode.IndexOf("\"") + 1, lineCode.Length - 2);

                //ProGuard!!!
                if (!sourceName.EndsWith(".java")) return null;

                sourceName = sourceName.Replace(".java", "");

                if (sourceName != null && !string.IsNullOrEmpty(sourceName)) return sourceName;
            }
            catch (Exception) {
                return null;
            }
            return null;
        }
    }
}
