﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using EnvDTE;
using innometrics_visual_studio.Controller.ActivityControllers;
using LogInForm;
using Microsoft.VisualStudio.Shell;
using Model.Model;
using Task = System.Threading.Tasks.Task;
using Thread = System.Threading.Thread;

namespace innometrics_visual_studio.Controller
{
    /// <summary>
    /// The main controller of the plugin
    /// It's singletone and can be created through InitializeAsync only
    /// </summary>
    public class MenuController:IDisposable
    {
        /// <summary>
        /// Support method to run LogIn form
        /// Provide refrence to current running application
        /// </summary>
        private Application App
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
                        App.Run();
                    });

                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    return app1;
                }

                return Application.Current;
            }
        }

        /// <summary>
        /// All activities collectors.
        /// </summary>
        private List<AbstractActivityController> _activityControllers;


        /// <summary>
        /// Refecrence to the DataManager instance,
        /// which manipulate with collected data
        /// </summary>
        private static DataManager _dataManager;

        /// <summary>
        /// Flag, that user are providing auth data and LogIn form is running
        /// </summary>
        private bool IsLoging { get; set; }

        /// <summary>
        /// Flag, that plugin can send data to backend
        /// </summary>
        public bool IsCanSendData { get; private set; }

        /// <summary>
        /// Reference to DTE instance of Visual studio,
        /// which provide interface to manage documents of the open project and VS itself
        /// </summary>
        public DTE Dte { get; private set; }


        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static MenuController Instance
        {
            get;
            private set;
        }


        /// <summary>
        /// Initialize MenuController instance
        /// </summary>
        private MenuController()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            IsLoging = false;
            _activityControllers = new List<AbstractActivityController>();
            _dataManager = new DataManager();

        }


        /// <summary>
        /// Initializes the singleton instance of the MenuController.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {

            if(package == null) throw new NullReferenceException("The package is null, please, provide reference");

            // Switch to the main thread - the call to AddCommand in ResumeSendData's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);


            Instance = new MenuController { Dte = await package.GetServiceAsync(typeof(DTE)) as DTE};
            
            //get all subclass of AbstractActivityController, which are represent activities and even new was added, the subClassTypes variable will contain it after restarting
            //therefore no need to change this method after adding new activities
            var subclassTypes = Assembly.GetAssembly(typeof(AbstractActivityController)).GetTypes().Where(t => t.IsSubclassOf(typeof(AbstractActivityController)) && !t.IsAbstract);
            if(Instance.Dte == null) throw new Exception("The plugin was initialize incorrectly, please, restart Visual Studio or reinstall plugin!");

            foreach (var activityType in subclassTypes)
            {
                if(!(Activator.CreateInstance(activityType) is AbstractActivityController activityController)) continue;

                Instance._activityControllers.Add(activityController);
                
                //Can be change on DTE2 or future DTE3 without any trouble, no other changes required for updating DTE# interface
                Instance.Dte.Events.DocumentEvents.DocumentOpened += activityController.StartActivity;
                Instance.Dte.Events.DocumentEvents.DocumentClosing += activityController.EndActivity;
                Instance.Dte.Events.DocumentEvents.DocumentSaved += activityController.EndActivity;

                Instance.Dte.Events.TextEditorEvents.LineChanged += activityController.OnChanged;
                activityController.OnMetricsUpdated += _dataManager.OnSendMetrics;
            }
        }

        /// <summary>
        /// The event on logout click button
        /// Send command to DataManager to clean auth dta cache
        /// </summary>
        public void OnLogoutClick()
        {
            _dataManager.UnAuthenticate();
        }


        /// <summary>
        /// The event on login click button
        /// Run LogIn Form application with Dispatcher
        /// </summary>
        public void OnLogInClick()
        {
            // Use of dispatcher necessary as this is a cross-thread operation
            App.Dispatcher.Invoke(() =>
            {
                MainWindow window = new MainWindow {OnCorrectDataProvided = OnCorrectDataProvided};
                window.Show();
            });           
        }

        /// <summary>
        /// The event on SendData click button
        /// Set flag "SenData" to true and MenuController say to DataManager, that he can send data
        /// </summary>
        public void OnSendData()
        {
            IsCanSendData = true;
        }

        /// <summary>
        /// The event on ResumeSendData click button
        /// Set flag "SenData" to false and MenuController say to DataManager, that he can't send data
        /// </summary>
        public void OnResumeSendData()
        {
            IsCanSendData = false;
        }

        /// <summary>
        /// the event on correct auth data provided by user
        /// Shutdown the login application
        /// Send to DataManager correct auth data, which are used for sending data to backend
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        private void OnCorrectDataProvided(string email, string password)
        {
            App.Dispatcher.Invoke(() => App.Shutdown());
            _dataManager.Authenticate(email,password);
            IsLoging = false;
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose()
        {
            foreach (AbstractActivityController activityController in _activityControllers)
            {
                _dataManager.OnSendMetrics(activityController);
            }
        }
    }
}
