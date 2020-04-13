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

namespace Draw
{
    public abstract class AbstractDraw
    {
        protected System.Windows.Controls.Canvas MainCanvas { get; }
        public Point[] Vertices { get; set; }
        public Int32 InitialVertexCount { get; set; }
        public int StepNumber { get; set; }

        protected const double vertexRadius = 2.5;


        public AbstractDraw(Canvas mainCanvas)
        {
            MainCanvas = mainCanvas;
        }

        public abstract void DrawInitial();
        public abstract void DrawFinal();
        public abstract void DrawNext(int stepNumber);
        public abstract void DrawPrevious(int stepNumber);
        protected static Point[] GetVertices(Point center, int number, int radius)
        {
            double angle = 360.0 / (double)(number);
            Point[] vertices = new Point[number];
            for (int i = 0; i < number; ++i)
            {
                vertices[i] = DegreesToXY(angle * (double)i, radius, center);
            }
            return vertices;
        }

        private static Point DegreesToXY(double degrees, float radius, Point start)
        {
            Point xy = new Point();
            double radians = degrees * Math.PI / 180.0;

            xy.X = (double)Math.Cos(radians) * radius + start.X;
            xy.Y = (double)Math.Sin(-radians) * radius + start.Y;

            return xy;
        }

        protected List<Ellipse> DrawVertices(bool isSmall = false)
        {
            List<Ellipse> vertexEllipses = new List<Ellipse>();

            Point center = new Point(MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2);
            Double radius = Math.Min((MainCanvas.ActualHeight) / 2, (MainCanvas.ActualWidth / 2)) * 0.9;
            if (isSmall) { radius /= 2; }

            Vertices = GetVertices(center, (int)InitialVertexCount, (int)radius);

            return AddVerticesToCanvas(Vertices, vertexRadius, true);
        }

        protected List<Ellipse> AddVerticesToCanvas(Point[] vertices, double radius, bool fill)
        {
            List<Ellipse> vertexEllipses = new List<Ellipse>();

            for (int i = 0; i < vertices.Length; i++)
            {
                Ellipse ell = new Ellipse()
                {
                    Uid = GenerateVertexUid(i),
                    Width = 2 * radius,
                    Height = 2 * radius,
                };
                if (fill)
                {
                    ell.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336");
                }
                else
                {
                    ell.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336");
                }

                MainCanvas.Children.Add(ell);
                Canvas.SetTop(ell, vertices[i].Y - (double)ell.Width / 2);
                Canvas.SetLeft(ell, vertices[i].X - (double)ell.Width / 2);

                vertexEllipses.Add(ell);
            }

            return vertexEllipses;
        }

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
    //        Debug.Assert(i != MainCanvas.Children.Count);
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

        protected string GenerateClusterUid(int level, int number)
        {
            return level + "." + number;
        }

        public void ActivateOrDeactivateVertex(int vertex, bool activate)
        {
            double top = 0, left = 0;
            string vertexID = GenerateVertexUid(vertex);
            for (int i = 0; i < MainCanvas.Children.Count; i++)
            {
                if (MainCanvas.Children[i].Uid == vertexID)
                {
                    left = Canvas.GetLeft(MainCanvas.Children[i]);
                    top = Canvas.GetTop(MainCanvas.Children[i]);
                    MainCanvas.Children.RemoveAt(i);
                    break;
                }
            }
            double radius = (activate ? 7 : 5);
            string color = (activate ? "#ff0000" : "#323336");
            Ellipse v = new Ellipse()
            {
                Uid = vertexID,
                Width = radius,
                Height = radius,
                Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(color)
            };
            MainCanvas.Children.Add(v);
            Canvas.SetLeft(v, left);
            Canvas.SetTop(v, top);
        }
    }
}

