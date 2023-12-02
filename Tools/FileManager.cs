namespace AdventOfCode2023.Tools
{
    public static class FileManager
    {
        static string dataDirectory = string.Empty;

        static FileManager()
        {
            string runtimePath = Directory.GetCurrentDirectory();
            DirectoryInfo runtimeDirectory = new DirectoryInfo(runtimePath);
            string projectDirectory = runtimeDirectory.Parent.Parent.Parent.FullName;
            dataDirectory = Path.Combine(projectDirectory, "Data");

            if (!Directory.Exists(dataDirectory))
            {
                Console.WriteLine($"Data directory: {dataDirectory} does not exists!");
            }
        }

        public static string[] LoadTextLines(string filePath)
        {
            string dataFile = Path.Combine(dataDirectory, filePath);
            if (!File.Exists(dataFile))
            {
                Console.WriteLine($"Data file: {filePath} does not exists in data directory: {dataDirectory}!");
                return Array.Empty<string>();
            }

            return File.ReadAllLines(dataFile);
        }
    }
}
