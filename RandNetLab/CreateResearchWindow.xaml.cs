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
                    GenerationParameter gp = (GenerationParameter)Enum.Parse(typeof(GenerationParameter), paramName);
                    GenerationParameterInfo[] info = (GenerationParameterInfo[])gp.GetType().GetField(gp.ToString()).GetCustomAttributes(typeof(GenerationParameterInfo), false);
                    paramType = info[0].Type;
                    String paramValue = ((TextBox)ParametersStackPanelValue.Children[i]).Text;

                    object value = Convert.ChangeType(paramValue, paramType,CultureInfo.InvariantCulture);
                    if (paramName.Equals("Probability") && !((double)value >= 0 && (double)value <= 1))
                    {
                        MessageBox.Show(paramName + " parameter value must be rational number between 0 and 1.", "Error");
                        return false;
                    }
                    if(paramName.Equals("Vertices") && (int)value > MAX_VERTEX_COUNT)
                    {
                        MessageBox.Show("Maximum number of vertices is " + MAX_VERTEX_COUNT);
                        return false;
                    }
                    LabSessionManager.SetGenerationParameterValue(gp, value);
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

        //private void SetAnalyzeOptionsValues()
        //{
        //    AnalyzeOption opts = AnalyzeOption.None;
        //    foreach (Control c in AnalyzeOptionsStackPanel.Children)
        //    {
        //        Debug.Assert(c is CheckBox);
        //        CheckBox cc = c as CheckBox;
        //        // TODO maybe better content and text
        //        AnalyzeOption current = (AnalyzeOption)Enum.Parse(typeof(AnalyzeOption), cc.Content.ToString());
        //        if (cc.IsChecked.GetValueOrDefault())
        //            opts |= current;
        //    }
        //    LabSessionManager.SetAnalyzeOptions(opts);
        //}        

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
            List<GenerationParameter> parameters = LabSessionManager.GetRequiredGenerationParameters(researchType, GetCurrentModelType());
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

        private ModelType GetCurrentModelType()
        {
            return (ModelType)ModelTypeComboBox.SelectedItem;
        }

        private string GetDefaultValueForGenerationParameter(GenerationParameter g)
        {
            GenerationParameterInfo gInfo = (GenerationParameterInfo)(g.GetType().GetField(g.ToString()).GetCustomAttributes(typeof(GenerationParameterInfo), false)[0]);
            return gInfo.DefaultValue;
        }

        #endregion
    }
}