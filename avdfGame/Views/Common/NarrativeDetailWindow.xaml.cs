using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace avdfGame
{
    /// <summary>
    /// Interaction logic for NarrativeDetailWindow.xaml
    /// </summary>
    public partial class NarrativeDetailWindow : Window
    {
        private Narrative _narrative;

        public NarrativeDetailWindow(Narrative narr)
        {
            InitializeComponent();

            _narrative = new Narrative();
            _narrative = narr;

        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.narrativeDetailControl.capabilityLabel.Content = _narrative.Capability.CapabilityName;
            this.narrativeDetailControl.playerName.Content = _narrative.AuthorName;

            foreach (NarrativeElement ne in _narrative.NarrativeData)
            {
                IEnumerable<NarrativeElementControl> controls = FindVisualChildren<NarrativeElementControl>(narrativeDetailControl);
                foreach (NarrativeElementControl nec in controls)
                {
                    if (nec.Tag.ToString() == ne.ElementName)
                    {
                        nec.contentTextBox.Text = ne.ElementContent;
                    }
                }
            }

        }
    }
}
