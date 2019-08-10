using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

namespace DCleaner.ViewModel
{
    using Model;
    
    public class DCleaning : INotifyPropertyChanged
    {
        private readonly Cleaner cleaner = Settings.ReadSettings();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach(string propertyName in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        public string FolderName
        {
            get
            {
                return cleaner.FolderName;
            }
            set
            {
                cleaner.FolderName = value;
                OnPropertyChanged("FolderName");
            }
        }

        public string Path
        {
            get
            {
                return cleaner.Path;
            }
            set
            {
                cleaner.Path = value;
                OnPropertyChanged("Path");
            }
        }

        public bool Container
        {
            get
            {
                return cleaner.Container;
            }
            set
            {
                cleaner.Container = value;
                OnPropertyChanged("Container");
            }
        }

        public bool Desktop
        {
            get
            {
                return cleaner.Desktop;
            }
            set
            {
                cleaner.Desktop = value;
                OnPropertyChanged("Desktop", "PathEnabled");
            }
        }

        public bool PathEnabled
        {
            get
            {
                return !cleaner.Desktop;
            }
        }

        public bool IsDeterminatedPB
        {
            get; set; //TO DO
        }

        public string TextBoxProcessing
        {
            get
            {
                MainWindow mw = new MainWindow();
                return mw.textBoxProccesing.Text;
            }
            set
            {
                MainWindow mw = new MainWindow();
                mw.textBoxProccesing.Text = value;
            }
        }

        private ICommand saveCommand;

        public ICommand Save
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommands(argument => { Settings.SaveSettings(cleaner); });
                return saveCommand;
            }
        }

        private ICommand defaultSettingsCommand;
         
        public ICommand DefaultSettings
        {
            get
            {
                if (defaultSettingsCommand == null)
                {
                    defaultSettingsCommand = new RelayCommands(
                        argument =>
                        {
                            FolderName = "DCleaner";
                            Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            Container = true;
                            Desktop = true;
                        },
                            argument => (FolderName != "DCleaner") || (Path != Environment.GetFolderPath(Environment.SpecialFolder.Desktop)) || (Container != true) || (Desktop != true)
                        );
                }
                return defaultSettingsCommand;
            }
        }

        private ICommand startCleanerCommand;

        public ICommand StartCleaner
        {
            get
            {
                if (startCleanerCommand == null)
                {
                    startCleanerCommand = new RelayCommands(
                        argument =>
                        {
                            //CO ROBIMY SZEFIE?
                            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Path);
                            System.IO.DirectoryInfo[] catalogs = di.GetDirectories();
                            System.IO.FileInfo[] files = di.GetFiles();

                            //Tworzenie Spisu
                            System.IO.FileStream fs;
                            System.IO.StreamWriter sw;
                            DateTime dt = new DateTime();
                            string fileName = dt.Date + " FilesIndex.txt";
                            IsDeterminatedPB = true;
                            if (Container == true)
                            {
                                Path += "\\" + Container + "\\";
                                try
                                {
                                    di = new System.IO.DirectoryInfo(Path);
                                    di.Create();
                                    TextBoxProcessing += "Utworzono folder: " + Path + "\n\r";
                                }
                                catch
                                {
                                    TextBoxProcessing += "Nie można utworzyć folderu: " + Path + "\n\r";
                                }
                                try
                                {
                                    fs = new System.IO.FileStream(Path + "\\" + fileName, System.IO.FileMode.OpenOrCreate);
                                    TextBoxProcessing += "Utworzono plik: " + fileName + " | " + Path + "\\\n\r";
                                }
                                catch
                                {
                                    TextBoxProcessing += "Nie można utworzyć pliku: " + fileName + " | " + Path + "\\\n\r";
                                }
                                try
                                {
                                    sw = new System.IO.StreamWriter(Path + fileName);
                                    TextBoxProcessing += "Otworzono plik: " + fileName + " | " + Path + "\\\n\r";
                                    sw.WriteLine("CATALOGS:");
                                    foreach (System.IO.DirectoryInfo catalog in catalogs)
                                    {
                                        sw.WriteLine(catalog.FullName);
                                        TextBoxProcessing += "Dopisano do spisu: " + catalog.FullName + "\n\r";
                                    }

                                    foreach(System.IO.FileInfo file in files)
                                    {
                                        sw.WriteLine(file.FullName);
                                        TextBoxProcessing += "Dopisano do spisu: " + file.FullName + "\n\r";
                                    }
                                    // koniec spisu

                                    //rozpoczynanie przenoszenia
                                    IsDeterminatedPB = false;
                                    di = new System.IO.DirectoryInfo(Path + "\\" + "Folders");
                                    di.Create();
                                    TextBoxProcessing += "Utworzono folder: " + Path + "\\" + "Folders\n\r";
                                    
                                    foreach (System.IO.DirectoryInfo catalog in catalogs)
                                    {
                                        try
                                        {
                                            TextBoxProcessing += "Rozpoczęto przenoszenie: " + catalog.Name + " do " + Path + "\\" + "Folders\n\r";
                                            catalog.MoveTo(di.ToString());
                                            TextBoxProcessing += "Zakończono przenoszenie : " + catalog.Name + "\n\r";
                                        }
                                        catch
                                        {
                                            TextBoxProcessing += "Przenoszenie zakończone niepowodzeniem: " + catalog.Name + " do " + Path + "\\" + "Folders\n\r";
                                            continue;
                                        }
                                    }

                                    foreach (System.IO.FileInfo file in files)
                                    {
                                        if (file.Extension == ".3gp" || file.Extension == ".aa" || file.Extension == ".aac" || file.Extension == ".aac" || file.Extension == ".aax" || file.Extension == ".act" || file.Extension == ".aiff" || file.Extension == ".amr" || file.Extension == ".ape" || file.Extension == ".au" || file.Extension == ".awb" || file.Extension == ".dct" || file.Extension == ".dss" || file.Extension == ".dvf" || file.Extension == ".flac" || file.Extension == ".gsm" || file.Extension == ".iklax" || file.Extension == ".ivs" || file.Extension == ".m4a" || file.Extension == ".m4b" || file.Extension == ".m4p" || file.Extension == ".mmf" || file.Extension == ".mp3" || file.Extension == ".mpc" || file.Extension == ".msv" || file.Extension == ".ogg" || file.Extension == ".oga" || file.Extension == ".mogg" || file.Extension == ".opus" || file.Extension == ".ra" || file.Extension == ".rm" || file.Extension == ".raw" || file.Extension == ".sln" || file.Extension == ".tta" || file.Extension == ".vox" || file.Extension == ".wav" || file.Extension == ".wma" || file.Extension == ".wv" || file.Extension == ".webm")
                                        {
                                            di = new System.IO.DirectoryInfo(Path + "\\" + "Music");
                                            di.Create();
                                            TextBoxProcessing += "Utworzono folder: " + Path + "\\" + "Music\n\r";

                                            try
                                            {
                                                TextBoxProcessing += "Rozpoczęto przenoszenie: " + file.Name + " do " + Path + "\\" + "Music\n\r";
                                                file.MoveTo(Path + "\\" + "Music\\");
                                                TextBoxProcessing += "Zakończono przenoszenie : " + file.Name + "\n\r";
                                            }
                                            catch
                                            {
                                                TextBoxProcessing += "Przenoszenie zakończone niepowodzeniem: " + file.Name + " do " + Path + "\\" + "Music\n\r";
                                                continue;
                                            }
                                        }
                                        
                                    }

                                    


                                }
                                catch
                                {
                                    TextBoxProcessing += "Nie można otworzyć pliku: " + fileName + " | " + Path + "\\\n\r";
                                }

                                
                               



                            }
                            else
                            {

                            }
                            
                        },
                        argument => (!FilePathHasInvalidChars(Path)));
                }
                return startCleanerCommand;
            }
        }

        private ICommand selectFileDirectoryCommand;

        public ICommand SelectFileDirectory
        {
            get
            {
                if (selectFileDirectoryCommand == null)
                    selectFileDirectoryCommand = new RelayCommands(
                        argument =>
                        {
                            System.Windows.Forms.FolderBrowserDialog selectFolder = new System.Windows.Forms.FolderBrowserDialog();
                            selectFolder.SelectedPath = Path;

                            System.Windows.Forms.DialogResult result = selectFolder.ShowDialog();
                            if (result.ToString() == "OK")
                            {
                                Path = selectFolder.SelectedPath;
                            }

                        });
                return selectFileDirectoryCommand;
            }
        }

        public static bool FilePathHasInvalidChars(string path)
        {

            return (!string.IsNullOrEmpty(path) && path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0);
        }
    }
}
