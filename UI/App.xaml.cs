using GalaSoft.MvvmLight.Messaging;
using System.IO;
using System.IO.Pipes;
using System.Windows;
using ViewModel;

namespace UI
{
    public partial class App : Application
    {
        private CancellationTokenSource cts = new();
        private static Mutex mutex;
        private MainVM _mainVM;

        protected async override void OnStartup(StartupEventArgs e)
        {
            string mutexName = "ShiftFlowMutex";
            bool createdNew;
            mutex = new(true, mutexName, out createdNew);

            if (!createdNew)
            {
                SendUriToRunningInstance(e.Args);
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);

            _mainVM = new MainVM();

            MainWindow mainWindow = new();
            mainWindow.DataContext = _mainVM;
            mainWindow.Show();

            if (e.Args.Length > 0)
            {
                string deepLink = e.Args[0];
                _mainVM.ProcessArgs(deepLink);
            }

            StartListeningForDeepLinks();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            mutex?.ReleaseMutex();
            mutex?.Dispose();
            base.OnExit(e);
        }

        private void SendUriToRunningInstance(string[] args)
        {
            if (args.Length > 0)
            {
                using (var client = new NamedPipeClientStream("ShiftFlowPipe"))
                {
                    using (var writer = new StreamWriter(client))
                    {
                        client.Connect(1000);
                        writer.WriteLine(args[0]);
                        writer.Flush();
                    }
                }
            }
        }

        private void StartListeningForDeepLinks()
        {
            Task.Run(async () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    using (var server = new NamedPipeServerStream("ShiftFlowPipe"))
                    {
                        await server.WaitForConnectionAsync(cts.Token);
                        using (var reader = new StreamReader(server))
                        {
                            string deepLink = await reader.ReadLineAsync();

                            if (deepLink != null)
                            {
                                await Current.Dispatcher.InvokeAsync(() =>
                                {
                                    Messenger.Default.Send(deepLink);
                                });
                            }
                        }
                    }
                }
            }, cts.Token);
        }

        public void StopListeningForDeepLinks()
        {
            cts.Cancel();
        }
    }
}