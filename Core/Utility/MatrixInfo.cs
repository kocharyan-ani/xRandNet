using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Utility
{
    /// <summary>
    /// Type of Research or Generation parameter, which represents input matrix file.
    /// Path - input matrix file full path.
    /// Size - size of network, which represents the matrix file.
    /// Matrix file can be {0, 1} matrix or list of pairs (n, m), which shows existance of link between n and m.
    /// If Size is 0, it is ignored and the size of matrix retrieved from file information.
    /// </summary>
    public struct MatrixPath
    {
        public String Path { get; set; }
        public int Size { get; set; }

        public override String ToString()
        {
            return Path;
        }
    }

    /// <summary>
    /// Base class for network info in file.
    /// FileName - full name of file, in which network is represented.
    /// Branches - branching information, if the network is not hierarchical.
    /// ActiveStates - active state information for network.
    /// <note>Branches property is null, if the network is not hierarchical.</note>
    /// <note>ActiveStates property is null, if active states information is not given/needed.</note>
    /// </summary>
    public abstract class NetworkInfoToRead
    {
        public String FileName;
        public List<List<int>> Branches;
        public BitArray ActiveStates;
    }

    /// <summary>
    /// Representation of adjacency matrix.  
    /// </summary>
    public class MatrixInfoToRead : NetworkInfoToRead
    {
        public BitArray[] Matrix;
    }

    /// <summary>
    /// Representation of neighbourship information of network.  
    /// </summary>
    public class NeighbourshipInfoToRead : NetworkInfoToRead
    {
        public List<int> Neighbours;
        public int Size;
    }

    /// <summary>
    /// Representation of adjacency matrix, branches and active states of the network to be written to file.
    /// <note>Branches property is null, if the network is not hierarchical.</note>
    /// <note>ActiveStates property is null, if active states information is not given/needed.</note>
    /// </summary>
    public struct MatrixInfoToWrite
    {
        public BitArray[] Matrix;
        public List<List<int>> Branches;
        public BitArray ActiveStates;
    }

    /// <summary>
    /// Representation of neighbourship info, branches and active states of the network to be written to file.
    /// <note>Branches property is null, if the network is not hierarchical.</note>
    /// <note>ActiveStates property is null, if active states information is not given/needed.</note>
    /// </summary>
    public struct NeighbourshipInfoToWrite
    {
        public List<KeyValuePair<int, int>> Neighbourship;
        public List<List<int>> Branches;
        public BitArray ActiveStates;
    }
}
