using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Wox.Infrastructure.Storage;
using Wox.Plugin;
using Control = System.Windows.Controls.Control;

namespace OneLineLogger
{
    public class Main : IPlugin, ISettingProvider
    {
        private PluginJsonStorage<Settings> _storage;
        private Settings _settings;

        public List<Result> Query(Query query)
        {
            if (string.IsNullOrWhiteSpace(query.Search)) return null;

            var prefix = string.IsNullOrWhiteSpace(_settings.FilePrefix) ? string.Empty : _settings.FilePrefix;
            var suffix = string.IsNullOrWhiteSpace(_settings.FileExtension)? "md" : _settings.FileExtension;
            
            var date = DateTime.Now;
            var logFileName = $"{prefix}{date:yyyy-MM-dd}.{suffix}";
            var timestamp = date.ToString("O");

            return new List<Result>
            {
                new Result()
                {
                    Title = $"Write to log: {query.Search}",
                    SubTitle = $"With timestamp {DateTime.Now:O}",
                    IcoPath = "logo.png",
                    Action = e =>
                    {
                        using (var streamWriter = File.AppendText($"{_settings.PathToFolder}\\{logFileName}"))
                        {
                            streamWriter.WriteLine($"[{timestamp}] {query.Search}");
                        }
                        return true;
                    }
                }
            };
        }

        public void Init(PluginInitContext context)
        {
            Context = context;
            _storage = new PluginJsonStorage<Settings>();
            _settings = _storage.Load();
            if (string.IsNullOrWhiteSpace(_settings.PathToFolder))
            {
                Context.CurrentPluginMetadata.Disabled = true;
                return;
            }
            if (Directory.Exists(_settings.PathToFolder) == false)
            {
                Directory.CreateDirectory(_settings.PathToFolder);
            }
        }

        private PluginInitContext Context { get; set; }

        public Control CreateSettingPanel()
        {
            return new SettingsPanel(_settings, _storage);
        }
    }
}