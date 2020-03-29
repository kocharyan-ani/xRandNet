using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Core.Enumerations;
using Draw;

namespace RandNetLab.Draw
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
                    draw = null;
                    break;
            }
            return draw;
        }
     }
}
