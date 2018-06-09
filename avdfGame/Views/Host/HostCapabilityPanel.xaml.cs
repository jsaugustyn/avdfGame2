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
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace avdfGame
{
    /// <summary>
    /// Interaction logic for HostCapabilityPanel.xaml
    /// </summary>
    public partial class HostCapabilityPanel : UserControl
    {
        public HostCapabilityPanel()
        {
            InitializeComponent();

            ShowAsHost = true;

            // handler for vote event received from CapabilittCardControl
            //AddHandler(CapabilityCardControl.VoteEvent, new RoutedEventHandler(VoteEventHandler));
        }

        private List<Capability> capabilities;
        public int VotedCapabilityIndex;
        public bool ShowAsHost;
        public int VoteDirection;

        public void SetCapabilities(List<Capability> caps)
        {
            capabilities = new List<Capability>();
            capabilities = caps;

            SortCapabilityList();

            RefreshCapabilityList();
        }

        private void RefreshCapabilityList()
        {
            hostBarChart.InitializeBarChart(capabilities);
            //capabilityList.Items.Clear();
            //foreach (Capability c in capabilities)
            //{
            //    CapabilityCardControl cc = new CapabilityCardControl(c, ShowAsHost);
            //    cc.Margin = new Thickness(0, 0, 0, 5);
            //    capabilityList.Items.Add(cc);
            //}
        }

        //// handler for vote events received from CapabiltyCardControl
        //private void VoteEventHandler(object sender, RoutedEventArgs e)
        //{
        //    CapabilityCardControl ccc = e.OriginalSource as CapabilityCardControl;

        //    // update voted on capability
        //    capabilities[ccc.CardCapability.CapabilityId].Votes = ccc.CardCapability.Votes;
        //    capabilities[ccc.CardCapability.CapabilityId].OwnerDidVote = ccc.CardCapability.OwnerDidVote;

        //    //  Sort capabilities by number of votes
        //    SortCapabilityList();

        //    // store a reference to the sending capabiity ID so MainWindow can grab it
        //    VotedCapabilityIndex = ((CapabilityCardControl)e.OriginalSource).CardCapability.CapabilityId;

        //    // refresh capability list to show sorted capabilities
        //    RefreshCapabilityList();

        //    // raise event to be handled on MainWindow
        //    RaiseEvent(new RoutedEventArgs(HostCapabilityPanel.VoteReceivedEvent));
        //}

        // bubble sort on the length of the string contained in each item.
        protected void SortCapabilityList()
        {
            if (capabilities.Count > 1)
            {
                bool swapped;
                do
                {
                    int counter = capabilities.Count - 1;
                    swapped = false;

                    while (counter > 0)
                    {
                        // Compare the items' length. 
                        if ( capabilities[counter].Votes > capabilities[counter-1].Votes )
                        {
                            // Swap the items.
                            Capability temp = capabilities[counter];
                            capabilities[counter] = capabilities[counter - 1];
                            capabilities[counter - 1] = temp;
                            swapped = true;
                        }
                        // Decrement the counter.
                        counter -= 1;
                    }
                }
                while ((swapped == true));
            }
        }


        // event declaration for event that gets bubbled up and caught by MainWindow
        public static readonly RoutedEvent VoteReceivedEvent =
            EventManager.RegisterRoutedEvent("VoteReceivedEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostCapabilityPanel));

        // Create RoutedEventHandler
        public event RoutedEventHandler VoteReceived
        {
            add { AddHandler(VoteReceivedEvent, value); }
            remove { RemoveHandler(VoteReceivedEvent, value); }
        }


        // Create RoutedEvent for HOST
        public static readonly RoutedEvent StartVoteEvent =
            EventManager.RegisterRoutedEvent("StartVoteEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostCapabilityPanel));

        // Create RoutedEventHandler
        public event RoutedEventHandler StartVote
        {
            add { AddHandler(StartVoteEvent, value); }
            remove { RemoveHandler(StartVoteEvent, value); }
        }

        // When the Join as Host button on the User Control is clicked, use RaiseEvent to fire the 
        // Custom Routed Event
        private void startVoteButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the custom routed event, this fires the event from the UserControl
            RaiseEvent(new RoutedEventArgs(HostCapabilityPanel.StartVoteEvent));

            voteStatusLabel.Content = "Voting is Live";
            voteStatusLabel.Background = Brushes.DeepSkyBlue;

        }


        // Create RoutedEvent for HOST
        public static readonly RoutedEvent StopVoteEvent =
            EventManager.RegisterRoutedEvent("StopVoteEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostCapabilityPanel));

        // Create RoutedEventHandler
        public event RoutedEventHandler StopVote
        {
            add { AddHandler(StopVoteEvent, value); }
            remove { RemoveHandler(StopVoteEvent, value); }
        }

        private void stopVoteButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the custom routed event, this fires the event from the UserControl
            RaiseEvent(new RoutedEventArgs(HostCapabilityPanel.StopVoteEvent));

            voteStatusLabel.Content = "Voting Stopped (Click Start Voting to go live)";
            voteStatusLabel.Background = Brushes.DarkGray;
        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent ShowVotesEvent =
            EventManager.RegisterRoutedEvent("ShowVotesEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostCapabilityPanel));

        // Create RoutedEventHandler
        public event RoutedEventHandler ShowVotes
        {
            add { AddHandler(ShowVotesEvent, value); }
            remove { RemoveHandler(ShowVotesEvent, value); }
        }

        private void showVoteButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the custom routed event, this fires the event from the UserControl
            RaiseEvent(new RoutedEventArgs(HostCapabilityPanel.ShowVotesEvent));

        }
    }
}
