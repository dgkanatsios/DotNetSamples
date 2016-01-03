using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using Coding4Fun.Kinect.Wpf.Controls;

namespace FirstWordsKinect
{
    static class Utilities
    {
        public static Random Random = new Random();
        public const int DistanceForSucking = 30;


        public static List<MoveableLetter> GetMoveableLetters()
        {
            List<MoveableLetter> movingLetters = new List<MoveableLetter>();
            HoverButton bor = new HoverButton();
            bor.IsHitTestVisible = true;
            bor.ImageSource = "/images/letters/uppercase/uc-a@2x.png";
            bor.ActiveImageSource = "/images/letters/uppercase/uc-a@2x.png";
            bor.TimeInterval = 2000;
            bor.Width = 166;
            bor.Height = 214;
            bor.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri("images/backBrushes/blue-plain@2x.png", UriKind.Relative)) };
            Canvas.SetLeft(bor, Utilities.Random.Next(300, 600));
            Canvas.SetTop(bor, Utilities.Random.Next(200, 500));

            HoverButton bor2 = new HoverButton();
            bor2.Width = 166;
            bor2.Height = 214;
            bor2.TimeInterval = 2000;
            bor2.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri("images/backBrushes/yellow-plain@2x.png", UriKind.Relative)) };
            bor2.IsHitTestVisible = true;
            bor2.ImageSource = "/images/letters/uppercase/uc-n@2x.png";
            bor2.ActiveImageSource = "/images/letters/uppercase/uc-n@2x.png";
            Canvas.SetLeft(bor2, Utilities.Random.Next(300, 600));
            Canvas.SetTop(bor2, Utilities.Random.Next(200, 500));

            HoverButton bor3 = new HoverButton();
            bor3.Width = 166;
            bor3.Height = 214;
            bor3.TimeInterval = 2000;
            bor3.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri("images/backBrushes/orange-plain@2x.png", UriKind.Relative)) };
            bor3.IsHitTestVisible = true;
            bor3.ImageSource = "/images/letters/uppercase/uc-t@2x.png";
            bor3.ActiveImageSource = "/images/letters/uppercase/uc-t@2x.png";
            Canvas.SetLeft(bor3, Utilities.Random.Next(300, 600));
            Canvas.SetTop(bor3, Utilities.Random.Next(200, 500));

            movingLetters.Add(new MoveableLetter() { HoverButton = bor, Index = 0 });
            movingLetters.Add(new MoveableLetter() { HoverButton = bor2, Index = 1 });
            movingLetters.Add(new MoveableLetter() { HoverButton = bor3, Index = 2 });

            return movingLetters;
        }

        internal static List<StaticLetter> GetStaticLetters()
        {
            List<StaticLetter> staticLetters = new List<StaticLetter>();
            Border bor = new Border();
            bor.Width = 166;
            bor.Height = 214;
            bor.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri("images/backBrushes/blue-plain@2x.png", UriKind.Relative)) };
            bor.Child = new Image() { Source = new BitmapImage(new Uri("images/letters/uppercase/uc-a@2x.png", UriKind.Relative)) };
            Canvas.SetLeft(bor, Utilities.Random.Next(300, 700));
            Canvas.SetTop(bor, Utilities.Random.Next(200, 600));

            Border bor2 = new Border();
            bor2.Width = 166;
            bor2.Height = 214;
            bor2.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri("images/backBrushes/yellow-plain@2x.png", UriKind.Relative)) };
            bor2.Child = new Image() { Source = new BitmapImage(new Uri("images/letters/uppercase/uc-n@2x.png", UriKind.Relative)) };
            Canvas.SetLeft(bor2, Utilities.Random.Next(300, 700));
            Canvas.SetTop(bor2, Utilities.Random.Next(200, 600));

            Border bor3 = new Border();
            bor3.Width = 166;
            bor3.Height = 214;
            bor3.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri("images/backBrushes/orange-plain@2x.png", UriKind.Relative)) };
            bor3.Child = new Image() { Source = new BitmapImage(new Uri("images/letters/uppercase/uc-t@2x.png", UriKind.Relative)) };
            Canvas.SetLeft(bor3, Utilities.Random.Next(300, 700));
            Canvas.SetTop(bor3, Utilities.Random.Next(200, 600));

            staticLetters.Add(new StaticLetter() { Border = bor, Index = 0 });
            staticLetters.Add(new StaticLetter() { Border = bor2, Index = 1 });
            staticLetters.Add(new StaticLetter() { Border = bor3, Index = 2 });

            bor.Opacity = bor2.Opacity = bor3.Opacity = 0.6;

            return staticLetters;
        }
    }
}
