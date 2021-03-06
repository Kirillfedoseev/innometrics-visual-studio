﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using innometrics_visual_studio.Controller;
using innometrics_visual_studio.View.Commands;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace innometrics_visual_studio.View
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = false)]
    [InstalledProductRegistration("#1110", "#1112", "1.0", IconResourceID =
        1400)] // Info on this package for Help/About
    [Guid(InnoMetricsMenu.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class InnoMetricsMenu : Package
    {
        /// <summary>
        /// InnoMetricsMenu GUID string.
        /// </summary>
        public const string PackageGuidString = "2fe52e27-a160-48be-b065-4bb5bdec6b0c";

        /// <summary>
        /// Initializes a new instance of the <see cref="InnoMetricsMenu"/> class.
        /// </summary>
        public InnoMetricsMenu()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }



        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            LogIn.Initialize(this);
            LogOut.Initialize(this);
            SendData.Initialize(this);
            ResumeSendData.Initialize(this);
            MenuController.Initialize(this);
        }

        protected override void Dispose(bool disposing)
        {
            MenuController.Instance.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}
