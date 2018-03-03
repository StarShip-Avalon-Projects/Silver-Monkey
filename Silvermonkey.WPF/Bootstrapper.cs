using Caliburn.Micro;
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
    {/// <summary>
     ///
     /// </summary>
        public Bootstrapper()
        {
            Initialize();
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