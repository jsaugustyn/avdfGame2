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
    /// Interaction logic for PlayerNarrativePanel.xaml
    /// </summary>
    public partial class PlayerNarrativePanel : UserControl
    {
        public PlayerNarrativePanel()
        {
            InitializeComponent();
        }

        public void ClearContent()
        {
            UIElementCollection collection = this.narrativeControl.controlsStackPanel.Children;
            foreach (UIElement uie in collection)
            {
                ((NarrativeElementControl)uie).contentTextBox.Text = "";
            }

        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            bool go = true;

            // validate data
            UIElementCollection collection = this.narrativeControl.controlsStackPanel.Children;
            foreach (UIElement uie in collection)
            {
                string tb = ((NarrativeElementControl)uie).contentTextBox.Text;
                if(tb == "" || tb.Length > int.Parse(((NarrativeElementControl)uie).MaxCharacters))
                {
                    go = false;
                    // display some kind of message
                    MessageBox.Show("Error on form validation");
                    break;
                }
            }

            // raise submission event
            if(go)
            {
                RaiseEvent(new RoutedEventArgs(PlayerNarrativePanel.NarrativeSubmitEvent));
            }

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            // clear form
            //IEnumerable<NarrativeElementControl> collection = this.LogicalChildren.OfType<NarrativeElementControl>();
            UIElementCollection collection = this.narrativeControl.controlsStackPanel.Children;
            foreach(UIElement uie in collection)
            {
                ((NarrativeElementControl)uie).contentTextBox.Clear();
            }

            // raise cancel event, which will clear the form and return to capability choice panel
            RaiseEvent(new RoutedEventArgs(PlayerNarrativePanel.NarrativeCancelEvent));

        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent NarrativeSubmitEvent =
            EventManager.RegisterRoutedEvent("NarrativeSubmitEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PlayerNarrativePanel));

        // Create RoutedEventHandler
        public event RoutedEventHandler NarrativeSubmit
        {
            add { AddHandler(NarrativeSubmitEvent, value); }
            remove { RemoveHandler(NarrativeSubmitEvent, value); }
        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent NarrativeCancelEvent =
            EventManager.RegisterRoutedEvent("NarrativeCancelEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PlayerNarrativePanel));

        // Create RoutedEventHandler
        public event RoutedEventHandler NarrativeCancel
        {
            add { AddHandler(NarrativeCancelEvent, value); }
            remove { RemoveHandler(NarrativeCancelEvent, value); }
        }


    }
}
