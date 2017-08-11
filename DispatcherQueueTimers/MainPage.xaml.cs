using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DispatcherQueueTimers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherQueueTimer _timer;

        public MainPage()
        {
            this.InitializeComponent();

            _timer = DispatcherQueue.GetForCurrentThread().CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);

            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(DispatcherQueueTimer sender, object args)
        {
            var now = DateTime.Now;
            txtTime.Text = now.ToString("HH:mm:ss");

            secondRotateTransform.Angle = now.Second * 6;
            minuteRotateTransform.Angle = now.Minute * 6;

            var currentHoursInMinutes = (now.Hour % 12) * 60 + now.Minute;
            const int minutesPerRound = 12 * 60;
            hourRotateTransform.Angle = ((double)(currentHoursInMinutes - minutesPerRound) / minutesPerRound) * 360;
        }
    }
}
