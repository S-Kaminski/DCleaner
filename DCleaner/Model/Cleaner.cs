using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCleaner.Model
{
    public class Cleaner
    {
        public string FolderName { get; set; }

        public string Path { get; set; }

        public bool Container { get; set; }

        public bool Desktop { get; set; }

        public bool PathEnabled { get; set; }

        public Cleaner (string folderName, string path, bool container, bool desktop, bool pathEnabled)
        {
            this.FolderName = folderName;
            this.Path = path;
            this.Container = container;
            this.Desktop = desktop;
            this.PathEnabled = pathEnabled;
        }
    }
}
