using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class NewInstanceOpCodeHandler : IBaseHandler
    {
        /// <summary>
        /// 处理instance操作码
        /// </summary>
        /// <param name="lineCode">NEED</param>
        /// <param name="smaliFileAnalyseModule">NONEED</param>
        /// <param name="tempRegister">NEED</param>
        /// <returns>NULL</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            //new-instance vx,type
            string register = lineCode.Replace("new-instance", "").Split(",")[0].Trim();
            tempRegister.removeRegister(register);
            return null;
        }
    }
}
