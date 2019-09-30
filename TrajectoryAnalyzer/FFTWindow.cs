using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TrajectoryAnalyzer
{
    public partial class FFTWindow : Form
    {
        public FFTWindow(Research research)
        {
            InitializeComponent();
            foreach (var datum in research.FftAct)
            {
                chart1.Series[0].Points.Add(datum);
            }

            chart1.ChartAreas[0].AxisX.ScaleView.Position = 1.0;
            chart1.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart1.ChartAreas[0].AxisX.ScaleView.Size = research.FftAct.Count / 100;
        }
    }
}
