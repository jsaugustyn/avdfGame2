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
    /// Interaction logic for PaletteContentWindow.xaml
    /// </summary>
    public partial class PaletteContentWindow : Window
    {
        public PaletteContentWindow()
        {
            InitializeComponent();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            // raise even to pass data
            DialogResult = true;
        }

        private void contentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            currChars.Content = contentTextBox.Text.Length;
            if( contentTextBox.Text.Length > int.Parse(maxChars.Content.ToString()))
            {
                currChars.Foreground = Brushes.Red;
            }
            else
            {
                currChars.Foreground = Brushes.Black;

            }

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
