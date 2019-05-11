using System.Windows;
using System.Windows.Controls;

namespace QBA.Qutilize.ClientApp.Helper.WPFControlHelper
{
    public static class CanvasControlHelper
    {
        public static Canvas CreateCanvas()
        {
            return new Canvas
            {
                Margin = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                MinHeight = 30,
                MinWidth = 304,


            };
        }
    }
}
