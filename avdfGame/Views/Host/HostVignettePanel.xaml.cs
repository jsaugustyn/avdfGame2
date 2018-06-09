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
    /// Interaction logic for HostVignettePanel.xaml
    /// </summary>
    public partial class HostVignettePanel : UserControl
    {
        public HostVignettePanel()
        {
            InitializeComponent();

            AddHandler(VignetteXpsControl.ChooseVignetteEvent, new RoutedEventHandler(ChooseVignetteEventHandler));
        }

        private void ChooseVignetteEventHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                string fname = System.Windows.Forms.Application.StartupPath + @"\Assets\avdf-scenarios\" + ((VignetteXpsControl)e.OriginalSource).Vignette.VignetteImage;

                BitmapImage bitmapImage;
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = System.IO.File.OpenRead(fname);
                bitmapImage.EndInit();

                ImageBrush background = new ImageBrush();
                background.Stretch = Stretch.Uniform;
                background.ImageSource = bitmapImage;

                hostVignetteViewerControl.vignetteInkCanvas.Background = background;
                ToggleControls();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ToggleControls()
        {
            if (hostVignetteSelectionControl.IsVisible)
            {
                hostVignetteSelectionControl.Visibility = Visibility.Hidden;
                hostVignetteViewerControl.Visibility = Visibility.Visible;
                vignetteControlToggleButton.Content = "CHANGE VIGNETTE";
                vignetteControlToggleButton.Visibility = Visibility.Visible;
            }
            else
            {
                hostVignetteSelectionControl.Visibility = Visibility.Visible;
                hostVignetteViewerControl.Visibility = Visibility.Hidden;
                vignetteControlToggleButton.Content = "VIEW VIGNETTE";
                vignetteControlToggleButton.Visibility = Visibility.Hidden;
            }
        }

        private void vignetteControlToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleControls();
        }
    }
}
