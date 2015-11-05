using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace HCITargetSelection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private String[] images = new String[] {
            "btn_1",
            "btn_2",
            "btn_3",
            "btn_4",
            "btn_5",
            "btn_6",
            "btn_7",
            "btn_8",
            "btn_9",
            "btn_10"
        };
        List<Image> toolbarImages = new List<Image>();

        public int targetsChosen = 0;
        public Stopwatch timer { get; set; }
        public List<ClickPoint> clicks = new List<ClickPoint>();
        Dictionary<string, double> defaultTopMargins = new Dictionary<string, double>();
        private Boolean ClickHandled = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PresentationSource source = PresentationSource.FromVisual(this);

            if (source != null)
            {
                //wpf by default scales. So our 50 pixel images became 75px images...
                Transform t = new ScaleTransform(1 / source.CompositionTarget.TransformToDevice.M11, 1 / source.CompositionTarget.TransformToDevice.M22);
                this.deco.LayoutTransform = t;
            }

            SetTargetImage("white");
            timer = new Stopwatch();
            timer.Start();


            toolbarImages.Add(this.btn_1);
            toolbarImages.Add(this.btn_2);
            toolbarImages.Add(this.btn_3);
            toolbarImages.Add(this.btn_4);
            toolbarImages.Add(this.btn_5);
            toolbarImages.Add(this.btn_6);
            toolbarImages.Add(this.btn_7);
            toolbarImages.Add(this.btn_8);
            toolbarImages.Add(this.btn_9);
            toolbarImages.Add(this.btn_10);

            foreach(Image img in toolbarImages)
            {
                defaultTopMargins.Add(img.Source.ToString(), img.Margin.Top);
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //we use this to not insert a click to the list on the window action
            ClickHandled = true;

            Image img = (Image)sender;
            int slashindex = img.Source.ToString().LastIndexOf("/") + 1;
            string btnType = img.Source.ToString().Substring(slashindex, img.Source.ToString().Length - slashindex);
            btnType = btnType.Replace(".jpg", "");
            ClickPointType type;
            string colour = null;
            Boolean exit = false;

            if(btnType == "white" && GetTargetImage() == "white") //clicked on white when supposed too.
            {
                Random r = new Random();
                SetTargetImage(images[r.Next(0, 9)]);

                type = ClickPointType.White;
                colour = btnType;

                start.Text = "Click the corresponding image from above >";
            }
            else if (GetTargetImage() == btnType) //click on the correct coloured image
            {
                SetTargetImage("white");
                targetsChosen++;
                this.counter.Visibility = Visibility.Visible;
                this.counter.Text = "You have performed " + targetsChosen + "/50 selections.";

                type = ClickPointType.ToolbarButton;
                colour = btnType;
                if(targetsChosen >= 10)
                {
                    exit = true;
                }
                start.Visibility = Visibility.Hidden;
            }
            else
            {
                type = ClickPointType.Miss;
            }
            clicks.Add(new ClickPoint(timer, Mouse.GetPosition(this.deco).X, Mouse.GetPosition(this.deco).Y, type, colour));

            if(exit)
            {
                ExportData();
                this.Close();
            }
        }

        private string GetTargetImage()
        {
            int slashindex = target.Source.ToString().LastIndexOf("/") + 1;
            string btnType = target.Source.ToString().Substring(slashindex, target.Source.ToString().Length - slashindex);
            btnType = btnType.Replace(".jpg", "");
            return btnType;
        }
        private void SetTargetImage(string imgName)
        {
            this.target.Source = new BitmapImage(new Uri(@"/HCITargetSelection;component/images/" + imgName + ".jpg", UriKind.Relative));
            this.target.Margin = new Thickness(0, 40, 0, 0);
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!ClickHandled)
            {
                clicks.Add(new ClickPoint(timer, Mouse.GetPosition(this.deco).X, Mouse.GetPosition(this.deco).Y, ClickPointType.Miss));
            }
            ClickHandled = false;
            //System.Diagnostics.Debugger.Break();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
#if DYNAMIC
            int maxSize = 200;
            int minSize = 50;
            double mouseX = e.GetPosition(this.deco).X;
            double mouseY = e.GetPosition(this.deco).Y;
            int radius = (int)(Application.Current.MainWindow.ActualHeight / 5*3);

            double offset = 0;
            this.imgParent.Height = 500;
            this.imgParent.Margin = new Thickness();
            for (int i = 0; i < toolbarImages.Count; i++)
            {
                Image img = toolbarImages[i];

                img.Width = 50;
                img.Height = 50;

                Point t = img.TransformToAncestor(this.deco).Transform(new Point(0, 0));
                double x = t.X + img.Width / 2;
                double y = t.Y + img.Height / 2;
                double distance = Math.Sqrt(Math.Pow(x - mouseX, 2) + Math.Pow(y - mouseY, 2));
                if (distance < radius)
                {
                    img.Width = minSize + (maxSize - minSize) * (Math.Max(radius - distance, 0) / radius);
                    img.Height = minSize + (maxSize - minSize) * (Math.Max(radius - distance, 0) / radius);
                }


                Thickness m = img.Margin;
                m.Top = defaultTopMargins[img.Source.ToString()] + offset;
                m.Left = -1 * (img.Width - minSize);
                img.Margin = m;

                offset += img.Height - minSize;

                this.imgParent.Height += img.Height - minSize;
            }
#endif
        }
        private string SerializeList()
        {
            string s = "Button, Time(Milliseconds)\r\n";
            double start = 0;
            foreach(ClickPoint p in clicks)
            {
                if(p.Type == ClickPointType.White)
                {
                    start = p.MilliSeconds;
                } else if(p.Type == ClickPointType.ToolbarButton)
                {
                    s += p.Button + ", " + (p.MilliSeconds - start);
                    s += "\r\n";
                }
            }
            return s;
        }
        protected void ExportData()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Comma Separated List (.csv)|*.csv";
            sfd.ShowDialog();
            string filename = sfd.FileName;
            string contents = SerializeList();
            if(String.IsNullOrWhiteSpace(filename))
            {
                MessageBoxResult m = MessageBox.Show("Are you sure you would like to continue without saving?", "Are you sure?", MessageBoxButton.YesNo);
                if(m == MessageBoxResult.No)
                {
                    ExportData();
                }
                return;
            }
            try
            {
                File.WriteAllText(filename, contents);
            } catch (IOException)
            {
                MessageBox.Show("The file could not be saved. If the file is being used in another program, please create a new file.");
                ExportData();
            }
        }
    }
}
