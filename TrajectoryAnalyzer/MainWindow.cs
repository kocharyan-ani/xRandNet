using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TrajectoryAnalyzer
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            firstChart.ChartAreas[0].AxisX.ScaleView.Zoomable = false;
            secondChart.ChartAreas[0].AxisX.ScaleView.Zoomable = false;
            firstChart.ChartAreas[0].AxisY.Maximum = 0.4;
            firstChart.ChartAreas[0].AxisY.Minimum = -0.2;
            firstChart.ChartAreas[0].AxisY.Interval = 0.1;
            secondChart.ChartAreas[0].AxisY.Maximum = 0.4;
            secondChart.ChartAreas[0].AxisY.Minimum = -0.2;
            secondChart.ChartAreas[0].AxisY.Interval = 0.1;
        }

        public void FirstOpenFile(string path)
        {
            distributionToolStripMenuItem.Enabled = true;
            autocorrelationToolStripMenuItem.Enabled = true;
            fftToolStripMenuItem.Enabled = true;
            secondTrajectoryToolStripMenuItem.Enabled = true;
            secondOpenToolStripMenuItem.Enabled = false;

            var data = Utility.GetDataFromFile(path);
            firstResearch = new Research(data);

            double mean = firstResearch.Mean;
            double std = firstResearch.Std;
            PlotFillBetween(firstChart, mean + 4 * std, 2 * std, Color.FromArgb(50, Color.Red));
            PlotFillBetween(firstChart, mean + 2 * std, 2 * std, Color.FromArgb(50, Color.Yellow));
            PlotFillBetween(firstChart, mean - 2 * std, 4 * std, Color.FromArgb(50, Color.Green));
            PlotFillBetween(firstChart, mean - 4 * std, 2 * std, Color.FromArgb(50, Color.Yellow));
            PlotFillBetween(firstChart, mean - 6 * std, 2 * std, Color.FromArgb(50, Color.Red));
            firstChart.Series[1].Points.Add(new DataPoint(1, mean + std));
            firstChart.Series[1].Points.Add(new DataPoint(data.Count, mean + std));
            firstChart.Series[2].Points.Add(new DataPoint(1, mean));
            firstChart.Series[2].Points.Add(new DataPoint(data.Count, mean));
            firstChart.Series[3].Points.Add(new DataPoint(1, mean - std));
            firstChart.Series[3].Points.Add(new DataPoint(data.Count, mean - std));

            int i = 0;
            foreach (var datum in firstResearch.GetData())
            {
                firstChart.Series[0].Points.Add(new DataPoint(++i, datum));
            }
        }

        public void SecondOpenFile(string path)
        {
            secondDistributionToolStripMenuItem.Enabled = true;
            secondAutocorrelationToolStripMenuItem.Enabled = true;
            secondFftToolStripMenuItem.Enabled = true;
            correlationToolStripMenuItem.Enabled = true;
            fftToolStripMenuItem.Enabled = true;
            secondOpenToolStripMenuItem.Enabled = false;

            var data = Utility.GetDataFromFile(path);
            secondResearch = new Research(data);

            double mean = secondResearch.Mean;
            double std = secondResearch.Std;
            PlotFillBetween(secondChart, mean + 4 * std, 2 * std, Color.FromArgb(50, Color.Red));
            PlotFillBetween(secondChart, mean + 2 * std, 2 * std, Color.FromArgb(50, Color.Yellow));
            PlotFillBetween(secondChart, mean - 2 * std, 4 * std, Color.FromArgb(50, Color.Green));
            PlotFillBetween(secondChart, mean - 4 * std, 2 * std, Color.FromArgb(50, Color.Yellow));
            PlotFillBetween(secondChart, mean - 6 * std, 2 * std, Color.FromArgb(50, Color.Red));
            secondChart.Series[1].Points.Add(new DataPoint(1, mean + std));
            secondChart.Series[1].Points.Add(new DataPoint(data.Count, mean + std));
            secondChart.Series[2].Points.Add(new DataPoint(1, mean));
            secondChart.Series[2].Points.Add(new DataPoint(data.Count, mean));
            secondChart.Series[3].Points.Add(new DataPoint(1, mean - std));
            secondChart.Series[3].Points.Add(new DataPoint(data.Count, mean - std));

            int i = 0;
            foreach (var datum in secondResearch.GetData())
            {
                secondChart.Series[0].Points.Add(new DataPoint(++i, datum));
            }
        }

        public void PlotFillBetween(Chart chart, double start, double interval, Color color)
        {
            StripLine stripline = new StripLine
            {
                Interval = 0,
                IntervalOffset = start,
                StripWidth = interval,
                BackColor = color
            };
            chart.ChartAreas[0].AxisY.StripLines.Add(stripline);
        }

        private void FirstOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Trajectory File|*.txt",
                Title = "Select a Trajectory File"
            };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FirstOpenFile(openFileDialog.FileName);
            }
        }

        private void SecondOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Trajectory File|*.txt",
                Title = "Select a Trajectory File"
            };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tableLayoutPanel.RowStyles[0].SizeType = SizeType.Percent;
                tableLayoutPanel.RowStyles[0].Height = 50F;
                tableLayoutPanel.RowStyles[1].SizeType = SizeType.Percent;
                tableLayoutPanel.RowStyles[1].Height = 50F;
                SecondOpenFile(openFileDialog.FileName);
            }
        }

        private void FirstDistributionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var DistributionWindow = new DistributionWindow(HasSelection1() ? GetSelectedResearch1() : firstResearch);
            DistributionWindow.Show();
        }

        private void SecondDistributionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var DistributionWindow = new DistributionWindow(HasSelection2() ? GetSelectedResearch2() : secondResearch);
            DistributionWindow.Show();
        }

        private void FirstAurocorrelationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var correlationWindow = new CorellationWindow(HasSelection1() ? GetSelectedResearch1() : firstResearch);
            correlationWindow.Show();
        }

        private void SecondAutocorrelationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var correlationWindow = new CorellationWindow(HasSelection2() ? GetSelectedResearch2() : secondResearch);
            correlationWindow.Show();
        }

        private void FFTToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var FFTWindow = new FFTWindow(HasSelection1() ? GetSelectedResearch1() : firstResearch);
            FFTWindow.Show();
        }

        private void FFTToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var FFTWindow = new FFTWindow(HasSelection2() ? GetSelectedResearch2() : secondResearch);
            FFTWindow.Show();
        }

        private void CorrelationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            correlation = Utility.Correlation(firstResearch.PropertyValues, secondResearch.PropertyValues);
            var FFTWindow = new CorellationWindow(correlation);
            FFTWindow.Show();
        }

        private void FFTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var FFTWindow = new FFTWindow(HasSelection2() ? GetSelectedResearch2() : secondResearch);
            FFTWindow.Show();
        }

        private void Chart1_SelectionRangeChanged(object sender, System.Windows.Forms.DataVisualization.Charting.CursorEventArgs e)
        {
            if (!HasSelection1())
            {
                firstSelectedResearch = null;
            }
        }

        private void FirstChart_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Delta < 0)
                {
                    firstChart.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                    firstChart.ChartAreas[0].AxisY.ScaleView.ZoomReset();
                }

                if (e.Delta > 0)
                {
                    double xMin = firstChart.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                    double xMax = firstChart.ChartAreas[0].AxisX.ScaleView.ViewMaximum;

                    double posXStart = firstChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                    double posXFinish = firstChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;

                    firstChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                    firstChart.ChartAreas[0].AxisX.ScaleView.Zoom(posXStart, posXFinish);
                    firstChart.ChartAreas[0].AxisX.ScaleView.Zoomable = false;
                }
            }
            catch { }
        }

        private bool HasSelection1()
        {
            return !double.IsNaN(firstChart.ChartAreas[0].CursorX.SelectionStart)
                && !double.IsNaN(firstChart.ChartAreas[0].CursorX.SelectionEnd)
                && firstChart.ChartAreas[0].CursorX.SelectionStart != firstChart.ChartAreas[0].CursorX.SelectionEnd;
        }
        private bool HasSelection2()
        {
            return !double.IsNaN(secondChart.ChartAreas[0].CursorX.SelectionStart)
                && !double.IsNaN(secondChart.ChartAreas[0].CursorX.SelectionEnd)
                && secondChart.ChartAreas[0].CursorX.SelectionStart != secondChart.ChartAreas[0].CursorX.SelectionEnd;
        }

        private void ZoomOnSelectToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            firstChart.ChartAreas[0].AxisX.ScaleView.Zoomable = zoomOnSelectToolStripMenuItem.Checked;
        }

        private Research GetSelectedResearch1()
        {
            Debug.Assert(HasSelection1());
            if (firstSelectedResearch == null)
            {
                int min = Convert.ToInt32(Math.Min(firstChart.ChartAreas[0].CursorX.SelectionStart, firstChart.ChartAreas[0].CursorX.SelectionEnd));
                int max = Convert.ToInt32(Math.Max(firstChart.ChartAreas[0].CursorX.SelectionStart, firstChart.ChartAreas[0].CursorX.SelectionEnd));
                firstSelectedResearch = new Research(firstResearch.PropertyValues.GetRange(min, max - min));
            }
            return firstSelectedResearch;
        }

        private Research GetSelectedResearch2()
        {
            Debug.Assert(HasSelection2());
            if (secondSelectedResearch == null)
            {
                int min = Convert.ToInt32(Math.Min(secondChart.ChartAreas[0].CursorX.SelectionStart, secondChart.ChartAreas[0].CursorX.SelectionEnd));
                int max = Convert.ToInt32(Math.Max(secondChart.ChartAreas[0].CursorX.SelectionStart, secondChart.ChartAreas[0].CursorX.SelectionEnd));
                secondSelectedResearch = new Research(secondResearch.PropertyValues.GetRange(min, max - min));
            }
            return secondSelectedResearch;
        }


        private List<double> correlation;
        private List<double> fft;

        private Research firstSelectedResearch;
        private Research firstResearch;

        private Research secondSelectedResearch;
        private Research secondResearch;
    }
}
