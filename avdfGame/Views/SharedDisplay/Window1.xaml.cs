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
using System.Windows.Shapes;

namespace avdfGame
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        public void ToggleView(string view)
        {
            sharedVignettePanel.Visibility = Visibility.Hidden;
            sharedCapabilityPanel.Visibility = Visibility.Hidden;
            sharedAssessmentPanel.Visibility = Visibility.Hidden;

            switch (view)
            {
                case "vignette":
                    sharedVignettePanel.Visibility = Visibility.Visible;
                    vignetteButton.IsChecked = true;
                    break;
                case "capability":
                    sharedCapabilityPanel.Visibility = Visibility.Visible;
                    capabilityButton.IsChecked = true;
                    break;
                case "assessment":
                    sharedAssessmentPanel.Visibility = Visibility.Visible;
                    assessmentButton.IsChecked = true;
                    break;
                default:
                    break;
            }
        }

        public void AddNarrative(NarrativeWidget nw)
        {
            //sharedNarrativeViewPanel.narrativeWrapPanel.Children.Add(nw);
        }

        public void SetVignette(Brush img)
        {
            sharedVignettePanel.vignetteInkCanvas.Background = img;
        }

        private void rb_Click(object sender, RoutedEventArgs e)
        {
            switch(((RadioButton)e.OriginalSource).Content)
            {
                case "VIGNETTE":
                    ToggleView("vignette");
                    break;
                case "CAPABILITIES":
                    ToggleView("capability");
                    break;
                case "NARRATIVES":
                    ToggleView("narrative");
                    break;
                case "ASSESSMENT":
                    ToggleView("assessment");
                    break;
                default:
                    break;
            }
        }

        private void closeAllButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    this.DragMove();
                }
        }

        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                this.ResizeMode = ResizeMode.CanResizeWithGrip;
            }
            else
            {
                this.ResizeMode = ResizeMode.NoResize;
                this.WindowState = WindowState.Maximized;
            }

        }
    }
}
