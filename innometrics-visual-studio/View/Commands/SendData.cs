using System;
using System.ComponentModel.Design;
using innometrics_visual_studio.Controller;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace innometrics_visual_studio.View.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class SendData
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4130;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("94c7a0a3-4e2b-48b7-83ae-1d3a56971e4a");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly InnoMetricsMenu package;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendData"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private SendData(Package package, OleMenuCommandService commandService)
        {
            this.package = package as InnoMetricsMenu ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static SendData Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider => package;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            // Switch to the main thread - the call to AddCommand in SendData's constructor requires
            // the UI thread.
            

            OleMenuCommandService commandService = ((IServiceProvider)package).GetService((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new SendData(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            MenuController.Instance.OnSendData();
        }
    }
}
