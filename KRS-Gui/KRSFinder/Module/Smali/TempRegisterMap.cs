using System;
using System.Collections.Generic;
using System.Text;

namespace KlazzRelationShipFinder.KRSFinder.Module.Smali
{
    /// <summary>
    /// 充当临时寄存器集
    /// </summary>
    class TempRegisterMap : Dictionary<string,TempRegister>
    {
        string TAG = "TempRegister";

        //若分支目标属于循环,则启用此标记
        public bool isLoop = false;

        /// <summary>
        /// 向临时寄存器集内储存寄存器
        /// </summary>
        /// <param name="reg">寄存器,格式如v0,p0</param>
        /// <param name="value">寄存器对应的值</param>
        public void putRegister(string reg,TempRegister value) {
            if (!checkRegister(reg)) return;
            this[reg] = value;
        }

        /// <summary>
        /// 从临时寄存器集中移除指定寄存器
        /// </summary>
        /// <param name="reg">指定寄存器</param>
        public void removeRegister(string reg) {
            if (!checkRegister(reg)) return;
            Remove(reg);
        }

        /// <summary>
        /// 从临时寄存器集中获取指定寄存器的值
        /// </summary>
        public object getRegister(string reg) {
            if (!checkRegister(reg)) return null;
            TempRegister value = this.GetValueOrDefault(reg, null);
            if (value == null) return null;
            return value.getValue();
        }

        /// <summary>
        /// 检测寄存器合法性
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        private bool checkRegister(string reg) {
            if (reg == null || reg.Length < 2)
            {
                throw new Exception("寄存器格式错误!");
                return false;
            }

            if (reg == "result") return true;

            string head = reg.Substring(0, 1);
            if (!head.Equals("v") && !head.Equals("p")) return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            try {
                TempRegisterMap temp = (TempRegisterMap)obj;
                if (this.Count == temp.Count)
                {
                    foreach (string k in this.Keys)
                    {
                        if (!temp[k].Equals(this[k])) {
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

        public TempRegisterMap Clone() {
            TempRegisterMap clone = new TempRegisterMap();
            foreach (string k in Keys) {
                clone[k] = this[k];
            }
            clone.isLoop = isLoop;
            return clone;
        }
    }
}
