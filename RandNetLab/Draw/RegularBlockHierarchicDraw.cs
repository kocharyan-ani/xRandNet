using Core.Enumerations;
using Session;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Draw
{
    class RegularBlockHierarchicDraw : HierarchicDraw
    {
        public RegularBlockHierarchicDraw(Canvas mainCanvas) : base(mainCanvas)
        {
          //  Debug.Assert(Level == (Int32)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.Level]);

            InitialVertexCount = (Int32)Math.Pow(BranchingIndex, Level);
        }
    }
}
