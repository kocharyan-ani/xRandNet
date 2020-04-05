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

        private List<List<Cluster>> clustersByLevel;
        private List<Point>[] centers;
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
            //branching.Add(new List<int>() { 1, 1 ,1, 1, 1, 1, 1, 1, 1 });
            //branching.Add(new List<int>() { 3, 3, 3 });
            //branching.Add(new List<int>() { 3 });
            List<int> l = new List<int>();
            for (int i = 0; i < 27; ++i)
            {
                l.Add(1);
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


            steps.Add(new List<EdgesAddedOrRemoved>() { new EdgesAddedOrRemoved(1, 3, true), new EdgesAddedOrRemoved(2, 3, true), new EdgesAddedOrRemoved(4, 5, true), new EdgesAddedOrRemoved(5, 6, true), new EdgesAddedOrRemoved(7, 8, true), new EdgesAddedOrRemoved(26, 25, true) });
            steps.Add(new List<EdgesAddedOrRemoved>() { new EdgesAddedOrRemoved(1, 2, true), new EdgesAddedOrRemoved(4, 5, true), new EdgesAddedOrRemoved(7, 8, true), new EdgesAddedOrRemoved(8, 9, true) });
            //
            BranchingIndex = (Int32)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.BranchingIndex];

            Level = branching.Count;

            BranchingIndex = 3;
            Level = 3;
        }

        public override void DrawFinal()
        {
            DrawLevel(Level);
        }

        public override void DrawInitial()
        {
            MainCanvas.Children.Clear();
            List<Cluster> level0 = DrawVertices();
            if (clustersByLevel.Count == 0)
            {
                clustersByLevel.Add(level0);
            }
        }

        public override void DrawNext(int stepNumber)
        {
            centers = new List<Point>[stepNumber];
            for (int i = 0; i < stepNumber; i++)
            {
                centers[i] = new List<Point>();
            }
            DrawLevel(stepNumber);
        }

        public override void DrawPrevious(int stepNumber)
        {
            DrawLevel(stepNumber - 1);
        }

        private void DrawLevel(int level)
        {
            Debug.Assert(level >= 0 && level <= Level);

            if (level == 0)
            {
                DrawInitial();
                return;
            }

            MainCanvas.Children.Clear();

            double maxRadius = Math.Min((MainCanvas.ActualHeight) / 2, (MainCanvas.ActualWidth / 2)) * 0.9;
            double radius = Math.Pow(3, level - 1) * (maxRadius / Math.Pow(3, Level - 1));
            Point center = new Point(MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2);

            Point[] clusterCenters = GetVertices(center, branching[level].Count, (int)(maxRadius - radius));

            //for (int i = 0; i < clusterCenters.Length; i++)
            //{
            //    centers[level - 1].Add(clusterCenters[i]);
            //}
            clustersByLevel.Add(AddVerticesToCanvas(clusterCenters, radius, false));
            for (int i = 0; i < clusterCenters.Length; ++i)
            {
                DrawPreviousLevels(radius, clusterCenters[i], level, i);
            }
            for (int i = 0; i < level; i++)
            {
                AddEdges(i + 1);
            }

        }

        private void DrawPreviousLevels(double radius, Point center, int level, int clusterNumber)
        {
            if (level == 0) { return; }

            // previous level nodes are vertices
            Point[] nodes = GetVertices(center, branching[level][clusterNumber], (int)(radius / 2));

            for (int i = 0; i < nodes.Length; i++)
            {
                centers[level - 1].Add(nodes[i]);
            }

            double r = radius / 3;
            bool fill = false;
            if (level == 1) { r = 2.5; fill = true; }
            AddVerticesToCanvas(nodes, r, fill);


            for (int i = 0; i < nodes.Length; ++i)
            {
                DrawPreviousLevels(radius / 3, nodes[i], level - 1, i);
            }

        }

        private void RemoveLevel(int level)
        {
            Debug.Assert(level >= 0 && level < clustersByLevel.Count());
        }


        private void AddEdges(int level)
        {
            double radius = 0;

            if (level != 1)
            {
                radius = clustersByLevel[level - 1][0].Width / 2;
            }

            List<Point> levelCenters = centers[level - 1];
            for (int i = 0; i < steps[level].Count; i++)
            {
                EdgesAddedOrRemoved edge = steps[level][i];

                Line edgeElem;

                if (level == 1)
                {
                    edgeElem = new Line
                    {
                        Uid = GenerateEdgeUid(edge),
                        Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336"),
                        X1 = levelCenters[edge.Vertex1 - 1].X,
                        Y1 = levelCenters[edge.Vertex1 - 1].Y,
                        X2 = levelCenters[edge.Vertex2 - 1].X,
                        Y2 = levelCenters[edge.Vertex2 - 1].Y
                    };
                }
                else
                {
                    Tuple<Point, Point> edgeCoordinates = GetEdgeCoordinates(levelCenters[edge.Vertex1 - 1],
                                                                             levelCenters[edge.Vertex2 - 1],
                                                                             radius);
                    Point coordinate1 = edgeCoordinates.Item1;
                    Point coordinate2 = edgeCoordinates.Item2;
                    edgeElem = new Line
                    {
                        Uid = GenerateEdgeUid(edge),
                        Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336"),
                        X1 = coordinate1.X,
                        Y1 = coordinate1.Y,
                        X2 = coordinate2.X,
                        Y2 = coordinate2.Y
                    };
                }

                MainCanvas.Children.Add(edgeElem);
            }
        }

        private Tuple<Point, Point> GetEdgeCoordinates(Point circleCenter1, Point circleCenter2, double radius)
        {
            double angle = Math.Atan((circleCenter2.Y - circleCenter1.Y) / (circleCenter2.X - circleCenter1.X));
            Point coordinate1 = new Point();
            Point coordinate2 = new Point();

            double x1, y1;
            double x3, y3;

            double x2, y2;
            double x4, y4;


            x1 = circleCenter1.X + radius * Math.Cos(angle);
            y1 = circleCenter1.Y + radius * Math.Sin(angle);
            x3 = circleCenter1.X + radius * Math.Cos(angle - Math.PI);
            y3 = circleCenter1.Y + radius * Math.Sin(angle - Math.PI);

            x2 = circleCenter2.X + radius * Math.Cos(angle);
            y2 = circleCenter2.Y + radius * Math.Sin(angle);
            x4 = circleCenter2.X + radius * Math.Cos(angle - Math.PI);
            y4 = circleCenter2.Y + radius * Math.Sin(angle - Math.PI);

            double distance1 = Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2);
            double distance2 = Math.Pow(x1 - x4, 2) + Math.Pow(y1 - y4, 2);
            double distance3 = Math.Pow(x3 - x2, 2) + Math.Pow(y3 - y2, 2);
            double distance4 = Math.Pow(x3 - x4, 2) + Math.Pow(y3 - y4, 2);
            double minDistance =  Math.Min(Math.Min(Math.Min(distance1, distance2), distance3), distance4);

            if (distance1 == minDistance)
            {
                coordinate1.X = x1;
                coordinate1.Y = y1;
                coordinate2.X = x2;
                coordinate2.Y = y2;
            }
            else if (distance2 == minDistance)
            {
                coordinate1.X = x1;
                coordinate1.Y = y1;
                coordinate2.X = x4;
                coordinate2.Y = y4;
            }
            else if (distance3 == minDistance)
            {
                coordinate1.X = x3;
                coordinate1.Y = y3;
                coordinate2.X = x2;
                coordinate2.Y = y2;
            }
            else
            {
                coordinate1.X = x3;
                coordinate1.Y = y3;
                coordinate2.X = x4;
                coordinate2.Y = y4;
            }

            Tuple<Point, Point> edgeCoordinates = new Tuple<Point, Point>(coordinate1, coordinate2);

            return edgeCoordinates;
        }
    }
}
