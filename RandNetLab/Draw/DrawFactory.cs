using Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Draw
{
    public static class DrawFactory
    {
        public static AbstractDraw getDrawObject(ModelType mt, Canvas canvas)
        {
            switch (mt)
            {
                case ModelType.BA:
                    return new BADraw(canvas);
                case ModelType.ER:
                    return new ERDraw(canvas);
                case ModelType.HMN:
                    return new HMNDraw(canvas);
                case ModelType.NonRegularHierarchic:
                    return new NonRegularHierarchicDraw(canvas);
                case ModelType.RegularHierarchic:
                    return new RegularHierarchicDraw(canvas);
                case ModelType.WS:
                    return new WSDraw(canvas);
                default:
                    return null;
            }
        }
    }
}
