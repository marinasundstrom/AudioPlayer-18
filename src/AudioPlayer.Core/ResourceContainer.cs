using System;
using System.Globalization;
using System.Resources;

namespace AudioPlayer
{
    public class ResourceContainer : IResourceContainer
    {
        public static string ResourceId = "AudioPlayer.Resources.AppResources"; // The namespace and name of your Resources file
        private CultureInfo _cultureInfo;
        private ResourceManager _resourceManager;

        public ResourceContainer(ResourceManager manager, ILocalize localize)
        {
            _cultureInfo = localize.GetCurrentCultureInfo();
            _resourceManager = manager;
        }

        public string GetString(string key)
        {
            return _resourceManager.GetString(key, _cultureInfo);
        }

        public string GetString(string key, params object[] args)
        {   
            return string.Format(GetString(key), args);
        }
    }
}
