using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace SilverMonkey.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Windows.DependencyObject" />
    public class HasDocument : DependencyObject
    {
        /// <summary>
        /// The document property
        /// </summary>
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document",
                                        typeof(FlowDocument),
                                        typeof(HasDocument),
                                        new PropertyMetadata(new PropertyChangedCallback(DocumentChanged)));

        private static void DocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("Document has changed");
        }

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        public FlowDocument Document
        {
            get { return GetValue(DocumentProperty) as FlowDocument; }
            set { SetValue(DocumentProperty, value); }
        }
    }
}