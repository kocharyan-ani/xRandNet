using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Core;
using Core.Enumerations;
using Session;
using Cluster = System.Windows.Shapes.Ellipse;

namespace Draw
{
    public class HierarchicDraw : AbstractDraw
    {
        protected int BranchingIndex { get; set; }
        private double Mu { get; set; }
        protected int Level { get; set; }
        private bool isFlat;
        public bool IsFlat {
            get
            {
                return isFlat;
            }
            set
            {
                isFlat = value;
                OnFlatChanged();
            }
        }

        private List<List<Cluster>> clustersByLevel;

        //*tmp
        private List<List<EdgesAddedOrRemoved>> steps;
        private List<List<int>> branching;
        public HierarchicDraw(Canvas mainCanvas) : base(mainCanvas)
        {
            clustersByLevel = new List<List<Cluster>>();
            //*tmp
            branching = new List<List<int>>();
            steps = new List<List<EdgesAddedOrRemoved>>();

            steps.Add(null);
            //branching.Add(new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            //branching.Add(new List<int>() { 3, 3, 3 });
            //branching.Add(new List<int>() { 3 });
            List<int> l = new List<int>();
            for (int i = 0; i < 27; ++i)
            {
                l.Add(0);
            }
            branching.Add(l);
            l.Clear();
            for (int i = 0; i < 9; ++i)
            {
                l.Add(3);
            }
            branching.Add(l);
            branching.Add(new List<int>() { 3, 3, 3 });
            branching.Add(new List<int>() { 3 });


            steps.Add(new List<EdgesAddedOrRemoved>() { new EdgesAddedOrRemoved(0, 1, true), new EdgesAddedOrRemoved(3, 5, true) });
            steps.Add(new List<EdgesAddedOrRemoved>() { new EdgesAddedOrRemoved(0, 2, true), new EdgesAddedOrRemoved(1, 2, true), new EdgesAddedOrRemoved(3, 4, true), new EdgesAddedOrRemoved(4, 5, true), new EdgesAddedOrRemoved(6, 7, true) });
            steps.Add(new List<EdgesAddedOrRemoved>() { new EdgesAddedOrRemoved(0, 1, true) });
            //
            BranchingIndex = (Int32)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.BranchingIndex];
            Mu = (Double)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.Mu];
            Level = branching.Count;

            BranchingIndex = 3;
            Level = 3;
        }

        protected void OnFlatChanged()
        {
            MainCanvas.Children.Clear();
            if (StepNumber == 0)
            {
                DrawInitial();
            }
            else
            {
                if (IsFlat)
                {
                    DrawFlatLevel(StepNumber);
                }
                else
                {
                    DrawNext(StepNumber);
                }
            }
        }

        public override void DrawFinal()
        {
            if (IsFlat)
            {
                DrawFlatLevel(Level);
            }
            else
            {
                DrawLevel(Level);
            }
        }

        public override void DrawInitial()
        {
            MainCanvas.Children.Clear();
            List<Cluster> level0;
            level0 = (StepNumber == 0) ? DrawVertices() : AddVerticesToCanvas(Vertices, vertexRadius, true) ;

            if (clustersByLevel.Count == 0)
            {
                clustersByLevel.Add(level0);
            }
            else
            {
                clustersByLevel[0] = level0;
            }
        }

        public override void DrawNext(int stepNumber)
        {
            if (IsFlat)
            {
                DrawFlatLevel(stepNumber);
            }
            else
            {
                DrawLevel(stepNumber);
            }
        }

        public override void DrawPrevious(int stepNumber)
        {
            if (IsFlat)
            {
                DrawFlatLevel(stepNumber - 1);
            }
            else
            {
                DrawLevel(stepNumber - 1);
            }
        }

        private void SetVerticesByLevel(int level)
        {
            DrawLevel(level, true);
        }

        // if setVeritices is true, doesn't draw the level, but only determine locations of actual vertices
        // setVertices = true -> called from flat mode
        private void DrawLevel(int level, bool setVertices = false)
        {
            Debug.Assert(level >= 0 && level <= Level);

            if (level == 0)
            {
                if (setVertices)
                {
                    double r = Math.Min((MainCanvas.ActualHeight) / 2, (MainCanvas.ActualWidth / 2)) * 0.9;
                    Point c = new Point(MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2);
                    Vertices = GetVertices(c, InitialVertexCount, (int)r);
                }
                else
                {
                    DrawInitial();
                }
                return;
            }

            if(!setVertices) MainCanvas.Children.Clear();

            double maxRadius = Math.Min((MainCanvas.ActualHeight) / 2, (MainCanvas.ActualWidth / 2)) * 0.9;
            double radius = Math.Pow(3, level - 1) * (maxRadius / Math.Pow(3, Level - 1));
            Point center = new Point(MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2);

            Point[] clusterCenters = GetVertices(center, branching[level].Count, (int)(maxRadius - radius));
            if(!setVertices) clustersByLevel.Add(AddVerticesToCanvas(clusterCenters, radius, false));
            for (int i = 0; i < clusterCenters.Length; ++i)
            {
                DrawPreviousLevels(radius, clusterCenters[i], level, i, setVertices);
            }
        }

        // for every cluster draws clusters which arecontained in it
        private void DrawPreviousLevels(double radius, Point center, int level, int clusterNumber, bool setVertices = false)
        {
            if (level == 0) { return; }

            Point[] nodes = GetVertices(center, branching[level][clusterNumber], (int)(radius / 2));

            double r = radius / 3;
            bool fill = false;
            // previous level nodes are vertices
            if (level == 1)
            {
                r = vertexRadius;  fill = true;
                if(setVertices)
                {
                    int begin, end;
                    getVerticesOfCluster(level, clusterNumber, out begin, out end);
                    for (int i = 0; i < nodes.Length; ++i)
                    {
                        Vertices[begin + i] = nodes[i];
                    }
                }
            }
            if(!setVertices) AddVerticesToCanvas(nodes, r, fill);

            int clusterBegin, clusterEnd;
            getPreviousLevelClustersOfCluster(level, clusterNumber, out clusterBegin, out clusterEnd);
            for (int i = 0; i < nodes.Length; ++i)
            {
                DrawPreviousLevels(radius / 3, nodes[i], level - 1, clusterBegin + i, setVertices);
            }

        }

        private void DrawFlatLevel(int level)
        {
            // to set Vertices member
            SetVerticesByLevel(level);
            DrawInitial();
            for (int i = 1; i <= level; ++i)
            {
                AddOrRemoveFlatLevel(i, true);
            }
        }

        // addes or removes edges of level in flat mode
        private void AddOrRemoveFlatLevel(int level, bool add)
        {
            List<EdgesAddedOrRemoved> edges = steps[level];
            foreach (var edge in edges)
            {
                ConnectOrDisconnectClustersFlat(level - 1, edge.Vertex1, edge.Vertex2, add);
            }
        }

        private void ConnectOrDisconnectClustersFlat(int level, int cluster1, int cluster2, bool connect)
        {
            if (level == 0)
            {
                if (connect) { 
                    AddEdgeFlat(cluster1, cluster2);
                } else {
                    RemoveEdge(new EdgesAddedOrRemoved(cluster1, cluster2, false));
                }
                return;
            }

            int innerClusters1Begin, innerClusters1End, innerClusters2Begin, innerClusters2End;
            getPreviousLevelClustersOfCluster(level, cluster1, out innerClusters1Begin, out innerClusters1End);
            getPreviousLevelClustersOfCluster(level, cluster2, out innerClusters2Begin, out innerClusters2End);

            for(int i = innerClusters1Begin; i <= innerClusters1End; ++i)
            {
                for (int j = innerClusters2Begin; j <= innerClusters2End; ++j)
                {
                    ConnectOrDisconnectClustersFlat(level - 1, i, j, connect);
                }
            }
        }

        private void AddEdgeFlat(int v1, int v2)
        {
            Line edge = new Line();
            edge.Uid = GenerateEdgeUid(new EdgesAddedOrRemoved(v1, v2, true));
            edge.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336");
            edge.X1 = Vertices[v1].X;
            edge.Y1 = Vertices[v1].Y;
            edge.X2 = Vertices[v2].X;
            edge.Y2 = Vertices[v2].Y;

            MainCanvas.Children.Add(edge);
        }

        // returns indeces of first and last vertices contained in cluster
        private void getVerticesOfCluster(int level, int clusterNumber, out int beginIndex, out int endIndex)
        {
            beginIndex = 0;
            for(int i = 0; i < clusterNumber; ++i)
            {
                getCountOfVerticesInCluster(level, i, ref beginIndex);
            }
            endIndex = 0;
            getCountOfVerticesInCluster(level, clusterNumber, ref endIndex);
            endIndex += (beginIndex - 1);
        }

        private void getCountOfVerticesInCluster(int level, int clusterNumber, ref int numberOfVertices)
        {
            if(level == 1)
            {
                numberOfVertices += branching[level][clusterNumber];
                return;
            }
            int clusterBegin, clusterEnd;
            getPreviousLevelClustersOfCluster(level, clusterNumber, out clusterBegin, out clusterEnd);
            for(int i = clusterBegin; i <= clusterEnd; ++i)
            {
                getCountOfVerticesInCluster(level - 1, i, ref numberOfVertices);
            }
        }

        // returns indeces of first and last clusters contained in cluster
        private void getPreviousLevelClustersOfCluster(int level, int clusterNumber, out int beginIndex, out int endIndex)
        {
            beginIndex = 0;
            List<int> levelBranching = branching[level];
            for (int i = 0; i < clusterNumber; ++i)
            {
                beginIndex += levelBranching[i];
            }
            endIndex = beginIndex + levelBranching[clusterNumber] - 1;
        }
    }
}
