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

namespace Draw
{
    public class ERDraw : NonHierarchicDraw
    {
        public ERDraw(Canvas mainCanvas) : base(mainCanvas)
        {
            LabSessionManager.Generate((int)this.InitialVertexCount, this.Probability);
            GetNetwork();
        }

        protected override void GetNetwork()
        {
            int stepCount = LabSessionManager.GetFinalStepNumber();
        
            try
            {
                for (int i = 0; i < stepCount; ++i)
                {
                    edgesBySteps.Add(LabSessionManager.GetStep(i));
                }
            }
            catch (System.ArgumentOutOfRangeException) { }
        }

        
        public override void DrawInitial()
        {
            MainCanvas.Children.Clear();
            DrawVertices();
        }

       
        protected override void AddEdge(EdgesAddedOrRemoved edge)
        {
            Line edgeElem = new Line
            {
                Uid = GenerateEdgeUid(edge),
                Stroke = (SolidColorBrush) new BrushConverter().ConvertFromString("#323336"),
                X1 = Vertices[edge.Vertex1].X,
                Y1 = Vertices[edge.Vertex1].Y,
                X2 = Vertices[edge.Vertex2].X,
                Y2 = Vertices[edge.Vertex2].Y
            };
            MainCanvas.Children.Add(edgeElem);
        }
    }
}
