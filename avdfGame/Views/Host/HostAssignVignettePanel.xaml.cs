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
    /// Interaction logic for HostAssignVignettePanel.xaml
    /// </summary>
    public partial class HostAssignVignettePanel : UserControl
    {
        public HostAssignVignettePanel()
        {
            InitializeComponent();
        }

        private void openNarrativesButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the custom routed event, this fires the event from the UserControl
            RaiseEvent(new RoutedEventArgs(HostAssignVignettePanel.OpenNarrativeWritingEvent));

        }

        // event declaration for event that gets bubbled up and caught by MainWindow
        public static readonly RoutedEvent OpenNarrativeWritingEvent =
            EventManager.RegisterRoutedEvent("OpenNarrativeWritingEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostAssignVignettePanel));

        // Create RoutedEventHandler
        public event RoutedEventHandler OpenNarrativeWriting
        {
            add { AddHandler(OpenNarrativeWritingEvent, value); }
            remove { RemoveHandler(OpenNarrativeWritingEvent, value); }
        }

    }
}
