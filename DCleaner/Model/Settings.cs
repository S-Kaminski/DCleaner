using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCleaner.Model
{
    static class Settings
    {
        public static void SaveSettings(Cleaner cleaner)
        {
            Properties.Settings settings = Properties.Settings.Default;
            settings.FolderName = cleaner.FolderName;
            settings.Path = cleaner.Path;
            settings.Container = cleaner.Container;
            settings.Desktop = cleaner.Desktop;
            settings.PathEnabled = cleaner.PathEnabled;
            settings.Save();
        }

        public static Cleaner ReadSettings()
        {
            Properties.Settings settings = Properties.Settings.Default;
            if (string.IsNullOrEmpty(settings.Path))
            {
                settings.Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                settings.Save();
            }
            return new Cleaner(settings.FolderName, settings.Path, settings.Container, settings.Desktop, settings.PathEnabled);
        }
    }
}
