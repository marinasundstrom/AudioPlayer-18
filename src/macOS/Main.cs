using AppKit;

namespace Axis.AudioPlayer.macOS
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            NSApplication.Init();
            NSApplication.SharedApplication.Delegate = new AppDelegate();
            NSApplication.SharedApplication.MainMenu = new NSMenu();
            NSApplication.Main(args);
        }
    }
}
