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
    /// Interaction logic for CapabilityCardControl.xaml
    /// </summary>
    public partial class CapabilityCardControl : UserControl
    {
        public CapabilityCardControl(Capability c, bool isHost)
        {
            InitializeComponent();

            CardCapability = new Capability();
            CardCapability = c;

            capabilityName.Text = String.Format("{0} ({1})", CardCapability.CapabilityName, CardCapability.CapabilityId);
            //voteTotal.Content = CardCapability.Votes;

            if(isHost)
            {
                voteButton.IsEnabled = false;
            }
            else
            {
                if (c.OwnerDidVote)
                    voteButton.Content = FindResource("Vote");
                else
                    voteButton.Content = FindResource("NoVote");
            }

        }

        public Capability CardCapability;

        private void infoButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void SetVoteButtonEnabled(bool isenabled)
        {
            voteButton.IsEnabled = isenabled;
        }

        private void voteButton_Click(object sender, RoutedEventArgs e)
        {
            if (voteButton.Content == FindResource("NoVote"))
            {
                //CardCapability.Votes++;
                //voteTotal.Content = CardCapability.Votes.ToString();
                CardCapability.OwnerDidVote = true;
                voteButton.Content = FindResource("Vote");
                // Raise the custom routed event, this fires the event from the UserControl
                RaiseEvent(new RoutedEventArgs(CapabilityCardControl.VoteEvent));

            }
            else
            {
                //CardCapability.Votes--;
                //CardCapability.Votes = Math.Max(0, CardCapability.Votes);
                //voteTotal.Content = CardCapability.Votes.ToString();
                CardCapability.OwnerDidVote = false;
                voteButton.Content = FindResource("NoVote");
                // Raise the custom routed event, this fires the event from the UserControl
                RaiseEvent(new RoutedEventArgs(CapabilityCardControl.UnvoteEvent));
            }



        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent VoteEvent =
            EventManager.RegisterRoutedEvent("VoteEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(CapabilityCardControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler Vote
        {
            add { AddHandler(VoteEvent, value); }
            remove { RemoveHandler(VoteEvent, value); }
        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent UnvoteEvent =
            EventManager.RegisterRoutedEvent("UnvoteEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(CapabilityCardControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler Unvote
        {
            add { AddHandler(UnvoteEvent, value); }
            remove { RemoveHandler(UnvoteEvent, value); }
        }


    }
}
