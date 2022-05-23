using KlazzRelationShipFinder.KRSFinder.Base;
using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System.Text.RegularExpressions;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class GetOpCodeHandler : IBaseHandler
    {
        public Var var = new Var();

        /// <summary>
        /// 分析get类型的操作码
        /// </summary>
        /// <param name="lineCode"></param>
        /// <returns></returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            //对象存放的目标寄存器
            string register = null;
            //成员隶属的类
            string klazz = null;
            //成员名
            string var_name = null;

            string temp_var_data = null;

            lineCode = lineCode.Trim();

            string head = lineCode.Split(" ")[0];

            //忽略aget
            if (head.Contains("aget")) return null;

            string[] blocks = lineCode.Replace(head, "").Trim().Split(",");

            if (head.Contains("iget"))
            {
                //iget v0, v1, LTest2;->i6:I
                register = blocks[0].Trim();
                temp_var_data = blocks[2].Trim();
            }
            else if (head.Contains("sget"))
            {
                //sget v0, LTest3;->is1:I
                register = blocks[0].Trim();
                temp_var_data = blocks[1].Trim();
            }

            Regex regex = new Regex("L(.+);->(.+):");
            Match match = regex.Match(temp_var_data);
            klazz = match.Groups[1].Value;
            var_name = match.Groups[2].Value;

            var.var_name = var_name;
            var.klazz = klazz;

            //向目标寄存器储存成员对象
            tempRegister.putRegister(register, new TempRegister(var));
            return null;
        }
    }
}
