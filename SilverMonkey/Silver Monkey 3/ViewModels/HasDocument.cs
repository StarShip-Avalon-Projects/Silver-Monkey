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
    public class HasDocument : DependencyObject
    {
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document",
                                        typeof(FlowDocument),
                                        typeof(HasDocument),
                                        new PropertyMetadata(new PropertyChangedCallback(DocumentChanged)));

        private static void DocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("Document has changed");
        }

        public FlowDocument Document
        {
            get { return GetValue(DocumentProperty) as FlowDocument; }
            set { SetValue(DocumentProperty, value); }
        }
    }
}