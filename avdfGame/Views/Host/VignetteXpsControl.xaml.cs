using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for VignetteXpsControl.xaml
    /// </summary>
    public partial class VignetteXpsControl : UserControl
    {
        private List<VignetteSummary> vignettes;
        public VignetteViewerControl vignetteViewer;
        public int VignetteIndex;
        public VignetteSummary Vignette { get; set; }

        public VignetteXpsControl()
        {
            InitializeComponent();
            Vignette = new VignetteSummary();

            LoadVignettes();
        }



        private void LoadVignettes()
        {
            vignettes = new List<VignetteSummary>();

            // load vignette file
            // load vignette options database
            string path = System.Windows.Forms.Application.StartupPath + @"\Assets\vignettes.json";

            // This text is added only once to the file.
            string json = "";
            try
            {
                // Create a file to write to.
                json = File.ReadAllText(path);

                vignettes = JsonConvert.DeserializeObject<List<VignetteSummary>>(json);

                foreach (VignetteSummary v in vignettes)
                {
                    vignetteListBox.Items.Add(v.VignetteName);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void luckyButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // select a random vignette from list
            Random rnd = new Random();
            vignetteListBox.SelectedIndex = rnd.Next(0, vignettes.Count);
        }

        private void vignetteListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // display info
            VignetteIndex = vignettes[vignetteListBox.SelectedIndex].VignetteId;

            // this is klugey - shluld refctor to get rid of temp variable
            VignetteSummary temp = vignettes[vignetteListBox.SelectedIndex];
            Vignette = temp;

            titleTextBox.Text = temp.VignetteName;
            descriptionTextBox.Text = temp.VignetteDescription;

            tagPanel.Children.Clear();
            foreach(string s in temp.VignetteTags)
            {
                Border b = new Border();
                b.BorderBrush = Brushes.DeepSkyBlue;
                b.BorderThickness = new Thickness(1);
                b.CornerRadius = new CornerRadius(3);
                b.Margin = new Thickness(5);

                Label lb = new Label();
                lb.HorizontalContentAlignment = HorizontalAlignment.Center;
                lb.VerticalContentAlignment = VerticalAlignment.Center;
                lb.MinWidth = 80;
                lb.FontSize = 12;
                lb.Padding = new Thickness(1);
                lb.Content = s.ToUpper();

                b.Child = lb;

                tagPanel.Children.Add(b);
            }

            useVignetteButton.IsEnabled = true;
        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent ChooseVignetteEvent =
            EventManager.RegisterRoutedEvent("ChooseVignetteEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(VignetteXpsControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler ChooseVignette
        {
            add { AddHandler(ChooseVignetteEvent, value); }
            remove { RemoveHandler(ChooseVignetteEvent, value); }
        }

        private void useVignetteButton_Click(object sender, RoutedEventArgs e)
        {
            // probably ought to raise an event so MainWindow knows that Window1 has been created and can close it appropriately
            RaiseEvent(new RoutedEventArgs(VignetteXpsControl.ChooseVignetteEvent));
        }
    }

    public class VignetteSummary
    {
        public int VignetteId { get; set; }
        public string VignetteName { get; set; }
        public string VignetteDescription { get; set; }
        public string VignetteImage { get; set; }
        public List<string> VignetteTags;

        public VignetteSummary()
        {
            VignetteTags = new List<string>();
        }
    }


}
