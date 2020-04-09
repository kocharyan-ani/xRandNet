using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Core.Enumerations;
using Draw;

namespace Draw
{
    public static class FactoryDraw
    {
        public static AbstractDraw CreateDraw(ModelType model,Canvas canvas)
        {
            AbstractDraw draw; 
            switch (model)
            {
                case ModelType.BA:
                    draw = new BADraw(canvas);
                    break;
                case ModelType.ER:
                    draw = new ERDraw(canvas);
                    break;
                case ModelType.WS:
                    draw = new WSDraw(canvas);
                    break;
                case ModelType.RegularHierarchic:
                    draw = new RegularBlockHierarchicDraw(canvas);
                    break;
                case ModelType.NonRegularHierarchic:
                    draw = new NonRegularBlockHierarchicDraw(canvas);
                    break;
                default:
                    Debug.Assert(false);
                    draw = null;
                    break;
            }
            return draw;
        }
     }

    public static class FactoryReserchDraw
    {
        public static AbstractResearchDraw CreateResearchDraw(ResearchType researchType, ModelType modelType)
        { 
            switch(researchType)
            {
                case ResearchType.Basic:
                    return new BasicResearchDraw(modelType);
                case ResearchType.Activation:
                    return new ActivationResearchDraw();
                case ResearchType.Evolution:
                    return new EvolutionResearchDraw();
                default:
                    Debug.Assert(false);
                    return null;
            }
        }
    }
}
