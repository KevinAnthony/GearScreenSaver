namespace NSSS
{
    #region

    using System;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Interop;

    using Application = System.Windows.Application;

    #endregion

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private MainWindow screenSaver;

        private HwndSource winWPFContent;

        #endregion

        #region Methods

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0) {
                // Preview mode--display in little window in Screen Saver dialog
                // (Not invoked with Preview button, which runs Screen Saver in
                // normal /s mode).
                if (e.Args[0].ToLower().StartsWith("/p")) {
                    this.screenSaver = new MainWindow();

                    Int32 previewHandle = Convert.ToInt32(e.Args[1]);
                    //WindowInteropHelper interopWin1 = new WindowInteropHelper(win);
                    //interopWin1.Owner = new IntPtr(previewHandle);

                    IntPtr pPreviewHnd = new IntPtr(previewHandle);

                    Win32Interop.RECT lpRect = new Win32Interop.RECT();
                    bool bGetRect = Win32Interop.Win32API.GetClientRect(pPreviewHnd, ref lpRect);

                    HwndSourceParameters sourceParameters = new HwndSourceParameters("sourceParams");

                    sourceParameters.PositionX = 0;
                    sourceParameters.PositionY = 0;
                    sourceParameters.Height = lpRect.Bottom - lpRect.Top;
                    sourceParameters.Width = lpRect.Right - lpRect.Left;
                    sourceParameters.ParentWindow = pPreviewHnd;
                    sourceParameters.WindowStyle = (int)(Win32Interop.WindowStyles.WS_VISIBLE | Win32Interop.WindowStyles.WS_CHILD | Win32Interop.WindowStyles.WS_CLIPCHILDREN);

                    this.winWPFContent = new HwndSource(sourceParameters);
                    this.winWPFContent.Disposed += this.WinWpfContentDisposed;
                    this.winWPFContent.RootVisual = this.screenSaver.ViewPort;
                    return;
                }

                // Normal screensaver mode.  Either screen saver kicked in normally,
                // or was launched from Preview button
                else if (e.Args[0].ToLower().StartsWith("/s")) {
                    foreach (Screen screen in Screen.AllScreens) {
                        //creates a form just for that screen 
                        //and passes it the bounds of that screen
                        MainWindow screensaver = new MainWindow(screen.Bounds);
                        screensaver.Show();
                    }
                    return;
                }

                // Config mode, launched from Settings button in screen saver dialog
                else if (e.Args[0].ToLower().StartsWith("/c")) {}
            }
            // If not running in one of the sanctioned modes, shut down the app
            // immediately (because we don't have a GUI).
            foreach (Screen screen in Screen.AllScreens)
            {
                MainWindow screensaver = new MainWindow(screen.Bounds);
                screensaver.Show();
            }

        }

        void WinWpfContentDisposed(object sender, EventArgs e)
        {
            this.screenSaver.Close();
        }

        #endregion
    }
}