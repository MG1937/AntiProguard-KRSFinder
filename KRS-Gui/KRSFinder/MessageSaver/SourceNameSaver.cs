using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KlazzRelationShipFinder.KRSFinder.MessageSaver
{
    class SourceNameSaver
    {
        /// <summary>
        /// Dictionary<OrigKlazzName,SourceKlazzName>
        /// </summary>
        private static Dictionary<string, string> sourceNameMap = new Dictionary<string, string>();

        /// <summary>
        /// 记录每个sourceName的总和
        /// </summary>
        private static Dictionary<string, int> sourceNameSum = new Dictionary<string, int>();


        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void saveSourceName(string origName, string sourceName)
        {
            if (origName == null || sourceName == null)
            {
                return;
            }

            sourceNameMap[origName] = sourceName;
            int sum = sourceNameSum.GetValueOrDefault(sourceName, 0);
            sum += 1;
            sourceNameSum[sourceName] = sum;
        }

        /// <summary>
        /// 获取指定sourceName出现过的次数
        /// </summary>
        /// <returns></returns>
        public static int getSourceNameSum(string sourceName)
        {
            if (sourceName == null) return 0;

            return sourceNameSum.GetValueOrDefault(sourceName, 0);
        }

        /// <summary>
        /// 根据origName获取sourceName
        /// </summary>
        /// <returns></returns>
        public static string getSourceName(string origName)
        {
            if (origName == null) return null;

            return sourceNameMap.GetValueOrDefault(origName, null);
        }
    }
}
