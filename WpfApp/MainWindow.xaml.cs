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

            Rectangle r1 = new Rectangle();
            r1.Width = 100;
            r1.Height = 100;
            Canvas.SetLeft(r1, 10);
            Canvas.SetTop(r1, 10);
            r1.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255/*0, 0, 0*/));
            r1.StrokeThickness = 1;
            r1.Fill = new ImageBrush(new BitmapImage(new Uri(@"apple.jpg", UriKind.Relative)));

            myCanvas.Children.Add(r1);
        }

        /// <summary>
        /// This method will create a renderer, and connect the client to a server.
        /// </summary>
        /// <param name="sender">The sender button.</param>
        /// <param name="e">The arguments of the event.</param>
        private void ButtonClick(object sender, RoutedEventArgs e)

        {
            var random = new Random();
            myCanvas.Children.Clear();

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
                    //r1.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255/*0, 0, 0*/));
                    //r1.StrokeThickness = 1;
                    //r1.Fill = new ImageBrush(new BitmapImage(new Uri(@"apple.jpg", UriKind.Relative)));
                    r1.Fill = new SolidColorBrush(Color.FromRgb(20, 255, 100/*0, 0, 0*/));
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
                }),
                new Command(obj =>
                {
                    Rectangle r = obj as Rectangle;
                    if (r != null)
                    {
                        Dispatcher.Invoke(new Action(() => myCanvas.Children.Remove(r)));
                    }
                }));
        }
    }

    
}
