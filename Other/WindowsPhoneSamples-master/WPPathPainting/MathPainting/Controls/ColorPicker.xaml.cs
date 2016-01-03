using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MathPainting
{
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ColorPicker_Loaded);
        }

        void ColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
            {

                List<ColorDetails> l = new List<ColorDetails>() { 
                new ColorDetails() { Color = Colors.Blue, Number = 1},
                new ColorDetails() { Color = Colors.Red, Number = 2},
                new ColorDetails() { Color = Colors.Yellow, Number = 3},
                new ColorDetails() { Color = Colors.Green, Number = 4},
                new ColorDetails() { Color = Colors.Gray, Number = 5},
                new ColorDetails() { Color = Colors.Black, Number = 6},
                new ColorDetails() { Color = Colors.Brown, Number = 7},
                new ColorDetails() { Color = Colors.Orange, Number = 8},
                new ColorDetails() { Color = Colors.Purple, Number = 9},
                new ColorDetails() { Color = Colors.Magenta, Number = 10},
                new ColorDetails() { Color = Colors.Cyan, Number = 11},
                new ColorDetails() { Color = Colors.DarkGray, Number = 12},
                new ColorDetails() { Color = Colors.LightGray, Number = 13},
                new ColorDetails() { Color = Colors.Yellow, Number = 14}
                };

                listBox.ItemsSource = l;

                IsLoaded = true;
            }
        }

        private bool IsLoaded = false;

        public event EventHandler<ColorEventArgs> ColorChanged;

        private void Rectangle_MouseUp(object sender, RoutedEventArgs e)
        {
            if (ColorChanged != null)
                ColorChanged(this, new ColorEventArgs() { Color = ((sender as Rectangle).Fill as SolidColorBrush).Color });
        }

        private class ColorDetails
        {
            public Color Color {get;set;}
            public int Number {get;set;}
        }
    }
}
