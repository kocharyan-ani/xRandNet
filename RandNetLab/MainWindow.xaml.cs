﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

using Session;
using Core.Enumerations;
using Draw;

namespace RandNetLab
{
    public struct ModelParameterStruct
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public struct ResearchToDrawStruct
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string ResearchType { get; set; }
        public string DrawStatus { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        private int stepNumber = -1;
        private AbstractDraw draw;

        private System.Windows.Forms.FolderBrowserDialog locationDlg = new System.Windows.Forms.FolderBrowserDialog();

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void BasicMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Basic);
        }

        private void EvolutionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Evolution);
        }

        private void ActivationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Activation);
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWnd = new SettingsWindow()
            {
                Owner = this
            };
            settingsWnd.ShowDialog();
            // TODO add settings in app.config
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (Start.Content.ToString() == "Start")
            {
                Start.Content = "Stop";
                stepNumber = -1;
                draw = Draw.FactoryDraw.CreateDraw(LabSessionManager.GetResearchModelType(), mainCanvas);
                               
                Initial.IsEnabled = true;
                Final.IsEnabled = true;
                Next.IsEnabled = true;
                Previous.IsEnabled = true;
                Save.IsEnabled = true;
            }
            else
            {
                Start.Content = "Start";

                Initial.IsEnabled = false;
                Final.IsEnabled = false;
                Next.IsEnabled = false;
                Previous.IsEnabled = false;
                Save.IsEnabled = false;
            }

            mainCanvas.Children.Clear();
        }

        private void Initial_Click(object sender, RoutedEventArgs e)
        {
            stepNumber = 0;
            mainCanvas.Children.Clear();
            draw.DrawInitial();            
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (draw != null)
            {
                if (stepNumber == 0)
                {
                    draw.DrawInitial();
                }
                else if (stepNumber == LabSessionManager.GetFinalStepNumber())
                {
                    draw.DrawFinal();
                }
                else
                {
                    draw.DrawInitial();
                    for (int i = 0; i <= stepNumber; ++i)
                    {
                        draw.DrawNext(i);
                    }
                }
            }
        }

        private void Final_Click(object sender, RoutedEventArgs e)
        {
            stepNumber = LabSessionManager.GetFinalStepNumber();
            mainCanvas.Children.Clear();
            draw.DrawFinal();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            // TODO change try/catch logic to if/else logic
            try
            {
                if (stepNumber == -1)
                {
                    draw.DrawInitial();
                }

                draw.DrawNext(stepNumber);
                stepNumber++;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                stepNumber = LabSessionManager.GetFinalStepNumber();
                MessageBox.Show("No more steps");
            }

            catch (System.IndexOutOfRangeException)
            {
                stepNumber = LabSessionManager.GetFinalStepNumber();
                MessageBox.Show("No more steps");
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            // TODO change try/catch logic to if/else logic
            try
            {
                stepNumber--;
                draw.DrawPrevious(stepNumber);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                stepNumber = 0;
                MessageBox.Show("No more steps");
            }
            catch (System.IndexOutOfRangeException)
            {
                stepNumber = 0;
                MessageBox.Show("No more steps");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (locationDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)(mainGrid.RenderSize.Width - 100),
                                                            (int)(mainGrid.RenderSize.Height),
                                                            100d,
                                                            100d,
                                                            System.Windows.Media.PixelFormats.Default);
                rtb.Render(mainCanvas);
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                // TODO retrieve file name
                string fileName = locationDlg.SelectedPath + "\\" + "a.png";
                using (var fs = System.IO.File.OpenWrite(fileName))
                {
                    pngEncoder.Save(fs);
                }
            }            
        }

        #endregion

        #region Utility

        private void ShowCreateResearchDialog(ResearchType researchType)
        {
            CreateResearchWindow createResearchWnd = new CreateResearchWindow(researchType)
            {
                Owner = this
            };
            createResearchWnd.ShowDialog();
            if (createResearchWnd.DialogResult.HasValue && createResearchWnd.DialogResult.Value)
            {
                Start.IsEnabled = true;
                AddResearchToTable();
                FillParametersTable();
            }
            Initial.IsEnabled = false;
            Final.IsEnabled = false;
            Next.IsEnabled = false;
            Previous.IsEnabled = false;
            Start.Content = "Start";
            mainCanvas.Children.Clear();
        }

        private void AddResearchToTable()
        {
            /*ResearchListView.Items.Clear();
            ResearchListView.SelectedIndex = ResearchListView.Items.Add(new ResearchToDrawStruct
            {
                ResearchType = LabSessionManager.GetResearchType().ToString(),
                Name = LabSessionManager.GetResearchName(),
                Model = LabSessionManager.GetResearchModelType().ToString(),
                DrawStatus = LabSessionManager.GetResearchStatus().ToString(),
            });

            researchInfoTable.Rows.Add("Research ID", r[0].ToString());
            researchInfoTable.Rows.Add("Research Name", StatSessionManager.GetResearchName(r[0]));
            researchInfoTable.Rows.Add("Research Type", StatSessionManager.GetResearchType(r[0]));
            researchInfoTable.Rows.Add("Model Type", StatSessionManager.GetResearchModelType(r[0]));
            researchInfoTable.Rows.Add("Realization Count", st.RealizationCountSum);
            researchInfoTable.Rows.Add("Date", StatSessionManager.GetResearchDate(r[0]));
            researchInfoTable.Rows.Add("Size", StatSessionManager.GetResearchNetworkSize(r[0]));
            researchInfoTable.Rows.Add("Edges", st.EdgesCountAvg);

            */
        }

        private void FillParametersTable()
        {
            ParametersGrid.Items.Clear();

            /*ResearchListView.SelectedIndex = ResearchListView.Items.Add(new ResearchToDrawStruct
            {
                ResearchType = LabSessionManager.GetResearchType().ToString(),
                Name = LabSessionManager.GetResearchName(),
                Model = LabSessionManager.GetResearchModelType().ToString(),
                DrawStatus = LabSessionManager.GetResearchStatus().ToString(),
            });

            researchInfoTable.Rows.Add("Research ID", r[0].ToString());
            researchInfoTable.Rows.Add("Research Name", StatSessionManager.GetResearchName(r[0]));
            researchInfoTable.Rows.Add("Research Type", StatSessionManager.GetResearchType(r[0]));
            researchInfoTable.Rows.Add("Model Type", StatSessionManager.GetResearchModelType(r[0]));
            researchInfoTable.Rows.Add("Realization Count", st.RealizationCountSum);
            researchInfoTable.Rows.Add("Date", StatSessionManager.GetResearchDate(r[0]));
            researchInfoTable.Rows.Add("Size", StatSessionManager.GetResearchNetworkSize(r[0]));
            researchInfoTable.Rows.Add("Edges", st.EdgesCountAvg);*/

            List<ModelParameterStruct> parametersList = new List<ModelParameterStruct>();

            Dictionary<GenerationParameter, object> keyValues = LabSessionManager.GetGenerationParameterValues();

            foreach (GenerationParameter key in keyValues.Keys)
            {
                object value = keyValues[key];
                if (value != null)
                {
                    ParametersGrid.Items.Add(new ModelParameterStruct { Name = key.ToString(), Value = value.ToString() });
                }
            }
        }

        #endregion
    }
}
