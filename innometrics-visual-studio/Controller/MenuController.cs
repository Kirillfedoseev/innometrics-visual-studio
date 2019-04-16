using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using EnvDTE;
using EnvDTE80;
using innometrics_visual_studio.Controller.ActivityControllers;
using innometrics_visual_studio.Model;
using LogInForm;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;
using Thread = System.Threading.Thread;

namespace innometrics_visual_studio.Controller
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

        private List<AbstractActivityController> _activityControllers;

        private DataManager _dataManager;

        private bool IsLoging { get; set; }

        public bool IsCanSendData { get; private set; }

        public DTE Dte { get; private set; }


        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static MenuController Instance
        {
            get;
            private set;
        }



        public MenuController()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _dataManager = new DataManager();
            IsLoging = false;
            _activityControllers = new List<AbstractActivityController>();             
        }


        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in ResumeSendData's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            Instance = new MenuController { Dte = await package.GetServiceAsync((typeof(DTE))) as DTE};

            var subclassTypes = Assembly.GetAssembly(typeof(AbstractActivityController)).GetTypes().Where(t => t.IsSubclassOf(typeof(AbstractActivityController)) && !t.IsAbstract);
            foreach (var activityType in subclassTypes)
            {
                var activityController = Activator.CreateInstance(activityType) as AbstractActivityController;
                Instance._activityControllers.Add(activityController);
            }
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
