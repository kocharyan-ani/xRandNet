using System.Collections.Generic;
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

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int stepNumber = 0;
        private int stepCount = 0;
        private AbstractDraw draw;

        private System.Windows.Forms.FolderBrowserDialog locationDlg = new System.Windows.Forms.FolderBrowserDialog();

        public MainWindow()
        {
            InitializeComponent();
            TextBoxStepNumber.Text = "0";
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
                stepNumber = 0;
                
                draw = Draw.FactoryDraw.CreateDraw(LabSessionManager.GetResearchModelType(), mainCanvas);
                stepCount = LabSessionManager.GetStepCount();

                Initial.IsEnabled = true;
                Final.IsEnabled = true;
                Next.IsEnabled = false;
                Previous.IsEnabled = true;
                Save.IsEnabled = true;

                DrawFinal();

            }
            else
            {
                Start.Content = "Start";

                Initial.IsEnabled = false;
                Final.IsEnabled = false;
                Next.IsEnabled = false;
                Previous.IsEnabled = false;
                Save.IsEnabled = false;
                mainCanvas.Children.Clear();

                TextBoxStepNumber.Text = "0";
            }
        }

        private void Initial_Click(object sender, RoutedEventArgs e)
        {
            stepNumber = 1;
            TextBoxStepNumber.Text = stepNumber.ToString();
            mainCanvas.Children.Clear();
            draw.DrawInitial();

            Initial.IsEnabled = false;
            Final.IsEnabled = true;
            Next.IsEnabled = true;
            Previous.IsEnabled = false;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (draw != null)
            {
                if (stepNumber == 1)
                {
                    draw.DrawInitial();
                }
                else if (stepNumber == stepCount)
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
            Previous.IsEnabled = true;
            Next.IsEnabled = false;
            DrawFinal();
        }

        private void DrawFinal()
        {
            stepNumber = LabSessionManager.GetStepCount();
            mainCanvas.Children.Clear();
            draw.DrawFinal();
            TextBoxStepNumber.Text = stepNumber.ToString();

            Next.IsEnabled = false;
            Previous.IsEnabled = true;
            Initial.IsEnabled = true;
            Final.IsEnabled = false;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (stepNumber == stepCount - 1)
            {
                Next.IsEnabled = false;
                Final.IsEnabled = false;
            }
            else if (stepNumber == stepCount)
            {
                return;
            }

            Previous.IsEnabled = true;
            Initial.IsEnabled = true;

            draw.DrawNext(stepNumber);
            stepNumber++;
            TextBoxStepNumber.Text = stepNumber.ToString();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (stepNumber == 2)
            {
                Previous.IsEnabled = false;
                Initial.IsEnabled = false;
            }
            else if (stepNumber == 1)
            {
                return;
            }

            Next.IsEnabled = true;
            Final.IsEnabled = true;
            
            stepNumber--;
            draw.DrawPrevious(stepNumber);
            TextBoxStepNumber.Text = stepNumber.ToString();
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
