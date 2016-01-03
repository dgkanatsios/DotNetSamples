using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Win8UXPatterns.Behaviors
{


        [SuppressMessage("Microsoft.Design", "CA1052:StaticHolderTypesShouldBeSealed", Justification = "Cannot be static and derive from DependencyObject.")]
        public class FeedbackEffect
        {


            public static bool GetIsFeedbackEnabled(DependencyObject obj)
            {
                return (bool)obj.GetValue(IsFeedbackEnabledProperty);
            }

            public static void SetIsFeedbackEnabled(DependencyObject obj, bool value)
            {
                obj.SetValue(IsFeedbackEnabledProperty, value);
            }

            // Using a DependencyProperty as the backing store for IsFeedbackEnabled.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty IsFeedbackEnabledProperty =
                DependencyProperty.RegisterAttached("IsFeedbackEnabled", typeof(bool), typeof(FeedbackEffect), new PropertyMetadata(false, OnIsFeedbackEnabledChanged));



            private static void OnIsFeedbackEnabledChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
            {
                FrameworkElement fe = target as FrameworkElement;
                if (fe != null)
                {
                    // Add / remove the event handler if necessary
                    if ((bool)args.NewValue == true)
                    {
                        //fe.PointerPressed += TiltEffect_PointerPressed;
                        fe.PointerMoved += fe_PointerMoved;
                        fe.PointerExited += fe_PointerExited;
                        fe.PointerPressed += fe_PointerPressed;
                        fe.PointerReleased += fe_PointerReleased;
                    }
                    else
                    {
                        //fe.PointerPressed -= TiltEffect_PointerPressed;
                        fe.PointerMoved -= fe_PointerMoved;
                        fe.PointerExited -= fe_PointerExited;
                    }
                }
            }

            static void fe_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
            {
                FrameworkElement fe = sender as FrameworkElement;
                CompositeTransform ct = fe.RenderTransform as CompositeTransform;
                if (ct == null)
                {
                    fe.RenderTransform = new CompositeTransform();
                    ct = fe.RenderTransform as CompositeTransform;
                }

                ct.ScaleX = ct.ScaleY = 1.0;
                fe.Opacity = 1;
            }

            static void fe_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
            {
                FrameworkElement fe = sender as FrameworkElement;
                CompositeTransform ct = fe.RenderTransform as CompositeTransform;
                if (ct == null)
                {
                    fe.RenderTransform = new CompositeTransform();
                    ct = fe.RenderTransform as CompositeTransform;
                }

                ct.ScaleX = ct.ScaleY = 1.0;
                fe.Opacity = 0.3;
            }

            static void fe_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
            {
                FrameworkElement fe = sender as FrameworkElement;
                CompositeTransform ct = fe.RenderTransform as CompositeTransform;
                if (ct == null)
                {
                    fe.RenderTransform = new CompositeTransform();
                    ct = fe.RenderTransform as CompositeTransform;
                }

                ct.ScaleX = ct.ScaleY = 1.0;
                fe.Opacity = 1;
            }

            private static void fe_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
            {
                FrameworkElement fe = sender as FrameworkElement;
                fe.RenderTransformOrigin = new Point(0.5, 0.5);
                CompositeTransform ct = fe.RenderTransform as CompositeTransform;
                if (ct == null)
                {
                    fe.RenderTransform = new CompositeTransform();
                    ct = fe.RenderTransform as CompositeTransform;
                }

                ct.ScaleX = ct.ScaleY = 1.05;
                fe.Opacity = 1;
            }


       
    }

}
