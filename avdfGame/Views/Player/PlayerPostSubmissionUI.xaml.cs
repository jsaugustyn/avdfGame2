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
    /// Interaction logic for PlayerPostSubmissionUI.xaml
    /// </summary>
    public partial class PlayerPostSubmissionUI : UserControl
    {
        public PlayerPostSubmissionUI()
        {
            InitializeComponent();
        }

        private void newNarrativeButton_Click(object sender, RoutedEventArgs e)
        {
            // raise event that will be caught on playerGameControl
            // raise an event to be caught on MainWindow that will save data and initiate a send to host
            RaiseEvent(new RoutedEventArgs(PlayerPostSubmissionUI.NewNarrativeEvent));

        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent NewNarrativeEvent =
            EventManager.RegisterRoutedEvent("NewNarrativeEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PlayerPostSubmissionUI));

        // Create RoutedEventHandler
        public event RoutedEventHandler NewNarrative
        {
            add { AddHandler(NewNarrativeEvent, value); }
            remove { RemoveHandler(NewNarrativeEvent, value); }
        }

    }
}
