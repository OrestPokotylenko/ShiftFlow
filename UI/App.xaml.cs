using Microsoft.Extensions.DependencyInjection;
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
        private string? _deepLink;
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
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
                _deepLink = e.Args[0];
            }

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainVM>();
            mainWindow.Show();

            StartListeningForDeepLinks();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainVM>(provider =>
            {
                return new MainVM(provider, _deepLink);
            });
            services.AddTransient<ResetPasswordVM>(provider =>
            {
                return new ResetPasswordVM(_deepLink);
            });

            services.AddTransient<MainWindow>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            mutex?.ReleaseMutex();
            mutex?.Dispose();
            cts.Cancel();
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
                                    var mainVM = ServiceProvider.GetRequiredService<MainVM>();
                                    mainVM.ProcessArgs(deepLink);

                                });
                            }
                        }
                    }
                }
            }, cts.Token);
        }
    }
}