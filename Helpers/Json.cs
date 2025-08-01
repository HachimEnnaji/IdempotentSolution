using System.Text.Json;

namespace IdempotentApi.Helpers;

internal static class Json
{

    internal static T SafeDeserialize<T>(string jsonBody) where T : new()
    {

        if (string.IsNullOrWhiteSpace(jsonBody))
        {
            return new T();
        }

        try
        {
            return JsonSerializer.Deserialize<T>(jsonBody) ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            return new T();
        }
    }

    internal static string SafeSerialize<T>(T obj)
    {
        if (obj is null)
        {
            return string.Empty;
        }

        try
        {
            return JsonSerializer.Serialize(obj);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error serializing object to JSON: {ex.Message}");
            return string.Empty;
        }
    }

}
