using System.Text.Json;

namespace ThinQ.CLI.Extensions;

public static class HttpContentExtensions
{
    /// <summary>
    /// Reads the JSON content from the HTTP response and deserializes it to the specified type.
    /// Throws an exception if the content cannot be deserialized or if the status code is not successful.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the JSON content to.</typeparam>
    /// <param name="response">The HTTP response message.</param>
    /// <returns>The deserialized content.</returns>
    /// <exception cref="HttpRequestException">Thrown if the status code is not successful or if deserialization fails.</exception>
    public static async Task<T> ReadContentFromJsonOrThrowAsync<T>(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        try
        {
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web))
                   ?? throw new Exception($"Status code: {response.StatusCode}. Message: {content}");
        }
        catch (Exception)
        {
            throw new Exception($"Status code: {response.StatusCode}. Message: {content}");
        }
    }
}
