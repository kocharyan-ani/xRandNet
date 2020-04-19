using Core;
using Core.Enumerations;
using RandNetLab;
using Session;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Draw
{
    public class EvolutionResearchDraw : AbstractResearchDraw
    {
        MainWindow MWindow = Application.Current.Windows[0] as MainWindow;
        public EvolutionResearchDraw() : base(ModelType.ER)
        {
            edgesAddedRemoved = LabSessionManager.GetEvolutionInformation();
            StepCount = edgesAddedRemoved.Count;
            MWindow.XAxisMaximum = StepCount + 1;
            MWindow.YAxisMaximum = LabSessionManager.GetMaximumTrianglesCount() + 1;
        }

        private List<List<EdgesAddedOrRemoved>> edgesAddedRemoved;
        private ObservableCollection<KeyValuePair<int, int>> trianglesCountCollection;
        private List<int> trianglesCount;
        private const int RESEARCH_STEP_DURATION_BY_MILISECONDS = 1000;
        private List<EdgesAddedOrRemoved> edgesToRemove;
        private List<EdgesAddedOrRemoved> edgesToChangeColor;
        public override void StartResearch()
        {
            //MWindow.Start.Content = "Stop";
            trianglesCountCollection = new ObservableCollection<KeyValuePair<int, int>>();
            trianglesCount = LabSessionManager.GetTrianglesCount();
            edgesToRemove = new List<EdgesAddedOrRemoved>();
            edgesToChangeColor = new List<EdgesAddedOrRemoved>();
           
            MWindow.Start.IsEnabled = false;
            MWindow.mainCanvas.Children.Clear();
            MWindow.ChartData = null;
            DrawObj.DrawFinal();
            MWindow.TextBoxStepNumber.Text = "0";
            DrawEvolutionProcess();
            MWindow.Start.IsEnabled = true;
            //MWindow.Start.Content = "Start";
        }

        public override void SaveResearch() { }

        public override void OnWindowSizeChanged() { }

        private void DrawEvolutionProcess()
        {
            for (int i = 0; i < StepCount; ++i)
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    trianglesCountCollection.Add(new KeyValuePair<int, int>(i, trianglesCount[i]));
                    MWindow.ChartData = trianglesCountCollection;

                    AddOrRemoveEdges(edgesAddedRemoved[i]);
                }));
                MWindow.TextBoxStepNumber.Text = i.ToString();
                System.Threading.Thread.Sleep(RESEARCH_STEP_DURATION_BY_MILISECONDS);
            }
        }

        public override void SetStatisticsParameters()
        {
            MWindow.listViewResearch.ColumnDefinitions[0].Width = new GridLength(3, GridUnitType.Star);
            MWindow.listViewResearch.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            MWindow.StatisticCanvas.Title = "Evolution process";
            ((LineSeries)MWindow.StatisticCanvas.Series[0]).Title = "";
        }
        public void AddOrRemoveEdges(List<EdgesAddedOrRemoved> edges)
        {
            for (int i = 0; i < edgesToRemove.Count; i++)
            {
                string edgeUid = DrawObj.GenerateEdgeUid(edgesToRemove[i]);
                for (int j = 0; j < MWindow.mainCanvas.Children.Count; j++)
                {
                    if (MWindow.mainCanvas.Children[j].Uid == edgeUid)
                    {
                        MWindow.mainCanvas.Children.RemoveAt(j);
                        break;
                    }
                }
            }

            for (int i = 0; i < edgesToChangeColor.Count; i++)
            {
                string edgeUid = DrawObj.GenerateEdgeUid(edgesToChangeColor[i]);
                for (int j = 0; j < MWindow.mainCanvas.Children.Count; j++)
                {
                    if (MWindow.mainCanvas.Children[j].Uid == edgeUid)
                    {
                        Line line = (Line)MWindow.mainCanvas.Children[j];
                        line.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("#323336");
                        break;
                    }
                }
            }

            edgesToRemove.Clear();
            edgesToChangeColor.Clear();
            for (int i = 0; i < edges.Count; i++)
            {
                string edgeUid = DrawObj.GenerateEdgeUid(edges[i]);

                if (edges[i].Added)
                {
                    edgesToChangeColor.Add(edges[i]);
                    AddEdge(edges[i]);    
                }
                else
                {
                    edgesToRemove.Add(edges[i]);

                    for (int j = 0; j < MWindow.mainCanvas.Children.Count; j++)
                    {
                        if (MWindow.mainCanvas.Children[j].Uid == edgeUid)
                        {
                            Line line = (Line)MWindow.mainCanvas.Children[j];
                            line.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("#ff0000");
                            break;
                        }
                    }
                }
            }
        }
        private void AddEdge(EdgesAddedOrRemoved edge)
        {
            Line edgeElem = new Line
            {
                Uid = DrawObj.GenerateEdgeUid(edge),
                Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString("#008000"),
                X1 = DrawObj.Vertices[edge.Vertex1].X,
                Y1 = DrawObj.Vertices[edge.Vertex1].Y,
                X2 = DrawObj.Vertices[edge.Vertex2].X,
                Y2 = DrawObj.Vertices[edge.Vertex2].Y
            };
            MWindow.mainCanvas.Children.Add(edgeElem);
        }
    }
}