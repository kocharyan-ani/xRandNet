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

        public AbstractResearchDraw(ModelType modelType = ModelType.ER)
        {
            DrawObj = FactoryDraw.CreateDraw(modelType, MWindow.mainCanvas);
        }

        public abstract void StartResearch();
        public abstract void StopResearch();
        public abstract void SaveResearch();

        public abstract void OnInitialButtonClick();
        public abstract void OnFinalButtonClick();
        public abstract void OnNextButtonClick();
        public abstract void OnPreviousButtonClick();

        public abstract void OnWindowSizeChanged();

    }
}
