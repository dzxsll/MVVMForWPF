using Newtonsoft.Json;

namespace MVVM.Module
{
    public class JsonConverter
    {
        public static string Serialize<T>(T dto)
        {
            return JsonConvert.SerializeObject(dto, Formatting.Indented);
        }

        public static T Deserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}