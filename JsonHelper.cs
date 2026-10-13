using System.Text.Json;
using System.Text.Json.Serialization;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions _serilaizeroptions= new JsonSerializerOptions
    {
      PropertyNamingPolicy= JsonNamingPolicy.CamelCase,
      WriteIndented=false,
      DefaultIgnoreCondition=JsonIgnoreCondition.WhenWritingNull,
      PropertyNameCaseInsensitive=true,  
    };

    public static string Serialize<T> (T obj)
    {
        return JsonSerializer.Serialize(obj, _serilaizeroptions);
    }

    public static T Deserialize<T>(string json)
    {
        if(string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentException("json string cannot be null or empty");
        }
        return JsonSerializer.Deserialize<T>(json,_serilaizeroptions);
    }
    public static bool TryDeserialize<T>(string json,out T result)
    {
     result= default(T);
     try
     {
        result= Deserialize<T>(json);
        return true;
     }
     catch
     {
        return false;
     }
    }


}