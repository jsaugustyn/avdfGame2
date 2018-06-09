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
    /// Interaction logic for NarrativeElementControl.xaml
    /// </summary>
    public partial class NarrativeElementControl : UserControl
    {
        public NarrativeElementControl()
        {
            InitializeComponent();
        }

        public string ElementName
        {
            get { return (string)GetValue(ElementNameProperty); }
            set { SetValue(ElementNameProperty, value); }
        }

        public string ElementDetail
        {
            get { return (string)GetValue(ElementDetailProperty); }
            set { SetValue(ElementDetailProperty, value); }
        }

        public string CharacterCount
        {
            get { return (string)GetValue(CharacterCountProperty); }
            set { SetValue(CharacterCountProperty, value); }
        }

        public string MaxCharacters
        {
            get { return (string)GetValue(MaxCharactersProperty); }
            set { SetValue(MaxCharactersProperty, value); }
        }

        public int TextHeight
        {
            get { return (int)GetValue(TextBoxHeightProperty); }
            set { SetValue(MaxCharactersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Property1.  
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementNameProperty = DependencyProperty.Register(
                  "ElementName",
                  typeof(string),
                  typeof(NarrativeElementControl),
                  new FrameworkPropertyMetadata(new PropertyChangedCallback(SetElementName)));

        private static void SetElementName(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as NarrativeElementControl).UpdateElement((string)e.NewValue);
        }

        private void UpdateElement(string newValue)
        {
            fieldName.Text = newValue;
        }



        public static readonly DependencyProperty ElementDetailProperty = DependencyProperty.Register(
          "ElementDetail",
          typeof(string),
          typeof(NarrativeElementControl),
          new FrameworkPropertyMetadata(new PropertyChangedCallback(SetElementDetail)));

        private static void SetElementDetail(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as NarrativeElementControl).UpdateElementDetail((string)e.NewValue);
        }

        private void UpdateElementDetail(string newValue)
        {
            fieldDetail.Text = newValue;
        }



        public static readonly DependencyProperty CharacterCountProperty = DependencyProperty.Register(
          "CharacterCount",
          typeof(string),
          typeof(NarrativeElementControl),
          new FrameworkPropertyMetadata(new PropertyChangedCallback(SetCharacterCount)));

        private static void SetCharacterCount(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as NarrativeElementControl).UpdateCharacterCount((string)e.NewValue);
        }

        private void UpdateCharacterCount(string newValue)
        {
            charCount.Text = newValue;
        }

        public static readonly DependencyProperty MaxCharactersProperty = DependencyProperty.Register(
          "MaxCharacters",
          typeof(string),
          typeof(NarrativeElementControl),
          new FrameworkPropertyMetadata(new PropertyChangedCallback(SetMaxCharacters)));

        private static void SetMaxCharacters(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as NarrativeElementControl).UpdateMaxCharacters((string)e.NewValue);
        }

        private void UpdateMaxCharacters(string newValue)
        {
            maxChars.Text = newValue;
        }


        public static readonly DependencyProperty TextBoxHeightProperty = DependencyProperty.Register(
  "TextHeight",
  typeof(int),
  typeof(NarrativeElementControl),
  new FrameworkPropertyMetadata(new PropertyChangedCallback(SetHeight)));

        private static void SetHeight(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as NarrativeElementControl).UpdateHeight((int)e.NewValue);
        }

        private void UpdateHeight(int newValue)
        {
            contentTextBox.Height = newValue;
        }

        private void contentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCharacterCount(contentTextBox.Text.Length.ToString());
            if(contentTextBox.Text.Length > int.Parse(MaxCharacters))
            {
                charCount.Foreground = Brushes.Red;
            }
            else
            {
                charCount.Foreground = Brushes.Black;
            }
        }
    }
}
