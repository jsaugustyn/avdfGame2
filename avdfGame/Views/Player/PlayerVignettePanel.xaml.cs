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
using System.IO;
using Newtonsoft.Json;

namespace avdfGame
{
    /// <summary>
    /// Interaction logic for PlayerVignettePanel.xaml
    /// </summary>
    public partial class PlayerVignettePanel : UserControl
    {
        private Object thisLock = new Object();

        public PlayerVignettePanel()
        {
            InitializeComponent();
            LoadVignettes();

            SetVignette(0);
        }

        public void SetVignette(int vi)
        {
            string fname;
            ImageBrush background = null;

            fname = System.Windows.Forms.Application.StartupPath + @"\Assets\avdf-scenarios\" + vignettes[vi].VignetteImage;

            BitmapImage bitmapImage;
            bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = File.OpenRead(fname);
            bitmapImage.EndInit();

            background = new ImageBrush();
            background.Stretch = Stretch.Uniform;
            background.ImageSource = bitmapImage;

            try
            {
                this.vignetteViewerControl.vignetteInkCanvas.Background = background;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        private List<VignetteSummary> vignettes = null;

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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

    }
}
