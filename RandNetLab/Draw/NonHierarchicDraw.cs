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
using Core;
using System.Diagnostics;

namespace Draw
{
    public abstract class NonHierarchicDraw : AbstractDraw
    {
        public Int32 InitialVertexCount { get; set; }
        public double Probability { get; set; }
        public Point[] Vertices { get; set; }

        
        // Keeps all edges separated by steps
        protected List<List<EdgesAddedOrRemoved>> edgesBySteps;

        public NonHierarchicDraw(Canvas mainCanvas) : base(mainCanvas)
        {
            InitialVertexCount = (Int32)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.Vertices];
            Probability = (double)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.Probability];
            edgesBySteps = new List<List<EdgesAddedOrRemoved>>();
        }

        protected void DrawVertices(bool isSmall = false)
        {
            Point center = new Point(MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2);
            Double radius = Math.Min((MainCanvas.ActualHeight) / 2, (MainCanvas.ActualWidth / 2)) * 0.9;
            if(isSmall) { radius /= 2; }

            Vertices = GetVertices(center, (int)InitialVertexCount, (int)radius);


            for (int i = 0; i < Vertices.Length; i++)
            {
                Ellipse ell = new Ellipse()
                {
                    Uid = GenerateVertexUid(i),
                    Width = 5,
                    Height = 5,
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336")
                };
                MainCanvas.Children.Add(ell);
                Canvas.SetTop(ell, Vertices[i].Y - (double)ell.Width/2);
                Canvas.SetLeft(ell, Vertices[i].X - (double) ell.Width/2);
            }
        }

        public static Point[] GetVertices(Point center, int number, int radius)
        {
            double angle = 360.0/(double)(number);
            Point[] vertices = new Point[number];
            for (int i = 0; i < number; ++i)
            {
                vertices[i] = DegreesToXY(angle * (double)i, radius, center);
            }
            return vertices;
        }

        protected static Point DegreesToXY(double degrees, float radius, Point start)
        {
            Point xy = new Point();
            double radians = degrees * Math.PI / 180.0;

            xy.X = (double)Math.Cos(radians) * radius + start.X;
            xy.Y = (double)Math.Sin(-radians) * radius + start.Y;

            return xy;
        }

        public override void DrawFinal()
        {
            DrawInitial();
           
            for (int i = 1; i < edgesBySteps.Count; ++i)
            {
                DrawNext(i);
            }
        }

        public override void DrawNext(int stepNumber)
        {
            List<EdgesAddedOrRemoved> edges = edgesBySteps[stepNumber];
            if (edges == null)
            {
                return;
            }
            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].Added)
                {
                    AddEdge(edges[i]);
                }
                else
                {
                    RemoveEdge(edges[i]);
                }
            }
        }

        public override void DrawPrevious(int stepNumber)
        {
            List<EdgesAddedOrRemoved> edges = edgesBySteps[stepNumber];
            if (edges == null)
            {
                return;
            }
            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].Added)
                {
                    RemoveEdge(edges[i]);
                }
                else
                {
                    AddEdge(edges[i]);
                }
            }
        }

        protected abstract void GetNetwork();

        protected abstract void AddEdge(EdgesAddedOrRemoved edge);

        protected void RemoveEdge(EdgesAddedOrRemoved edge)
        {
            string edgeUid = GenerateEdgeUid(edge);

            int i = 0;
            for (; i < MainCanvas.Children.Count; i++)
            {
                if (MainCanvas.Children[i].Uid == edgeUid)
                {
                    MainCanvas.Children.RemoveAt(i);
                    break;
                }
            }
            Debug.Assert(i != MainCanvas.Children.Count);
        }

        protected string GenerateEdgeUid(EdgesAddedOrRemoved edge)
        {
            return edge.Vertex1 < edge.Vertex2 ? "v" + edge.Vertex1.ToString() + "v" + edge.Vertex2.ToString() :
                                                 "v" + edge.Vertex2.ToString() + "v" + edge.Vertex1.ToString();
        }

        protected string GenerateVertexUid(int vertexNumber)
        {
            return "v" + vertexNumber.ToString();
        }
    }
}
