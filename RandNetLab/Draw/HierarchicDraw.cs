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
            for(int i = 0; i < 27; ++i)
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


            steps.Add(new List<EdgesAddedOrRemoved>() { new EdgesAddedOrRemoved(1, 3, true), new EdgesAddedOrRemoved(2, 3, true), new EdgesAddedOrRemoved(4, 5, true), new EdgesAddedOrRemoved(5, 6, true), new EdgesAddedOrRemoved(7, 8, true) });
            steps.Add(new List<EdgesAddedOrRemoved>() { new EdgesAddedOrRemoved(1, 2, true) });
            //
            BranchingIndex = (Int32)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.BranchingIndex];
            Mu = (Double)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.Mu];
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
            clustersByLevel.Add(AddVerticesToCanvas(clusterCenters, radius, false));
            for (int i = 0; i < clusterCenters.Length; ++i)
            {
                DrawPreviousLevels(radius, clusterCenters[i], level, i);
            }
        }

        private void DrawPreviousLevels(double radius, Point center, int level, int clusterNumber)
        {
            if (level == 0) { return; }

            // previous level nodes are vertices
            Point[] nodes = GetVertices(center, branching[level][clusterNumber], (int)(radius / 2));

            double r = radius / 3;
            bool fill = false;
            if (level == 1) { r = 2.5;  fill = true; }
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
    }
}
