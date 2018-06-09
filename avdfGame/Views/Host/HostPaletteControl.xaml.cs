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
    /// Interaction logic for HostPaletteControl.xaml
    /// </summary>
    public partial class HostPaletteControl : UserControl
    {
        public HostPaletteControl()
        {
            InitializeComponent();
        }

        public string SelectedPlayerName { get; set; }

        private void endAssessmentButton_Click(object sender, RoutedEventArgs e)
        {
            // raise event to be caught on host game control
            RaiseEvent(new RoutedEventArgs(HostPaletteControl.EndAssessmentEvent));
        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent EndAssessmentEvent =
            EventManager.RegisterRoutedEvent("EndAssessmentEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostPaletteControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler EndAssessment
        {
            add { AddHandler(EndAssessmentEvent, value); }
            remove { RemoveHandler(EndAssessmentEvent, value); }
        }

        private void playerNamesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedPlayerName = ((ListBox)sender).SelectedItem.ToString();

            // raise event to request data from gameData...shoul call a setassessmentdta function
            RaiseEvent(new RoutedEventArgs(HostPaletteControl.RequestAssessmentDataEvent));
        }

        public string GetSelectedPlayerName()
        {
            return SelectedPlayerName;
        }

        public void SetAssessmentData(List<PaletteItemData> data, string capName)
        {
            this.selectedPlayerCapability.Text = capName;
            this.paletteControl.BuildPaletteFromData(data);
        }

        public static readonly RoutedEvent RequestAssessmentDataEvent =
    EventManager.RegisterRoutedEvent("RequestAssessmentDataEvent", RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(HostPaletteControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler RequestAssessmentData
        {
            add { AddHandler(RequestAssessmentDataEvent, value); }
            remove { RemoveHandler(RequestAssessmentDataEvent, value); }
        }

        private void startAssessmentButton_Click(object sender, RoutedEventArgs e)
        {
            endAssessmentButton.IsEnabled = true;
            startAssessmentButton.IsEnabled = false;

            // raise event
            RaiseEvent(new RoutedEventArgs(HostPaletteControl.StartAssessmentEvent));

        }
        // Create RoutedEvent for HOST
        public static readonly RoutedEvent StartAssessmentEvent =
            EventManager.RegisterRoutedEvent("StartAssessmentEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostPaletteControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler StartAssessment
        {
            add { AddHandler(StartAssessmentEvent, value); }
            remove { RemoveHandler(StartAssessmentEvent, value); }
        }

        private void sharePaletteButton_Click(object sender, RoutedEventArgs e)
        {
            //send to shared display
            RaiseEvent(new RoutedEventArgs(HostPaletteControl.ShowAssessmentDataEvent));
        }
        public static readonly RoutedEvent ShowAssessmentDataEvent =
            EventManager.RegisterRoutedEvent("ShowAssessmentDataEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostPaletteControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler ShowAssessmentData
        {
            add { AddHandler(ShowAssessmentDataEvent, value); }
            remove { RemoveHandler(ShowAssessmentDataEvent, value); }
        }


        private void getRatingsButton_Click(object sender, RoutedEventArgs e)
        {
            // tell players to display rating control
            RaiseEvent(new RoutedEventArgs(HostPaletteControl.GetRatingsEvent));

        }
        public static readonly RoutedEvent GetRatingsEvent =
            EventManager.RegisterRoutedEvent("GetRatingsEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostPaletteControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler GetRatings
        {
            add { AddHandler(GetRatingsEvent, value); }
            remove { RemoveHandler(GetRatingsEvent, value); }
        }
    }
}
