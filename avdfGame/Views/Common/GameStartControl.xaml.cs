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
    /// Interaction logic for GameStartControl.xaml
    /// </summary>
    public partial class GameStartControl : UserControl
    {
        //MainWindow parent;

        public GameStartControl()
        {
            InitializeComponent();

            //parent = w;
        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent HostJoinedEvent =
            EventManager.RegisterRoutedEvent("HostJoinedEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(GameStartControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler HostJoined
        {
            add { AddHandler(HostJoinedEvent, value); }
            remove { RemoveHandler(HostJoinedEvent, value); }
        }

        // When the Join as Host button on the User Control is clicked, use RaiseEvent to fire the 
        // Custom Routed Event
        private void hostJoinButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the custom routed event, this fires the event from the UserControl
            RaiseEvent(new RoutedEventArgs(GameStartControl.HostJoinedEvent));
        }


        // Create RoutedEvent for PLAYER
        public static readonly RoutedEvent PlayerJoinedEvent =
            EventManager.RegisterRoutedEvent("PlayerJoinedEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(GameStartControl));

        // Create RoutedEventHandler
        // This adds the Custom Routed Event to the WPF Event System and allows it to be 
        // accessed as a property from within xaml if you so desire
        public event RoutedEventHandler PlayerJoined
        {
            add { AddHandler(PlayerJoinedEvent, value); }
            remove { RemoveHandler(PlayerJoinedEvent, value); }
        }

        // When the Join as Host button on the User Control is clicked, use RaiseEvent to fire the 
        // Custom Routed Event
        private void playerJoinButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the custom routed event, this fires the event from the UserControl
            RaiseEvent(new RoutedEventArgs(GameStartControl.PlayerJoinedEvent));
        }


    }
}
