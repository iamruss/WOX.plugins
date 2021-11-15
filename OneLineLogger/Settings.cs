using System;
using Newtonsoft.Json;

namespace OneLineLogger
{
    public class Settings
    {
        private string _fileExtension;
        private string _filePrefix;
        private string _pathToFolder;

        [JsonProperty]
        public string PathToFolder
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_pathToFolder))
                {
                    _pathToFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                }
                return _pathToFolder;
            }
            set => _pathToFolder = value;
        }

        [JsonProperty]
        public string FilePrefix
        {
            get => _filePrefix ?? "log";
            set => _filePrefix = value;
        }

        [JsonProperty]
        public string FileExtension
        {
            get => _fileExtension ?? "md";
            set => _fileExtension = value;
        }
    }
}