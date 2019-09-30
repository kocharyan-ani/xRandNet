using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrajectoryAnalyzer
{
    public partial class CorellationWindow : Form
    {
        public CorellationWindow(Research research)
        {
            InitializeComponent();

            foreach (var datum in research.Act)
            {
                chart1.Series[0].Points.Add(datum);
            }
        }

        public CorellationWindow(List<double> data)
        {
            InitializeComponent();

            foreach (var datum in data)
            {
                chart1.Series[0].Points.Add(datum);
            }
        }
    }
}
