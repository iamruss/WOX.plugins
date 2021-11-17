using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Navigation;
using Wox.Infrastructure.Storage;
using UserControl = System.Windows.Controls.UserControl;

namespace OneLineLogger
{
    public partial class SettingsPanel : UserControl
    {
        private readonly Settings _settings;
        private readonly PluginJsonStorage<Settings> _storage;

        public SettingsPanel(Settings settings, PluginJsonStorage<Settings> storage)
        {
            _settings = settings;
            _storage = storage;

            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TbPath.Text = _settings.PathToFolder;
            TbFilePrefix.Text = _settings.FilePrefix;
            TbExtension.Text = _settings.FileExtension;
            TbTimeStampFormat.Text = _settings.TimeStampFormat;
            TbTimeStampPreview.Content = DateTime.Now.ToString(_settings.TimeStampFormat);
        }

        private void OnClick_PickFolder(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath)) return;

                TbPath.Text = fbd.SelectedPath;
                _settings.PathToFolder = fbd.SelectedPath;
                _storage.Save();
            }
        }

        private void TbFilePrefix_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var text = ((System.Windows.Controls.TextBox)e.Source).Text;
            _settings.FilePrefix = (text ?? string.Empty).Trim();
            _storage.Save();
        }

        private void TbExtension_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var text = ((System.Windows.Controls.TextBox)e.Source).Text;
            _settings.FileExtension = (text ?? "md").Trim('.', ' ');
            _storage.Save();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Hyperlink_ShowFolder(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo("explorer.exe", _settings.PathToFolder));
            e.Handled = true;
        }

        private void TbTimeStampFormat_TextChanged(object sender, TextChangedEventArgs e)
        {
            string format;
            var control = (System.Windows.Controls.TextBox)e.Source;
            
            if (string.IsNullOrWhiteSpace(control.Text))
            {
                control.Text = Constants.DefaultTimeFormat;
            }

            try
            {
                format = control.Text;
                var preview = DateTime.Now.ToString(format);
                TbTimeStampPreview.Content = preview;
            }
            catch
            {
                TbTimeStampFormat.Text = Constants.DefaultTimeFormat;
                TbTimeStampPreview.Content = string.Empty;
                return;
            }

            _settings.TimeStampFormat = format;
            _storage.Save();
        }
    }
}