using Core;
using Core.Enumerations;
using Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Globalization;


namespace Draw
{
    public class WSDraw : NonHierarchicDraw
    {
        public int StepCount { get; }
        public int Edges { get; }
        private List<EdgesAddedOrRemoved> initialNetwork;

        public WSDraw(Canvas mainCanvas) : base(mainCanvas)
        {
            StepCount = (int)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.StepCount];
            Edges = (int)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.Edges];

            //LabSessionManager.Generate((int)this.InitialVertexCount, this.Probability, (int)StepCount, (int)Edges);
            GetNetwork();
        }

        protected override void GetNetwork()
        {
            edgesBySteps.Add(LabSessionManager.GetStep(StepCount));
            
            for (int i = 0; i < StepCount; ++i)
            {
                edgesBySteps.Add(LabSessionManager.GetStep(i));
            }
            
            initialNetwork = edgesBySteps[0];
        }

        public override void DrawInitial()
        {
            MainCanvas.Children.Clear();
            DrawVertices();
            // There are edges of initial network in edgesBySteps[0].
            for (int i = 0; i < initialNetwork.Count; i++)
            {
                if (initialNetwork[i].Added)
                {
                    AddEdge(initialNetwork[i]);
                }
                else
                {
                    RemoveEdge(initialNetwork[i]);
                }
            }
        }

        public override void DrawNext(int stepNumber)
        {
            base.DrawNext(stepNumber);
        }

        public override void DrawPrevious(int stepNumber)
        {
            base.DrawPrevious(stepNumber);
        }

        protected override void AddEdge(EdgesAddedOrRemoved edge)
        {
            Line edgeElem = new Line()
            {
                Uid = GenerateEdgeUid(edge),
                Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336"),
                X1 = Vertices[edge.Vertex1].X,
                Y1 = Vertices[edge.Vertex1].Y,
                X2 = Vertices[edge.Vertex2].X,
                Y2 = Vertices[edge.Vertex2].Y
            };
            MainCanvas.Children.Add(edgeElem);
        }

        protected void AddInitialEdge(EdgesAddedOrRemoved edge)
        {
            int vertexDif = Math.Abs(edge.Vertex2 - edge.Vertex1);

            if (vertexDif == InitialVertexCount - 1 || vertexDif == 1)
            {
                Line edgeElem = new Line()
                {
                    Uid = GenerateEdgeUid(edge),
                    Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336"),
                    X1 = Vertices[edge.Vertex1].X,
                    Y1 = Vertices[edge.Vertex1].Y,
                    X2 = Vertices[edge.Vertex2].X,
                    Y2 = Vertices[edge.Vertex2].Y,
                };
                MainCanvas.Children.Add(edgeElem);

            }
            else
            {
                Path path = new Path()
                {
                    Uid = GenerateEdgeUid(edge),
                    Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336"),
                    StrokeThickness = 1
                };

                PathGeometry pathGeometry = new PathGeometry();
                PathFigure pathFigure = new PathFigure()
                {
                    StartPoint = Vertices[Math.Min(edge.Vertex1, edge.Vertex2)]
                };

                ArcSegment arc = new ArcSegment()
                {
                    Point = Vertices[Math.Max(edge.Vertex2, edge.Vertex1)],
                    Size = new Size(5, 5),
                    SweepDirection = SweepDirection.Counterclockwise
                };

                pathFigure.Segments.Add(arc);

                pathGeometry.Figures.Add(pathFigure);
                path.Data = pathGeometry;

                MainCanvas.Children.Add(path);
            }
        }
    }
}
