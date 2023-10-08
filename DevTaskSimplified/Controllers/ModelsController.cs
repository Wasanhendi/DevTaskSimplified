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
        string baseUrl = _configuration.GetValue<string>("BaseUrl");
        string path = $"{baseUrl}{getModelsUri}{make}/modelyear/{modelYear}?format=json";

        models = await GetModelsAsync(models, path);
        B();
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

    private void B()
    {
        CsvContext csvContext = new CsvContext();
        var volume = csvContext.Read<IdVolumeNameRow>(Path.Combine(_HostEnvironment.WebRootPath, "CarMake.csv"))
                .Where(i => i.make_id == "464")
                .Select(i => i.make_name)
                .FirstOrDefault();
    }


}
public class IdVolumeNameRow
{
    [CsvColumn(FieldIndex = 1)]
    public string make_id { get; set; }

    [CsvColumn(FieldIndex = 2)]
    public string make_name { get; set; }
}