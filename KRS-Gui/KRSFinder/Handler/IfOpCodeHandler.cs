using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System.Collections.Generic;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class IfOpCodeHandler : IBaseHandler
    {
        public string target_block { set; get; }

        /// <summary>
        /// 分析分支的目标区块
        /// </summary>
        /// <param name="lineCode"></param>
        /// <param name="method"></param>
        /// <param name="tempRegister"></param>
        /// <returns>返回目标区块名</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            //if-eq vx,vy,:L0
            string[] blocks = lineCode.Split(",");
            target_block = blocks[blocks.Length - 1].Replace(":", "").Trim();
            return target_block;
        }

        /// <summary>
        /// 设置目标分支的临时寄存器集
        /// </summary>
        /// <param name="temp">储存其他区块的临时寄存器集</param>
        /// <param name="tempMap">主寄存器集</param>
        /// Dictionary<string, List<TempRegisterMap>>
        public void setTargetBlockTempRegister(Dictionary<string, Dictionary<TempRegisterMap, List<string>>> temp, TempRegisterMap tempMap, List<string> analysedMap)
        {
            //若分支目标成员为空,则需先执行lineHandler函数
            if (target_block == null) return;

            //若目标转跳区块已经存在于temp中
            if (temp.ContainsKey(target_block))
            {
                /**
                 * 若目标转跳区块作为key已经存在于temp成员中(即跳转目标实际上已被解析过,即此处为loop状态)
                 * 则需要验证这个key下的寄存器集是否
                 * 与当前传入的临时寄存器集tempMap有一致
                 * 若一致,则完全没有必要再将当前的寄存器集添加进temp成员
                 */
                foreach (TempRegisterMap t in (temp[target_block].Keys))
                {
                    if (t.Equals(tempMap))
                    {
                        t.isLoop = tempMap.isLoop;
                        return;
                    }
                }

                temp[target_block][tempMap] = new List<string>(analysedMap);
                return;
            }

            //若temp成员中确实不存在与当前传入的临时寄存器集一致的寄存器集
            //则将传入的寄存器集加入temp成员
            Dictionary<TempRegisterMap, List<string>> news = new Dictionary<TempRegisterMap, List<string>>();
            news[tempMap] = new List<string>(analysedMap);
            temp[target_block] = news;
        }
    }
}
