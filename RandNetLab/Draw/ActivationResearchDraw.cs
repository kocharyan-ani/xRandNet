using Core.Enumerations;
using RandNetLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Draw
{
    public class ActivationResearchDraw : AbstractResearchDraw
    {
        MainWindow MWindow = Application.Current.Windows[0] as MainWindow;

        private const int RESEARCH_STEP_DURATION_BY_MILISECONDS = 5000;

        //*tmp
        private List< List< Tuple< int, bool > > > activationSteps; 
        public ActivationResearchDraw() : base(ModelType.ER) 
        {
            activationSteps = new List<List<Tuple<int, bool>>>();
            activationSteps.Add(new List<Tuple<int, bool>>() { new Tuple<int, bool>(0, true) });
            activationSteps.Add(new List<Tuple<int, bool>>() { new Tuple<int, bool>(0, false),
                                                               new Tuple<int, bool>(1, true), 
                                                               new Tuple<int, bool>(DrawObj.InitialVertexCount - 1, true) });
            activationSteps.Add(new List<Tuple<int, bool>>() { new Tuple<int, bool>(1, false),
                                                               new Tuple<int, bool>(2, true),
                                                               new Tuple<int, bool>(DrawObj.InitialVertexCount - 1, false),
                                                               new Tuple<int, bool>(DrawObj.InitialVertexCount - 2, true) });

        }

    public override void StartResearch() 
        {
            MWindow.Start.Content = "Stop";
            MWindow.mainCanvas.Children.Clear();
            DrawObj.DrawFinal();
            DrawActivationProcess();
            MWindow.Start.Content = "Start";
        }
        public override void StopResearch() { }
        public override void SaveResearch() { }

        public override void OnWindowSizeChanged() { }

        private void DrawActivationProcess()
        { 
            for(int i = 0; i < activationSteps.Count; ++i)
            {
                List<Tuple<int, bool>> step = activationSteps[i];
                foreach (var s in step)
                {
                   DrawObj.ActivateOrDeactivateVertex(s.Item1, s.Item2);
                }
                System.Threading.Thread.Sleep(RESEARCH_STEP_DURATION_BY_MILISECONDS);
            }
        }
    }
}
