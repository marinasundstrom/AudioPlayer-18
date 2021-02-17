using System;
using System.Collections.Generic;
using System.Text;
using AudioPlayer.ViewModels;
using Xamarin.Forms;
using MenuItem = AudioPlayer.ViewModels.MenuItem;

namespace AudioPlayer
{
    public class MainItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WelcomeTemplate { get; set; }
        public DataTemplate DashboardTemplate { get; set; }
        public DataTemplate MusicTemplate { get; set; }
        public DataTemplate AnnouncementTemplate { get; set; }
        public DataTemplate SettingsTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is MenuItem menuItem)
            {
                switch (menuItem.Title)
                {
                    case "Welcome":
                        return WelcomeTemplate;

                    case "Dashboard":
                        return DashboardTemplate;

                    case "Music":
                        return MusicTemplate;

                    case "Announcement":
                        return AnnouncementTemplate;

                    case "Settings":
                        return SettingsTemplate;
                }
            }
            throw new ArgumentException(nameof(item));
        }
    }
}
