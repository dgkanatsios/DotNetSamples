using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace FirstWordsKinect
{
    public static class KinectWrapper
    {
        public static KinectSensor KinectSensor {get;set;}
        static KinectWrapper()
        {
            if (KinectSensor.KinectSensors.Count == 0) return;

            KinectSensor = KinectSensor.KinectSensors[0];

            if (KinectSensor.Status != KinectStatus.Connected) return;

            InitializeSmoothing();


            KinectSensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(KinectSensor_AllFramesReady);

            KinectSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            KinectSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

            KinectSensor.Start();
        }

        static void KinectSensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            KinectAllFramesReady(sender, e);
        }

        public static SkeletonStream SkeletonStream
        {
            get { return KinectSensor.SkeletonStream; }
        }

        private static void InitializeSmoothing()
        {
            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.3f,
                Correction = 0.0f,
                Prediction = 0.0f,
                JitterRadius = 1.0f,
                MaxDeviationRadius = 0.5f
            };
            KinectSensor.SkeletonStream.Enable(parameters);
        }


        public static void Stop()
        {
            KinectSensor.Stop();
            KinectSensor.AudioSource.Stop();
        }

        public static event EventHandler<AllFramesReadyEventArgs> KinectAllFramesReady;
       
    }
}
