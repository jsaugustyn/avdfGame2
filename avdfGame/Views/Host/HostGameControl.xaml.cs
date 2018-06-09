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
    /// Interaction logic for TestControl.xaml
    /// </summary>
    public partial class HostGameControl : UserControl
    {
        public HostGameControl()
        {
            InitializeComponent();

            AddHandler(HostNarrativePanel.OpenAssessmentEvent, new RoutedEventHandler(OpenAssessmentEventHandler));
            AddHandler(HostPaletteControl.StartAssessmentEvent, new RoutedEventHandler(StartAssessmentEventHandler));
            AddHandler(HostPaletteControl.EndAssessmentEvent, new RoutedEventHandler(EndAssessmentEventHandler));

        }

        private void StartAssessmentEventHandler(object sender, RoutedEventArgs e)
        {
            // raise event to send message to player

            // raise event to be handled on MainWindow that will send a message to players opening assessment
            RaiseEvent(new RoutedEventArgs(HostGameControl.StartAssessmentEvent));

        }

        private void EndAssessmentEventHandler(object sender, RoutedEventArgs e)
        {
            // update UI
            hostEndSessionTab.Visibility = Visibility.Visible;
            gameTabs.SelectedItem = hostEndSessionTab;
        }

        public string HostPlayerName;

        private void OpenAssessmentEventHandler(object sender, RoutedEventArgs e)
        {
            // update UI
            hostAssessmentTab.Visibility = Visibility.Visible;
            gameTabs.SelectedItem = hostAssessmentTab;

            hostPaletteControl.paletteControl.PlayerName = this.HostPlayerName;
            
            // raise event to be handled on MainWindow that will send a message to players opening assessment
            RaiseEvent(new RoutedEventArgs(HostGameControl.OpenAssessmentEvent));
        }

        //public void InitializeAssessmentControl(List<string> players)
        //{
        //    //foreach (string p in players)
        //    //    hostPaletteControl.playerNamesListBox.Items.Add(p);
        //}

        public void AddPlayerNameToAssessmentControl(string name)
        {
            if(!hostPaletteControl.playerNamesListBox.Items.Contains(name))
                hostPaletteControl.playerNamesListBox.Items.Add(name);
        }

        private void openNarrativesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // EVENTS
        public static readonly RoutedEvent OpenAssessmentEvent =
            EventManager.RegisterRoutedEvent("OpenAssessmentEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostGameControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler OpenAssessment
        {
            add { AddHandler(OpenAssessmentEvent, value); }
            remove { RemoveHandler(OpenAssessmentEvent, value); }
        }

        // EVENTS
        public static readonly RoutedEvent StartAssessmentEvent =
            EventManager.RegisterRoutedEvent("StartAssessmentEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostGameControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler StartAssessment
        {
            add { AddHandler(StartAssessmentEvent, value); }
            remove { RemoveHandler(StartAssessmentEvent, value); }
        }

        private void endSessionButton_Click(object sender, RoutedEventArgs e)
        {
            // reset hostGameControl
            hostEndSessionTab.Visibility = Visibility.Hidden;
            hostAssessmentTab.Visibility = Visibility.Hidden;
            capabilityVoteTab.Visibility = Visibility.Hidden;
            choosevignetteTab.Visibility = Visibility.Hidden;

            gameTabs.SelectedItem = settingsTab;
            hostGameEntryControl.sessionNameTextBox.Text = "";
            hostGameEntryControl.sessionNameTextBox.IsReadOnly = false;
            hostGameEntryControl.sessionNameTextBox.IsEnabled = true;

            // raise event on MainWindow to save and reset session data
            RaiseEvent(new RoutedEventArgs(HostGameControl.EndSessionEvent));

        }
        public static readonly RoutedEvent EndSessionEvent =
    EventManager.RegisterRoutedEvent("EndSessionEvent", RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(HostGameControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler EndSession
        {
            add { AddHandler(EndSessionEvent, value); }
            remove { RemoveHandler(EndSessionEvent, value); }
        }

    }
}
