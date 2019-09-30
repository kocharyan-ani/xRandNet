using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;

using Core.Enumerations;
using Core.Model;
using Core.Exceptions;
using NetworkModel;
using RandomNumberGeneration;

namespace BBModel
{
    /// <summary>
    /// Implementation of generator of random network of Baraba´si-Albert's model.
    /// </summary>
    class BBNetworkGenerator : AbstractNetworkGenerator
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
            Debug.Assert(genParam.ContainsKey(GenerationParameter.FitnessDensityFunction));

            Int32 numberOfVertices = Convert.ToInt32(genParam[GenerationParameter.Vertices]);
            Int32 edges = Convert.ToInt32(genParam[GenerationParameter.Edges]);
            String function = genParam[GenerationParameter.FitnessDensityFunction].ToString();
            Double probability = Double.Parse(genParam[GenerationParameter.Probability].ToString(), CultureInfo.InvariantCulture);            

            if (probability < 0 || probability > 1)
                throw new InvalidGenerationParameters();

            //container.Size = 0;
            Generate(numberOfVertices, edges, probability, function);
        }

        private RNGCrypto rand = new RNGCrypto();

        private void Generate(Int32 numberOfVertices, Int32 edges, Double probability, String function)
        {
             int initial_vertex_count_ = 1;
             int additional_verteses_count_ = numberOfVertices;
             int vertexcount_ = additional_verteses_count_ + initial_vertex_count_;
             int edgespervertex_ = edges;
             double vertex_addition_probability_ = probability;
             DistributedRandom distributed_random_ = new DistributedRandom(function);
             List<double> fitnesses_ =  new List<double>(vertexcount_);

            for (int current_initial_vertex = 0; current_initial_vertex < initial_vertex_count_; ++current_initial_vertex)
            {
                fitnesses_.Add(distributed_random_.get_distributed_probability());
                container.AddVertex();
                if (current_initial_vertex % 2 == 1)
                {
                    container.AddConnection(current_initial_vertex, current_initial_vertex - 1);
                }
                if (current_initial_vertex == initial_vertex_count_ - 1 && current_initial_vertex != 0 && current_initial_vertex % 2 == 0)
                {
                    container.AddConnection(current_initial_vertex, current_initial_vertex - 1);
                }

            }

            int current_vertex = initial_vertex_count_;
            while (current_vertex != vertexcount_)
            {
                double prob = distributed_random_.get_probability();
                if (prob <= vertex_addition_probability_)
                {
                    fitnesses_.Add(distributed_random_.get_distributed_probability());
                    container.AddVertex();
                    for (int j = 0; j < Math.Min(current_vertex, edgespervertex_); ++j)
                    {
                        int vertex_to_connect = get_vertex_to_connect(current_vertex, fitnesses_, distributed_random_);
                        Debug.Assert(vertex_to_connect >= 0, "internal error");
                        container.AddConnection(current_vertex, vertex_to_connect);
                    }
                    current_vertex++;
                }
                else
                {
                    add_internal_links(1, fitnesses_, distributed_random_);
                }

            }
        }

        private int get_vertex_to_connect(int current_vertex, List<double> fitnesses_, DistributedRandom distributed_random_)
        {
            double sum = 0;
            for (int i = 0; i < current_vertex; ++i)
            {
                sum += (container.GetVertexDegree(i) * fitnesses_[i]);
            }

            double probabilty = distributed_random_.get_probability() * sum;

            sum = 0;
            for (int i = 0; i < current_vertex; ++i)
            {
                sum += (container.GetVertexDegree(i) * fitnesses_[i]);
                if (probabilty <= sum)
                {
                    return i;
                }
            }

            return -1;
        }

        public void add_internal_links(int internal_links_count, List<double> fitnesses_, DistributedRandom distributed_random_)
        {
            if (fitnesses_.Count <= 1)
            {
                return;
            }

            for (int i = 0; i < internal_links_count; ++i)
            {
                int first_vertex;
                int second_vertex;
                vertices_to_connect(out first_vertex, out second_vertex, fitnesses_, distributed_random_);
                Debug.Assert(first_vertex >= 0 && second_vertex >= 0, "internal error");
                container.AddConnection(first_vertex, second_vertex);
            }

        }

        private void vertices_to_connect(out int first, out int second, List<double> fitnesses_, DistributedRandom distributed_random_)
        {
            double sum = 0;
            for (int i = 0; i < fitnesses_.Count; ++i)
            {
                for (int j = i + 1; j < fitnesses_.Count; ++j)
                {
                    sum += (container.GetVertexDegree(i) * fitnesses_[i]) * (container.GetVertexDegree(j) * fitnesses_[j]);
                }
            }

            double probabilty = distributed_random_.get_probability() * sum;

            sum = 0;
            for (int i = 0; i < fitnesses_.Count; ++i)
            {
                for (int j = i + 1; j < fitnesses_.Count; ++j)
                {
                    sum += (container.GetVertexDegree(i) * fitnesses_[i]) * (container.GetVertexDegree(j) * fitnesses_[j]);
                    if (probabilty <= sum)
                    {
                        first = i;
                        second = j;
                        return;
                    }
                }
            }
            first = 0;
            second = 1;
        }

    }

}
