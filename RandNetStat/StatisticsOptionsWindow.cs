﻿using System;
using System.ComponentModel;
using System.Windows.Forms;

using Core.Enumerations;

namespace RandNetStat
{
    public partial class StatisticsOptionsWindow : Form
    {
        public StatisticsOptionsWindow()
        {
            InitializeComponent();

            InitializeApproximationType();
            InitializeThickeningType();
        }

        public ApproximationType ApproximationType
        {
            get
            {
                return (ApproximationType)Enum.Parse(typeof(ApproximationType),
                    approximationTypeCmb.Text);
            }
            set 
            {
                approximationTypeCmb.Text = value.ToString();
            }
        }

        public ThickeningType ThickeningType
        {
            get
            {
                return (ThickeningType)Enum.Parse(typeof(ThickeningType),
                    thickeningTypeCmb.Text);
            }
            set
            {
                thickeningTypeCmb.Text = value.ToString();
            }
        }

        public int ThickeningValue
        {
            get
            {
                return int.Parse(thickeningValueTxt.Text);
            }
            set
            {
                thickeningValueTxt.Text = value.ToString();
            }
        }

        #region Event Handlers

        private void thickeningValueTxt_Validating(Object sender, CancelEventArgs e)
        {
            // TODO add validation if needed
        }

        #endregion

        #region Utilities

        private void InitializeApproximationType()
        {
            approximationTypeCmb.Items.Clear();
            string[] approximationTypeNames = Enum.GetNames(typeof(ApproximationType));
            for (int i = 0; i < approximationTypeNames.Length; ++i)
                approximationTypeCmb.Items.Add(approximationTypeNames[i]);

            if (approximationTypeCmb.Items.Count != 0)
                approximationTypeCmb.SelectedIndex = 1;
        }

        private void InitializeThickeningType()
        {
            thickeningTypeCmb.Items.Clear();
            string[] thickeningTypeNames = Enum.GetNames(typeof(ThickeningType));
            for (int i = 0; i < thickeningTypeNames.Length; ++i)
                thickeningTypeCmb.Items.Add(thickeningTypeNames[i]);

            if (thickeningTypeCmb.Items.Count != 0)
                thickeningTypeCmb.SelectedIndex = 1;
        }

        #endregion
    }
}
