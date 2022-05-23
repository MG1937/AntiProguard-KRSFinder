using System.Collections;

namespace KlazzRelationShipFinder.KRSFinder.Base
{
    /// <summary>
    /// 此公共类的内容任何模块都可以获取或修改
    /// 用于储存一些处理对象的基础值,如储存路径一类
    /// </summary>
    class Config
    {
        public static ArrayList smaliFiles { set; get; }

        public static int totalFiles { set; get; }

        public static bool isBakSmali = true;
    }
}
