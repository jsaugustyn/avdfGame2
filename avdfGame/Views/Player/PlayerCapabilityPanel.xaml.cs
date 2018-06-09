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
    /// Interaction logic for PlayerCapabilityPanel.xaml
    /// </summary>
    public partial class PlayerCapabilityPanel : UserControl
    {
        public PlayerCapabilityPanel()
        {
            InitializeComponent();

            // handler for vote event received from CapabilittCardControl
            AddHandler(CapabilityCardControl.VoteEvent, new RoutedEventHandler(PlayerVoteEventHandler));
            // handler for vote event received from CapabilittCardControl
            AddHandler(CapabilityCardControl.UnvoteEvent, new RoutedEventHandler(PlayerUnvoteEventHandler));

        }

        private List<Capability> capabilities;
        private List<Capability> capabilitiesVotedFor;

        public int VoteDirection;
        public int VotedCapabilityIndex;

        public void SetCapabilities(List<Capability> caps)
        {
            capabilities = new List<Capability>();
            capabilities = caps;

            RefreshCapabilityList();
        }

        private void RefreshCapabilityList()
        {
            capabilityList.Items.Clear();
            foreach (Capability c in capabilities)
            {
                CapabilityCardControl cc = new CapabilityCardControl(c, false);
                cc.Margin = new Thickness(0, 0, 0, 5);
                capabilityList.Items.Add(cc);
            }
        }

        public void DisableVoting()
        {
            for(int i=0; i < capabilityList.Items.Count; i++)
            {
                this.Dispatcher.Invoke(() =>
                {
                    ((CapabilityCardControl)capabilityList.Items[i]).voteButton.IsEnabled = false;
                });
            }
        }

        public void EnableVoting()
        {
            for (int i = 0; i < capabilityList.Items.Count; i++)
            {
                ((CapabilityCardControl)capabilityList.Items[i]).voteButton.IsEnabled = true;
            }
        }

        // handler for vote events received from CapabiltyCardControl
        private void PlayerVoteEventHandler(object sender, RoutedEventArgs e)
        {
            VoteCommonTasks(e.OriginalSource as CapabilityCardControl);

            // raise event to be handled on MainWindow
            RaiseEvent(new RoutedEventArgs(PlayerCapabilityPanel.VoteReceivedEvent));
        }
        // handler for vote events received from CapabiltyCardControl
        private void PlayerUnvoteEventHandler(object sender, RoutedEventArgs e)
        {
            VoteCommonTasks(e.OriginalSource as CapabilityCardControl);

            // raise event to be handled on MainWindow
            RaiseEvent(new RoutedEventArgs(PlayerCapabilityPanel.UnvoteReceivedEvent));
        }
        private void VoteCommonTasks(CapabilityCardControl ccc)
        {
            capabilities[ccc.CardCapability.CapabilityId].Votes = ccc.CardCapability.Votes;
            capabilities[ccc.CardCapability.CapabilityId].OwnerDidVote = ccc.CardCapability.OwnerDidVote;

            // store a reference to the sending capabiity ID so MainWindow can grab it
            VotedCapabilityIndex = ccc.CardCapability.CapabilityId;
        }


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
                        if (capabilities[counter].Votes > capabilities[counter - 1].Votes)
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
            typeof(RoutedEventHandler), typeof(PlayerCapabilityPanel));

        // Create RoutedEventHandler
        public event RoutedEventHandler VoteReceived
        {
            add { AddHandler(VoteReceivedEvent, value); }
            remove { RemoveHandler(VoteReceivedEvent, value); }
        }


        // event declaration for event that gets bubbled up and caught by MainWindow
        public static readonly RoutedEvent UnvoteReceivedEvent =
            EventManager.RegisterRoutedEvent("UnvoteReceivedEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PlayerCapabilityPanel));

        // Create RoutedEventHandler
        public event RoutedEventHandler UnvoteReceived
        {
            add { AddHandler(UnvoteReceivedEvent, value); }
            remove { RemoveHandler(UnvoteReceivedEvent, value); }
        }


    }
}
