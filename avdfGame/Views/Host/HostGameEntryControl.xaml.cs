using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for HostStartScreen.xaml
    /// </summary>
    public partial class HostGameEntryControl : UserControl
    {
        public ObservableCollection<string> playerNames;
        private Object thisLock = new Object();

        public string SessionName;

        public HostGameEntryControl()
        {
            InitializeComponent();
            playerNames = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(playerNames, thisLock);

            lock (thisLock)
            {
                //playerNames.Add("test");
            }
            playersListBox.ItemsSource = playerNames;

        }

        public string AddName(string name)
        {
            try
            {
                lock (thisLock)
                {
                    playerNames.Add(name);
                }
                return (String.Format("player {0} added", name));
            }
            catch (Exception e)
            {
                return (e.ToString());
            }

        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent StartSessionEvent =
            EventManager.RegisterRoutedEvent("StartSessionEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(HostGameEntryControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler StartSession
        {
            add { AddHandler(StartSessionEvent, value); }
            remove { RemoveHandler(StartSessionEvent, value); }
        }

        private void startSessionButton_Click(object sender, RoutedEventArgs e)
        {
            SessionName = sessionNameTextBox.Text;
            RaiseEvent(new RoutedEventArgs(HostGameEntryControl.StartSessionEvent));
        }

        private void sessionNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)e.OriginalSource).Text.Length > 0)
                startSessonButton.IsEnabled = true;
            else
                startSessonButton.IsEnabled = false;
        }
    }

}
