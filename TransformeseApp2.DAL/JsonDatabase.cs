using System.Text.Json;

namespace TransformeseApp2.DAL
{
    public static class JsonDatabase
    {
        private static readonly string DataFolder = Path.Combine(Environment.CurrentDirectory, "Data");

        static JsonDatabase()
        {
            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
            }
        }

        public static List<T> Ler<T>(string filename) where T : class
        {
            string path = Path.Combine(DataFolder, filename);

            if (!File.Exists(path))
            {
                File.WriteAllText(path, "[]");
            }

            string json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        public static void Salvar<T>(string filename, List<T> lista) where T : class
        {
            string path = Path.Combine(DataFolder, filename);

            string json = JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
        }
    }
}
