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
    /// Interaction logic for PlayerNarrativeCapabilityChoicePanel.xaml
    /// </summary>
    public partial class PlayerNarrativeCapabilityChoicePanel : UserControl
    {
        public PlayerNarrativeCapabilityChoicePanel()
        {
            InitializeComponent();
        }

        private List<Capability> capabilityList;
        public Capability ChosenCapability;

        public void SetCapabilities(List<Capability> capabilities)
        {
            capabilityList = new List<Capability>();
            capabilityList = capabilities;

            foreach(Capability c in capabilityList)
            {
                capabilitiesListBox.Items.Add(c.CapabilityName);
            }
        }

        private void chooseCapabilityButton_Click(object sender, RoutedEventArgs e)
        {
            ChosenCapability = new Capability();
            ChosenCapability = capabilityList[capabilitiesListBox.SelectedIndex];

            // raise event
            RaiseEvent(new RoutedEventArgs(PlayerNarrativeCapabilityChoicePanel.CapabilityChosenEvent));
        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent CapabilityChosenEvent =
            EventManager.RegisterRoutedEvent("CapabilityChosenEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PlayerNarrativeCapabilityChoicePanel));

        // Create RoutedEventHandler
        public event RoutedEventHandler CapabilityChosen
        {
            add { AddHandler(CapabilityChosenEvent, value); }
            remove { RemoveHandler(CapabilityChosenEvent, value); }
        }


        private void capabilitiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chooseCapabilityButton.IsEnabled = true;
        }
    }
}
