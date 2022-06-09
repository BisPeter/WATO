using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Threading;
using WATO;
using WpfApp.ViewModels;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The main view model, which is used for the view.
        /// </summary>
        private readonly WpfClientHandlerVM mainVM;


        public MainWindow()
        {

            InitializeComponent();

            this.mainVM = new WpfClientHandlerVM();
            this.DataContext = this.mainVM;

            /* Rectangle r1 = new Rectangle();
             r1.Width = 100;
             r1.Height = 100;
             Canvas.SetLeft(r1, 10);
             Canvas.SetTop(r1, 10);
             r1.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255/*0, 0, 0*//*));
             r1.StrokeThickness = 1;
             r1.Fill = new ImageBrush(new BitmapImage(new Uri(@"apple.jpg", UriKind.Relative)));

             myCanvas.Children.Add(r1);*/
        }

        private Bitmap Bi { get; set; } = new Bitmap(10000, 10000);


        /// <summary>
        /// This method will create a renderer, and connect the client to a server.
        /// </summary>
        /// <param name="sender">The sender button.</param>
        /// <param name="e">The arguments of the event.</param>
        private void ButtonClick(object sender, RoutedEventArgs e)

        {
            var random = new Random();

            // Create source.

            long t1 = DateTime.Now.Ticks;

            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < 10000; j++)
                {
                    if (random.NextDouble() > 0.5)
                        Bi.SetPixel(i, j, System.Drawing.Color.AntiqueWhite);
                    else
                        Bi.SetPixel(i, j, System.Drawing.Color.Green);
                }
            }

            long f1 = DateTime.Now.Ticks -t1;

            long t2 = DateTime.Now.Ticks;
            Parallel.For(0, 10000, i =>
            {
                Parallel.For(0, 10000, j =>
                {
                    if (random.NextDouble() > 0.5)
                        Bi.SetPixel(i, j, System.Drawing.Color.AntiqueWhite);
                    else
                        Bi.SetPixel(i, j, System.Drawing.Color.Green);
                });
            });

            long f2 = DateTime.Now.Ticks - t2;

            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)Bi).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            myImage.Source = image;
            /*myCanvas.Children.Clear();

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    if (random.NextDouble() > 0.5)
                        continue;

                    Rectangle r1 = new Rectangle();
                    r1.Width = 1;
                    r1.Height = 1;
                    Canvas.SetLeft(r1, i);
                    Canvas.SetTop(r1, j);
                    //r1.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255/*0, 0, 0*///));
                                                                                            //r1.StrokeThickness = 1;
                                                                                            //r1.Fill = new ImageBrush(new BitmapImage(new Uri(@"apple.jpg", UriKind.Relative)));
            /*r1.Fill = new SolidColorBrush(Color.FromRgb(20, 255, 100/*0, 0, 0*//*));
            myCanvas.Children.Add(r1);
        }                
    }


    ObjectCanvasRenderer renderer = new ObjectCanvasRenderer(
        myCanvas,
        new Command(obj =>
        {
        /*RenderObjectsArgs args = obj as RenderObjectsArgs;
        if (args != null)
        {
            Dispatcher.Invoke(() => this.drawAction.Invoke(args));
        }*/
            /* }),
             new Command(obj =>
             {
                 Rectangle r = obj as Rectangle;
                 if (r != null)
                 {
                     Dispatcher.Invoke(new Action(() => myCanvas.Children.Remove(r)));
                 }
             }));*/
        }

        public byte[] RawImage { get; set; } = new byte[10000 * 10000];

        /// <summary>
        /// This method will create a renderer, and connect the client to a server.
        /// </summary>
        /// <param name="sender">The sender button.</param>
        /// <param name="e">The arguments of the event.</param>
        private void ButtonClick2(object sender, RoutedEventArgs e)

        {
            PixelFormat pf = PixelFormats.Bgra32;

            int width = 10000;
            int height = width;
            //int stride = (width * pf.BitsPerPixel + 7) / 8;
            int stride = width;
            byte[] pixels = new byte[height * stride];

            // Try creating a new image with a custom palette.
            List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
            colors.Add(System.Windows.Media.Colors.AntiqueWhite);
            colors.Add(System.Windows.Media.Colors.Green);
            BitmapPalette myPalette = new BitmapPalette(colors);


            var random = new Random();

            random.NextBytes(pixels);

            // Creates a new empty image with the pre-defined palette

            BitmapSource image = BitmapSource.Create(
                width,
                height,
                1000,
                1000,
                PixelFormats.Indexed1,// pf,//PixelFormats.Indexed1,
                myPalette,
                pixels,
                stride);

            myImage.Source = image;
        }
    }
}
