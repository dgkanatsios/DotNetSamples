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
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace MathPainting
{
    public partial class PaintPage : PhoneApplicationPage
    {
        public PaintPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PaintPage_Loaded);
        }

        private Color selectedColor;

        void PaintPage_Loaded(object sender, RoutedEventArgs e)
        {
            colorPicker1.ColorChanged += new EventHandler<ColorEventArgs>(colorPicker1_ColorChanged);

            AddEventHandlersForPaths();
            //InsertNumbersIntoPaths();
        }

        private void InsertNumbersIntoPaths()
        {
            int i = 1;

            

            foreach (var item in path11.mainCanvas.Children)
            {



                if (!(item is Path)) return;
                Path p = item as Path;

                Point lowestPoint = new Point(10000,10000);
                foreach (var figure in (p.Data as PathGeometry).Figures)
                {
                    PathSegmentCollection col = figure.Segments;
                    foreach (var segment in col)
                    {
                        if (segment is LineSegment)
                        {
                            LineSegment l = segment as LineSegment;

                            Debug.WriteLine("line segment {0} ", l.Point.ToString());

                            InsertEllipseHere(l.Point,p);

                            if (l.Point.X < lowestPoint.X)
                                lowestPoint = l.Point;

                            
                        }
                        else if (segment is BezierSegment)
                        {
                            BezierSegment b = segment as BezierSegment;

                            Debug.WriteLine("bezier 1 segment {0} ", b.Point1.ToString());
                            Debug.WriteLine("bezier 2 segment {0} ", b.Point2.ToString());
                            Debug.WriteLine("bezier 3 segment {0} ", b.Point3.ToString());


                            InsertEllipseHere(b.Point1, p);
                            InsertEllipseHere(b.Point2, p);
                            InsertEllipseHere(b.Point3, p);

                            if (b.Point1.X < lowestPoint.X)
                                lowestPoint = b.Point1;

                            if (b.Point2.X < lowestPoint.X)
                                lowestPoint = b.Point2;

                            if (b.Point3.X < lowestPoint.X)
                                lowestPoint = b.Point3;
                        }
                    }
                }

                GeneralTransform gt = p.TransformToVisual(LayoutRoot as UIElement);
                Point offset = gt.Transform(new Point(0, 0));
                double controlTop = offset.Y + lowestPoint.Y;
                double controlLeft = offset.X + lowestPoint.X;


                Border bor = new Border();
                bor.Width = 20;
                bor.Height = 20;
                bor.Background = new SolidColorBrush(Colors.Black);
                bor.Child = new TextBlock() { Width = 18, Text = i.ToString() };
               

                if (controlTop + bor.Height >= path11.Height)
                    controlTop = path11.Height - bor.Height;

                if (controlLeft + bor.Width >= path11.Width)
                    controlLeft = path11.Width - bor.Width;

                Canvas.SetZIndex(bor, 100);
                Canvas.SetTop(bor, controlTop);
                Canvas.SetLeft(bor, controlLeft);
                LayoutRoot.Children.Add(bor);

                i++;
            }
        }

        private void InsertEllipseHere(Point point, Path path)
        {
            GeneralTransform gt = path.TransformToVisual(LayoutRoot as UIElement);
            Point offset = gt.Transform(new Point(0, 0));
            double controlTop = offset.Y + point.Y;
            double controlLeft = offset.X + point.X;
            Ellipse el = new Ellipse();
            el.Width = 10;
            el.Height = 10;
            el.Fill = new SolidColorBrush(Colors.Yellow);

            Canvas.SetZIndex(el, 100);
            Canvas.SetTop(el, controlTop);
            Canvas.SetLeft(el, controlLeft);

            LayoutRoot.Children.Add(el);
        }

        private void AddEventHandlersForPaths()
        {
            foreach (var item in path11.mainCanvas.Children)
            {
                if (!(item is Path)) return;
                Path p = item as Path;
                p.MouseLeftButtonDown += new MouseButtonEventHandler(MainPage_MouseLeftButtonDown);
                p.Fill = new RadialGradientBrush(new GradientStopCollection() { new GradientStop() { Color = Colors.White }, new GradientStop() { Color = Colors.White } });
            }
        }

        void MainPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Path p = sender as Path;

            Storyboard sb = new Storyboard();



            ColorAnimationUsingKeyFrames c1 = new ColorAnimationUsingKeyFrames();
            c1.KeyFrames.Add(new EasingColorKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)), Value = Colors.White });
            c1.KeyFrames.Add(new EasingColorKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2)), Value = selectedColor });
            Storyboard.SetTarget(c1, p);
            Storyboard.SetTargetProperty(c1, new PropertyPath("(Path.Fill).(GradientBrush.GradientStops)[1].(GradientStop.Color)"));


            DoubleAnimationUsingKeyFrames c2 = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(c2, p);
            Storyboard.SetTargetProperty(c2, new PropertyPath("(Path.Fill).(GradientBrush.GradientStops)[1].(GradientStop.Offset)"));
            c2.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)), Value = 0 });
            c2.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2)), Value = 1 });

            DoubleAnimationUsingKeyFrames c3 = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(c3, p);
            Storyboard.SetTargetProperty(c3, new PropertyPath("(Path.Fill).(GradientBrush.GradientStops)[0].(GradientStop.Offset)"));
            c3.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)), Value = 0 });
            c3.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2)), Value = 1 });

            ColorAnimationUsingKeyFrames c4 = new ColorAnimationUsingKeyFrames();
            Storyboard.SetTarget(c4, p);
            Storyboard.SetTargetProperty(c4, new PropertyPath("(Path.Fill).(GradientBrush.GradientStops)[0].(GradientStop.Color)"));
            c4.KeyFrames.Add(new EasingColorKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)), Value = Colors.White });
            c4.KeyFrames.Add(new EasingColorKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2)), Value = selectedColor });


            sb.Children.Add(c1); sb.Children.Add(c2); sb.Children.Add(c3); sb.Children.Add(c4);

            sb.Begin();



        }

        void colorPicker1_ColorChanged(object sender, ColorEventArgs e)
        {
            selectedColor = e.Color;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap wb = new WriteableBitmap(path11, null);

            // Create a file name for the JPEG file in isolated storage.
            String tempJPEG = "TempJPEG";

            // Create a virtual store and file stream. Check for duplicate tempJPEG files.
            var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (myStore.FileExists(tempJPEG))
            {
                myStore.DeleteFile(tempJPEG);
            }

            IsolatedStorageFileStream myFileStream = myStore.CreateFile(tempJPEG);



            // Encode the WriteableBitmap object to a JPEG stream.
            wb.SaveJpeg(myFileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
            myFileStream.Close();

            // Create a new stream from isolated storage, and save the JPEG file to the media library on Windows Phone.
            myFileStream = myStore.OpenFile(tempJPEG, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            // Save the image to the camera roll or saved pictures album.
            MediaLibrary library = new MediaLibrary();


            // Save the image to the saved pictures album.
            Picture pic = library.SavePicture("SavedPicture.jpg", myFileStream);
            MessageBox.Show("Image saved to saved pictures album");


            myFileStream.Close();

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            InsertNumbersIntoPaths();
        }
    }
}