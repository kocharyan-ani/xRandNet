using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetworkModel;

namespace ConnectedHierarchicModel
{
    class ConnectedNonRegularHierarchicContainer : NonHierarchicContainer
    {
        private List<List<int>> branching;

        public Int32 Level
        {
            get { return branching.Count; }
        }
        public List<List<int>> Branching
        {
            set { branching = value; }
        }
        public override List<List<int>> GetBranches()
        {
            return branching;
        }
    }
}
