using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Globalization;

using Core.Attributes;
using Core.Enumerations;
using Session;
using System.Linq;
using System.Windows.Media;

namespace RandNetLab
{
    public partial class CreateResearchWindow : Window
    {
        private const int MAX_VERTEX_COUNT = 50;
        private ResearchType researchType;

        public CreateResearchWindow(ResearchType rt)
        {
            researchType = rt;

            InitializeComponent();
            InitializeGeneralGroup();          
        }

        #region Event Handlers

        private void ModelTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitializeParametersGroup();
            InitializeAnalyzeOptionsGroup();
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

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ResearchNameTextBox.Text == "")
            {
                MessageBox.Show("Research Name should be specified.", "Error");
                ResearchNameTextBox.Focus();
                return;
            }
            else
            {
                LabSessionManager.CreateResearch(researchType);
                SetGeneralValues();
                if (!SetParameterValues())
                {
                    return;
                }
                SetAnalyzeOptionsValues();

                LabSessionManager.StartResearch();

                DialogResult = true;
                Close();
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        #endregion

        #region Utility

        private void InitializeGeneralGroup()
        {
            ResearchTypeTextBox.Text = researchType.ToString();
            InitializeModelTypeCmb();
        }

        private void InitializeModelTypeCmb()
        {
            ModelTypeComboBox.Items.Clear();
            foreach (ModelType m in LabSessionManager.GetAvailableModelTypes(researchType))
                ModelTypeComboBox.Items.Add(m);
        }

        private void SetGeneralValues()
        {
            LabSessionManager.SetResearchName(ResearchNameTextBox.Text);
            LabSessionManager.SetResearchModelType(GetCurrentModelType());
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

                    bool status = false;
                    if (Enum.TryParse(paramName, out GenerationParameter gp))
                    {
                        status = SetGenerationParameterValues(gp, ParametersStackPanelValue.Children[i], ref paramType);
                    }
                    else if (Enum.TryParse(paramName, out ResearchParameter rp))
                    {
                        status = SetResearchParameterValues(rp, ParametersStackPanelValue.Children[i], ref paramType);
                    }
                    if(!status) { return status; }
                }
            }
            // TODO make validators
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

        private bool SetGenerationParameterValues(GenerationParameter gp, UIElement uIValue, ref Type paramType)
        {
            GenerationParameterInfo[] info = (GenerationParameterInfo[])gp.GetType().GetField(gp.ToString()).GetCustomAttributes(typeof(GenerationParameterInfo), false);
            string paramName = info[0].FullName;
            paramType = info[0].Type; 
            String paramValue = ((TextBox)uIValue).Text;

            object value = Convert.ChangeType(paramValue, paramType, CultureInfo.InvariantCulture);
            if (paramName.Equals("Probability") && !((double)value >= 0 && (double)value <= 1))
            {
                MessageBox.Show(paramName + " parameter value must be rational number between 0 and 1.", "Error");
                return false;
            }
            if (paramName.Equals("Vertices") && (int)value > MAX_VERTEX_COUNT)
            {
                MessageBox.Show("Maximum number of vertices is " + MAX_VERTEX_COUNT);
                return false;
            }
            LabSessionManager.SetGenerationParameterValue(gp, value);
            return true;
        }
        private bool SetResearchParameterValues(ResearchParameter rp, UIElement uIValue, ref Type paramType)
        {
            ResearchParameterInfo[] info = (ResearchParameterInfo[])rp.GetType().GetField(rp.ToString()).GetCustomAttributes(typeof(ResearchParameterInfo), false);
            paramType = info[0].Type;
            object value = null;
            if (paramType == typeof(Boolean))
            {
                value = (bool)((CheckBox)uIValue).IsChecked ? true : false;
            }
            else
            {
                String paramValue = ((TextBox)uIValue).Text;
                value = Convert.ChangeType(paramValue, paramType, CultureInfo.InvariantCulture);
            }
            // TODO validate
            LabSessionManager.SetResearchParameterValue(rp, value);
            return true;
        }

        private void SetAnalyzeOptionsValues()
        {
            AnalyzeOption opts = AnalyzeOption.None;
            foreach (Control c in AnalyzeOptionsStackPanel.Children)
            {
                Debug.Assert(c is CheckBox);
                CheckBox cc = c as CheckBox;
                // TODO maybe better content and text
                AnalyzeOption current = (AnalyzeOption)Enum.Parse(typeof(AnalyzeOption), cc.Content.ToString());
                if (cc.IsChecked.GetValueOrDefault())
                    opts |= current;
            }
            LabSessionManager.SetAnalyzeOptions(opts);
        }        

        private void InitializeEditResearchDialog()
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

        private void InitializeParametersGroup()
        {
            ParametersStackPanelName.Children.Clear();
            ParametersStackPanelValue.Children.Clear();
            List<ResearchParameter> researchParameters = LabSessionManager.GetRequiredResearchParameters(researchType);
            foreach (ResearchParameter p in researchParameters)
            {
                TextBlock pName = new TextBlock
                {
                    Text = p.ToString() + ":",
                    Margin = new Thickness(7),
                    Name = p.ToString() + "Name"
                };
                ParametersStackPanelName.Children.Add(pName);
                if (GetTypeForResearchParameter(p) == typeof(Boolean))
                {
                    CheckBox pValue = new CheckBox()
                    {
                        Margin = new Thickness(50, 5, 50, 5),
                        BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#aaaaaa")
                };
                    ParametersStackPanelValue.Children.Add(pValue);
                }
                else 
                { 
                    TextBox pValue = new TextBox
                    {
                        Height = 20,
                        Width = 100,
                        Margin = new Thickness(5),
                        Name = p.ToString(),
                        Text = GetDefaultValueForResearchParameter(p)
                    };
                    ParametersStackPanelValue.Children.Add(pValue);
                }

            }
            List<GenerationParameter> generationParameters = LabSessionManager.GetRequiredGenerationParameters(researchType, GetCurrentModelType());
            foreach (GenerationParameter p in generationParameters)
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

        private void InitializeAnalyzeOptionsGroup()
        {
            AnalyzeOptionsStackPanel.Children.Clear();
            AnalyzeOption options = LabSessionManager.GetAvailableAnalyzeOptions(researchType, GetCurrentModelType());
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

        private ModelType GetCurrentModelType()
        {
            return (ModelType)ModelTypeComboBox.SelectedItem;
        }

        private string GetDefaultValueForGenerationParameter(GenerationParameter g)
        {
            GenerationParameterInfo gInfo = (GenerationParameterInfo)(g.GetType().GetField(g.ToString()).GetCustomAttributes(typeof(GenerationParameterInfo), false)[0]);
            return gInfo.DefaultValue;
        }

        private string GetDefaultValueForResearchParameter(ResearchParameter g)
        {
            ResearchParameterInfo gInfo = (ResearchParameterInfo)(g.GetType().GetField(g.ToString()).GetCustomAttributes(typeof(ResearchParameterInfo), false)[0]);
            return gInfo.DefaultValue;
        }

        private Type GetTypeForResearchParameter(ResearchParameter g)
        {
            ResearchParameterInfo gInfo = (ResearchParameterInfo)(g.GetType().GetField(g.ToString()).GetCustomAttributes(typeof(ResearchParameterInfo), false)[0]);
            return gInfo.Type;
        }
        #endregion
    }
}