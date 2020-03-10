using System;
using System.Collections.Generic;
using System.Diagnostics;

using Core.Enumerations;
using Core.Utility;

namespace Core.Model
{
    public abstract class AbstractNetworkGenerator : INetworkGenerator
    {
        public abstract List<List<EdgesAddedOrRemoved>> GenerationSteps { get; protected set; }

        public abstract INetworkContainer Container { get; set; }

        public abstract void RandomGeneration(Dictionary<GenerationParameter, Object> genParam, bool visualMode);

        public virtual void StaticGeneration(NetworkInfoToRead info)
        {
            if (info is MatrixInfoToRead)
            {
                MatrixInfoToRead mi = info as MatrixInfoToRead;
                Container.SetMatrix(mi.Matrix);
            }
            else
            {
                Debug.Assert(info is NeighbourshipInfoToRead);
                NeighbourshipInfoToRead ni = info as NeighbourshipInfoToRead;
                Container.SetNeighbourship(ni.Neighbours, ni.Size);
            }
            if (info.ActiveStates != null)
                Container.SetActiveStatuses(info.ActiveStates);
        }
    }
}
