using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace KlazzRelationShipFinder.KRSFinder.Module.Smali
{
    class OpCode
    {
        /**
         * 注释CHECK的操作码说明该操作码
         * 与已经存在于临时寄存器集中的寄存器有关
         * 并接下来可能影响这些寄存器分配
         * E.g:
         * 假设临时寄存器集中仅仅存在v1
         * 那么假设分析到此句 move v1,v0
         * 那么该句可能会影响寄存器集中v1寄存器的分配
         * 但如果是以下语句
         * move v2,v0
         * 该句无论如何都不会影响临时寄存器集中的任何值
         * 
         * 注释MUST的操作码说明该操作码
         * 无论如何都可能会影响临时寄存器集中的寄存器分配或整个分析路线
         * 
         * 注释PASS的操作码表明该操作码
         * 无论如何都不会影响临时寄存器集中的寄存器分配
         * 
         * Binary:
         * 0000 0000 0xyz
         * x:CHECK
         * y:MUST
         * z:PASS
         */
        public const int TYPE_CHECK = 0x4;

        public const int TYPE_MUST = 0x2;

        public const int TYPE_PASS = 0x1;

        public const int GET_OPC_MUST = 0x00 | TYPE_MUST;//MUST

        public const int PUT_OPC_CHECK = 0x10 | TYPE_CHECK;//CHECK

        public const int INVOKE_OPC_MUST = 0x20 | TYPE_MUST;//MUST

        public const int CONST_OPC_MUST = 0x30 | TYPE_MUST;

        public const int LOCAL_OPC_CHECK = 0x40 | TYPE_CHECK;

        public const int SOURCE_OPC = 0x50 | TYPE_PASS;//PASS

        public const int KLAZZ_OPC = 0x60 | TYPE_PASS;//PASS

        public const int MOVE_OPC_CHECK = 0x70 | TYPE_CHECK;//CHECK

        public const int METHOD_START = 0x80 | TYPE_PASS;//PASS

        public const int METHOD_END = 0x90 | TYPE_PASS;//PASS

        public const int IF_OPC_MUST = 0xa0 | TYPE_MUST;

        public const int SWITCH_OPC_MUST = 0xb0 | TYPE_MUST;

        public const int GOTO_OPC_MUST = 0xc0 | TYPE_MUST;

        public const int NEW_INSTANCE_OPC_CHECK = 0xd0 | TYPE_CHECK;

        public const int RETURN_OPC_MUST = 0xe0 | TYPE_MUST;

        public const int INSTANCE_OF_OPC_CHECK = 0xf0 | TYPE_CHECK;

        public const int ARRAY_OPC_CHECK = 0x100 | TYPE_CHECK;

        public const int CMP_OPC_CHECK = 0x200 | TYPE_CHECK;

        public const int CALC_OPC_CHECK = 0x300 | TYPE_CHECK;

        public const int NOP = TYPE_PASS;//PASS

        /// <summary>
        /// 获取操作码类型
        /// </summary>
        /// <param name="opCode"></param>
        /// <returns></returns>
        public static int getType(int opCode) {
            int type = opCode & 0xf;
            return type;
        }

        /// <summary>
        /// 通过smali的opCode返回有必要进行操作的OpCode编号
        /// </summary>
        /// <param name="lineCode"></param>
        /// <returns></returns>
        public static int getOpCode(string lineCode) {
            string opCode = lineCode.Trim().Split(" ")[0].Trim();
            Regex regex = new Regex("^[a-z\\-\\.]+$", RegexOptions.Multiline);
            if (!regex.Match(opCode).Success) return NOP;

            if (opCode.Contains("get"))
            {
                return GET_OPC_MUST;
            }
            else if (opCode.Contains("put"))
            {
                return PUT_OPC_CHECK;
            }
            else if (opCode.Contains("invoke"))
            {
                return INVOKE_OPC_MUST;
            }
            else if (opCode.Contains("const"))
            {
                return CONST_OPC_MUST;
            }
            else if (opCode.Contains("local"))
            {
                return LOCAL_OPC_CHECK;
            }
            else if (opCode.Contains(".source"))
            {
                return SOURCE_OPC;
            }
            else if (opCode.Contains(".class"))
            {
                return KLAZZ_OPC;
            }
            else if (opCode.Contains("move"))
            {
                return MOVE_OPC_CHECK;
            }
            else if (lineCode.Trim().StartsWith(".method"))
            {
                return METHOD_START;
            }
            else if (opCode.Contains(".end"))
            {
                if (lineCode.Contains(".end method"))
                {
                    return METHOD_END;
                }
            }
            else if (opCode.Contains("if-"))
            {
                return IF_OPC_MUST;
            }
            else if (opCode.Contains("-switch") && !opCode.Contains("."))
            {
                return SWITCH_OPC_MUST;
            }
            else if (opCode.Contains("goto"))
            {
                return GOTO_OPC_MUST;
            }
            else if (opCode.Contains("new-instance"))
            {
                return NEW_INSTANCE_OPC_CHECK;
            }
            else if (opCode.Contains("array"))
            {
                return ARRAY_OPC_CHECK;
            }
            else if (opCode.Contains("return"))
            {
                return RETURN_OPC_MUST;
            }
            else if (opCode.Contains("instance-of"))
            {
                return INSTANCE_OF_OPC_CHECK;
            }
            else if (opCode.Contains("cmp"))
            {
                return CMP_OPC_CHECK;
            }
            else if (opCode.Contains("-")) 
            {
                return CALC_OPC_CHECK;
            }
            return NOP;
        }
    }
}
