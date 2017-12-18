using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StopWatchProject
{

    public sealed partial class MainPage : Page
    {
        // Variables
        private DispatcherTimer dispatcherTimer, demoDispatcher;
        private int timeTicked = 1;
        private double ProgressAmount = 0;
        private DateTime startedTime;
        private TimeSpan timePassed, timeSinceLastStop;
        int lapCount = 0;

        bool isStop = false;

        // Constructor
        public MainPage()
        {
            this.InitializeComponent();
        }

        // Methods
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (isStop == false)
            {
                isStop = true;
                startedTime = DateTime.Now;
                DispatcherTimerSetup();
                Start.Content = "Stop";
            }
            else
            {
                isStop = false;
                dispatcherTimer.Stop();
                demoDispatcher.Stop();
                Hour.Text = "00:00:00:00:000";
                Start.Content = "Start";
                ProgressControl.setBarLength(0.0);
                ProgressAmount = 0;
            }
        }

        private void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();

            timeSinceLastStop = TimeSpan.Zero;
            Hour.Text = "00:00:00:00:000";
            demoDispatcher = new DispatcherTimer();
            demoDispatcher.Tick += demoDispatcher_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            demoDispatcher.Start();
        }

        private String MakeDigitString(int number, int count)
        {
            // Variables
            string result = "0";

            // Using if statement to show the number
            if (count == 2)
            {
                if (number < 10)
                    result = "0" + number;
                else
                    result = number.ToString();
            }
            else if (count == 3)
            {
                if (number < 10)
                    result = "0" + number;
                else if (number > 9 && number < 100)
                    result = "0" + number;
                else
                    result = number.ToString();
            }// end of if statement
            return result;
        }

        private void demoDispatcher_Tick(object sender, object e)
        {
            timePassed = DateTime.Now - startedTime;
            Hour.Text = MakeDigitString((timeSinceLastStop + timePassed).Hours, 2) + ":" + MakeDigitString((timeSinceLastStop + timePassed).Minutes, 2) +
                ":" + MakeDigitString((timeSinceLastStop + timePassed).Seconds, 2) + ":" + MakeDigitString((timeSinceLastStop + timePassed).Milliseconds, 3);
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            timeTicked++;
            ProgressControl.setBarLength(ProgressAmount);
            ProgressAmount += (1.0 / 60.0) + (7.95 * 60.0);
            if (ProgressAmount > 1.0)
                ProgressAmount = 0.0;
        }

        private void Lap_Click(object sender, RoutedEventArgs e)
        {
            lapCount++;
            txtLap.Text += "Lap" + lapCount + ": " + Hour.Text + "\n";
        }
    }
}// end of main
