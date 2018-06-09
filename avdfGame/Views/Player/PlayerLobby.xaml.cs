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
    /// Interaction logic for HostLobby.xaml
    /// </summary>
    public partial class PlayerLobby : UserControl
    {

        public PlayerLobby()
        {
            InitializeComponent();
            serverSettingsPanel.Visibility = Visibility.Hidden;
        }

        private void showHideServerInfo_Click(object sender, RoutedEventArgs e)
        {
            if(serverSettingsPanel.IsVisible)
            {
                serverSettingsPanel.Visibility = Visibility.Hidden;
                showHideServerInfo.Content = "Show Server Settings";
            }
            else
            {
                serverSettingsPanel.Visibility = Visibility.Visible;
                showHideServerInfo.Content = "Hide Server Settings";
            }
        }

        // Create RoutedEvent
        public static readonly RoutedEvent JoinRoomEvent =
            EventManager.RegisterRoutedEvent("JoinRoomEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostLobby));

        // Create RoutedEventHandler
        public event RoutedEventHandler JoinRoom
        {
            add { AddHandler(JoinRoomEvent, value); }
            remove { RemoveHandler(JoinRoomEvent, value); }
        }

        private void joinRoomButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the custom routed event, this fires the event from the UserControl
            RaiseEvent(new RoutedEventArgs(PlayerLobby.JoinRoomEvent));
        }

    }
}
