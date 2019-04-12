using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Diagnostics;

using Core.Enumerations;
using Core.Model;
using Core.Exceptions;
using NetworkModel;
using RandomNumberGeneration;

namespace BAModel
{
    /// <summary>
    /// Implementation of generator of random network of Baraba´si-Albert's model.
    /// </summary>
    class BANetworkGenerator : AbstractNetworkGenerator
    {
        private NonHierarchicContainer container = new NonHierarchicContainer();
        
        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (NonHierarchicContainer)value; }
        }

        public override void RandomGeneration(Dictionary<GenerationParameter, Object> genParam)
        {
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Vertices));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Edges));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.Probability));
            Debug.Assert(genParam.ContainsKey(GenerationParameter.StepCount));

            Int32 numberOfVertices = Convert.ToInt32(genParam[GenerationParameter.Vertices]);
            Int32 edges = Convert.ToInt32(genParam[GenerationParameter.Edges]);
            Double probability = Double.Parse(genParam[GenerationParameter.Probability].ToString(), CultureInfo.InvariantCulture);
            Int32 stepCount = Convert.ToInt32(genParam[GenerationParameter.StepCount]);

            if ((probability < 0 || probability > 1) || (edges > numberOfVertices))
                throw new InvalidGenerationParameters();

            container.Size = numberOfVertices;
            Generate(stepCount, probability, edges);
        }

        private RNGCrypto rand = new RNGCrypto();

        private void Generate(Int32 stepCount, Double probability, Int32 edges)
        {
            GenerateInitialGraph(probability);
            while (stepCount > 0)
            {
                Double[] probabilyArray = CalculateProbabilities();
                container.AddVertex();
                RefreshNeighbourships(MakeGenerationStep(probabilyArray, edges));
                --stepCount;
            }
        }

        private void GenerateInitialGraph(Double probability)
        {
            for (Int32 i = 0; i < container.Size; ++i)
                for (Int32 j = i + 1; j < container.Size; ++j)
                {
                    if (rand.NextDouble() < probability)
                        container.AddConnection(i, j);
                }
        }

        private Double[] CalculateProbabilities()
        {
            Double[] result = new Double[container.Size];

            Double graphDegree = (Double)container.GetVertexDegreeSum();
            if (graphDegree != 0)
            {
                for (Int32 i = 0; i < result.Length; ++i)
                    result[i] = (Double)container.GetVertexDegree(i) / graphDegree;
            }
            else
            {
                for (Int32 i = 0; i < result.Length; ++i)
                    result[i] = 1.0 / result.Length;
            }

            return result;
        }

        public void RefreshNeighbourships(Boolean[] generatedVector)
        {
            Int32 newVertexDegree = 0;
            for (Int32 i = 0; i < generatedVector.Length; ++i)
            {
                if (generatedVector[i])
                {
                    ++newVertexDegree;
                    container.AddConnection(i, container.Size - 1);
                }
            }
            ++container.Degrees[newVertexDegree];
        }

        private Boolean[] MakeGenerationStep(Double[] probabilityArray, Int32 edges)
        {
            Dictionary<Int32, Double> resultDic = new Dictionary<Int32, Double>();
            Int32 count = 0;
            for (Int32 i = 0; i < probabilityArray.Length; ++i)
                resultDic.Add(i, probabilityArray[i] - rand.NextDouble());
          
            var items = from pair in resultDic
                        orderby pair.Value descending
                        select pair;
            
            Boolean[] result = new Boolean[resultDic.Count];
            foreach (var item in items)
            {
                if (count < edges)
                {
                    result[item.Key] = true;
                    count++;
                }
            }

            return result;
        }
    }
}
