﻿using Core.Enumerations;
using RandNetLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Draw
{
    public abstract class AbstractResearchDraw
    {
        MainWindow MWindow = Application.Current.Windows[0] as MainWindow;

        public AbstractDraw DrawObj { get; set; }
        protected int StepCount { get; set; }

        public AbstractResearchDraw(ModelType modelType = ModelType.ER)
        {
            DrawObj = FactoryDraw.CreateDraw(modelType, MWindow.mainCanvas);
        }

        public abstract void StartResearch();
        public void StopResearch()
        {
            MWindow.Start.Content = "Start";

            MWindow.Initial.IsEnabled = false;
            MWindow.Final.IsEnabled = false;
            MWindow.Next.IsEnabled = false;
            MWindow.Previous.IsEnabled = false;
            MWindow.Save.IsEnabled = false;
            MWindow.mainCanvas.Children.Clear();

            MWindow.TextBoxStepNumber.Text = "0";
        }
        public abstract void SaveResearch();

        public abstract void OnWindowSizeChanged();

    }
}
