using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Forms;
using File = TagLib.File;

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
            CommandRemoveSuffix = new RelayCommand(RemoveSuffix, CanUseButtons);
            CommandRemoveSuffixAndDoTagging = new RelayCommand(RemoveSuffixAndDoTagging, CanUseButtons);
            CommandReplaceUnderscore = new RelayCommand(ReplaceUnderscore, CanUseButtons);
            SuffixToRemove = "myfreemp3.vip";
        }

        public RelayCommand CommandRemoveSuffix { get; set; }
        public RelayCommand CommandRemoveSuffixAndDoTagging { get; set; }
        public RelayCommand CommandOpenFolder { get; set; }
        public RelayCommand CommandReplaceUnderscore { get; set; }

        private bool CanUseButtons()
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

        private void ReplaceUnderscore(object param)
        {
            var di = new DirectoryInfo(_pathToFolder);

            foreach (var file in di.GetFiles())
            {
                if (file.Name.Contains("_"))
                {
                    var newName = file.Name.Replace("_", " ");
                    file.MoveTo(file.DirectoryName+ "/" + newName);
                    continue;
                }
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
            RemoveSuffix(new object());

            var di = new DirectoryInfo(_pathToFolder);

            var errorList = new List<string>();

            foreach (var file in di.GetFiles())
            {
                var fileExtension = file.Extension;
                var fileNameWithoutExtension = file.Name.Replace(fileExtension, "");
                var names = fileNameWithoutExtension.Split("-");
                names[0] = names[0].Trim();
                names[1] = names[1].Trim();

                try
                {
                    var song = File.Create(file.FullName);
                    song.Tag.Performers = new[] { names[0] };
                    song.Tag.Title = names[1];
                    song.Save();
                }
                catch (Exception)
                {
                    errorList.Add($"There was error in file '{file.FullName}'");
                }
            }

            if (errorList.Count > 0)
            {
                var errorString = "";
                foreach (var error in errorList)
                {
                    errorString += error + Environment.NewLine + Environment.NewLine;
                }

                MessageBox.Show(errorString, "Error!");
            }
        }
    }
}
