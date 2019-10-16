using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Core;
using Core.Attributes;
using Core.Enumerations;
using Session;

namespace RandNetLab
{
    public enum DialogType
    {
        Create,
        Edit,
        Clone
    };

    public partial class CreateResearch : Window
    {
        private ResearchType researchType;
        private bool isCreateBoolean;

        public CreateResearch(ResearchType rt, bool isCreate)
        {
            InitializeComponent();

            isCreateBoolean = isCreate;

            InitializeTexts();
            researchType = rt;
            ResearchTypeTextBox.Text = rt.ToString();

            AddModelTypes();
            ModelTypeComboBox.SelectionChanged += new SelectionChangedEventHandler(OnModelTypeChanged);

            ModelType mt = (ModelType)Enum.Parse(typeof(ModelType), ModelTypeComboBox.Text);

            AppendAnalyzeOptions(mt);
            AppendParameters(mt);


            InitializeEditResearchDialog();

        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ResearchNameTextBox.Text == "")
            {
                MessageBox.Show("Research Name should be specified.", "Error");
            }
            else
            {
                LabSessionManager.CreateResearch(researchType);
                SetGeneralValues();
                if (SetParameterValues())
                {
                    this.DialogResult = true;

                    this.Close();
                }
            }
        }


        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ResearchNameTextBox.Text == "")
            {
                MessageBox.Show("Research Name should be specified.", "Error");
            }
            else
            {
                SetGeneralValues();
                SetParameterValues();
                this.DialogResult = true;
                this.Close();
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void SelectAll_Button_Click(object sender, RoutedEventArgs e)
        {
            UIElementCollection options = AnalyzeOptionsStackPanel.Children;
            for (int i = 0; i < options.Count; ++i)
            {
                CheckBox option = options[i] as CheckBox;
                option.IsChecked = true;
            }
        }

        private void DeselectAll_Button_Click(object sender, RoutedEventArgs e)
        {
            UIElementCollection options = AnalyzeOptionsStackPanel.Children;
            for (int i = 0; i < options.Count; ++i)
            {
                CheckBox option = options[i] as CheckBox;
                option.IsChecked = false;
            }
        }


        private void AddModelTypes()
        {
            List<ModelType> modelTypes = LabSessionManager.GetAvailableModelTypes(researchType);
            for (int i = 0; i < modelTypes.Count; ++i)
            {
                ModelTypeComboBox.Items.Add(modelTypes[i].ToString());
            }
        }

        private void AppendAnalyzeOptions(ModelType mt)
        {
            AnalyzeOptionsStackPanel.Children.Clear();
            AnalyzeOption options = LabSessionManager.GetAvailableAnalyzeOptions(researchType, mt);
            for (int i = 0; i < Enum.GetNames(typeof(AnalyzeOption)).Length; ++i)
            {
                int k = (int)options & (1 << i);
                if (k != 0)
                {
                    CheckBox option = new CheckBox()
                    {
                        Content = ((AnalyzeOption)k).ToString()
                    };
                    AnalyzeOptionsStackPanel.Children.Add(option);
                }
            }
        }

        private void AppendParameters(ModelType mt)
        {
            ParametersStackPanelName.Children.Clear();
            ParametersStackPanelValue.Children.Clear();
            List<GenerationParameter> parameters = LabSessionManager.GetRequiredGenerationParameters(researchType, mt);
            foreach (GenerationParameter p in parameters)
            {
                TextBlock pName = new TextBlock
                {
                    Text = p.ToString() + ":",
                    Margin = new Thickness(7),
                    Name = p.ToString() + "Name"
                };
                ParametersStackPanelName.Children.Add(pName);

                TextBox pValue = new TextBox
                {
                    Height = 20,
                    Width = 100,
                    Margin = new Thickness(5),
                    Name = p.ToString(),
                    Text = GetDefaultValueForGenerationParameter(p)
                };
                ParametersStackPanelValue.Children.Add(pValue);
            }
        }

        private bool SetParameterValues()
        {
            String paramName = "";
            Type paramType = typeof(UInt32);
            try
            {
                for (int i = 0; i < ParametersStackPanelValue.Children.Count; ++i)
                {
                    paramName = ((TextBlock)ParametersStackPanelName.Children[i]).Text;
                    paramName = paramName.Substring(0, paramName.Length - 1);
                    GenerationParameter gp = (GenerationParameter)Enum.Parse(typeof(GenerationParameter), paramName);
                    GenerationParameterInfo[] info = (GenerationParameterInfo[])gp.GetType().GetField(gp.ToString()).GetCustomAttributes(typeof(GenerationParameterInfo), false);
                    paramType = info[0].Type;
                    String paramValue = ((TextBox)ParametersStackPanelValue.Children[i]).Text;

                    object value = Convert.ChangeType(paramValue, paramType);
                    if (paramName.Equals("Probability") && !((double)value >= 0 && (double)value <= 1))
                    {
                        MessageBox.Show(paramName + " parameter value must be rational number between 0 and 1.", "Error");
                        return false;
                    }
                    LabSessionManager.SetGenerationParameterValue(gp, value);
                }
            }
            catch (Exception)
            {
                if (paramType == typeof(UInt32))
                {
                    MessageBox.Show(paramName + " parameter value must be non negative integer.", "Error");
                }
                else if (paramType == typeof(Double))
                {
                    MessageBox.Show(paramName + " parameter value must be non negative rational number.", "Error");
                }
                else
                {
                    MessageBox.Show("Wrong value of parameter " + paramName, "Error");
                }
                return false;
            }
            return true;
        }

        private string GetDefaultValueForGenerationParameter(GenerationParameter g)
        {
            GenerationParameterInfo gInfo = (GenerationParameterInfo)(g.GetType().GetField(g.ToString()).GetCustomAttributes(typeof(GenerationParameterInfo), false)[0]);
            return gInfo.DefaultValue;
        }

        private void OnModelTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            string mtString = (sender as ComboBox).SelectedItem as string;
            ModelType mt = (ModelType)Enum.Parse(typeof(ModelType), mtString);
            ResearchType rt = (ResearchType)Enum.Parse(typeof(ResearchType), ResearchTypeTextBox.Text);

            AppendAnalyzeOptions(mt);
            AppendParameters(mt);
        }

        //private void SetParameterValues()
        //{
        //    foreach (Control c in ParametersStackPanelValue.Children)
        //    {
        //        if (c is Label)
        //            continue;
        //        string pName = c.Name;
        //        Object pValue = GetValueFromControl(c);

        //        ResearchParameter rp;
        //        GenerationParameter gp;
        //        if (Enum.TryParse(pName, out rp))
        //            LabSessionManager.SetResearchParameterValue(rp, pValue);
        //        else if (Enum.TryParse(pName, out gp))
        //            LabSessionManager.SetGenerationParameterValue(gp, pValue);
        //    }
        //}

        private Object GetValueFromControl(Control c)
        {
            //Debug.Assert(!(c is Label));
            if (c is TextBox)
            {
                return (c as TextBox).Text;
            }
            else if (c is CheckBox)
            {
                return (c as CheckBox).IsChecked;
            }
            else if (c is ComboBox)
            {
                return (c as ComboBox).Text;
            }
            else
            {
                return null;
            }
        }

        private void SetGeneralValues()
        {
            LabSessionManager.SetResearchName(ResearchNameTextBox.Text);
            LabSessionManager.SetResearchModelType(GetCurrentModelType());
        }

        private ModelType GetCurrentModelType()
        {
            if (ModelTypeComboBox.Text == "")
            {
                return ModelType.BA;
            }
            return (ModelType)Enum.Parse(typeof(ModelType), ModelTypeComboBox.Text);
        }

        private void InitializeTexts()
        {
            if (isCreateBoolean)
            {
                UpdateButton.Visibility = Visibility.Collapsed;
                CreateButton.Visibility = Visibility.Visible;
                this.Title = "Create Research";
            }
            else
            {
                UpdateButton.Visibility = Visibility.Visible;
                CreateButton.Visibility = Visibility.Collapsed;
                this.Title = "Edit Research";
            }
        }

        private void InitializeEditResearchDialog()
        {
            if (!isCreateBoolean)
            {
                ResearchTypeTextBox.Text = LabSessionManager.GetResearchType().ToString();
                ResearchNameTextBox.Text = LabSessionManager.GetResearchName();
                ModelTypeComboBox.Text = LabSessionManager.GetResearchModelType().ToString();

                Dictionary<GenerationParameter, object> paramValues = LabSessionManager.GetGenerationParameterValues();
                List<ResearchParameter> param = LabSessionManager.GetRequiredResearchParameters(LabSessionManager.GetResearchType());
                for (int i = 0; i < ParametersStackPanelValue.Children.Count; i++)
                {

                    GenerationParameter g;
                    Enum.TryParse(((TextBox)ParametersStackPanelValue.Children[i]).Name, out g);
                    ((TextBox)(ParametersStackPanelValue.Children[i])).Text = paramValues[g].ToString();
                }
            }
        }

    }
}
