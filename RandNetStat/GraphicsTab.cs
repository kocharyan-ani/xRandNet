﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

using Core.Enumerations;
using Core.Attributes;
using Session.StatEngine;

namespace RandNetStat
{
    public partial class GraphicsTab : UserControl
    {
        private String researchName;
        private AnalyzeOption option;
        private SortedDictionary<Double, Double> values;
        private DrawingOption drawingOptions;
        private StatisticsOption statisticsOptions;

        public GraphicsTab(String rName, AnalyzeOption o, SortedDictionary<Double, Double> v)
        {
            researchName = rName;
            option = o;
            values = v;
            drawingOptions = new DrawingOption(Color.Black, true);
            statisticsOptions = new StatisticsOption(ApproximationType.None, ThickeningType.None, 0);

            InitializeComponent();
            InitializeChart();
        }

        public void SaveChartToPng(string location)
        {
            string fileName = location + "\\" + researchName + "_" + option.ToString() + ".png";
            graphic.SaveImage(fileName, ChartImageFormat.Png);
        }

        #region Event Handlers

        private void GraphicsTab_Load(Object sender, EventArgs e)
        {
            Series s = new Series(option.ToString());
            InitializeAxis();
            InitializeDrawingOptions(s);
            InitializeValues(s);
            graphic.Series.Add(s);
        }

        private void drawingOptionsToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            Debug.Assert(graphic.Series.Count == 1);
            DrawingOptionsWindow w = new DrawingOptionsWindow();
            w.LineColor = drawingOptions.LineColor;
            w.Points = drawingOptions.IsPoints;
            if (w.ShowDialog() == DialogResult.OK)
            {
                drawingOptions.LineColor = w.LineColor;
                drawingOptions.IsPoints = w.Points;
                InitializeDrawingOptions(graphic.Series[0]);
            }
        }

        private void statisticsOptionsToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            Debug.Assert(graphic.Series.Count == 1);
            StatisticsOptionsWindow w = new StatisticsOptionsWindow();
            w.ApproximationType = statisticsOptions.ApproximationType;
            w.ThickeningType = statisticsOptions.ThickeningType;
            w.ThickeningValue = statisticsOptions.ThickeningValue;
            if (w.ShowDialog() == DialogResult.OK)
            {
                statisticsOptions.ApproximationType = w.ApproximationType;
                statisticsOptions.ThickeningType = w.ThickeningType;
                statisticsOptions.ThickeningValue = w.ThickeningValue;
                InitializeValues(graphic.Series[0]);
            }
        }

        private void combineToolStripMenuItem_Click(Object sender, EventArgs e)
        {

        }

        #endregion

        #region 

        private void InitializeChart()
        {
            graphic.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            graphic.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            graphic.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            graphic.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
        }

        private void InitializeAxis()
        {
            AnalyzeOptionInfo[] oInfo = (AnalyzeOptionInfo[])option.GetType().GetField(option.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false);
            graphic.ChartAreas[0].AxisX.Title = oInfo[0].XAxisName;
            graphic.ChartAreas[0].AxisY.Title = oInfo[0].YAxisName;
        }

        private void InitializeDrawingOptions(Series s)
        {
            s.ChartType = drawingOptions.IsPoints ? SeriesChartType.Point : SeriesChartType.Line;
            s.Color = drawingOptions.LineColor;
        }

        private void InitializeValues(Series s)
        {
            s.Points.Clear();
            SortedDictionary<Double, Double> result = new SortedDictionary<double, double>();
            ApplyThickening(ref result);
            ApplyApproximation(ref result);
            foreach (KeyValuePair<double, double> d in result)
                s.Points.Add(new DataPoint(d.Key, d.Value));
        }

        private void ApplyThickening(ref SortedDictionary<Double, Double> v)
        {
            int delta = 0;
            switch(statisticsOptions.ThickeningType)
            {
                case ThickeningType.None:
                    v = values;
                    return;
                case ThickeningType.Delta:
                    delta = statisticsOptions.ThickeningValue;
                    break;
                case ThickeningType.Percent:
                    delta = (int)(values.Count() * statisticsOptions.ThickeningValue / 100.0);
                    break;
                default:
                    Debug.Assert(false);
                    return;
            }
            
            int currentStep = 0, step = delta;
            double sum = 0;
            foreach (KeyValuePair<Double, Double> d in values)
            {
                if (currentStep < delta)
                {
                    sum += d.Value;
                    ++currentStep;
                }
                else
                {
                    v.Add(step, sum / delta);
                    currentStep = 0;
                    sum = 0;
                    step += delta;
                }
            }
            
            v.Add(values.Count(), sum / ((values.Count() % delta == 0) ? delta : values.Count() % delta));
        }

        private void ApplyApproximation(ref SortedDictionary<Double, Double> v)
        {
            if (statisticsOptions.ApproximationType == ApproximationType.None)
                return;

            SortedDictionary<Double, Double> t = new SortedDictionary<double, double>(v);
            v.Clear();
            foreach (KeyValuePair<double, double> d in t)
                ApplyApproximation(ref v, d.Key, d.Value);
        }

        private void ApplyApproximation(ref SortedDictionary<Double, Double> v, double x, double y)
        {
            switch (statisticsOptions.ApproximationType)
            {
                case ApproximationType.Degree:
                    x = (x == 0) ? x : Math.Log(x);
                    y = (y == 0) ? y : Math.Log(y);
                    break;
                case ApproximationType.Exponential:
                    y = (y == 0) ? y : Math.Log(y);
                    break;
                case ApproximationType.Gaus:
                    x *= x;
                    y = (y == 0) ? y : Math.Log(y);
                    break;
                default:
                    Debug.Assert(false);
                    return;
            }
            Debug.Assert(!Double.IsInfinity(x) && !Double.IsInfinity(y));
            v.Add(x, y);
        }

        #endregion
    }
}
