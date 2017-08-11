using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DispatcherQueueToUpdateUIThread
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await StartLongRunningOperationAsync();
        }

        private async Task StartLongRunningOperationAsync()
        {
            var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            await Task.Run(async () =>
             {
                 dispatcherQueue.TryEnqueue(() => progressBar.Value = 10);

                 // Task Delay is used to simulate some work on this Thread
                 await Task.Delay(1000);

                 // Just accessing the progressBar does not work, as we are on a separate thread.
                 // But try it: Comment out this line, and you'll get an Exception
                 // progressBar.Value = 50;

                 dispatcherQueue.TryEnqueue(() => progressBar.Value = 50);
                 

                 await Task.Delay(1000);
                 dispatcherQueue.TryEnqueue(() => progressBar.Value = 100);
             });
        }
    }
}
