using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Draw
{
    public abstract class AbstractDraw
    {
        protected System.Windows.Controls.Canvas MainCanvas { get; }
       

        public AbstractDraw(Canvas mainCanvas)
        {
            MainCanvas = mainCanvas;
        }

        public abstract void DrawInitial();
        public abstract void DrawFinal();
        public abstract void DrawNext(int stepNumber);
        public abstract void DrawPrevious(int stepNumber);

      
     }
 }

