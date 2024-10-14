using Microsoft.AspNetCore.Mvc;
using RickAndMortyWebServices.Models;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private const string baseUrl = "https://rickandmortyapi.com/api/location/";

    public LocationController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Obtener todas las locaciones
    [HttpGet("all-locations")]
    public async Task<IActionResult> GetAllLocations()
    {
        var url = $"{baseUrl}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Error fetching data from Rick and Morty API.");
        }

        var content = await response.Content.ReadAsStringAsync();
        var allLocations = JsonSerializer.Deserialize<ApiModels.ApiResponse<ApiModels.Location>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (allLocations == null || allLocations.Results == null || allLocations.Results.Count == 0)
        {
            return NoContent();
        }

        return Ok(allLocations.Results);
    }

    // Obtener una locación por ID
    [HttpGet("location/{id}")]
    public async Task<IActionResult> GetLocationById(int id)
    {
        var url = $"{baseUrl}{id}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, $"Location with ID {id} not found.");
        }

        var content = await response.Content.ReadAsStringAsync();
        var location = JsonSerializer.Deserialize<ApiModels.Location>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return Ok(location);
    }

    // Búsqueda de locaciones (nombre, tipo y dimensión)
    [HttpGet("search")]
    public async Task<IActionResult> SearchLocations([FromQuery] string? name = null, [FromQuery] string? type = null, [FromQuery] string? dimension = null)
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(name)) queryParams.Add($"name={Uri.EscapeDataString(name)}");
        if (!string.IsNullOrEmpty(type)) queryParams.Add($"type={Uri.EscapeDataString(type)}");
        if (!string.IsNullOrEmpty(dimension)) queryParams.Add($"dimension={Uri.EscapeDataString(dimension)}");

        var queryString = string.Join("&", queryParams);
        var url = $"{baseUrl}?{queryString}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "No locations found with the given search criteria.");
        }

        var content = await response.Content.ReadAsStringAsync();
        var locations = JsonSerializer.Deserialize<ApiModels.ApiResponse<ApiModels.Location>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (locations == null || locations.Results == null || locations.Results.Count == 0)
        {
            return NoContent();
        }

        return Ok(locations.Results);
    }
}
