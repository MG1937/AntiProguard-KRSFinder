using KlazzRelationShipFinder.KRSFinder;
using KlazzRelationShipFinder.KRSFinder.Base;
using KlazzRelationShipFinder.KRSFinder.MessageSaver;
using KlazzRelationShipFinder.KRSFinder.Module.Smali;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace KlazzRelationShipFinder
{
    public class MainRun
    {
        static void Main(string[] args)
        {
            //Config.isBakSmali = true;
            new SmaliHandler("H:\\AndroidStudioProjects\\ObfuseTest\\app\\build\\outputs\\apk\\debug\\app-debug").analyseSmaliFiles();
            Dictionary<string, List<Var>> relations = RelationSaver.relations;
            foreach (string k in relations.Keys) {
                foreach(Var v in relations[k])
                {
                    foreach (string c in v.comments) {
                        Console.WriteLine(k + "::" + v.var_name + "::" + c);
                    }
                }
            }
        }

    }
}
