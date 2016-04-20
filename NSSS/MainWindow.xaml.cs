using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Point = System.Windows.Point;

namespace NSSS
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ModelPath = "D:\\Dropbox\\Documents\\Gear House\\2015-06-Model\\HomeAutomation.obj";
        private Point _originalLocation = new Point(int.MaxValue, int.MaxValue);

        public MainWindow()
        {
            Close();
        }

        public MainWindow(Rectangle bounds)
        {
            InitializeComponent();
            LoadedModel();
            Width = bounds.Width;
            Height = bounds.Height;
            Left = bounds.Left;
            Top = bounds.Top;
            Loaded += (sender, args) => WindowState = WindowState.Maximized;
            StateChanged += MainWindow_StateChanged;
            MouseMove += OnMouseMove;
            KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            Application.Current.Shutdown();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                // hide the window before changing window style
                Visibility = Visibility.Collapsed;
                Topmost = true;
                WindowStyle = WindowStyle.None;
                ResizeMode = ResizeMode.NoResize;
                // re-show the window after changing style
                Visibility = Visibility.Visible;
            }
            else
            {
                Topmost = false;
                WindowStyle = WindowStyle.SingleBorderWindow;
                ResizeMode = ResizeMode.CanResize;
            }
        }

        private void LoadedModel()
        {
            var currentHelixObjReader = new ObjReader();
            var coinModel = currentHelixObjReader.Read(ModelPath);

            // Display the model
            Coin.Content = coinModel;
            var camera = ViewPort.Camera as PerspectiveCamera;
            if (camera == null) return;
            camera.LookDirection = new Vector3D(0, 30, 0);
            camera.Position = new Point3D(0, -30, 0);
            camera.UpDirection = new Vector3D(0, 0, -1);

            ViewPort.Children.Add(new ModelVisual3D {Content = new AmbientLight(Colors.White)});

            ViewPort.Loaded += ViewPortOnLoaded;

            //hide the cursor
            Cursor = Cursors.None;
        }

        private void OnMouseMove(object sender, MouseEventArgs args)
        {
            var e = args.GetPosition(this);
            //see if originallocation has been set
            if (_originalLocation.X.Equals(int.MaxValue) &&
                _originalLocation.Y.Equals(int.MaxValue))
            {
                _originalLocation = e;
            }
            //see if the mouse has moved more than 20 pixels 
            //in any direction. If it has, close the application.
            if (Math.Abs(e.X - _originalLocation.X) > 20 |
                Math.Abs(e.Y - _originalLocation.Y) > 20)
            {
                Application.Current.Shutdown();
            }
        }

        private void ViewPortOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var da = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(5)));
            da.RepeatBehavior = RepeatBehavior.Forever;
            CamRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
        }
    }
}