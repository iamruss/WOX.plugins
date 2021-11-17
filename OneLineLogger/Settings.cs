using System;
using Newtonsoft.Json;

namespace OneLineLogger
{
    public class Settings
    {
        private string _fileExtension;
        private string _filePrefix;
        private string _pathToFolder;
        private string _timeStampFormat;

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
            get => _filePrefix ?? Constants.DefaultFilePrefix;
            set => _filePrefix = value;
        }

        [JsonProperty]
        public string FileExtension
        {
            get => _fileExtension ?? Constants.DefaultFileExtension;
            set => _fileExtension = value;
        }

        [JsonProperty]
        public string TimeStampFormat
        {
            get => _timeStampFormat ?? Constants.DefaultTimeFormat;
            set => _timeStampFormat = value;
        }
    }
}