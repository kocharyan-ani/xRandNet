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
        public double Probability { get; set; }

        
        // Keeps all edges separated by steps
        protected List<List<EdgesAddedOrRemoved>> edgesBySteps;

        public NonHierarchicDraw(Canvas mainCanvas) : base(mainCanvas)
        {
            InitialVertexCount = (Int32)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.Vertices];
            Probability = (double)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.Probability];
            edgesBySteps = new List<List<EdgesAddedOrRemoved>>();
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



    }
}
