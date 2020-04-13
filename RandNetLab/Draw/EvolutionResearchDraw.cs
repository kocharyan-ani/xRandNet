using Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw
{
    public class EvolutionResearchDraw : AbstractResearchDraw
    {
        public EvolutionResearchDraw() : base(ModelType.ER) { }

        public override void StartResearch() { }
        public override void StopResearch() { }
        public override void SaveResearch() { }

        public override void OnWindowSizeChanged() { }
    }
}
