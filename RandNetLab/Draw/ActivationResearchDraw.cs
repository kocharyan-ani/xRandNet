using Core.Enumerations;
using RandNetLab;
using Session;
using System;
using System.Collections;
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

        private const int RESEARCH_STEP_DURATION_BY_MILISECONDS = 50;

        public ActivationResearchDraw() : base(ModelType.ER) 
        {
            StepCount = LabSessionManager.GetActivationStepCount();
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

        private async void DrawActivationProcess()
        {
            for (int i = 0; i < StepCount; ++i)
            {
                BitArray step = LabSessionManager.GetActivationStep(i);
                for (int j = 0; j < step.Count; ++j)
                {
                    Dispatcher.CurrentDispatcher.Invoke(new Action(() => {DrawObj.ActivateOrDeactivateVertex(j, step[j]); }));
                }
                //System.Threading.Thread.Sleep(RESEARCH_STEP_DURATION_BY_MILISECONDS);
                await Task.Delay(RESEARCH_STEP_DURATION_BY_MILISECONDS);
            }
        }
    }
}
