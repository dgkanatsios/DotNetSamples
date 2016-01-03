using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Coding4Fun.Kinect.Wpf.Controls;

namespace FirstWordsKinect
{
    class Letter
    {
        
        public bool IsMatched { get; set; }
        public int Index { get; set; }
    }

    class StaticLetter : Letter
    {
        public Border Border { get; set; }
    }

    class MoveableLetter : Letter
    {
        public HoverButton HoverButton { get; set; }
    }
}
