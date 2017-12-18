using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
namespace StopWatchProject
{
    public sealed partial class CircleProgress : UserControl
    {
        // Variables
        private double ProgressValue = 0.0;

        // Creating constructor
        public CircleProgress()
        {
            this.InitializeComponent();
            DataContext = this;
        }

        #region SizeProperty
        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(double), typeof(CircleProgress), new PropertyMetadata((double)28.0, OnSizePropertyChanged));

        private static void OnSizePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            CircleProgress TheControl = (source as CircleProgress);
            if (TheControl != null)
                TheControl.UpdateSizing();
        }

        private double InsideSize { get { return Size - LineWidth; } }

        private double HalfInsideSize { get { return (Size - LineWidth) / 2; } }

        // Updating the timer everytimes i clicked
        private void UpdateSizing()
        {
            ThePath.Stroke = Color;
            ThePath.StrokeThickness = LineWidth;
            TheGrid.Width = Size;
            TheGrid.Height = Size;
            ThePathFigure.StartPoint = new Point(HalfInsideSize, InsideSize);
            TheSegment.Size = new Windows.Foundation.Size(HalfInsideSize, InsideSize);
            setBarLength(ProgressValue);
        }

        // Creating the barlength for the stopwatch
        public void setBarLength(double progressValue)
        {
            if (DesignMode.DesignModeEnabled)
                setBarLengthUI(progressValue);
            else if (CoreApplication.MainView.CoreWindow.Dispatcher.HasThreadAccess)
                setBarLengthUI(progressValue);
            else
            {
                double LocalValue = progressValue; 
                IAsyncAction IgnoreMe = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { setBarLengthUI(progressValue); });
            }
        }

        private void setBarLengthUI(double progressValue)
        {
            progressValue = Math.Max(0, Math.Min(1.0, progressValue));
            ProgressValue = progressValue;

            double Angle = 2 * 3.14159265 * ProgressValue;

            double X = HalfInsideSize - Math.Sin(Angle) * HalfInsideSize;
            double Y = HalfInsideSize + Math.Cos(Angle) * HalfInsideSize;

            if (progressValue > 0 && (int)X == HalfInsideSize && (int)Y == InsideSize)
                X += 0.01;

            TheSegment.IsLargeArc = Angle > 3.14159625;
            TheSegment.Point = new Point(X, Y);
        }
        #endregion SizeProperty

        #region LineWidth
        public double LineWidth
        {
            get { return (double)GetValue(LineWidthProperty); }
            set { SetValue(LineWidthProperty, value); }
        }
        public static readonly DependencyProperty LineWidthProperty = DependencyProperty.Register("LineWidth", typeof(double), typeof(CircleProgress), new PropertyMetadata((double)4.0, OnLineWidthPropertyChanged));
        private static void OnLineWidthPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            CircleProgress TheControl = source as CircleProgress;
            if (TheControl != null)
                TheControl.UpdateSizing();
        }
        #endregion LineWidth

        #region Color
        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(CircleProgress), new PropertyMetadata(new SolidColorBrush(Colors.White), OnColorPropertyChanged));
        private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircleProgress TheControl = d as CircleProgress;
            if (TheControl != null)
            TheControl.UpdateSizing();
        }
        #endregion Color
    }
}// end of circleprogress
