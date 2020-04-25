using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

using Session;
using Core.Enumerations;
using Draw;
using System;
using System.Windows.Controls;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
       
 //       private AbstractDraw draw;
        private AbstractResearchDraw researchDraw;

        private ObservableCollection<KeyValuePair<int, int>> chartData;
        public ObservableCollection<KeyValuePair<int, int>> ChartData
        { 
            get { return chartData; }
            set
            {
                chartData = value;
                NotifyPropertyChanged();
            }
        }

        private int xAxisMaximum;
        public int XAxisMaximum
        {
            get { return xAxisMaximum; }
            set 
            { 
                xAxisMaximum = value;
                NotifyPropertyChanged();
            }
        }
        private int yAxisMaximum;
        public int YAxisMaximum
        {
            get { return yAxisMaximum; }
            set
            {
                yAxisMaximum = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string caller = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        private System.Windows.Forms.FolderBrowserDialog locationDlg = new System.Windows.Forms.FolderBrowserDialog();

        public MainWindow()
        {
            InitializeComponent();
            TextBoxStepNumber.Text = "0";
            this.DataContext = this;
        }

        #region Event Handlers

        private void BasicMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Basic);
            AddAnalizeOptionsList();
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
                researchDraw.StartResearch();
                Flat.IsEnabled = true;
            }
            else 
            {
                researchDraw.StopResearch();
                Flat.IsEnabled = false;
            }
        }

        private void Initial_Click(object sender, RoutedEventArgs e)
        {
            Debug.Assert(researchDraw is BasicResearchDraw);
            ((BasicResearchDraw)researchDraw).OnInitialButtonClick();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (researchDraw != null && Start.Content.ToString() == "Stop")
            {
                researchDraw.OnWindowSizeChanged();
            }
        }

        private void Final_Click(object sender, RoutedEventArgs e)
        {
            Debug.Assert(researchDraw is BasicResearchDraw);
            ((BasicResearchDraw)researchDraw).OnFinalButtonClick();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Debug.Assert(researchDraw is BasicResearchDraw);
            ((BasicResearchDraw)researchDraw).OnNextButtonClick();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            Debug.Assert(researchDraw is BasicResearchDraw);
            ((BasicResearchDraw)researchDraw).OnPreviousButtonClick();
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

        private void Flat_Checked(object sender, RoutedEventArgs e)
        {
            Debug.Assert(researchDraw is BasicResearchDraw);
            if (researchDraw.DrawObj != null)
            {
                HierarchicDraw hierDraw = researchDraw.DrawObj as HierarchicDraw;
                hierDraw.IsFlat = true;
            }
        }

        private void Flat_Unchecked(object sender, RoutedEventArgs e)
        {
            Debug.Assert(researchDraw is BasicResearchDraw);
            if (researchDraw.DrawObj != null)
            {
                HierarchicDraw hierDraw = researchDraw.DrawObj as HierarchicDraw;
                hierDraw.IsFlat = false;
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
            if (!(createResearchWnd.DialogResult.HasValue && createResearchWnd.DialogResult.Value)) { return; }

            researchDraw = FactoryReserchDraw.CreateResearchDraw(researchType, LabSessionManager.GetResearchModelType());

            Start.Visibility = Visibility.Visible;
            Start.IsEnabled = true;
            Save.Visibility = Visibility.Visible;
            Save.IsEnabled = true;
            AddResearchToTable();
            FillParametersTable();
            researchDraw.SetStatisticsParameters();
            if (researchType == ResearchType.Basic)
            {
                Initial.Visibility = Visibility.Visible;
                Initial.IsEnabled = false;
                Final.Visibility = Visibility.Visible;
                Final.IsEnabled = false;
                Next.Visibility = Visibility.Visible;
                Next.IsEnabled = false;
                Previous.Visibility = Visibility.Visible;
                Previous.IsEnabled = false;
                Grid.SetColumn(Save, 5);
            }
            else 
            {
                Initial.Visibility = Visibility.Collapsed;
                Final.Visibility = Visibility.Collapsed;
                Next.Visibility = Visibility.Collapsed;
                Previous.Visibility = Visibility.Collapsed;
                Grid.SetColumn(Save, 1);
            }
            Start.Content = "Start";

            mainCanvas.Children.Clear();

            if (LabSessionManager.GetResearchType() == ResearchType.Basic && 
                (LabSessionManager.GetResearchModelType() == ModelType.RegularHierarchic || LabSessionManager.GetResearchModelType() == ModelType.NonRegularHierarchic))
            {
                Flat.Visibility = System.Windows.Visibility.Visible;
                Flat.IsEnabled = false;
            }
            else
            {
                Flat.Visibility = System.Windows.Visibility.Hidden;
            }
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

        private void AddAnalizeOptionsList()
        {
            if(!LabSessionManager.IsResearchCreated())
            {
                return;
            }
            AnalyzeOptionsMenuItem.Items.Clear();
            AnalyzeOption options = LabSessionManager.GetAvailableAnalyzeOptions(LabSessionManager.GetResearchType(), LabSessionManager.GetResearchModelType());
            for (int i = 0; i < Enum.GetNames(typeof(AnalyzeOption)).Length; ++i)
            {
                int k = (int)options & (1 << i);
                if (k != 0)
                {
                    CheckBox option = new CheckBox()
                    {
                        Content = ((AnalyzeOption)k).ToString()
                    };
                    AnalyzeOptionsMenuItem.Items.Add(option);
                }
            }
        }

        #endregion
    }
}
