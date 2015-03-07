using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEPrLib
{
    //This class is just a standard text book binary search tree, using in-order traversal
    public class Tree<TItem> : IEnumerable<TItem> where TItem : IComparable<TItem>
    {

        public Tree(TItem nodeValue)
        {
            this.NodeData = nodeValue;
            this.LeftTree = null;
            this.RightTree = null;
        }

        public void Insert(TItem newItem)
        {

            TItem currentNodeValue = this.NodeData;
            if (currentNodeValue.CompareTo(newItem) > 0)
            {
                if (this.LeftTree == null)
                    this.LeftTree = new Tree<TItem>(newItem);
                else
                    this.LeftTree.Insert(newItem);
            }
            else
            {
                if (this.RightTree == null)
                    this.RightTree = new Tree<TItem>(newItem);
                else
                    this.RightTree.Insert(newItem);
            }
        }

        public void WalkTree()
        {
            if (this.LeftTree != null)
                this.LeftTree.WalkTree();  //visit ledt node

            Console.WriteLine(this.NodeData.ToString()); //visit the right node

            if (this.RightTree != null)
                this.RightTree.WalkTree();  //visit the right node
        }

        public TItem NodeData { get; set; }
        public Tree<TItem> LeftTree { get; set; }
        public Tree<TItem> RightTree { get; set; }

        #region IEnumerable<TItem> Members

        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        {
            if (this.LeftTree != null)
            {
                foreach (TItem item in this.LeftTree)
                    yield return item;
            }

            yield return this.NodeData;

            if (this.RightTree != null)
            {
                foreach (TItem item in this.RightTree)
                    yield return item;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    //This class creates an association between two int64s, where the first is always
    //lower then the second, creating a closed pair
    public class IntPair : IComparable<IntPair>
    {
        private Int64 fromInt;
        private Int64 toInt;
        private int geprId;
        private int givenId;
        private int groupId;

        public Int64 FromInt
        {
            get { return fromInt; }
            /*set
            {
                if (value.CompareTo(ToInt) == 1)
                    throw new ApplicationException("Bad Range");
                else
                    FromInt = value;
            }*/
        }
        public Int64 ToInt
        {
            get { return toInt; }
            /*set
            {
                if (value.CompareTo(fromInt) == 1)
                    throw new ApplicationException("Bad Range");
                else
                    toInt = value;
            }*/
        }
        public int GEPRId
        {
            get { return geprId; }
            /*set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Must be zero or greater");
                else
                    geprId = value;
            }*/
        }
        public int GivenId
        {
            get { return givenId; }
            /*set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Must be zero or greater");
                else
                    givenId = value;
            }*/
        }
        public int GroupId
        {
            get { return groupId; }
            set { groupId = value;}
        }

        //There are two constructors, the first for natural input
        public IntPair(Int64 frint, Int64 toint, int id, int grid)
        {
            if ((frint > toint) || (id < 0))
                throw new Exception("Not a valid Range");
            fromInt = frint;
            toInt = toint;
            geprId = id;
            givenId = id;
            groupId = grid;
        }

        //This constructor is for a contrived pairs that store the identifiers from wich they were contrived.
        internal IntPair(Int64 frInt, Int64 tInt, int geprid, int givenid, int grid)
        {
            if ((frInt > tInt) || (geprid < 0))
                throw new Exception("Not a valid range");
            fromInt = frInt;
            toInt = tInt;
            geprId = geprid;
            givenId = givenid;
            groupId = grid;
        }

        public override string ToString()
        {
            return String.Format("GEPrId: {0}, GivenId: {1}, From:{2}, To: {3}, GroupId: {4}", this.geprId, this.givenId,
              this.FromInt, this.ToInt, this.GroupId);
        }

        //by using FromInt, the tree will sort on the left side.
        int IComparable<IntPair>.CompareTo(IntPair other)
        {
            return this.FromInt.CompareTo(other.FromInt);
        }


    }

    public class IntPairDerivation
    {
        //private Queue<RetrievedGEPr> geprChart;
        private Tree<IntPair> geprTree;
        private Tree<IntPair> encTree;

        /*public Queue<RetrievedGEPr> GEPRChart
        {
            get { return geprChart; }
            //set { geprChart = value; }
        }*/

        public Tree<IntPair> GEPRTree
        {
            get { return geprTree; }
            set { geprTree = value; }
        }

        public Tree<IntPair> EncTree
        {
            get { return encTree; }
            //   set { EncSortedSet = value; }
        }

        public IntPairDerivation(/*Tree<IntPair> pDRList*/)
        {
            geprTree = null;
            encTree = null;
            //geprChart = makeGEPrChart(pDRList);

        }

        // This function is untested
        /*private Queue<RetrievedGEPr> makeGEPrChart(Tree<IntPair> pDRList)
        {
            if ((pDRList == null))
                throw new ArgumentNullException("Tree",
                          "Specify a non-null argument.");
            var allDR = from dr in pDRList.ToList<IntPair>() select dr;
            bool ini = true;

            Queue<RetrievedGEPr> rDRQueue = new Queue<RetrievedGEPr>();
            IntPair GEPrDR = null;
            IntPair NoPrDR = null;
            foreach (var curDR in allDR)
            {
                if (ini)
                {
                    GEPrDR = curDR;
                    NoPrDR = curDR;
                    rDRQueue.Enqueue(new RetrievedGEPr(curDR, curDR, curDR));
                    ini = false;
                }
                else
                {
                    rDRQueue.Enqueue(new RetrievedGEPr(curDR, GEPrDR, NoPrDR));
                    NoPrDR = curDR;
                    if (curDR.ToInt.CompareTo(GEPrDR.ToInt) <= 0)  //it is enclosed, add it to enclosed tree
                    {
                        if (encTree == null)
                            encTree = new Tree<IntPair>(curDR);
                        else if (encTree.NodeData.FromInt.CompareTo(curDR.FromInt) == -1)
                            encTree.Insert(curDR);
                    }

                }

                if (curDR.ToInt.CompareTo(GEPrDR.ToInt) == 1)
                {
                    //this is identified as a new GPr so add it to the GEPrTree if it's not there
                    if (geprTree == null)
                        geprTree = new Tree<IntPair>(GEPrDR);
                    else
                        geprTree.Insert(curDR);
                    GEPrDR = curDR;  //retained for next it
                }
            }
            return rDRQueue;
        }*/

        //This function traverses the tree of IntPairs creating an association between the given pair and 
        //the correct one that started before it, in order to correctly ascertain a GAP


        public static Tree<IntPair> NegateIntPairList(Tree<IntPair> pIPTree)
        {
            if ((pIPTree == null))
                throw new ArgumentNullException("Tree","Specify a non-null argument.");
            var allIP = from ip in pIPTree select ip;

            bool iniIP = true;  //boolean for next iteration

            Tree<IntPair> rIPTree = null;
            IntPair GEPrIP = null;
            Dictionary<int, IntPair> GEPrDict = new Dictionary<int, IntPair>();
            foreach (var curIP in allIP)
            {
                if (!GEPrDict.ContainsKey(curIP.GroupId))
                    GEPrDict.Add(curIP.GroupId, curIP);
                
                GEPrIP = GEPrDict[curIP.GroupId];

                if (GEPrIP.ToInt.CompareTo(curIP.FromInt) == -1)
                {
                    if (iniIP)  //the only difference between this and the else is The tree is new
                    {
                        rIPTree = new Tree<IntPair>(new IntPair(GEPrIP.ToInt, curIP.FromInt,
                            GEPrIP.GivenId, curIP.GivenId, curIP.GroupId));
                        iniIP = false;
                    }
                    else
                        rIPTree.Insert(new IntPair(GEPrIP.ToInt, curIP.FromInt, GEPrIP.GivenId, 
                            curIP.GivenId, curIP.GroupId));
                }

                if (curIP.ToInt.CompareTo(GEPrIP.ToInt) == 1)  //This condition establishes if the current record will be saved as the next GEPR
                    GEPrDict[curIP.GroupId] = curIP;  //retained for next it
            }
            return rIPTree;
        }


        public static Tree<IntPair> FuseIntPairList(Tree<IntPair> pIPTree)
        {

            if ((pIPTree == null))
                throw new ArgumentNullException("Tree",
                          "Specify a non-null argument.");

            var allIP = from ip in pIPTree select ip;
            bool iniIP = true;

            Tree<IntPair> rDRTree = null;
            IntPair GEPrIP = null;
            Dictionary<int, IntPair> GEPrDict = new Dictionary<int, IntPair>();
            IntPair EarliestFromInt = null;
            Dictionary<int, IntPair> EarliestFromDict = new Dictionary<int, IntPair>();

            //do the traversal
            foreach (var curIP in allIP)
            {
                if (!GEPrDict.ContainsKey(curIP.GroupId))
                    GEPrDict.Add(curIP.GroupId, curIP);
                GEPrIP = GEPrDict[curIP.GroupId];

                if (!EarliestFromDict.ContainsKey(curIP.GroupId))
                    EarliestFromDict.Add(curIP.GroupId, curIP);
                EarliestFromInt = EarliestFromDict[curIP.GroupId];

                if (GEPrIP.ToInt.CompareTo(curIP.FromInt) == -1)  //not the first anymore
                {
                    if (iniIP)  //need to call new on the Tree
                    {
                        rDRTree = new Tree<IntPair>(new IntPair(EarliestFromInt.FromInt, GEPrIP.ToInt, 
                            EarliestFromInt.GivenId, GEPrIP.GivenId, EarliestFromInt.GroupId));
                        iniIP = false;
                    }
                    else  // here, just insert, because we already have a Tree
                        rDRTree.Insert(new IntPair(EarliestFromInt.FromInt, GEPrIP.ToInt, 
                            EarliestFromInt.GivenId, GEPrIP.GivenId, EarliestFromInt.GroupId));
                    
                    EarliestFromDict[curIP.GroupId] = curIP;
                }

                //This condition establishes wether or not to store the current record as the new GEPR for next time
                if (curIP.ToInt.CompareTo(GEPrIP.ToInt) == 1)  //This condition establishes if the current record will be saved as the next GEPR
                    GEPrDict[curIP.GroupId] = curIP;  //retained for next it
            }

            //The final records do not process, but because they are in the dictionaries
            //process them now
            var allIP2 = from ip in EarliestFromDict select ip;
            foreach (var curip in allIP2)
                rDRTree.Insert(new IntPair(curip.Value.FromInt, GEPrDic[curip.Value.GroupId].ToInt, 
                    curip.Value.GivenId, curip.Value.GroupId));

            return rDRTree;
        }
    }


}
