using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for PaletteControl.xaml
    /// </summary>
    public partial class PaletteControl : UserControl
    {
        public PaletteControl()
        {
            InitializeComponent();
            nextUpdate = System.DateTime.Now;

            this.Data = new List<PaletteItemData>();
            this.Thumbs = new List<Thumb>();

            this.IsReadOnly = false;

            quadrants = new List<PaletteQuadrant>();
            quadrants.Add(new PaletteQuadrant("ENABLING S&T", "#F2C057"));
            quadrants.Add(new PaletteQuadrant("BENEFITS", "#486824"));
            quadrants.Add(new PaletteQuadrant("DESIGN FEATURES", "#D13525"));
            quadrants.Add(new PaletteQuadrant("RISKS", "#4C3F54"));

            assumptionBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(quadrants[0].HexColor));
            benefitBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(quadrants[1].HexColor));
            riskBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(quadrants[2].HexColor));
            questionBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(quadrants[3].HexColor));

        }

        public List<PaletteItemData> Data;
        public PaletteItemData NewestItem { get; set; }
        public List<Thumb> Thumbs;

        public string PlayerName { get; set; }
        public int CapabilityIndex { get; set; }
        public bool IsReadOnly { get; set; }

        private static DateTime nextUpdate;
        private bool IsDragging;
        private List<PaletteQuadrant> quadrants;

        SolidColorBrush assumptionBrush;
        SolidColorBrush benefitBrush;
        SolidColorBrush questionBrush;
        SolidColorBrush riskBrush;

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if(!IsReadOnly)
            {
                IsDragging = true;

                UIElement thumb = e.Source as UIElement;
                Thumb th = ((Thumb)thumb);

                bool go = false;
                Point point;

                if (th.Tag.ToString() == quadrants[0].Name)
                {
                    point = Mouse.GetPosition(assumptionsPolygon);
                    if (assumptionsPolygon.RenderedGeometry.FillContains(point))
                    {
                        go = true;
                    }
                }
                else if (th.Tag.ToString() == quadrants[1].Name)
                {
                    point = Mouse.GetPosition(benefitsPolygon);
                    if (benefitsPolygon.RenderedGeometry.FillContains(point))
                    {
                        go = true;
                    }
                }
                else if (th.Tag.ToString() == quadrants[2].Name)
                {
                    point = Mouse.GetPosition(questionsPolygon);
                    if (questionsPolygon.RenderedGeometry.FillContains(point))
                    {
                        go = true;
                    }
                }
                else if (th.Tag.ToString() == quadrants[3].Name)
                {
                    point = Mouse.GetPosition(risksPolygon);
                    if (risksPolygon.RenderedGeometry.FillContains(point))
                    {
                        go = true;
                    }
                }

                if (go && nextUpdate <= DateTime.Now)
                {
                    Canvas.SetLeft(thumb, Canvas.GetLeft(thumb) + e.HorizontalChange);
                    Canvas.SetTop(thumb, Canvas.GetTop(thumb) + e.VerticalChange);

                    // update data
                    Data[Thumbs.IndexOf(th)].Position = new Point(Canvas.GetLeft(thumb) + (th.Width / 2), Canvas.GetTop(thumb) + (th.Height / 2));
                    UpdateScore(Data[Thumbs.IndexOf(th)]);

                    nextUpdate = DateTime.Now.AddMilliseconds(20);
                }


            }
        }

        // ADDS AN ITEM THAT WAS ALREADY CREATED ON THE SERVER, WITH ITS OWN ID
        public void AddItemFromServer(PaletteItemData pid)
        {
            // code from current AddItem function
            Thumb tmbDragThumb = new Thumb();
            tmbDragThumb.Width = 15;
            tmbDragThumb.Height = 15;

            tmbDragThumb.DragDelta += new DragDeltaEventHandler(Thumb_DragDelta);
            tmbDragThumb.DragCompleted += new DragCompletedEventHandler(Thumb_DragCompleted);
            tmbDragThumb.MouseDoubleClick += new MouseButtonEventHandler(Thumb_MouseDown);

            ControlTemplate template = new ControlTemplate();
            var fec = new FrameworkElementFactory(typeof(Ellipse));
            fec.SetValue(Ellipse.FillProperty, Brushes.White);

            if(pid.Quadrant == quadrants[0].Name)
            {
                fec.SetValue(Ellipse.StrokeProperty, assumptionBrush);
                fec.SetValue(Ellipse.FillProperty, assumptionBrush);
            }
            else if(pid.Quadrant == quadrants[1].Name)
            {
                fec.SetValue(Ellipse.StrokeProperty, benefitBrush);
                fec.SetValue(Ellipse.FillProperty, benefitBrush);
            }
            else if (pid.Quadrant == quadrants[2].Name)
            {
                fec.SetValue(Ellipse.StrokeProperty, questionBrush);
                fec.SetValue(Ellipse.FillProperty, questionBrush);
            }
            else if (pid.Quadrant == quadrants[3].Name)
            {
                fec.SetValue(Ellipse.StrokeProperty, riskBrush);
                fec.SetValue(Ellipse.FillProperty, riskBrush);
            }

            tmbDragThumb.Tag = pid.Quadrant;
            template.VisualTree = fec;
            tmbDragThumb.Template = template;

            Canvas.SetLeft(tmbDragThumb, pid.Position.X - (tmbDragThumb.Width / 2));
            Canvas.SetTop(tmbDragThumb, pid.Position.Y - (tmbDragThumb.Width / 2));

            tmbDragThumb.ToolTip = pid.Content;
            mainCanvas.Children.Add(tmbDragThumb);

            // ADD TO DATA LISTS
            //pid.ItemId = Data.Count; // NOT SURE THIS WILL STAY IN SYNC...SEE WHO CALLS THIS FUNCTION
            Data.Add(pid);
            Thumbs.Add(tmbDragThumb);

            UpdateScore(pid);

        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if(IsDragging)
            {
                IsDragging = false;
                NewestItem = Data[Thumbs.IndexOf(sender as Thumb)];

                // UPDATE SCORE
                UpdateScore(Data[Thumbs.IndexOf(sender as Thumb)]);

                // RAISE EVENT TO UPDATE DATA RECORD ON PLAYER AND SEND UPDATE MESSAGE TO HOST WITH NEW POSITION AND SCORE
                RaiseEvent(new RoutedEventArgs(PaletteControl.PaletteItemDragCompletedEvent));

            }
        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent PaletteItemDragCompletedEvent =
            EventManager.RegisterRoutedEvent("PaletteItemDragCompletedEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PaletteControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler PaletteItemDragCompleted
        {
            add { AddHandler(PaletteItemDragCompletedEvent, value); }
            remove { RemoveHandler(PaletteItemDragCompletedEvent, value); }
        }


        public void BuildPaletteFromData(List<PaletteItemData> data)
        {
            // remove existing thumbs
            foreach(Thumb th in Thumbs)
            {
                mainCanvas.Children.Remove(th);                
            }
            
            // set new data
            Data = data;
            Thumbs = new List<Thumb>();

            // add thumbs
            foreach (PaletteItemData pid in Data)
            {
                MakeThumb(pid);
                //mainCanvas.Children.Remove(Thumbs[pi.ItemId]);
            }

        }

        private void MakeThumb(PaletteItemData pid)
        {
            Thumb tmbDragThumb = new Thumb();
            tmbDragThumb.Width = 15;
            tmbDragThumb.Height = 15;

            tmbDragThumb.DragDelta += new DragDeltaEventHandler(Thumb_DragDelta);
            tmbDragThumb.DragCompleted += new DragCompletedEventHandler(Thumb_DragCompleted);
            tmbDragThumb.MouseDoubleClick += new MouseButtonEventHandler(Thumb_MouseDown);

            ControlTemplate template = new ControlTemplate();
            var fec = new FrameworkElementFactory(typeof(Ellipse));
            fec.SetValue(Ellipse.FillProperty, Brushes.White);

            if(pid.Quadrant == quadrants[0].Name)
            {
                fec.SetValue(Ellipse.StrokeProperty, assumptionBrush);
                fec.SetValue(Ellipse.FillProperty, assumptionBrush);
            }
            else if(pid.Quadrant == quadrants[1].Name)
            {
                fec.SetValue(Ellipse.StrokeProperty, benefitBrush);
                fec.SetValue(Ellipse.FillProperty, benefitBrush);
            }
            else if (pid.Quadrant == quadrants[2].Name)
            {
                fec.SetValue(Ellipse.StrokeProperty, questionBrush);
                fec.SetValue(Ellipse.FillProperty, questionBrush);
            }
            else if (pid.Quadrant == quadrants[3].Name)
            {
                fec.SetValue(Ellipse.StrokeProperty, riskBrush);
                fec.SetValue(Ellipse.FillProperty, riskBrush);
            }


            tmbDragThumb.Tag = pid.Quadrant;
            template.VisualTree = fec;
            tmbDragThumb.Template = template;
            tmbDragThumb.ToolTip = pid.Content;

            Canvas.SetLeft(tmbDragThumb, pid.Position.X - (tmbDragThumb.Width / 2));
            Canvas.SetTop(tmbDragThumb, pid.Position.Y - (tmbDragThumb.Width / 2));

            mainCanvas.Children.Add(tmbDragThumb);

            Thumbs.Add(tmbDragThumb);

        }

        public void AddItem(Polygon poly, MouseButtonEventArgs e)
        {
            if(!IsReadOnly)
            {
                PaletteItemData dt = new PaletteItemData();

                Thumb tmbDragThumb = new Thumb();
                tmbDragThumb.Width = 15;
                tmbDragThumb.Height = 15;

                tmbDragThumb.DragDelta += new DragDeltaEventHandler(Thumb_DragDelta);
                tmbDragThumb.DragCompleted += new DragCompletedEventHandler(Thumb_DragCompleted);
                tmbDragThumb.MouseDoubleClick += new MouseButtonEventHandler(Thumb_MouseDown);

                ControlTemplate template = new ControlTemplate();
                var fec = new FrameworkElementFactory(typeof(Ellipse));
                fec.SetValue(Ellipse.FillProperty, Brushes.White);

                switch (poly.Name)
                {
                    case "assumptionsPolygon":
                        dt.Quadrant = quadrants[0].Name;
                        fec.SetValue(Ellipse.StrokeProperty, assumptionBrush);
                        fec.SetValue(Ellipse.FillProperty, assumptionBrush);
                        break;
                    case "benefitsPolygon":
                        dt.Quadrant = quadrants[1].Name;
                        fec.SetValue(Ellipse.StrokeProperty, benefitBrush);
                        fec.SetValue(Ellipse.FillProperty, benefitBrush);
                        break;
                    case "questionsPolygon":
                        dt.Quadrant = quadrants[2].Name;
                        fec.SetValue(Ellipse.StrokeProperty, questionBrush);
                        fec.SetValue(Ellipse.FillProperty, questionBrush);
                        break;
                    case "risksPolygon":
                        dt.Quadrant = quadrants[3].Name;
                        fec.SetValue(Ellipse.StrokeProperty, riskBrush);
                        fec.SetValue(Ellipse.FillProperty, riskBrush);
                        break;
                    default:
                        break;
                }

                tmbDragThumb.Tag = dt.Quadrant;
                template.VisualTree = fec;
                tmbDragThumb.Template = template;

                Canvas.SetLeft(tmbDragThumb, e.GetPosition(mainCanvas).X - (tmbDragThumb.Width / 2));
                Canvas.SetTop(tmbDragThumb, e.GetPosition(mainCanvas).Y - (tmbDragThumb.Width / 2));

                dt.Position = e.GetPosition(mainCanvas);

                // get content
                PaletteContentWindow pc = new PaletteContentWindow();
                pc.quadrantName.Content = dt.Quadrant.ToUpper();
                //pc.Left = dt.WindowLeft;
                //pc.Top = dt.WindowTop;
                if (dt.Quadrant == quadrants[0].Name)
                {
                    pc.quadrantName.Background = assumptionBrush;
                }
                else if (dt.Quadrant == quadrants[1].Name)
                {
                    pc.quadrantName.Background = benefitBrush;
                }
                else if (dt.Quadrant == quadrants[2].Name)
                {
                    pc.quadrantName.Background = questionBrush;
                }
                else if (dt.Quadrant == quadrants[3].Name)
                {
                    pc.quadrantName.Background = riskBrush;
                }
                else
                {
                    pc.quadrantName.Background = Brushes.Black;
                }
                pc.ShowDialog();
                if (pc.DialogResult.HasValue && pc.DialogResult.Value)
                {
                    if (pc.DialogResult == true)
                    {
                        dt.Author = this.PlayerName;
                        dt.CapabilityIndex = this.CapabilityIndex;
                        dt.Content = pc.contentTextBox.Text;
                        dt.ItemId = GetRandomId(8);
                        tmbDragThumb.ToolTip = dt.Content;
                        mainCanvas.Children.Add(tmbDragThumb);

                        // ADD TO DATA LISTS
                        Data.Add(dt);
                        Thumbs.Add(tmbDragThumb);

                        pc.Close();
                    }
                    else
                    {
                        dt = null;
                    }
                }
                UpdateScore(dt);

                // raise event on MainWindow to send info to host
                NewestItem = new PaletteItemData();
                NewestItem = dt;
                RaiseEvent(new RoutedEventArgs(PaletteControl.NewPaletteItemEvent));


                Console.WriteLine("");

            }
        }

        private static Random random = new Random();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private string GetRandomId(int nchar)
        {
            return new string(Enumerable.Repeat(chars, nchar).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent NewPaletteItemEvent =
            EventManager.RegisterRoutedEvent("NewPaletteItemEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PaletteControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler NewPaletteItem
        {
            add { AddHandler(NewPaletteItemEvent, value); }
            remove { RemoveHandler(NewPaletteItemEvent, value); }
        }

        private void Thumb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PaletteItemData _data = new PaletteItemData();

            PaletteContentWindow pc = null;

            _data = Data[Thumbs.IndexOf((Thumb)sender)];
            pc = new PaletteContentWindow();
            pc.contentTextBox.Text = _data.Content;
            pc.quadrantName.Content = _data.Quadrant.ToUpper();
            pc.submitButton.Content = "UPDATE";

            if(IsReadOnly)
            {
                pc.submitButton.Content = "CLOSE";
                pc.cancelButton.Visibility = Visibility.Hidden;
                pc.contentTextBox.IsReadOnly = true;
            }

            if (_data.Quadrant == quadrants[0].Name)
            {
                pc.quadrantName.Background = assumptionBrush;
            }
            else if (_data.Quadrant == quadrants[1].Name)
            {
                pc.quadrantName.Background = benefitBrush;
            }
            else if (_data.Quadrant == quadrants[2].Name)
            {
                pc.quadrantName.Background = questionBrush;
            }
            else if (_data.Quadrant == quadrants[3].Name)
            {
                pc.quadrantName.Background = riskBrush;
            }
            else
            {
                pc.quadrantName.Background = Brushes.Black;
            }

            if(pc != null)
            {
                pc.ShowDialog();
                if (pc.DialogResult.HasValue && pc.DialogResult.Value)
                {
                    if (pc.DialogResult == true)
                    {
                        if(!IsReadOnly)
                        {
                            _data.Content = pc.contentTextBox.Text;
                            Thumbs[Data.IndexOf(_data)].ToolTip = _data.Content;
                        }
                        pc.Close();
                    }
                    else
                    {
                    }
                }

            }

            // Raise event to update data record on pplayer and UPDATE SERVER
            NewestItem = Data[Thumbs.IndexOf(sender as Thumb)];

            // RAISE EVENT TO UPDATE DATA RECORD ON PLAYER AND SEND UPDATE MESSAGE TO HOST WITH NEW POSITION AND SCORE
            RaiseEvent(new RoutedEventArgs(PaletteControl.PaletteItemContentUpdateEvent));

        }

        // Create RoutedEvent for HOST
        public static readonly RoutedEvent PaletteItemContentUpdateEvent =
            EventManager.RegisterRoutedEvent("PaletteItemContentUpdateEvent", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PaletteControl));

        // Create RoutedEventHandler
        public event RoutedEventHandler PaletteItemContentUpdate
        {
            add { AddHandler(PaletteItemContentUpdateEvent, value); }
            remove { RemoveHandler(PaletteItemContentUpdateEvent, value); }
        }

        private void UpdateScore(PaletteItemData pd)
        {
            Thumb th = Thumbs[Data.IndexOf(pd)];

            if (pd.Quadrant == quadrants[0].Name)
            {
                pd.Score = Canvas.GetLeft(th);
            }
            else if (pd.Quadrant == quadrants[1].Name)
            {
                pd.Score = Canvas.GetTop(th);
            }
            else if (pd.Quadrant == quadrants[2].Name)
            {
                pd.Score = Canvas.GetRight(th);
            }
            else if (pd.Quadrant == quadrants[3].Name)
            {
                pd.Score = Canvas.GetBottom(th);
            }
        }

        public void UpdatePaletteItem(PaletteItemData pid)
        {
            foreach(PaletteItemData p in Data)
            {
                if(p.ItemId == pid.ItemId)
                {
                    Data[Data.IndexOf(p)] = pid;

                    // update thumb
                    Thumb th = Thumbs[Data.IndexOf(p)];
                    Canvas.SetLeft(th, p.Position.X - (th.Width/2));
                    Canvas.SetTop(th, p.Position.Y - (th.Height / 2));
                    th.ToolTip = p.Content;
                    break;
                }
            }
        }

        private void Polygon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddItem(((Polygon)sender), e);
        }

    }

    public class PaletteItemData
    {
        public string ItemId { get; set; }
        public string Quadrant { get; set; } //
        public Point Position { get; set; } //
        public double WindowLeft {get; set; }
        public double WindowTop {get; set; }

        public int CapabilityIndex;

        public string Content { get; set; } // create only...need to do edit
        public string Author { get; set; } // but needs to get set by hostgame cntrol

        public double Score { get; set; } //
    }

    public class PaletteQuadrant
    {
        public string HexColor { get; set; }
        public string Name { get; set; }

        public PaletteQuadrant(string name, string color)
        {
            Name = name;
            HexColor = color;
        }
    }

}
