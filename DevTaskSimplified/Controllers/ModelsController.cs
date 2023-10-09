using DevTaskSimplified.Dtos;
using LINQtoCSV;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace DevTaskSimplified.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModelsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IHostingEnvironment _HostEnvironment;
    private static string getModelsUri = "vehicles/GetModelsForMakeIdYear/makeId/";
    public ModelsController(IConfiguration configuration, IHostingEnvironment hostEnvironment)
    {
        _configuration = configuration;
        _HostEnvironment = hostEnvironment;
    }

    static HttpClient client = new HttpClient();

    [HttpGet]
    public async Task<IEnumerable<string>> GetAsync([FromQuery] int modelYear, [FromQuery] string make)
    {
        IEnumerable<string> models = Enumerable.Empty<string>();
        string baseUrl = _configuration.GetValue<string>("BaseUrl") ?? string.Empty;

        int? makeId = GetMakeId(make.ToLower());
        if (makeId.HasValue)
        {
            string path = $"{baseUrl}{getModelsUri}{makeId}/modelyear/{modelYear}?format=json";
            models = await GetModelsAsync(models, path);
        }

        return models;
    }

    private static async Task<IEnumerable<string>> GetModelsAsync(IEnumerable<string> models, string path)
    {
        HttpResponseMessage httpResponse = await client.GetAsync(path);

        if (httpResponse.IsSuccessStatusCode)
        {
            Response response = await httpResponse.Content.ReadAsAsync<Response>();
            models = response.Results.Select(x => x.Model_Name);
        }

        return models;
    }

    private int? GetMakeId(string make)
    {
        CsvContext csvContext = new CsvContext();
        string? volume = csvContext.Read<MakeCsvMapper>(Path.Combine(_HostEnvironment.WebRootPath, "CarMake.csv"))
                .Where(i => i.make_name.ToLower().Contains(make))
                .Select(i => i.make_name)
                .FirstOrDefault();

        return volume == null ? null : int.Parse(volume);
    }
}