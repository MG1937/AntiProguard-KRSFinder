using KlazzRelationShipFinder.KRSFinder.Module;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;

namespace KlazzRelationShipFinder.KRSFinder.Handler
{
    class InstanceOfOpCodeHandler : IBaseHandler
    {
        /// <summary>
        /// 处理instance操作码
        /// </summary>
        /// <param name="lineCode">NEED</param>
        /// <param name="smaliFileAnalyseModule">NONEED</param>
        /// <param name="tempRegister">NEED</param>
        /// <returns>null</returns>
        public object lineHandler(string lineCode, SmaliFileAnalyseModule smaliFileAnalyseModule = null, TempRegisterMap tempRegister = null)
        {
            //instance-of vx,vy,type_id
            string register = lineCode.Replace("instance-of", "").Split(",")[0].Trim();
            tempRegister.removeRegister(register);
            return null;
        }
    }
}
