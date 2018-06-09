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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace avdfGame
{
    /// <summary>
    /// Interaction logic for HostNarrativePanel.xaml
    /// </summary>
    public partial class HostNarrativePanel : UserControl
    {
        public HostNarrativePanel()
        {
            InitializeComponent();
        }

        public void AddNarrative(Narrative narr)
        {
            NarrativeWidget nw = new NarrativeWidget(narr);
            nw.Margin = new Thickness(20);
            nw.authorNameTextBox.Content = narr.AuthorName;
            nw.capabilityTitleTextBlock.Text = narr.Capability.CapabilityName;

            narrativeWrapPanel.Children.Add(nw);
        }

        private void openAssessmentButton_Click(object sender, RoutedEventArgs e)
        {
            // raise event to be caught on host game control
            RaiseEvent(new RoutedEventArgs(HostNarrativePanel.OpenAssessmentEvent));

        }

        // EVENTS
        // Create RoutedEvent for HOST
        public static readonly RoutedEvent OpenAssessmentEvent =
            EventManager.RegisterRoutedEvent("OpenAssessmentEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostNarrativePanel));

        // Create RoutedEventHandler
        public event RoutedEventHandler OpenAssessment
        {
            add { AddHandler(OpenAssessmentEvent, value); }
            remove { RemoveHandler(OpenAssessmentEvent, value); }
        }

    }
}
