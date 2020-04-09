using System;
using System.Collections.Generic;
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
        private int stepCount = 0;

        public BasicResearchDraw(ModelType modelType = ModelType.ER) : base(modelType) { }

        public override void StartResearch()
        {
            stepNumber = 0;
            if (MWindow.Start.Content.ToString() == "Start")
            {
                MWindow.Start.Content = "Stop";

                DrawObj = Draw.FactoryDraw.CreateDraw(LabSessionManager.GetResearchModelType(), MWindow.mainCanvas);
                stepCount = LabSessionManager.GetStepCount();

                // *tmp
                stepCount = 3;
                stepCount = 4;

                //Initial.IsEnabled = true;
                //Final.IsEnabled = true;
                //Next.IsEnabled = false;
                //Previous.IsEnabled = true;
                //Save.IsEnabled = true;

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
        }
        public override void StopResearch() { }
        public override void SaveResearch() { }

        public override void OnInitialButtonClick()
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
        public override void OnFinalButtonClick()
        {
            stepNumber = stepCount - 1;
            DrawObj.StepNumber = stepNumber;
            MWindow.Previous.IsEnabled = true;
            MWindow.Next.IsEnabled = false;
            DrawFinal();
        }
        public override void OnNextButtonClick()
        {
            if (stepNumber == stepCount - 1)
            {
                MWindow.Next.IsEnabled = false;
                MWindow.Final.IsEnabled = false;
            }
            else if (stepNumber == stepCount)
            {
                return;
            }

            MWindow.Previous.IsEnabled = true;
            MWindow.Initial.IsEnabled = true;

            stepNumber++;
            DrawObj.StepNumber = stepNumber;
            DrawObj.DrawNext(stepNumber);
            if (stepNumber == stepCount - 1)
            {
                MWindow.Next.IsEnabled = false;
                MWindow.Final.IsEnabled = false;
            }
            MWindow.TextBoxStepNumber.Text = stepNumber.ToString();
        }
        public override void OnPreviousButtonClick()
        {
            MWindow.Next.IsEnabled = true;
            MWindow.Final.IsEnabled = true;

            DrawObj.StepNumber = stepNumber;
            DrawObj.DrawPrevious(stepNumber);
            stepNumber--;
            if (stepNumber == 0)
            {
                MWindow.Previous.IsEnabled = false;
            }
            MWindow.TextBoxStepNumber.Text = stepNumber.ToString();
        }
               
        public override void OnWindowSizeChanged()
        {
            if (DrawObj != null)
            {
                if (stepNumber == 1)
                {
                    DrawObj.DrawInitial();
                }
                else if (stepNumber == stepCount)
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
    }
}
