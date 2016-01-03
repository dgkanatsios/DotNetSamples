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
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;

namespace RevealPaint
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(MainPage_MouseLeftButtonDown);
            this.MouseMove += new MouseEventHandler(MainPage_MouseMove);

            gc = new GeometryGroup();

            //br = new ImageBrush() { ImageSource = new BitmapImage(new Uri("/a.png", UriKind.Relative)), Stretch = Stretch.None, AlignmentX = AlignmentX.Center, AlignmentY = AlignmentY.Center };
            img= new Image { Source = new BitmapImage(new Uri("/a.png", UriKind.Relative)), Stretch = Stretch.None };

            img.Clip = gc;
            
            LayoutRoot.Children.Add(img);
        }

        ImageBrush br;
        Image img;
        GeometryGroup gc;
        void MainPage_MouseMove(object sender, MouseEventArgs e)
        {
            double x = e.GetPosition(this).X;
            double y = e.GetPosition(this).Y;
            gc.Children.Add(new EllipseGeometry() { Center = new Point(x, y), RadiusX = 25, RadiusY = 25 });
            img.Clip = gc;
        }


        int zindex = 5;
        void MainPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            double x = e.GetPosition(this).X;
            double y = e.GetPosition(this).Y;
            gc.Children.Add(new EllipseGeometry() { Center = new Point(x, y), RadiusX = 25, RadiusY = 25 });
            img.Clip = gc;
        }
    }
}