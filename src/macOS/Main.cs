using AppKit;

namespace AudioPlayer.macOS
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            NSApplication.Init();
            NSApplication.SharedApplication.Delegate = new AppDelegate();
            NSApplication.SharedApplication.MainMenu = new NSMenu();
            NSApplication.Main(args);
        }
    }
}
