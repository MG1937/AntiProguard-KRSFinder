using KlazzRelationShipFinder.KRSFinder.Base;
using KlazzRelationShipFinder.KRSFinder.Handler;
using KlazzRelationShipFinder.KRSFinder.LogPrinter;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace KlazzRelationShipFinder.KRSFinder.Module
{
    class MethodCodeAnalyseModule
    {
        public const string TAG = "MethodCodeAnalyseModule";

        //当forceBreak到达阀值时直接跳出死循环
        int forceBreak = 0;

        private SmaliFileAnalyseModule smaliFileAnalyseModule { set; get; }

        //主寄存器集
        //private TempRegisterMap mainRegisterMap = new TempRegisterMap();

        //分析地图,每个单次分析开始时需要将已分析路线录入此对象!!
        private List<string> analysedMap = new List<string>();

        //记录goto操作码循环状态的目标区块
        //private List<string> loopGoto = new List<string>();

        //储存方法区块代码
        private Dictionary<string, string> methodBlocks = new Dictionary<string, string>();

        //储存其他区块需要使用的临时寄存器集
        //Key:L0
        //Dictionary<string, List<TempRegisterMap>>
        private Dictionary<string, Dictionary<TempRegisterMap, List<string>>> tempRegisterMap = new Dictionary<string, Dictionary<TempRegisterMap, List<string>>>();

        private Dictionary<string, List<string>> registerAnalysedMap = new Dictionary<string, List<string>>();

        //为每个寄存器储存相应的分析地图
        //private Dictionary<TempRegisterMap, List<string>> registerAnalysedMap = new Dictionary<TempRegisterMap, List<string>>();

        //储存switch的转跳内容
        //Key:L0
        private Dictionary<string, List<string>> switchs = new Dictionary<string, List<string>>();

        //goto操作码的目标区块
        private string gotoTarget = null;

        public MethodCodeAnalyseModule(SmaliFileAnalyseModule smaliFileAnalyseModule, string methodCode) {
            this.smaliFileAnalyseModule = smaliFileAnalyseModule;
            if (Config.isBakSmali)
            {
                methodCode = "  :TOP\n" + methodCode + "\n  :";
            }
            else {
                methodCode = "    :TOP\n" + methodCode + "\n    :";
            }

            Dictionary<TempRegisterMap, List<string>> top = new Dictionary<TempRegisterMap, List<string>>();
            top[new TempRegisterMap()] = analysedMap;
            tempRegisterMap["TOP"] = top;

            /**
             *  :L6
             *  .sparse-switch
             *   0 -> :L4
             *   200 -> :L5
             *  .end sparse-switch
             * 
             *  Key:L6
             *  List{L4,L5}
             */
            //在正式分析方法体前捕获switch相关内容
            //baksmali:^\\s{2}:(\\w{2,})\\n^\\s+\\.\\w+?-switch.*?\\n(.+?)\\n\\s{2}\\.
            string reg_str = "^\\s{2}:(\\w{2,})\\n^\\s+\\.\\w+?-switch.*?\\n(.+?)\\n\\s{2}\\.";
            if (!Config.isBakSmali) reg_str = "^\\s{4}:(\\w{2,})\n^\\s+\\.\\w+?-switch.*?\n(.+?)\\n\\s{4}\\.";
            Regex regex = new Regex(reg_str, RegexOptions.Multiline|RegexOptions.Singleline);
            Dictionary<string, string> temp_switchs = new Dictionary<string, string>();
            foreach (Match m in regex.Matches(methodCode)) {
                temp_switchs[m.Groups[1].Value] = m.Groups[2].Value;
            }

            if (temp_switchs.Count != 0) {
                //提取switch内容的转跳方法区块
                Regex block = new Regex(":(\\w+)");
                foreach (string k in temp_switchs.Keys) {
                    List<string> switch_block = new List<string>();
                    foreach (Match m in block.Matches(temp_switchs[k])) {
                        //获取每个switch内容的转跳区块
                        switch_block.Add(m.Groups[1].Value);
                    }
                    switchs[k] = switch_block;
                }

                //替换所有switch分支为if分支,方便分析
                foreach (string b in switchs.Keys) {
                    string if_switch = "";
                    foreach (string goto_ in switchs[b]) {
                        if_switch += "   if-switch,:" + goto_ + "\n";
                    }
                    Regex reg = new Regex("^\\s+.+switch.+:" + b,RegexOptions.Multiline);
                    methodCode = reg.Replace(methodCode, if_switch);
                }
            }

            //开始分割方法区块
            string reg_str1 = "^\\s{2}:(\\w+)";
            if(!Config.isBakSmali) reg_str1 = "^\\s{4}:(\\w+)";
            Regex blockName = new Regex(reg_str1,RegexOptions.Multiline);
            foreach (Match m in blockName.Matches(methodCode)) {
                string key = m.Groups[1].Value;
                //根据方法区块名match对应的方法区块
                string reg_str2 = "(?:^\\s{2}:" + key + ")\\n(.+?)(?:^\\s{2}:)";
                if (!Config.isBakSmali) reg_str2 = "(?:^\\s{4}:" + key + ")\\n(.+?)(?:^\\s{4}:)";
                Regex methodCode_ = new Regex(reg_str2, RegexOptions.Multiline | RegexOptions.Singleline);
                Match match = methodCode_.Match(methodCode);
                
                methodBlocks[key] = match.Groups[1].Value;
            }

            //Log.log(TAG, "Construct Over");
        }

        /// <summary>
        /// 执行器
        /// </summary>
        public void execute() {
            while (tempRegisterMap.Count != 0) {
                if (forceQuit()) break;

                Dictionary<string, Dictionary<TempRegisterMap, List<string>>> temp = new Dictionary<string, Dictionary<TempRegisterMap, List<string>>>(tempRegisterMap);
                //取出临时寄存器集中保存的方法区块名
                foreach (string k in temp.Keys) {
                    if (forceQuit()) break;

                    //取出此区块名下所有保存的临时寄存器
                    Dictionary<TempRegisterMap, List<string>> tempList = new Dictionary<TempRegisterMap, List<string>>(temp[k]);
                    foreach (TempRegisterMap registerMap in tempList.Keys) {
                        if (forceQuit()) break;

                        //在每个单次分析开始前清理一次路线对象
                        analysedMap.Clear();
                        if (tempRegisterMap[k].ContainsKey(registerMap)) {
                            analysedMap = tempRegisterMap[k][registerMap];
                        }
                        bool gotoLoop = false;//标记由于goto操作码造成的源码层面上的循环
                        bool begin = false;
                        do
                        {
                            //遍历每个方法区块
                            foreach (string blockName in methodBlocks.Keys)
                            {
                                if (forceQuit()) return;
                                //若当前区块名与保持寄存器的区块名不一致
                                //则认为当前区块不匹配当前取出的寄存器,继续查找下一个区块
                                if (!begin && blockName.Equals(k))
                                {
                                    begin = true;
                                }
                                else if(!begin && !blockName.Equals(k)) {
                                    continue;
                                }

                                if (gotoTarget != null)
                                {
                                    //若当前已经被标记loop,且目标仍然在分析路线中
                                    //则认为单次分析已经没有必要,清理goto相关数据后直接跳出循环
                                    if (gotoLoop && analysedMap.Contains(gotoTarget)) {
                                        gotoLoop = false;
                                        //loopGoto.Add(gotoTarget);
                                        gotoTarget = null;
                                        break;
                                    }

                                    //若goto操作码标记的目标区块非当前区块,则直接跳过当前区块的分析
                                    if (!blockName.Equals(gotoTarget))
                                    {
                                        continue;
                                    }
                                    else {
                                        //若在goto标记的目标区块符合当前遍历到的方法区块
                                        //并且在分析线路中已经包含该区块(该区块已经被分析过一遍)
                                        if (!gotoLoop && analysedMap.Contains(gotoTarget)) {
                                            gotoLoop = true;//启用loop标记
                                        }
                                    }
                                }

                                gotoTarget = null;

                                if (tempRegisterMap.ContainsKey(k)) tempRegisterMap[k].Remove(registerMap);

                                //为路线储存当前的区块名
                                if (!analysedMap.Contains(blockName)) analysedMap.Add(blockName);

                                //分析当前方法区块
                                methodAnalyse(blockName, registerMap);
                            }
                        } while (gotoTarget != null && !forceQuit());//若gotoTarget不为空则表示仍然需要循环一次来寻找目标区块
                        
                        //registerAnalysedMap.Remove(registerMap);
                        
                        if (tempRegisterMap.ContainsKey(k) && tempRegisterMap[k].Count == 0)
                        {
                            tempRegisterMap.Remove(k);
                        }
                    }
                    
                    if (tempRegisterMap.ContainsKey(k) && tempRegisterMap[k].Count == 0)
                    {
                        tempRegisterMap.Remove(k);
                    }
                    
                }
            }
        }

        /// <summary>
        /// 判断死循环阀值
        /// </summary>
        /// <returns></returns>
        private bool forceQuit() {
            return forceBreak >= 1500;
        }

        /// <summary>
        /// 分析指定方法区块
        /// </summary>
        /// <param name="methodBlockName"></param>
        private void methodAnalyse(string methodBlockName,TempRegisterMap register) {
            forceBreak += 1;
            Console.Write(forceBreak + "   \r");
            /**
             * 分析期间可以直接对临时寄存器集进行操作
             */
            string methodCode = methodBlocks[methodBlockName];

            IBaseHandler handler;
            using (StringReader codeReader = new StringReader(methodCode)) {
                string line = null;
                while ((line = codeReader.ReadLine()) != null) {

                    //在正式进入分析阶段前需将当前行前后空格剔除,防止在handler模块中出现错误
                    line = line.Trim();

                    //获取当前行的操作码
                    int opCode = OpCode.getOpCode(line);
                    if (opCode == OpCode.NOP) continue;
                    int opCode_Type = OpCode.getType(opCode);

                    switch (opCode_Type) {
                        case OpCode.TYPE_MUST:
                            {
                                if (opCode == OpCode.GET_OPC_MUST)
                                {
                                    handler = new GetOpCodeHandler();
                                    handler.lineHandler(line, null, register);
                                }
                                else if (opCode == OpCode.INVOKE_OPC_MUST)
                                {
                                    handler = new InvokeOpCodeHandler();
                                    handler.lineHandler(line, null, register);
                                }
                                else if (opCode == OpCode.IF_OPC_MUST)
                                {
                                    IfOpCodeHandler ifhandler = new IfOpCodeHandler();
                                    string if_target = (string)ifhandler.lineHandler(line, null, null);

                                    //若寄存器已被标记在源码层上的循环,并且目标区块已经出现在分析地图中
                                    //则不需要对该if操作码进行任何操作,直接读取下一行代码
                                    bool rewalk = analysedMap.Contains(if_target);
                                    if (register.isLoop && rewalk) {
                                        return;
                                    }

                                    TempRegisterMap temp = register;
                                    //若if操作码的目标区块已经在分析地图中
                                    if (rewalk)
                                    {
                                        temp.isLoop = true;
                                    }
                                    ifhandler.setTargetBlockTempRegister(tempRegisterMap, temp,analysedMap);
                                    //registerAnalysedMap[temp] = new List<string>(analysedMap);
                                }
                                else if (opCode == OpCode.GOTO_OPC_MUST)
                                {
                                    handler = new GotoOpCodeHandler();
                                    gotoTarget = (string)handler.lineHandler(line, null, null);
                                    //防止goto操作码导致的死循环
                                    //if (loopGoto.Contains(gotoTarget)) gotoTarget = null;
                                    return;
                                }
                                else if (opCode == OpCode.RETURN_OPC_MUST)
                                {
                                    return;
                                }
                                else if (opCode == OpCode.CONST_OPC_MUST)
                                {
                                    handler = new ConstOpCodeHandler();
                                    handler.lineHandler(line, null, register);
                                }
                                break;
                            }
                        case OpCode.TYPE_CHECK: 
                            {
                                foreach (string reg in register.Clone().Keys) {
                                    if (line.Contains(reg)) {
                                        if (opCode == OpCode.ARRAY_OPC_CHECK)
                                        {
                                            handler = new ArrayOpCodeHandler();
                                            handler.lineHandler(line, null, register);
                                        }
                                        else if (opCode == OpCode.CALC_OPC_CHECK)
                                        {
                                            handler = new CalcOpCodeHandler();
                                            handler.lineHandler(line, null, register);
                                        }
                                        else if (opCode == OpCode.CMP_OPC_CHECK)
                                        {
                                            handler = new CmpOpCodeHandler();
                                            handler.lineHandler(line, null, register);
                                        }
                                        else if (opCode == OpCode.LOCAL_OPC_CHECK)
                                        {
                                            handler = new LocalNameHandler();
                                            handler.lineHandler(line, smaliFileAnalyseModule, register);
                                        }
                                        else if (opCode == OpCode.INSTANCE_OF_OPC_CHECK) {
                                            handler = new InstanceOfOpCodeHandler();
                                            handler.lineHandler(line, null, register);
                                        }
                                        else if (opCode == OpCode.MOVE_OPC_CHECK)
                                        {
                                            handler = new MoveOpCodeHandler();
                                            handler.lineHandler(line, smaliFileAnalyseModule, register);
                                        }
                                        else if (opCode == OpCode.NEW_INSTANCE_OPC_CHECK)
                                        {
                                            handler = new NewInstanceOpCodeHandler();
                                            handler.lineHandler(line, null, register);
                                        }
                                        else if (opCode == OpCode.PUT_OPC_CHECK)
                                        {
                                            handler = new PutOpCodeHandler();
                                            handler.lineHandler(line, smaliFileAnalyseModule, register);
                                        }
                                    }
                                }
                                break;
                            }
                    
                    }
                }
                
            }
        }
    }
}
