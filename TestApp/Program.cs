using System;
using System.Threading;
using System.Windows;
using LogInForm;

namespace TestApp
{
    class Program
    {
        static LogInForm.App app;

        static void Main(string[] args)
        {
            Action<string,string> OnCorrectDataProvided = (s, s1) => Console.WriteLine($@"{s}  {s1}");

            var appthread = new Thread(new ThreadStart(() =>
            {
                app = new App {ShutdownMode = ShutdownMode.OnExplicitShutdown};
                app.Run();
            }));
            appthread.SetApartmentState(ApartmentState.STA);
            appthread.Start();
            AuthData authData;

            while (true)
            {
                var key = Console.ReadKey().Key;
                // Press 1 to create a window
                if (key == ConsoleKey.D1)
                {
                    // Use of dispatcher necessary as this is a cross-thread operation
                    app.Dispatcher.Invoke(() =>
                    {
                        MainWindow window = new MainWindow();
                        window.OnCorrectDataProvided = (s, s1) => authData = new AuthData(s, s1);
                        window.Show();
                    });
                }

                // Press 2 to exit
                if (key == ConsoleKey.D2)
                {

                    app.Dispatcher.Invoke(() => app.Shutdown());
                    break;
                }
            }
        }

    }

    internal class AuthData
    {
        internal string Email;
        internal string Password;

        public AuthData(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

}