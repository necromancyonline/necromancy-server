using System.IO;
using Necromancy.Server.Common;

namespace Necromancy.Server.Setting
{
    public class SettingProvider
    {
        private readonly string _directory;


        public SettingProvider(string directory = null)
        {
            if (Directory.Exists(directory))
                _directory = directory;
            else
                _directory = Util.RelativeExecutingDirectory();
        }

        public string GetSettingsPath(string file)
        {
            return Path.Combine(_directory, file);
        }

        public void Save<T>(T settings, string file)
        {
            string path = GetSettingsPath(file);
            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(path, json);
        }

        public T Load<T>(string file)
        {
            T settings;
            string path = GetSettingsPath(file);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                settings = JsonSerializer.Deserialize<T>(json);
            }
            else
            {
                settings = default;
            }

            return settings;
        }
    }
}
