using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf.Controls;
using Coding4Fun.Kinect.Wpf;
using System.Diagnostics;
using System.Windows.Media.Animation;
using Kinect.Toolbox;
using Kinect.Toolbox.Voice;


namespace FirstWordsKinect
{

    public enum KinectState
    {
        MovingCursor,
        MovingObject
    }

    public partial class MainScreenView : UserControl
    {
        public MainScreenView()
        {
            InitializeComponent();
            cursor.SetValue(Canvas.ZIndexProperty, 999);
            this.Loaded += new RoutedEventHandler(MainScreenView_Loaded);
            this.Unloaded += new RoutedEventHandler(MainScreenView_Unloaded);
        }

        void MainScreenView_Unloaded(object sender, RoutedEventArgs e)
        {
            KinectWrapper.Stop();
        }

        private VoiceCommander voiceCommander;

        KinectState kinectState = KinectState.MovingCursor;

        
        List<MoveableLetter> movingLetters = new List<MoveableLetter>();
        List<StaticLetter> staticLetters = new List<StaticLetter>();
        SwipeGestureDetector swipeGestureDetector = new SwipeGestureDetector();
        HoverButton movingButton;


        void MainScreenView_Loaded(object sender, RoutedEventArgs e)
        {
            KinectWrapper.KinectAllFramesReady+=new EventHandler<AllFramesReadyEventArgs>(KinectWrapper_KinectAllFramesReady);
            InitializeLetters();
            swipeGestureDetector.OnGestureDetected += new Action<string>(detector_OnGestureDetected);
            kinectColorViewer.Kinect = KinectWrapper.KinectSensor;
            kinectSkeletonViewer.Kinect = KinectWrapper.KinectSensor;
            kinectDepthViewer.Kinect = KinectWrapper.KinectSensor;
            voiceCommander = new VoiceCommander("stop");
            voiceCommander.OrderDetected += new Action<string>(voiceCommander_OrderDetected);
            voiceCommander.Start(KinectWrapper.KinectSensor);
        }

        void voiceCommander_OrderDetected(string obj)
        {
            if (obj == "stop")
            {
                kinectState = KinectState.MovingCursor;
                if (movingButton != null)
                {
                    movingButton.Release();
                    movingButton = null;
                }
            }
        }

       
        void detector_OnGestureDetected(string obj)
        {
            if (obj == "SwipeToRight")
            {
                kinectState = KinectState.MovingCursor;
                if (movingButton != null)
                {
                    movingButton.Release();
                    movingButton = null;
                }
            }
        }

        private void InitializeLetters()
        {

            //init static letters
            staticLetters = Utilities.GetStaticLetters();
            foreach (var item in staticLetters)
            {
                lettersStackPanel.Children.Add(item.Border);
            }
            movingLetters = Utilities.GetMoveableLetters();
            foreach (var letter in movingLetters)
            {
                gameCanvas.Children.Add(letter.HoverButton);
                letter.HoverButton.Click += new RoutedEventHandler(HoButton_Click);
            }
        }

        void HoButton_Click(object sender, RoutedEventArgs e)
        {
            //user has captured a letter
            kinectState = KinectState.MovingObject;
            movingButton = sender as HoverButton;
        }



        void KinectWrapper_KinectAllFramesReady(object sender, AllFramesReadyEventArgs e)
        {

            using (SkeletonFrame allSkeletons = e.OpenSkeletonFrame())
            {
                if (allSkeletons == null) return;

                Skeleton[] lala = new Skeleton[allSkeletons.SkeletonArrayLength];
                allSkeletons.CopySkeletonDataTo(lala);
                Skeleton skeleton = (from s in lala
                                         where s.TrackingState == SkeletonTrackingState.Tracked
                                         select s).FirstOrDefault();

                if (skeleton == null) return;

                Joint HandRight = skeleton.Joints[JointType.HandRight].ScaleTo((int)this.Width, (int)this.Height, .3f, .3f);
                //Debug.WriteLine(HandRight.Position.X + "    " + HandRight.Position.Y);

                Canvas.SetLeft(cursor, HandRight.Position.X);
                Canvas.SetTop(cursor, HandRight.Position.Y);


                SkeletonPoint cursorPoint = HandRight.Position;

                if (kinectState == KinectState.MovingCursor)
                {
                    foreach (MoveableLetter letter in movingLetters)
                    {
                        if (letter.IsMatched) continue;

                        HoverButton item = letter.HoverButton;
                        if (CheckIfCursorOnTopOfLetter(item))
                        {
                            MaximizeZIndex(item);
                            item.Hovering();
                            break;
                        }
                        else
                            item.Release();
                    }

                    if (CheckIfCursorOnTopOfAnimal())
                        hoverButton.Hovering();
                    else
                        hoverButton.Release();
                }

                else if (kinectState == KinectState.MovingObject) //cursor is carrying a letter
                {
                    //move the cursor
                    Canvas.SetLeft(movingButton, HandRight.Position.X - (movingButton as HoverButton).Width / 2);
                    Canvas.SetTop(movingButton, HandRight.Position.Y - (movingButton as HoverButton).Height / 2);
                    CheckForCollision();
                }

                ProcessFrameForGestureRecognition(skeleton);
            }
        }

        private void MaximizeZIndex(HoverButton item)
        {
            int max = -1;
            foreach (var item2 in movingLetters)
            {
                if (Canvas.GetZIndex(item2.HoverButton) > max)
                    max = Canvas.GetZIndex(item2.HoverButton);
            }
            Canvas.SetZIndex(item, ++max);
        }

        private void CheckForCollision()
        {
            //get stackpanel top and left compared to the canvas
            double stackPanelLeft = Canvas.GetLeft(lettersStackPanel);
            double stackPanelTop = Canvas.GetTop(lettersStackPanel);

            //get moving letter's top and left
            double letterLeft = Canvas.GetLeft(movingButton);
            double letterTop = Canvas.GetTop(movingButton);

            int width = 0;
            //check for collision
            for (int i = 0; i < lettersStackPanel.Children.Count; i++)
            {
                Border b = lettersStackPanel.Children[i] as Border;
                if (staticLetters[i].IsMatched)
                {
                    width += (int)b.Width;
                    continue; //already matched
                }


                double distance = Math.Sqrt(Math.Pow(stackPanelLeft + width - letterLeft, 2) + Math.Pow(stackPanelTop - letterTop, 2));

                StaticLetter staticLetter = staticLetters.Where(x => x.Border == b).Single();
                MoveableLetter moveableLetter = movingLetters.Where(x => x.HoverButton == movingButton).Single();

                if (distance < Utilities.DistanceForSucking && staticLetter.Index == moveableLetter.Index)  //if it's the same letter
                {
                    kinectState = KinectState.MovingCursor;
                    PiecesMatched(staticLetter, moveableLetter);
                    return;

                }
                width += (int)b.Width;
            }

        }

        private void PiecesMatched(StaticLetter staticLetter, MoveableLetter moveableLetter)
        {
            moveableLetter.HoverButton.IsHitTestVisible = false;
            staticLetter.IsMatched = moveableLetter.IsMatched = true;

            double newLeft = Canvas.GetLeft(lettersStackPanel) + staticLetter.Border.Width * staticLetter.Index;
            double newTop = Canvas.GetTop(lettersStackPanel);


            Storyboard sb = new Storyboard();

            DoubleAnimation daX = new DoubleAnimation();
            daX.To = newLeft;
            daX.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            DoubleAnimation daY = new DoubleAnimation();
            daY.To = newTop;
            daY.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            Storyboard.SetTarget(daX, movingButton);
            Storyboard.SetTarget(daY, movingButton);

            Storyboard.SetTargetProperty(daX, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTargetProperty(daY, new PropertyPath("(Canvas.Top)"));


            sb.Children.Add(daX);
            sb.Children.Add(daY);
            sb.Begin();
            //Canvas.SetLeft(movingButton, newLeft);
            //Canvas.SetTop(movingButton, newTop);
        }

        private void ProcessFrameForGestureRecognition(Skeleton skeleton)
        {
            //regarding gesture recognition
            foreach (Joint joint in skeleton.Joints)
            {
                if (joint.TrackingState != JointTrackingState.Tracked)
                    continue;

                if (joint.JointType ==  JointType.HandLeft)
                {
                    swipeGestureDetector.Add(joint.Position, KinectWrapper.KinectSensor);
                }
            }
        }



        private bool CheckIfCursorOnTopOfLetter(HoverButton letter)
        {
            double cursorCanvasLeft = Canvas.GetLeft(cursor);
            double cursorCanvasTop = Canvas.GetTop(cursor);

            double letterCanvasLeft = Canvas.GetLeft(letter);
            double letterCanvasTop = Canvas.GetTop(letter);

            if (cursorCanvasLeft > letterCanvasLeft && cursorCanvasLeft < letterCanvasLeft + letter.Width
                && cursorCanvasTop > letterCanvasTop && cursorCanvasTop < letterCanvasTop + letter.Height)
                return true;

            else return false;

        }

        private bool CheckIfCursorOnTopOfAnimal()
        {
            double cursorCanvasLeft = Canvas.GetLeft(cursor);
            double cursorCanvasTop = Canvas.GetTop(cursor);

            double letterCanvasLeft = Canvas.GetLeft(hoverButton);
            double letterCanvasTop = Canvas.GetTop(hoverButton);

            if (cursorCanvasLeft > letterCanvasLeft && cursorCanvasLeft < letterCanvasLeft + hoverButton.Width
                && cursorCanvasTop > letterCanvasTop && cursorCanvasTop < letterCanvasTop + hoverButton.Height)
                return true;

            else return false;

        }

        private void antButton_Click(object sender, RoutedEventArgs e)
        {
            (this.Resources["Storyboard1"] as Storyboard).Begin();
        }
    }
}
