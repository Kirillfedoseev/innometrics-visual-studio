using System.Threading;
using System.Windows;
using innometrics_visual_studio.Model;
using LogInForm;

namespace innometrics_visual_studio
{
    public class MenuController
    {

        private static App app;

        private DataManager _dataManager;

        public MenuController()
        {
            _dataManager = new DataManager();
        }

        private void OnLogoutClick()
        {

        }


        private void OnLogInClick()
        {
            var thread = new Thread(() =>
            {
                app = new App { ShutdownMode = ShutdownMode.OnExplicitShutdown };
                app.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            // Use of dispatcher necessary as this is a cross-thread operation
            app.Dispatcher.Invoke(() =>
            {
                MainWindow window = new MainWindow {OnCorrectDataProvided = OnCorrectDataProvided};
                window.Show();
            });           
        }

        private void OnCorrectDataProvided(string email, string password)
        {
            app.Dispatcher.Invoke(() => app.Shutdown());
            _dataManager.Authenticate(email,password);
        }
    }
}
