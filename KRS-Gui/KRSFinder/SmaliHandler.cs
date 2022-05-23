using KlazzRelationShipFinder.KRSFinder.Base;
using KlazzRelationShipFinder.KRSFinder.LogPrinter;
using KlazzRelationShipFinder.KRSFinder.Module;
using System;
using System.Collections;
using System.IO;

namespace KlazzRelationShipFinder.KRSFinder
{
    public class SmaliHandler
    {
        string TAG = "SmaliHandler";
        string BakSmaliDecodePath { set; get; }

        ArrayList SmaliPaths = new ArrayList();

        /// <summary>
        /// 初始化SmaliHandler类
        /// </summary>
        /// <param name="baksmaliDecode">apk经baksmali反编译后的文件夹路径</param>
        public SmaliHandler(string baksmaliDecode)
        {
            try
            {
                BakSmaliDecodePath = baksmaliDecode;
                Log.log(TAG, "set direct:" + BakSmaliDecodePath);

                foreach (string path in Directory.EnumerateDirectories(BakSmaliDecodePath, "smali*"))
                {
                    SmaliPaths.Add(path);
                    Log.log(TAG, "[-]Smali path:" + path);
                }

                if (SmaliPaths.Count == 0)
                {
                    Log.show("No smali dir!");
                    return;
                }

                //if (SmaliPaths.Count > 1) isMultiDex = true;

                ArrayList temp_smali = new ArrayList();

                foreach (string path in SmaliPaths)
                {
                    listSmaliFileFromDir(path, temp_smali);
                }

                //储存smali文件列表至Config类,以便其他模块调用
                Config.smaliFiles = temp_smali;

                //释放对象,至此初始化结束
                temp_smali = null;
            }
            catch (Exception)
            {
                Log.show("Something goes wrong with your path");
            }
        }


        /// <summary>
        /// 列出指定目录下的所有.smali文件,并存入指定的ArrayList对象
        /// </summary>
        /// <param name="path">指定目录</param>
        /// <param name="temp">存入对象</param>
        private void listSmaliFileFromDir(string path, ArrayList temp)
        {
            foreach (string p in Directory.EnumerateFiles(path, "*.smali"))
            {
                temp.Add(p);
                Log.log(TAG, "[-]List:" + p);
            }

            foreach (string dir in Directory.EnumerateDirectories(path))
            {
                listSmaliFileFromDir(dir, temp);
            }
        }

        /// <summary>
        /// 开始分析搜集到的Smali文件
        /// </summary>
        public void analyseSmaliFiles()
        {
            if (Config.smaliFiles == null) return;
            Config.totalFiles = Config.smaliFiles.Count;
            //TODO:多线程
            int sum = 0;
            SmaliFileAnalyseModule lineAnalyseModule = new SmaliFileAnalyseModule();
            foreach (string smaliFile in Config.smaliFiles)
            {
                sum += 1;
                Log.show(sum);
                using (StreamReader reader = new StreamReader(smaliFile))
                {
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        //该方法只需要关心读取smali代码的问题
                        //所有读取到的行由LineAnalyseModule处理
                        lineAnalyseModule.lineAnalyse(line);
                    }
                }
            }
        }

    }
}
