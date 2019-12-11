using System;
using System.Collections.Generic;

using NetworkModel;

namespace ConnectedHierarchicModel
{
    class ConnectedHierarchicContainer : NonHierarchicContainer
    {
        private Int32 level;
        private Int32 branchingIndex;

        public Int32 BranchingIndex
        {
            get { return branchingIndex; }
            set { branchingIndex = value; }
        }

        public Int32 Level
        {
            get { return level; }
            set { level = value; }
        }
        public override List<List<int>> GetBranches()
        {
            List<List<int>> branches = new List<List<int>>(new List<int>[Level]);
            for (int i = 0; i < Level; ++i)
            {
                int levelVertexCount = Convert.ToInt32(Math.Pow(branchingIndex, i));
                branches[i] = new List<int>(new int[levelVertexCount]);
                for (int j = 0; j < levelVertexCount; ++j)
                {
                    branches[i][j] = branchingIndex;
                }
            }
            return branches;
        }
    }
}
