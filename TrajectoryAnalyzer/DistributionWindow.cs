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
    public partial class DistributionWindow : Form
    {
        public DistributionWindow(Research research)
        {
            InitializeComponent();
            foreach (var pair in research.GetHisData().OrderBy(x => x.Key))
            {
                chart1.Series[0].Points.Add(new DataPoint(pair.Key, pair.Value));
            }

            foreach (var pair in research.GetNormalDistribution().OrderBy(x => x.Key))
            {
                chart1.Series[1].Points.Add(new DataPoint(pair.Key, pair.Value));
            }

            chart1.ChartAreas[0].AxisX.Maximum = 0.25; // (Math.Ceiling((max / 10)) * 10);
            chart1.ChartAreas[0].AxisX.Minimum = 0.0; //(Math.Floor((min / 10)) * 10);
        }
    }
}
