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
    /// Interaction logic for PlayerGameControl.xaml
    /// </summary>
    public partial class PlayerGameControl : UserControl
    {
        public List<NarrativeElement> NarrativeData;
        public Capability ChosenCapability;
        private string playerName;

        public PlayerGameControl()
        {
            InitializeComponent();

            AddHandler(PlayerNarrativeCapabilityChoicePanel.CapabilityChosenEvent, new RoutedEventHandler(PlayerCapabilityChosenEventHandler));
            AddHandler(PlayerNarrativePanel.NarrativeSubmitEvent, new RoutedEventHandler(NarrativeSubmitEventHandler));
            AddHandler(PlayerNarrativePanel.NarrativeCancelEvent, new RoutedEventHandler(NarrativeCancelEventHandler));
            AddHandler(PlayerPostSubmissionUI.NewNarrativeEvent, new RoutedEventHandler(NewNarrativeEventHandler));
        }

        public void SetPlayerName(string n)
        {
            playerName = n;
        }

        public string GetPlayerName()
        {
            return playerName;
        }

        public void SetVignette(int i)
        {
            playerVignettePanel.SetVignette(i);

            choosevignetteTab.Visibility = Visibility.Visible;
            gameTabs.SelectedItem = choosevignetteTab;

        }

        public void SetCapability(Capability c)
        {
            ChosenCapability = new Capability();
            ChosenCapability = c;
        }

        public Capability GetCapability()
        {
            return ChosenCapability;
        }

        public void StartAssessment()
        {
            assessmentTab.Visibility = Visibility.Visible;
            gameTabs.SelectedItem = assessmentTab;
            playerPaletteControl.playerPalette.PlayerName = playerName;
        }

        public void StartVoting(List<Capability> caps)
        {
            this.playerCapabilityPanel.SetCapabilities(caps);
            capabilityVoteTab.Visibility = Visibility.Visible;
            gameTabs.SelectedItem = capabilityVoteTab;

        }



        // handler to handle event raised by playerNarrativeChoicePanel
        private void NarrativeCancelEventHandler(object sender, RoutedEventArgs e)
        {
            // return to choice control
            //this.playerNarrativeControl.Visibility = Visibility.Hidden;
            this.playerNarrativeChoicePanel.Visibility = Visibility.Visible;
        }

        // handler to handle event raised by playerNarrativeChoicePanel
        private void NarrativeSubmitEventHandler(object sender, RoutedEventArgs e)
        {
            // save narratibe info into a public instance of a narrative class
            // initialize data structure for narrative data
            // this is written to be generic - it can handle an arbitray number of narrative elements, so the only place that's hard-coded right now is on the playerNarrativeControl.xaml
            NarrativeData = new List<NarrativeElement>();
           // UIElementCollection collection = this.playerNarrativeControl.narrativeControl.controlsStackPanel.Children;
            //foreach (UIElement uie in collection)
            //{
            //    NarrativeElement ne = new NarrativeElement();
            //    ne.ElementName = ((NarrativeElementControl)uie).ElementName;
            //    ne.ElementContent = ((NarrativeElementControl)uie).contentTextBox.Text;
            //    NarrativeData.Add(ne);
            //}
            
            // raise an event to be caught on MainWindow that will save data and initiate a send to host
            RaiseEvent(new RoutedEventArgs(PlayerGameControl.NarrativeSubmittedEvent));

            // Enable post submission UI
            EnablePostSubmissionUI();
        }

        public void EnablePostSubmissionUI()
        {
            // display post submission panel
            this.Dispatcher.Invoke(() =>
            {
                //playerNarrativeControl.Visibility = Visibility.Hidden;
                //playerPostSubmissionUI.Visibility = Visibility.Visible;
            });
        }

        private void NewNarrativeEventHandler(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                //playerPostSubmissionUI.Visibility = Visibility.Hidden;
                //playerNarrativeChoicePanel.Visibility = Visibility.Visible;
                //playerNarrativeControl.ClearContent();
            });


        }

        public void OpenAssessment()
        {
            // if you wanted to lock down further narrative submissions, this would be the place to do it

            // enable assessment UI
            assessmentTab.IsEnabled = true;
            gameTabs.SelectedItem = assessmentTab;
        }

        // handler to handle event raised by playerNarrativeChoicePanel
        private void PlayerCapabilityChosenEventHandler(object sender, RoutedEventArgs e)
        {
            // store capability
            SetCapability(((PlayerNarrativeCapabilityChoicePanel)e.OriginalSource).ChosenCapability);
            playerPaletteControl.chosenCapabilityTextBlock.Text = ChosenCapability.CapabilityName;
            playerPaletteControl.playerPalette.CapabilityIndex = ChosenCapability.CapabilityId;

            this.Dispatcher.Invoke(() =>
            {
                playerNarrativeChoicePanel.Visibility = Visibility.Hidden;
                playerPaletteControl.Visibility = Visibility.Visible;
            });

            // raise event to pass this to MainWindow
            RaiseEvent(new RoutedEventArgs(PlayerGameControl.CapabilityChosenEvent));

        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent CapabilityChosenEvent =
            EventManager.RegisterRoutedEvent("CapabilityChosenEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PlayerGameControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler CapabilityChosen
        {
            add { AddHandler(CapabilityChosenEvent, value); }
            remove { RemoveHandler(CapabilityChosenEvent, value); }
        }



        // Create RoutedEvent for HOST
        public static readonly RoutedEvent NarrativeSubmittedEvent =
            EventManager.RegisterRoutedEvent("NarrativeSubmittedEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PlayerGameControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler NarrativeSubmitted
        {
            add { AddHandler(NarrativeSubmittedEvent, value); }
            remove { RemoveHandler(NarrativeSubmittedEvent, value); }
        }

        public void EndSession()
        {
            endSessionTab.Visibility = Visibility.Hidden;
            assessmentTab.Visibility = Visibility.Hidden;
            capabilityVoteTab.Visibility = Visibility.Hidden;
            choosevignetteTab.Visibility = Visibility.Hidden;

            gameTabs.SelectedItem = startSessionTab;
        }
    }
}
