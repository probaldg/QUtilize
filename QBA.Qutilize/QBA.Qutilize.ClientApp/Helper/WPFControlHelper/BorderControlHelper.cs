﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QBA.Qutilize.ClientApp.Helper.WPFControlHelper
{
    public static class BorderControlHelper
    {
        public static Border CreateBorder()
        {
            return new Border()
            {
                BorderThickness = new Thickness()
                {
                    Bottom = 1,
                    Left = 1,
                    Right = 1,
                    Top = 1
                },
                BorderBrush = new SolidColorBrush(Colors.Black),

                Margin = new Thickness(10, 5, 0, 5)
            };
        }

        public static Border CreateBorderForSelectedControl()
        {
            return new Border()
            {
                BorderThickness = new Thickness()
                {
                    Bottom = 1,
                    Left = 10,
                    Right = 1,
                    Top = 1
                },
                BorderBrush = new SolidColorBrush(Colors.Green),

                Margin = new Thickness(10, 5, 0, 5)
            };
        }


        public static Border CreateBorderForTask()
        {
            return new Border()
            {
                BorderThickness = new Thickness()
                {
                    Bottom = 1,
                    Left = 0,
                    Right = 0,
                    Top = 0
                },
                BorderBrush = new SolidColorBrush(Colors.White),

                Margin = new Thickness(0)
            };
        }

    }
}
