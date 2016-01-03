using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AsyncDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage : Page
    {
        public BlankPage()
        {
            this.InitializeComponent();
        }

        private void btn_Start(object sender, RoutedEventArgs e)
        {
            DoWorkTheVeryOldWay();
            //DoWorkTheOldWay();
            //DoWorkTheNewWay();
        }

        private void DoWorkTheVeryOldWay()
        {
            progress.IsActive = true;
            //will slow down performance
            BigNumber x = CalculatePi();
            txtResult.Text = x.Print();
            progress.IsActive = false;
        }



        private void DoWorkTheOldWay()
        {
            progress.IsActive = true;
            Task t = new Task(() =>
                {
                    BigNumber x = CalculatePi();
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            txtResult.Text = x.Print();
                            progress.IsActive = false;
                        });
                });
            t.Start();
        }

        private async void DoWorkTheNewWay()
        {
            progress.IsActive = true;
            BigNumber x = await CalculatePiAsync();
            txtResult.Text = x.Print();
            progress.IsActive = false;
        }


        private BigNumber CalculatePi()
        {
            BigNumber bn = null;

            BigNumber x = new BigNumber(25000);
            BigNumber y = new BigNumber(25000);
            x.ArcTan(16, 5);
            y.ArcTan(4, 239);
            x.Subtract(y);
            bn = x;
            return bn;

        }

        private async Task<BigNumber> CalculatePiAsync()
        {
            BigNumber bn = null;
            await Task.Run(() =>
            {
                BigNumber x = new BigNumber(25000);
                BigNumber y = new BigNumber(25000);
                x.ArcTan(16, 5);
                y.ArcTan(4, 239);
                x.Subtract(y);
                bn = x;
            });
            return bn;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

      
    }
}
