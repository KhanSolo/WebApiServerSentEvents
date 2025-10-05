using Microsoft.AspNetCore.Mvc;
using AccessRightsMaster.ExternalApi.Models;
using System.Text.Json;

namespace AccessRightsMaster.ExternalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ImportController : ControllerBase
{
    [HttpPut("stream")]
    public async Task Put()
    {
        //Response.ContentType = "text/event-stream";
        Response.StatusCode = StatusCodes.Status200OK;
        Response.ContentType = "application/json";
        //Response.Headers.Append("Transfer-Encoding", "chunked");

        const int TotalPages = 5;
        for (var i = 0; i < TotalPages; i++)
        {
            var user = new ImportProgress
            {
                Page = i + 1,
                TotalPages = TotalPages,
            };

            var json = JsonSerializer.Serialize(user);
            await Response.WriteAsync($"{json}\n\n");
            await Response.Body.FlushAsync();

            await Task.Delay(1000); // Delay to simulate real-time updates (every 2 seconds)
        }
    }
}
