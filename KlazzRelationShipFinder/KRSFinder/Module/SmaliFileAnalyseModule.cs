using KlazzRelationShipFinder.KRSFinder.Base;
using KlazzRelationShipFinder.KRSFinder.Handler;
using KlazzRelationShipFinder.KRSFinder.LogPrinter;
using KlazzRelationShipFinder.KRSFinder.MessageSaver;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlazzRelationShipFinder.KRSFinder.Module
{
    /// <summary>
    /// 所有smali文件中读取的每一行,都应该由此Module具体处理
    /// </summary>
    class SmaliFileAnalyseModule
    {
        private string TAG = "LineAnalyseModule";

        //判断当前文件由什么工具反编译
        private bool toolJudge = false;

        //决定是否录入方法体
        private bool recodeMethodCode = false;

        //方法体代码
        private string methodCode = "";

        //代表当前的klazz名
        public string klazz_name { set; get; }

        //记录当前方法体中的方法名
        public string method { set; get; }

        public void lineAnalyse(string lineCode) {
            //opcode vx,some-dalvik-code
            int OPC = OpCode.getOpCode(lineCode);

            IBaseHandler handler = null;
            //.class public LA/B/C;
            /**
             * 处理smali文件开头部分
             */
            if (OPC == OpCode.KLAZZ_OPC && string.IsNullOrEmpty(method))
            {
                /**
                 * 在处理.class操作码时意味着一个新的smali文件开始读取
                 * 故要对之前储存smali数据的成员进行完全清理
                 */
                handler = new KlazzNameHandler();
                //更换当前klazz值
                klazz_name = (string)handler.lineHandler(lineCode);

                if (klazz_name == null || string.IsNullOrEmpty(klazz_name)) {
                    Log.log(TAG, "klazz is null!!!");
                    throw new Exception("Klazz is null!!!\nLineCode:" + lineCode);
                }
                Log.log(TAG, "Analyse Smali:" + klazz_name);
                return;
            }
            else if (OPC == OpCode.SOURCE_OPC && string.IsNullOrEmpty(method)) {
                /**
                 * 处理到.source操作码时代表当前的原klazz名有可能恢复
                 * 若可恢复,应存储sourceName至MessageSaver模块
                 */
                handler = new SourceNameHandler();

                string source_name = (string)handler.lineHandler(lineCode);

                if (source_name != null) {
                    if (!source_name.Equals(klazz_name)) {
                        SourceNameSaver.saveSourceName(klazz_name, source_name);
                    }
                }

                return;
            }

            /**
             * 对方法体进行判断
             */
            if (OPC == OpCode.METHOD_START && !recodeMethodCode)
            {
                handler = new MethodNameHandler();
                //方法开始时更新当前方法名成员
                method = (string)handler.lineHandler(lineCode);
                //开始录入方法体代码
                recodeMethodCode = true;
                return;
            }
            else if (OPC == OpCode.METHOD_END && recodeMethodCode) {
                Log.log(TAG, method);
                //将方法体传入分析模块
                new MethodCodeAnalyseModule(this,methodCode).execute();
                //方法结束时置空当前方法名成员
                method = null;
                //结束录入方法体代码
                recodeMethodCode = false;
                //方法体置空
                methodCode = "";
                toolJudge = false;
                return;
            }

            //若录入标记为true,则开始录入方法体
            if (recodeMethodCode) {
                methodCode += lineCode + "\n";
                if (!toolJudge&&lineCode.Trim().StartsWith(":")) {
                    toolJudge = true;
                    if (lineCode.Trim().Replace(":", "").StartsWith("L")) {
                        Config.isBakSmali = true;
                    }
                    else
                    {
                        Config.isBakSmali = false;
                    }
                }
                //录入当前行后立刻结束此次函数执行,等待下一行读入
                return;
            }
        }
    }
}
