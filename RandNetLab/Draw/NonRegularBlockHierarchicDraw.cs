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
    public class NonRegularBlockHierarchicDraw : HierarchicDraw
    {
        public NonRegularBlockHierarchicDraw(Canvas mainCanvas) : base(mainCanvas)
        {
            InitialVertexCount = (Int32)LabSessionManager.GetGenerationParameterValues()[GenerationParameter.Vertices];
        }
    }
}
