using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        }

        private void RemoveSuffix(object param)
        {

        }

        private void RemoveSuffixAndDoTagging(object param)
        {

        }








    }
}
