using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace MP3Tagger
{
    class MainWindowViewModel : ViewModelBase
    {
        private string _pathToFolder;
        public string PathToFolder
        {
            get => _pathToFolder;
            set
            {
                _pathToFolder = value;
                OnPropertyChange("PathToFolder");
            }
        }

        private string _suffixToRemove;
        public string SuffixToRemove
        {
            get => _suffixToRemove;
            set
            {
                _suffixToRemove = value;
                OnPropertyChange("SuffixToRemove");
            }
        }

        public MainWindowViewModel()
        {
            CommandOpenFolder = new RelayCommand(OpenFolder, CanOpenFolder);
            CommandRemoveSuffix = new RelayCommand(RemoveSuffix, CanTagAndDelete);
            CommandRemoveSuffixAndDoTagging = new RelayCommand(RemoveSuffixAndDoTagging, CanTagAndDelete);
            SuffixToRemove = "myfreemp3.vip";
        }

        public RelayCommand CommandRemoveSuffix { get; set; }
        public RelayCommand CommandRemoveSuffixAndDoTagging { get; set; }
        public RelayCommand CommandOpenFolder { get; set; }

        private bool CanTagAndDelete()
        {
            if (string.IsNullOrWhiteSpace(_pathToFolder) || string.IsNullOrWhiteSpace(_suffixToRemove))
            {
                return false;
            }
            return true;
        }

        private bool CanOpenFolder()
        {
            return true;
        }

        private void OpenFolder(object param)
        {
            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                PathToFolder = fbd.SelectedPath;
            }
        }

        private void RemoveSuffix(object param)
        {
            var di = new DirectoryInfo(_pathToFolder);

            foreach (var file in di.GetFiles())
            {
                if (string.IsNullOrEmpty(file.Extension) == false)
                {
                    var fileExtension = file.Extension;
                    var fileNameWithoutExtension = file.FullName.Replace(fileExtension, "");
                    var newFileName = fileNameWithoutExtension.Replace(_suffixToRemove, "").TrimEnd();
                    if (newFileName.ToLower().Contains("(original mix)"))
                    {
                        //removing duplicate original mix
                        newFileName = newFileName.ToLower().Replace("(original mix)", "").TrimEnd() + " (Original Mix)";
                    }

                    file.MoveTo(newFileName + fileExtension);
                }
            }
        }

        private void RemoveSuffixAndDoTagging(object param)
        {

        }
    }
}
