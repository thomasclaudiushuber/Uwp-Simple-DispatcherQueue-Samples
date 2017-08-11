using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DispatcherQueueDedicatedThreads
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherQueueController _controller;
        private DispatcherQueue _uiDispatcherQueue;

        public MainPage()
        {
            this.InitializeComponent();
            _uiDispatcherQueue = DispatcherQueue.GetForCurrentThread();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "";
            btnStop.IsEnabled = true;
            btnStart.IsEnabled = false;
            _controller = DispatcherQueueController.CreateOnDedicatedThread();

            _controller.DispatcherQueue.TryEnqueue(() =>
            {
                var primeNumbers = new List<int>();
                for (int i = 1; i <= 100000; i++)
                {
                    var count = 0;
                    for (int j = 2; j < i; j++)
                    {
                        if ((i % j) == 0)
                        {
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        primeNumbers.Add(i);
                    }

                    UpdateProgressBar(i / 1000);

                }

                UpateTextBlock(primeNumbers);
            });
        }

        private void UpateTextBlock(List<int> primeNumbers)
        {
            _uiDispatcherQueue.TryEnqueue(() => txtResult.Text = string.Join(" ", primeNumbers));
        }

        private void UpdateProgressBar(int value)
        {
            _uiDispatcherQueue.TryEnqueue(() => progressBar.Value = value);
        }

        private async void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            await _controller.ShutdownQueueAsync();
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
        }
    }
}
