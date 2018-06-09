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
    /// Interaction logic for SharedCapabilityViewControl.xaml
    /// </summary>
    public partial class SharedCapabilityViewControl : UserControl
    {
        public SharedCapabilityViewControl()
        {
            InitializeComponent();
        }

        private List<Capability> capabilities;

        public void DisplayCapabilities(List<Capability> caps)
        {

            capabilities = caps;

            // make controls
            capabilityStackPanel.Children.Clear();
            foreach (Capability c in capabilities)
            {
                CapabilityCardControl cc = new CapabilityCardControl(c, true);
                cc.capabilityName.Text = c.CapabilityName;
                cc.Margin = new Thickness(0, 0, 0, 5);
                capabilityStackPanel.Children.Add(cc);
            }

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

    }
}
