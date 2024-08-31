using Model;
using Service.ModelServices;
using System.IO;
using System.IO.Pipes;
using System.Windows;

namespace UI
{
    public partial class App : Application
    {
        private CancellationTokenSource cts = new();
        private static Mutex mutex;

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

            if (e.Args.Length > 0)
            {
                string deepLink = e.Args[0];

                MainWindow mainWindow = new(deepLink);
                mainWindow.Show();
                StartListeningForDeepLinks(mainWindow);
            }
            else
            {
                MainWindow mainWindow = new();
                mainWindow.Show();
                StartListeningForDeepLinks(mainWindow);
            }
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

        private void StartListeningForDeepLinks(MainWindow mainWindow)
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
                                await Current.Dispatcher.Invoke(async () =>
                                {
                                    //mainWindow?.ProcessArgs(deepLink);
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