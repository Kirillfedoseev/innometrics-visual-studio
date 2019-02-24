using System.Threading;
using System.Windows;
using innometrics_visual_studio.Model;
using LogInForm;

namespace innometrics_visual_studio
{
    public class MenuController
    {

        private Application app
        {
            get
            {
                if (Application.Current == null)
                {
                    Application app1 = null;
                    var thread = new Thread(() =>
                    {
                        if (IsLoging) return;
                        IsLoging = true;
                        app1 = new Application {ShutdownMode = ShutdownMode.OnExplicitShutdown};
                        app.Run();
                    });

                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    return app1;
                }
                else return Application.Current;
            }
        }


        private DataManager _dataManager;

        private bool IsLoging { get; set; }

        public bool IsCanSendData { get; private set; }

        public MenuController()
        {
            _dataManager = new DataManager();
            IsLoging = false;
        }


        public void OnLogoutClick()
        {

        }


        public void OnLogInClick()
        {          
            // Use of dispatcher necessary as this is a cross-thread operation
            app.Dispatcher.Invoke(() =>
            {
                MainWindow window = new MainWindow {OnCorrectDataProvided = OnCorrectDataProvided};
                window.Show();
            });           
        }


        public void OnSendData()
        {
            IsCanSendData = true;
        }


        public void OnResumeSendData()
        {
            IsCanSendData = false;
        }



        private void OnCorrectDataProvided(string email, string password)
        {
            app.Dispatcher.Invoke(() => app.Shutdown());
            _dataManager.Authenticate(email,password);
            IsLoging = false;
        }
    }
}
