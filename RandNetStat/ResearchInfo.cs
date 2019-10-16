using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

using Core.Enumerations;
using Session;
using Session.StatEngine;

namespace RandNetStat
{
    public partial class ResearchInfo : UserControl
    {
        public ResearchInfo()
        {
            InitializeComponent();
        }

        public List<Guid> Researches
        {
            set
            {
                FillInformation(value);
            }
        }

        private void FillInformation(List<Guid> r)
        {
            Debug.Assert(r.Count() != 0);

            // TODO optimize
            StatisticResult st = new StatisticResult(r);
            researchInfoTable.Rows.Add("Research ID", r[0].ToString());
            researchInfoTable.Rows.Add("Research Name", StatSessionManager.GetResearchName(r[0]));
            researchInfoTable.Rows.Add("Research Type", StatSessionManager.GetResearchType(r[0]));
            researchInfoTable.Rows.Add("Model Type", StatSessionManager.GetResearchModelType(r[0]));
            researchInfoTable.Rows.Add("Realization Count", st.RealizationCountSum);
            researchInfoTable.Rows.Add("Date", StatSessionManager.GetResearchDate(r[0]));
            researchInfoTable.Rows.Add("Size", StatSessionManager.GetResearchNetworkSize(r[0]));
            researchInfoTable.Rows.Add("Edges", st.EdgesCountAvg);

            int i = researchInfoTable.Rows.Add("Parameters", "");
            researchInfoTable.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;

            Dictionary<ResearchParameter, Object> rValues = StatSessionManager.GetResearchParameterValues(r[0]);
            Dictionary<GenerationParameter, Object> gValues = StatSessionManager.GetGenerationParameterValues(r[0]);
            foreach (ResearchParameter rr in rValues.Keys)
                researchInfoTable.Rows.Add(rr.ToString(), rValues[rr].ToString());
            foreach (GenerationParameter g in gValues.Keys)
                researchInfoTable.Rows.Add(g.ToString(), gValues[g].ToString());
        }
    }
}
