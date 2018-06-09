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
    /// Interaction logic for HorizontalBarChartControl.xaml
    /// </summary>
    public partial class HorizontalBarChartControl : UserControl
    {
        public HorizontalBarChartControl()
        {
            InitializeComponent();
        }

        List<Capability> capabilities;

        public void InitializeBarChart(List<Capability> c)
        {
            barGrid.Children.Clear();
            //barStackPanel.Children.Clear();

            capabilities = new List<Capability>();
            capabilities = c;

            foreach(Capability cap in capabilities)
            {
                barGrid.RowDefinitions.Add(new RowDefinition());

                TextBlock tb = new TextBlock();
                tb.Text = cap.CapabilityName;
                tb.Width = 350;
                tb.TextWrapping = TextWrapping.WrapWithOverflow;
                tb.HorizontalAlignment = HorizontalAlignment.Right;
                tb.TextAlignment = TextAlignment.Right;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.FontSize = 14;
                tb.Margin = new Thickness(5,10,5,10);
                tb.Foreground = Brushes.LightGray;
                Grid.SetRow(tb, barGrid.RowDefinitions.Count - 1);
                Grid.SetColumn(tb, 0);
                barGrid.Children.Add(tb);

                Rectangle r = new Rectangle();
                r.Fill = Brushes.LightGray;
                r.Width = 1;
                Grid.SetRow(r, 0);
                Grid.SetRowSpan(r, barGrid.RowDefinitions.Count);
                Grid.SetColumn(r, 1);
                barGrid.Children.Add(r);

                Label voteLabel = new Label();
                voteLabel.Content = cap.Votes;
                voteLabel.Width = 30;
                voteLabel.FontSize = 11;
                voteLabel.Foreground = Brushes.LightGray;
                voteLabel.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(voteLabel, barGrid.RowDefinitions.Count-1);
                Grid.SetColumn(voteLabel, 2);

                double margin = 20;
                double barspacing = 2;
                double maxVotes = 18;
                double barMaxWidth = barGrid.ActualWidth - (tb.Width + r.Width + voteLabel.Width + margin);
                double barChunkWidth = (barMaxWidth / maxVotes) - barspacing;
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                Grid.SetRow(sp, barGrid.RowDefinitions.Count - 1);
                Grid.SetColumn(sp, 2);

                for(int i=0;i<cap.Votes;i++)
                {
                    Rectangle chunk = new Rectangle();
                    chunk.Width = barChunkWidth;
                    chunk.Height = 30;
                    chunk.Margin = new Thickness(0, 0, barspacing, 0);
                    chunk.Fill = Brushes.DeepSkyBlue;
                    sp.Children.Add(chunk);
                }
                sp.Children.Add(voteLabel);
                barGrid.Children.Add(sp);

                //HorizontalBarChartElement hbe = new HorizontalBarChartElement();
                //hbe.ElementText.Text = cap.CapabilityName;
                //hbe.Padding = new Thickness(0, 5, 0, 5);
                //barStackPanel.Children.Add(hbe);

                //double voteLabelWidth = 40.0f;
                //double maxVotes = 18; // temporarily hard-coded...this should be passed by parent
                //double barSpacing = 2;
                //double margin = 50;
                //double barWidth = ((barStackPanel.ActualWidth - (hbe.ElementText.Width + hbe.baseLine.Width + voteLabelWidth+margin)) / maxVotes) - barSpacing;
                //double barHeight = 20;
                //for(int i=0;i<cap.Votes;i++)
                //{
                //    Rectangle r = new Rectangle();
                //    r.Width = barWidth;
                //    r.Height = barHeight;
                //    r.Fill = Brushes.DeepSkyBlue;
                //    r.Margin = new Thickness(0, 0, barSpacing, 0);
                //    hbe.BarStack.Children.Add(r);
                //}
                //Label votelbl = new Label();
                //votelbl.Content = cap.Votes;
                //votelbl.Width = voteLabelWidth;
                //votelbl.FontSize = 12;
                //votelbl.Foreground = Brushes.LightGray;
                //votelbl.HorizontalContentAlignment = HorizontalAlignment.Left;
                //votelbl.VerticalContentAlignment = VerticalAlignment.Center;
                //hbe.BarStack.Children.Add(votelbl);
            }

        }
    }
}
