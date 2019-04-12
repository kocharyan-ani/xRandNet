using System;
using System.Collections.Generic;

namespace NetworkModel.Engines
{
    // Code translated from java. Not reviewed, checked and tested.
    class NonHierarchicAnalyzerCompleteEngine
    {
        private readonly NonHierarchicContainer container;

        public SortedDictionary<Double, Double> CompleteDistribution { get; private set; }

        public NonHierarchicAnalyzerCompleteEngine(NonHierarchicContainer c)
        {
            container = c;
            CompleteDistribution = new SortedDictionary<Double, Double>();
        }

        public void Calculate()
        {
            SortedDictionary<int, List<List<int>>> subGraphs = BronKerbosch();
            SortedDictionary<int, List<List<int>>> distribution = new SortedDictionary<int, List<List<int>>>();

            foreach (int i in subGraphs.Keys)
            {
                distribution.Add(i, subGraphs[i]);
            }

            for (int i = container.Size; i >= 2; i--)
            {
                List<List<int>> sets = distribution[i];
                foreach (List<int> set in sets)
                {
                    List<List<int>> subset = AllSubsetsWithLessByOnePower(set);
                    List<List<int>> sets1 = distribution[i - 1];
                    if (subset != null)
                    {
                        foreach (List<int> sub in subset)
                        {
                            sets1.Add(sub);
                        }
                    }
                    distribution[i - 1] = sets1;
                }
            }
        }

        private SortedDictionary<int, List<List<int>>> BronKerbosch()
        {
            List<int> r = new List<int>();

            List<int> p = new List<int>();
            for (int v = 1; v <= container.Size; v++)
            {
                p.Add(v);
            }

            List<int> x = new List<int>();

            SortedDictionary<int, List<List<int>>> distribution = new SortedDictionary<int, List<List<int>>>();

            BronKerbosch(distribution, 0, r, p, x);

            return distribution;
        }

        private List<List<int>> AllSubsetsWithLessByOnePower(List<int> set)
        {
            if (set.Count == 0) return null;

            List<List<int>> subsets = new List<List<int>>();
            List<int> list = new List<int>(set);

            for (int i = 0; i < list.Count; i++)
            {
                int item = list[i];
                list.Remove(i);
                subsets.Add(new List<int>(list));
                list.Insert(i, item);
            }

            return subsets;
        }

        private void BronKerbosch(SortedDictionary<int, List<List<int>>> distribution, int recStep,
            List<int> r, List<int> p, List<int> x)
        {
            // If p and x are both empty:
            if (p.Count == 0 && x.Count == 0)
            {
                // Report r as a maximal clique
                // TODO should be opened
                /*List<List<int>> d = distribution.computeIfAbsent(r.Count, k-> new HashSet<>());
                d.Add(r);*/
                return;
            }

            // for each vertex v in p:
            List<int> vertices = p;
            foreach (int v in vertices)
            {
                // bronKerbosch(r ⋃ {v}, p ⋂ n(v), x ⋂ n(v))
                List<int> nv = container.GetAdjacentVertices(v);
                BronKerbosch(distribution, recStep + 1, Union(r, v), Intersect(p, nv), Intersect(x, nv));

                // p = p \ {v}
                p = Subtract(p, v);
                // x = x ⋃ {v}
                x = Union(x, v);
            }
        }

        private List<int> Union(List<int> items, int item)
        {
            List<int> result = new List<int>(items);

            result.Add(item);

            return result;
        }

        private List<int> Intersect(List<int> first, List<int> second)
        {
            List<int> result = new List<int>();

            foreach (int item in first)
            {
                if (second.Contains(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }        

        private List<int> Subtract(List<int> items, int item)
        {
            List<int> result = new List<int>(items);

            result.Remove(item);

            return result;
        }
    }
}
