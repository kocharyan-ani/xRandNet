using Core.Enumerations;
using RandNetLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace Draw
{
    public class EvolutionResearchDraw : AbstractResearchDraw
    {
        MainWindow MWindow = Application.Current.Windows[0] as MainWindow;
        public EvolutionResearchDraw() : base(ModelType.ER) { }

        public override void StartResearch() { }
        public override void SaveResearch() { }

        public override void OnWindowSizeChanged() { }

        public override void SetStatisticsParameters() 
        {
            MWindow.listViewResearch.ColumnDefinitions[0].Width = new GridLength(3, GridUnitType.Star);
            MWindow.listViewResearch.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            MWindow.StatisticCanvas.Title = "Evolution process";
            ((LineSeries)MWindow.StatisticCanvas.Series[0]).Title = "";
        }
    }
}
