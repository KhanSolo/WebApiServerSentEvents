using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AccessRightsMaster.Client;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Press enter to start");
        Console.ReadLine();

        using var client = new HttpClient();
        const string url = "http://localhost:5050/Import/stream";

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);

            // sendin request
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            // reading
            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var progress = JsonSerializer.Deserialize<ImportProgress>(line);
                    string message = progress is null
                        ? "Data was read but it is null!"
                        : $"Page {progress.Page} of {progress.TotalPages}";

                    Console.WriteLine(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
        }
    }
}

public class ImportProgress
{
    public int Page { get; set; }
    public int TotalPages { get; set; }
}
