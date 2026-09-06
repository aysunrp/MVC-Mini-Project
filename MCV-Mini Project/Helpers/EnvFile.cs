namespace MCV_Mini_Project.Helpers
{
    public static class EnvFile
    {
        public static void Load(string path)
        {
            if (!File.Exists(path))
                return;

            foreach (var raw in File.ReadAllLines(path))
            {
                var line = raw.Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                    continue;

                var index = line.IndexOf('=');
                if (index <= 0)
                    continue;

                var key = line[..index].Trim();
                var value = line[(index + 1)..].Trim();

                if (value.Length >= 2 &&
                    ((value.StartsWith('"') && value.EndsWith('"')) ||
                     (value.StartsWith('\'') && value.EndsWith('\''))))
                {
                    value = value[1..^1];
                }

                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
                    Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}
