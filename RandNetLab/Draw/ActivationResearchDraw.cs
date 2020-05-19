using Core.Enumerations;
using RandNetLab;
using Session;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;
using System.Windows.Threading;

namespace Draw
{
    public class ActivationResearchDraw : AbstractResearchDraw
    {
        MainWindow MWindow = Application.Current.Windows[0] as MainWindow;

        private ObservableCollection<KeyValuePair<int, int>> activesCount;

        public ActivationResearchDraw() : base(ModelType.ER) 
        {
            StepCount = LabSessionManager.GetActivationStepCount();
            activesCount = new ObservableCollection<KeyValuePair<int, int>>();
            MWindow.XAxisMaximum = StepCount + 1;
            MWindow.YAxisMaximum = LabSessionManager.GetMaximumActiveNodesCount() + 1;
        }

    public override void StartResearch() 
        {
            //MWindow.Start.Content = "Stop";
            MWindow.Start.IsEnabled = false;
            MWindow.mainCanvas.Children.Clear();
            MWindow.ChartData = null;
            activesCount.Clear();
            ResearchStepNumber = 0;
            UpdateStepNumber();
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                DrawObj.DrawFinal();
            }));
            DrawActivationProcess();
            MWindow.Start.IsEnabled = true;
            //MWindow.Start.Content = "Start";
        }
        public override void SaveResearch() { }

        public override void OnWindowSizeChanged() { }

        private void DrawActivationProcess()
        {
            for (; ResearchStepNumber < StepCount; ++ResearchStepNumber)
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    //List<bool> list = new List<bool>();
                    //foreach (var e in step)
                    //{
                    //    list.Add((bool)e);
                    //}
                    UpdateStepNumber();
                    DrawAcivationStep();
                    UpdateChart();
                }));
                System.Threading.Thread.Sleep(RESEARCH_STEP_DURATION_BY_MILISECONDS);
            }
        }

        public override void SetStatisticsParameters()
        {
            MWindow.listViewResearch.ColumnDefinitions[0].Width = new GridLength(3, GridUnitType.Star);
            MWindow.listViewResearch.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            MWindow.StatisticCanvas.Title = "Activation process";
            ((LineSeries)MWindow.StatisticCanvas.Series[0]).Title = "ANC";
            MWindow.ChartData = null;
        }

        private void DrawAcivationStep()
        {
            BitArray step = LabSessionManager.GetActivationStep(ResearchStepNumber);
            for (int j = 0; j < step.Count; ++j)
            {
                DrawObj.ActivateOrDeactivateVertex(j, step[j]);
            }
        }

        private void UpdateChart()
        {
            activesCount.Add(new KeyValuePair<int, int>(ResearchStepNumber, LabSessionManager.GetActiveNodesCountbyStep(ResearchStepNumber)));
            MWindow.ChartData = activesCount;
        }
    }
}
