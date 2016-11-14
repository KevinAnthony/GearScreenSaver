using System.Diagnostics;
using Microsoft.Win32;

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


        #region Methods

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            string filelocation = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\NoSide\ScreenSaver", "nsss_path", string.Empty) as string;
            if (filelocation == null) return;
            if (e.Args.Length > 0) {
                // Preview mode--display in little window in Screen Saver dialog
                // (Not invoked with Preview button, which runs Screen Saver in
                // normal /s mode).
                if (e.Args[0].ToLower().StartsWith("/p")) {
                    Process notePad = new Process();
                    notePad.StartInfo.FileName = filelocation;
                    notePad.StartInfo.Arguments = "-multidisplay";
                    notePad.Start();
                    return;
                }

                // Normal screensaver mode.  Either screen saver kicked in normally,
                // or was launched from Preview button
                else if (e.Args[0].ToLower().StartsWith("/s")) {
                    Process notePad = new Process();
                    notePad.StartInfo.FileName = filelocation;
                    notePad.StartInfo.Arguments = "-multidisplay";
                    notePad.Start();
                    return;
                }

                // Config mode, launched from Settings button in screen saver dialog
                else if (e.Args[0].ToLower().StartsWith("/c")) {}
            }

        }

        #endregion
    }
}