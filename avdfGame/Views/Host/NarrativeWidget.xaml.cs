﻿using System;
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
    /// Interaction logic for VignettePanelControl.xaml
    /// </summary>
    public partial class NarrativeWidget : UserControl
    {
        public NarrativeWidget(Narrative narr)
        {
            InitializeComponent();

            _narrative = new Narrative();
            _narrative = narr;

        }

        private Narrative _narrative;

        private void showDetailButton_Click(object sender, RoutedEventArgs e)
        {
            NarrativeDetailWindow nd = new NarrativeDetailWindow(_narrative);
            nd.ShowDialog();
        }
    }
}
