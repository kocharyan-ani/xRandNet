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
using Core.Enumerations;
using Session;

namespace Draw
{
    public class BADraw : NonHierarchicDraw
    {
        public int StepCount { get; }
        public int Edges { get; }
        private List<Ellipse> newVertices;
        private List<Ellipse> VerticesAddedToCanvas;
        private readonly Point[] addedVertexPoints;
        private List<EdgesAddedOrRemoved> initialNetwork;
        Random rand;

        public BADraw(Canvas mainCanvas) : base(mainCanvas)
        {
            StepCount = (int)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.StepCount];
            Edges = (int)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.Edges];
            newVertices = new List<Ellipse>();
            VerticesAddedToCanvas = new List<Ellipse>();
            addedVertexPoints = new Point[StepCount];

            //LabSessionManager.Generate((int)this.InitialVertexCount, this.Probability, (int)StepCount, (int)Edges);
            GetNetwork();

            rand = new Random(DateTime.Now.Millisecond);
        }

        protected override void GetNetwork()
        {
            
            for (int i = 0; i < StepCount; i++)
            {
                edgesBySteps.Add(LabSessionManager.GetStep(i));
            }

            edgesBySteps.Add(LabSessionManager.GetStep(StepCount));


            initialNetwork = edgesBySteps[0];
        }

        public override void DrawInitial()
        {
            MainCanvas.Children.Clear();
            DrawVertices();

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
            int vertexNumber = (int)InitialVertexCount + stepNumber - 1;
            DrawVertex(vertexNumber);
            MakeVertexGray(vertexNumber - 1);
            base.DrawNext(stepNumber);
        }

        public override void DrawPrevious(int stepNumber)
        {
            int vertexNumber = (int)InitialVertexCount + stepNumber - 1;
            base.DrawPrevious(stepNumber);
            RemoveVertex(vertexNumber);
            if (vertexNumber != Vertices.Length)
            {
                MakeVertexRed(vertexNumber - 1);
            }
        }

        protected override void AddEdge(EdgesAddedOrRemoved edge)
        {
            Line edgeElem = new Line();
            edgeElem.Uid = GenerateEdgeUid(edge);
            edgeElem.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336");
            if (edge.Vertex1 < Vertices.Length)
            {
                edgeElem.X1 = Vertices[edge.Vertex1].X;
                edgeElem.Y1 = Vertices[edge.Vertex1].Y;
            }
            else
            {
                // ModelType = BA, VertexCount = 5, Edges = 10, Probability = 0.1, StepCount = 10
                // Cashes here. IndexOutOfBounds Exception
                // Vertex1 = 5, InitialVertexCount = 5
                // Crashes on master also.
                edgeElem.X1 = addedVertexPoints[edge.Vertex1 - InitialVertexCount].X;
                edgeElem.Y1 = addedVertexPoints[edge.Vertex1 - InitialVertexCount].Y;
            }
            if (edge.Vertex2 < Vertices.Length)
            {
                edgeElem.X2 = Vertices[edge.Vertex2].X;
                edgeElem.Y2 = Vertices[edge.Vertex2].Y;
            }
            else
            {
                edgeElem.X2 = addedVertexPoints[edge.Vertex2 - InitialVertexCount].X;
                edgeElem.Y2 = addedVertexPoints[edge.Vertex2 - InitialVertexCount].Y;
            }
            MainCanvas.Children.Add(edgeElem);
        }


        private void DrawVertex(int vertexNumber)
        {
            string vertexUid = GenerateVertexUid(vertexNumber);

            int x;
            int y;

            if(vertexNumber >= Vertices.Length + VerticesAddedToCanvas.Count)
            {
                // If vertex appears at first time , generate random coordinamtes to place it

                x = rand.Next(0, (int)MainCanvas.ActualWidth);
                y = rand.Next(0, (int)MainCanvas.ActualHeight);
                addedVertexPoints[vertexNumber - InitialVertexCount] = new Point()
                {
                    X = x,
                    Y = y
                };
            }
            else
            {
                x = (int)addedVertexPoints[vertexNumber - InitialVertexCount].X;
                y = (int)addedVertexPoints[vertexNumber - InitialVertexCount].Y;
            }

            Ellipse vertex = new Ellipse
            {
                Uid = vertexUid,
                Width = 5,
                Height = 5,
                Fill = Brushes.Red

            };

            VerticesAddedToCanvas.Add(vertex);
            MainCanvas.Children.Add(vertex);

            Canvas.SetTop(vertex, y - (double)vertex.Width / 2);
            Canvas.SetLeft(vertex, x - (double)vertex.Width / 2);
        }

        private void RemoveVertex(int vertexNumber)
        {
            string Uid = GenerateVertexUid(vertexNumber);
            for (int i = 0; i < MainCanvas.Children.Count; i++)
            {
                if (MainCanvas.Children[i].Uid == Uid)
                {
                    Ellipse vertex = (Ellipse)MainCanvas.Children[i];
                    vertex.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336");
                    newVertices.Remove(vertex);
                    MainCanvas.Children.RemoveAt(i);
                    return;
                }
            }
        }

        private void MakeVertexGray(int vertexNumber)
        {
            string Uid = GenerateVertexUid(vertexNumber);
            for (int i = 0; i < MainCanvas.Children.Count; i++)
            {
                if (MainCanvas.Children[i].Uid == Uid)
                {
                    ((Ellipse)MainCanvas.Children[i]).Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336");
                    return;
                }
            }
        }

        private void MakeVertexRed(int vertexNumber)
        {
            string Uid = GenerateVertexUid(vertexNumber);
            for (int i = 0; i < MainCanvas.Children.Count; i++)
            {
                if (MainCanvas.Children[i].Uid.Equals(Uid))
                {
                    ((Ellipse)MainCanvas.Children[i]).Fill = Brushes.Red;
                    return;
                }
            }
        }
    }
}
