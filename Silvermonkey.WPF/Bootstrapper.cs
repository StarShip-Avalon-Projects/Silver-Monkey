using Caliburn.Micro;
using MonkeyCore.Logging;
using SilverMonkey.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SilverMonkey
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Caliburn.Micro.BootstrapperBase" />
    public class Bootstrapper : BootstrapperBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
            Initialize();
        }

        /// <summary>
        /// Gets a value indicating whether to handle Unhandled exceptions.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do handle]; otherwise, <c>false</c>.
        /// </value>
        public bool DoHandle { get; private set; } = true;

        /// <summary>
        /// Called when [unhandled exception].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
        protected override void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (DoHandle)
            {
                var ErrorLog = new ErrorLogging(e.Exception, sender);

                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        /// <summary>
        /// Called when [startup].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}