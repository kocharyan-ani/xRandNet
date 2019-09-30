using System;
using System.Collections;
using System.Collections.Generic;

using Core.Model;

namespace HierarchicModel
{
    /// <summary>
    /// Implementation of generalized block-hierarchic network's container.
    /// </summary>
    class HierarchicNetworkContainer : AbstractHierarchicContainer
    {
        public override Int32 Size
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override Int32 EdgesCount
        {
            get { throw new NotImplementedException(); }
        }

        public override List<int> Degrees => throw new NotImplementedException();

        public override List<int> GetAdjacentVertices(int vertex)
        {
            throw new NotImplementedException();
        }

        public override int GetVertexDegree(int vertex)
        {
            throw new NotImplementedException();
        }

        public override void SetMatrix(BitArray[] matrix)
        {
            throw new NotImplementedException();
        }

        public override Object Clone()
        {
            throw new NotImplementedException();
        }

        public override List<List<int>> GetBranches()
        {
            throw new NotImplementedException();
        }

        public override bool AreConnected(int v1, int v2)
        {
            throw new NotImplementedException();
        }
    }
}
