﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Core.Enumerations;
using RandNetLab;
using Session;

namespace Draw
{
    public class BasicResearchDraw : AbstractResearchDraw
    {
        MainWindow MWindow = Application.Current.Windows[0] as MainWindow;

        private int stepNumber = 0;

        public BasicResearchDraw(ModelType modelType = ModelType.ER) : base(modelType) 
        {        
            StepCount = LabSessionManager.GetStepCount();
        }

        public override void StartResearch()
        {
            stepNumber = 0;
            if (DrawObj != null)
            {
                DrawObj.StepNumber = stepNumber;
            }
            
            Debug.Assert(MWindow.Start.Content.ToString() == "Start");
            MWindow.Start.Content = "Stop";


            // *tmp
            //StepCount = 3;
            //StepCount = 4;

            MWindow.Initial.IsEnabled = false;
            MWindow.Final.IsEnabled = true;
            MWindow.Next.IsEnabled = true;
            MWindow.Previous.IsEnabled = false;
            MWindow.Save.IsEnabled = true;

            if ((bool)MWindow.Flat.IsChecked)
            {
                if (DrawObj != null)
                {
                    HierarchicDraw hierDraw = DrawObj as HierarchicDraw;
                    hierDraw.IsFlat = true;
                }
            }

            DrawObj.DrawInitial();
        }
        public override void SaveResearch() { }

        public void OnInitialButtonClick()
        {
            stepNumber = 0;
            DrawObj.StepNumber = stepNumber;
            MWindow.TextBoxStepNumber.Text = stepNumber.ToString();
            MWindow.mainCanvas.Children.Clear();
            DrawObj.DrawInitial();

            MWindow.Initial.IsEnabled = false;
            MWindow.Final.IsEnabled = true;
            MWindow.Next.IsEnabled = true;
            MWindow.Previous.IsEnabled = false;
        }
        public void OnFinalButtonClick()
        {
            stepNumber = StepCount - 1;
            DrawObj.StepNumber = stepNumber;
            MWindow.Previous.IsEnabled = true;
            MWindow.Next.IsEnabled = false;
            DrawFinal();
        }
        public void OnNextButtonClick()
        {

            MWindow.Previous.IsEnabled = true;
            MWindow.Initial.IsEnabled = true;

            stepNumber++;
            DrawObj.StepNumber = stepNumber;
            DrawObj.DrawNext(stepNumber);
            if (stepNumber == StepCount - 1)
            {
                MWindow.Next.IsEnabled = false;
                MWindow.Final.IsEnabled = false;
            }
            MWindow.TextBoxStepNumber.Text = stepNumber.ToString();
        }
        public void OnPreviousButtonClick()
        {
            MWindow.Next.IsEnabled = true;
            MWindow.Final.IsEnabled = true;
            DrawObj.StepNumber = stepNumber - 1;
            DrawObj.DrawPrevious(stepNumber);
            stepNumber--;
            
            if (stepNumber == 0)
            {
                MWindow.Previous.IsEnabled = false;
                MWindow.Initial.IsEnabled = false;
            }
            MWindow.TextBoxStepNumber.Text = stepNumber.ToString();
        }
               
        public override void OnWindowSizeChanged()
        {
            if (DrawObj != null)
            {
                if (stepNumber == 0 && StepCount != 0 )
                {
                    DrawObj.DrawInitial();
                }
                else if (stepNumber == StepCount - 1 && StepCount != 0)
                {
                    DrawObj.DrawFinal();
                }
                else
                {
                    DrawObj.DrawInitial();
                    for (int i = 0; i <= stepNumber; ++i)
                    {
                        DrawObj.DrawNext(i);
                    }
                }
            }
        }


        private void DrawFinal()
        {
            stepNumber = LabSessionManager.GetStepCount() - 1;
            MWindow.mainCanvas.Children.Clear();
            DrawObj.DrawFinal();
            MWindow.TextBoxStepNumber.Text = stepNumber.ToString();

            MWindow.Next.IsEnabled = false;
            MWindow.Previous.IsEnabled = true;
            MWindow.Initial.IsEnabled = true;
            MWindow.Final.IsEnabled = false;

        }

        public override void SetStatisticsParameters() 
        {
            MWindow.listViewResearch.ColumnDefinitions[0].Width = new GridLength(4, GridUnitType.Star);
            MWindow.listViewResearch.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Star);
        }
    }
}
