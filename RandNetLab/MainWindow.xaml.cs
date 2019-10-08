using Core.Enumerations;
using Session;
using System.Collections.Generic;
using System.Windows;

using Core;

using RandNetLab.Draw;
using Draw;
using System.Windows.Media.Imaging;

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
        private AbstractDraw draw;
        private int stepNumber;
        public MainWindow()
        {
            InitializeComponent();
            stepNumber = -1;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (Start.Content.ToString() == "Start")
            {
                Start.Content = "Stop";
                stepNumber = -1;
                draw = Draw.FactoryDraw.CreateDraw(LabSessionManager.GetResearchModelType(), mainCanvas);

                EditResearchContextMenu.IsEnabled = false;
                EditResearchContextMenuCanvas.IsEnabled = false;
               
                Initial.IsEnabled = true;
                Final.IsEnabled = true;
                Next.IsEnabled = true;
                Previous.IsEnabled = true;
                Save.IsEnabled = true;

            }
            else
            {
                Start.Content = "Start";

                EditResearchContextMenu.IsEnabled = true;
                EditResearchContextMenuCanvas.IsEnabled = true;

                Initial.IsEnabled = false;
                Final.IsEnabled = false;
                Next.IsEnabled = false;
                Previous.IsEnabled = false;
                Save.IsEnabled = false;
            }

            mainCanvas.Children.Clear();
        }

        private void BasicMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Basic, true);
        }

        private void EvolutionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Evolution, true);
        }

        private void CollectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Collection, true);
        }

        private void StructuralMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Structural, true);
        }

        private void ActivationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Activation, true);
        }

        #region functions
        private void ShowCreateResearchDialog(ResearchType researchType, bool isCreate)
        {
            CreateResearch createResearch = new CreateResearch(researchType, isCreate)
            {
                Owner = this,
                ShowInTaskbar = false
            };
            createResearch.ShowDialog();

            if (createResearch.DialogResult.HasValue && createResearch.DialogResult.Value)
            {
                Start.IsEnabled = true;
                EditResearchContextMenu.IsEnabled = true;
                EditResearchContextMenuCanvas.IsEnabled = true;
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
            ResearchListView.Items.Clear();
            ResearchListView.SelectedIndex = ResearchListView.Items.Add(new ResearchToDrawStruct
            {
                ResearchType = LabSessionManager.GetResearchType().ToString(),
                Name = LabSessionManager.GetResearchName(),
                Model = LabSessionManager.GetResearchModelType().ToString(),
                DrawStatus = LabSessionManager.GetResearchStatus().ToString(),
            });

        }

        private void FillParametersTable()
        {
            ParametersGrid.Items.Clear();
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

        private void BasicContext_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Basic, true);
        }

        private void EvolutionContext_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Evolution, true);
        }

        private void CollectionContext_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Collection, true);
        }

        private void StructuralContext_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Structural, true);
        }

        private void ActivationContext_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Activation, true);
        }

        private void EditResearchContextMenu_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Basic, false);
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Initial_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Children.Clear();
            draw.DrawInitial();
            stepNumber = 0;
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
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)(mainGrid.RenderSize.Width -100),
                                                            (int)(mainGrid.RenderSize.Height),
                                                            100d, 
                                                            100d, 
                                                            System.Windows.Media.PixelFormats.Default);
            rtb.Render(mainCanvas);

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = System.IO.File.OpenWrite(@"C:\New folder\aa.png"))
            {
                pngEncoder.Save(fs);
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }
    }
}
