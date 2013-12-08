using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GEPrLib;

namespace GEPr
{
    class Program
    {
        static void Main()
        {
            //This section loads compiled test data
            Tree<IntPair> IPTree =
                new Tree<IntPair>(new IntPair(1,30, 37,2));
            IPTree.Insert(new IntPair(5, 10, 22, 1));
            IPTree.Insert(new IntPair(15, 25, 65,1));
            IPTree.Insert(new IntPair(7, 9, 50, 1));
            IPTree.Insert(new IntPair(16, 22, 78,1));
            IPTree.Insert(new IntPair(40, 70, 44, 2));
            IPTree.Insert(new IntPair(45, 50, 43, 1));
            IPTree.Insert(new IntPair(47, 49, 77, 1));
            IPTree.Insert(new IntPair(55, 65, 52, 1));
            IPTree.Insert(new IntPair(57, 63, 41, 1));




            //Show Tree         
            var allIP = from ip in IPTree.ToList<IntPair>() select ip;
            foreach (var curIP in allIP)
                System.Console.WriteLine(curIP.ToString());


            //of the following three snippets only one can be run at a time,
            //because they operate on the same object

            //finds the negative space
            /*
            System.Console.WriteLine("Negatives");
            IPTree = IntPairDerivation.NegateIntPairList(IPTree);
            allIP = from ip in IPTree.ToList<IntPair>() select ip;
            foreach (var curDR in allIP)
                System.Console.WriteLine(curDR.ToString());
            */

            //fuse the larger groups
            
            System.Console.WriteLine("Fusion");
            IPTree = IntPairDerivation.FuseIntPairList(IPTree);
            allIP = from ip in IPTree.ToList<IntPair>() select ip;
            foreach (var curDR in allIP)
                System.Console.WriteLine(curDR.ToString());
            

            //can't even  remember what the chart is for
            /*System.Console.WriteLine("Chart");
            IntPairTransform Chart = new IntPairTransform(DRTree);
            allDR = from dr in Chart.GEPrTree.ToList<IntPair>() select dr;
            foreach (var curDR in Chart.GEPrTree)
                System.Console.WriteLine(curDR.ToString());*/

            System.Console.Read();
        }
    }
}
