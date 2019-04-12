using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using Core.Enumerations;
using Core.Model.Engines;
using Model.Eigenvalues;

namespace Core.Model
{
    /// <summary>
    /// Abstract class presenting random network analyzer.
    /// </summary>
    public abstract class AbstractNetworkAnalyzer : INetworkAnalyzer
    {
        protected readonly AbstractNetwork network;

        protected AbstractNetworkContainer abstractContainer;
        public virtual INetworkContainer Container
        {
            get { return abstractContainer; }
            set { abstractContainer = (AbstractNetworkContainer)value; }
        }

        public AbstractNetworkAnalyzer(AbstractNetwork n)
        {
            network = n;
        }

        /// <summary>
        /// Calculates count of edges of the network.
        /// </summary>
        /// <returns>Count of edges.</returns>
        public Double CalculateEdgesCount()
        {
            return CalculateEdgesCountOfNetwork();
        }

        /// <summary>
        /// Calculates given options value.
        /// </summary>
        /// <param name="option">Option to calculate.</param>
        /// <returns>Calculated value.</returns>
        public Object CalculateOption(AnalyzeOption option)
        {
            switch (option)
            {
                // Globals //
                case AnalyzeOption.AvgPathLength:
                    return CalculateAveragePath();
                case AnalyzeOption.Diameter:
                    return CalculateDiameter();
                case AnalyzeOption.AvgDegree:
                    return CalculateAverageDegree();
                case AnalyzeOption.AvgClusteringCoefficient:
                    return CalculateAverageClusteringCoefficient();
                case AnalyzeOption.Cycles3:
                    return CalculateCycles3();
                case AnalyzeOption.Cycles4:
                    return CalculateCycles4();
                case AnalyzeOption.Cycles5:
                    return CalculateCycles5();
                /*case AnalyzeOption.Dr:
                    return CalculateDr();*/
                // Eigens //
                case AnalyzeOption.EigenValues:
                    return CalculateEigenValues();
                case AnalyzeOption.EigenDistanceDistribution:
                    return CalculateEigenDistanceDistribution();
                case AnalyzeOption.LaplacianEigenValues:
                    return CalculateLaplacianEigenValues();
                // Distributions //
                case AnalyzeOption.DegreeDistribution:
                    return CalculateDegreeDistribution();
                case AnalyzeOption.ClusteringCoefficientDistribution:
                    return CalculateClusteringCoefficientDistribution();
                case AnalyzeOption.ClusteringCoefficientPerVertex:
                    return CalculateClusteringCoefficientPerVertex();
                case AnalyzeOption.CompleteComponentDistribution:
                    return CalculateCompleteComponentDistribution();
                case AnalyzeOption.ConnectedComponentDistribution:
                    return CalculateConnectedComponentDistribution();
                case AnalyzeOption.SubtreeDistribution:
                    return CalculateSubtreeDistribution();
                case AnalyzeOption.DistanceDistribution:
                    return CalculateDistanceDistribution();
                case AnalyzeOption.TriangleByVertexDistribution:
                    return CalculateTriangleByVertexDistribution();
                /*case AnalyzeOption.DrDistribution:
                    return CalculateDrDistribution();*/
                // Trajectories //
                case AnalyzeOption.Cycles3Evolution:
                    return CalculateCycles3Evolution();
                case AnalyzeOption.Algorithm_1_By_All_Nodes:
                    return ActivationAlgorithm1();
                case AnalyzeOption.Algorithm_2_By_Active_Nodes_List:
                    return ActivationAlgorithm2();
                case AnalyzeOption.Algorithm_Final:
                    return ActivationAlgorithmFinal();
                // Centralities //
                case AnalyzeOption.BetweennessCentrality:
                    return CalculateBetweennessCentrality();
                case AnalyzeOption.ClosenessCentrality:
                    return CalculateClosenessCentrality();
                case AnalyzeOption.DegreeCentrality:
                    return CalculateDegreeCentrality();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Calculates count of edges of the network.
        /// </summary>
        /// <returns>Count of edges.</returns>
        protected virtual Double CalculateEdgesCountOfNetwork()
        {
            return Container.EdgesCount;
        }

        /// <summary>
        /// Calculates the average path length of the network.
        /// </summary>
        /// <returns>Average path length.</returns>
        protected virtual Double CalculateAveragePath()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the diameter of the network.
        /// </summary>
        /// <returns>Diameter.</returns>
        protected virtual Double CalculateDiameter()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the average value of vertex degrees in the network.
        /// </summary>
        /// <returns>Average degree.</returns>
        protected virtual Double CalculateAverageDegree()
        {
            return Container.EdgesCount * 2 / (double)Container.Size;
        }

        /// <summary>
        /// Calculates the average value of vertex clustering coefficients in the network.  
        /// </summary>
        /// <returns>Average clustering coefficient.</returns>
        protected virtual Double CalculateAverageClusteringCoefficient()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the number of cycles of length 3 in the network.
        /// </summary>
        /// <returns>Number of cycles 3.</returns>
        protected virtual Double CalculateCycles3()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the number of cycles of length 4 in the network.
        /// </summary>
        /// <returns>Number of cycles 4.</returns>
        protected virtual Double CalculateCycles4()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the number of cycles of length 5 in the network.
        /// </summary>
        /// <returns>Number of cycles 5.</returns>
        protected virtual Double CalculateCycles5()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the Dr option in the network.
        /// </summary>
        /// <returns>Number of cycles 5.</returns>
        /*protected virtual Double CalculateDr()
        {
            throw new NotImplementedException();
        }*/

        private AnalyzerEigenValuesEngine eigenValuesEngine;

        /// <summary>
        /// Calculates the eigenvalues of adjacency matrix of the network.
        /// </summary>
        /// <returns>List of eigenvalues.</returns>
        protected List<Double> CalculateEigenValues()
        {
            CheckEigenValuesEngine();
            Debug.Assert(eigenValuesEngine != null);
            return eigenValuesEngine.EigenValues;
        }
       
        /// <summary>
        /// Calculates distances between eigenvalues.
        /// </summary>
        /// <returns>(distance, count) pairs.</returns>
        protected SortedDictionary<Double, Double> CalculateEigenDistanceDistribution()
        {
            CheckEigenValuesEngine();
            Debug.Assert(eigenValuesEngine != null);
            return eigenValuesEngine.EigenDistances;            
        }

        /// <summary>
        /// Calculates the eigenvalues of Laplacian matrix of adjacency matrix of the network.
        /// </summary>
        /// <returns>List of eigenvalues of Laplacian matrix.</returns>
        protected List<Double> CalculateLaplacianEigenValues()
        {
            BitArray[] m = Container.GetMatrix();
            int size = m.Length;
            double[,] lm = new double[size, size];
            for (int i = 0; i < size; ++i)
            {
                int connection_count = 0;
                for (int j = 0; j < size; ++j)
                {
                    if (i == j)
                        continue;
                    if (m[i][j])
                    {
                        lm[i, j] = -1;
                        ++connection_count;
                    }
                    else
                    {
                        lm[i, j] = 0;
                    }

                }
                lm[i, i] = connection_count;
            }

            EigenValueUtils eg = new EigenValueUtils();
            try
            {
                return eg.CalculateEigenValue(lm);
            }
            catch (SystemException)
            {
                return new List<double>();
            }
        }

        /// <summary>
        /// Calculates degrees of vertices in the network.
        /// </summary>
        /// <returns>(degree, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateDegreeDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates clustering coefficients of vertices in the network.
        /// </summary>
        /// <returns>(clustering coefficient, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateClusteringCoefficientDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates clustering coefficient for each vertex in the network.
        /// </summary>
        /// <returns>(vertex, coefficient) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateClusteringCoefficientPerVertex()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates counts of connected components in the network.
        /// </summary>
        /// <returns>(order of connected component, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateConnectedComponentDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates counts of complete components in the network.
        /// </summary>
        /// <returns>(order of complete component, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateCompleteComponentDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates counts of subtrees in the network.
        /// </summary>
        /// <returns>(order of connected component, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateSubtreeDistribution()
        {
            throw new NotImplementedException();
        }        

        /// <summary>
        /// Calculates minimal path lengths in the network.
        /// </summary>
        /// <returns>(minimal path length, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateDistanceDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates counts of triangles by vertices in the network.
        /// </summary>
        /// <returns>(triangle count, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateTriangleByVertexDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the DrDistribution option in the network.
        /// </summary>
        /// <returns>Number of cycles 5.</returns>
        /*protected virtual SortedDictionary<Double, Double> CalculateDrDistribution()
        {
            throw new NotImplementedException();
        }*/

        /// <summary>
        /// Calculates the counts of cycles 3 in the network during evolution process.
        /// </summary>
        /// <returns>(step, cycles 3 count)</returns>
        protected virtual SortedDictionary<Double, Double> CalculateCycles3Evolution()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Calculates the part of active nodes in the network during activation process.
        /// </summary>
        /// <returns>(step, part of active nodes)</returns>
        protected virtual SortedDictionary<Double, Double> ActivationAlgorithm1()
        {
            RetrieveActivationResearchParameters(out Int32 s, out Double m, out Double l, out Int32 t);
            AnalyzerActivationEngine engine = new AnalyzerActivationEngine(network, abstractContainer, s, m, l, t);
            engine.Calculate(AlgorithmType.Algorithm1);
            return engine.Trajectory;
        }

        /// <summary>
        /// Calculates the part of active nodes in the network during activation process.
        /// </summary>
        /// <returns>(step, part of active nodes)</returns>
        protected virtual SortedDictionary<Double, Double> ActivationAlgorithm2()
        {
            RetrieveActivationResearchParameters(out Int32 s, out Double m, out Double l, out Int32 t);
            AnalyzerActivationEngine engine = new AnalyzerActivationEngine(network, abstractContainer, s, m, l, t);
            engine.Calculate(AlgorithmType.Algorithm2);
            return engine.Trajectory;
        }

        /// <summary>
        /// Calculates the part of active nodes in the network during activation process - the final algorithm.
        /// </summary>
        /// <returns>(step, part of active nodes)</returns>
        protected virtual SortedDictionary<Double, Double> ActivationAlgorithmFinal()
        {
            RetrieveActivationResearchParameters(out Int32 s, out Double m, out Double l, out Int32 t);
            AnalyzerActivationEngine engine = new AnalyzerActivationEngine(network, abstractContainer, s, m, l, t);
            engine.Calculate(AlgorithmType.Final);
            return engine.Trajectory;
        }

        /// <summary>
        /// Calculates degree centralities for each vertex.
        /// </summary>
        /// <returns>List of degree centralities.</returns>
        protected virtual List<Double> CalculateDegreeCentrality()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates closeness centralities for each vertex.
        /// </summary>
        /// <returns>List of closeness centralities.</returns>
        protected virtual List<Double> CalculateClosenessCentrality()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates betweenness centralities for each vertex.
        /// </summary>
        /// <returns>List of closeness centralities.</returns>
        protected virtual List<Double> CalculateBetweennessCentrality()
        {
            throw new NotImplementedException();
        }

        protected void RetrieveEvolutionResearchParameters(out Int32 s, out Double o, out Boolean p, out Int32 t)
        {
            Debug.Assert(network.ResearchType == ResearchType.Evolution, "Research type have to be Evolution");
            Debug.Assert(network.ResearchParameterValues != null);
            Debug.Assert(network.ResearchParameterValues.ContainsKey(ResearchParameter.EvolutionStepCount));
            Debug.Assert(network.ResearchParameterValues.ContainsKey(ResearchParameter.Omega));
            Debug.Assert(network.ResearchParameterValues.ContainsKey(ResearchParameter.PermanentDistribution));
            Debug.Assert(network.ResearchParameterValues.ContainsKey(ResearchParameter.TracingStepIncrement));

            s = Convert.ToInt32(network.ResearchParameterValues[ResearchParameter.EvolutionStepCount]);
            o = Double.Parse(network.ResearchParameterValues[ResearchParameter.Omega].ToString(), CultureInfo.InvariantCulture);
            p = Convert.ToBoolean(network.ResearchParameterValues[ResearchParameter.PermanentDistribution]);
            Object v = network.ResearchParameterValues[ResearchParameter.TracingStepIncrement];
            t = ((v != null) ? Convert.ToInt32(v) : 0);
        }

        protected void RetrieveActivationResearchParameters(out Int32 s, out Double m, out Double l, out Int32 t)
        {
            Debug.Assert(network.ResearchType == ResearchType.Activation, "Research type have to be Activation");
            Debug.Assert(network.ResearchParameterValues != null);
            Debug.Assert(network.ResearchParameterValues.ContainsKey(ResearchParameter.ActivationStepCount));
            Debug.Assert(network.ResearchParameterValues.ContainsKey(ResearchParameter.DeactivationSpeed));
            Debug.Assert(network.ResearchParameterValues.ContainsKey(ResearchParameter.ActivationSpeed));
            Debug.Assert(network.ResearchParameterValues.ContainsKey(ResearchParameter.TracingStepIncrement));

            s = Convert.ToInt32(network.ResearchParameterValues[ResearchParameter.ActivationStepCount]);
            m = Double.Parse(network.ResearchParameterValues[ResearchParameter.DeactivationSpeed].ToString(), CultureInfo.InvariantCulture);
            l = Double.Parse(network.ResearchParameterValues[ResearchParameter.ActivationSpeed].ToString(), CultureInfo.InvariantCulture);
            Object v = network.ResearchParameterValues[ResearchParameter.TracingStepIncrement];
            t = ((v != null) ? Convert.ToInt32(v) : 0);
        }

        private void CheckEigenValuesEngine()
        {
            if(eigenValuesEngine == null)
            {
                eigenValuesEngine = new AnalyzerEigenValuesEngine(Container);
                eigenValuesEngine.Calculate();
            }
        }
    }
}
