using Microsoft.AspNetCore.Mvc;
using RickAndMortyWebServices.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private const string baseUrl = "https://rickandmortyapi.com/api/character/";

    public CharacterController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("all-characters")]
    public async Task<IActionResult> GetAllCharacters()
    {
        var url = $"{baseUrl}";

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error fetching data from Rick and Morty API.");
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return NoContent();
            }

          
            var allCharacters = JsonSerializer.Deserialize<ApiModels.ApiResponse<ApiModels.Character>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            });

            if (allCharacters == null || allCharacters.Results == null || allCharacters.Results.Count == 0)
            {
                return NoContent(); 
            }

            return Ok(allCharacters.Results);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpGet("character/{id}")]
    public async Task<IActionResult> GetCharacterById(int id)
    {
        var url = $"{baseUrl}{id}";

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Character with ID {id} not found.");
            }

            var content = await response.Content.ReadAsStringAsync();

            
            Console.WriteLine(content);

            if (string.IsNullOrWhiteSpace(content))
            {
                return NoContent();
            }

            var character = JsonSerializer.Deserialize<ApiModels.Character>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Ok(character);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
    [HttpGet("search")]
    public async Task<IActionResult> SearchCharacters([FromQuery] string? name = null, [FromQuery] string? status = null, [FromQuery] string? species = null, [FromQuery] string? gender = null)
    {
        
        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(name)) queryParams.Add($"name={Uri.EscapeDataString(name)}");
        if (!string.IsNullOrEmpty(status)) queryParams.Add($"status={Uri.EscapeDataString(status)}");
        if (!string.IsNullOrEmpty(species)) queryParams.Add($"species={Uri.EscapeDataString(species)}");
        if (!string.IsNullOrEmpty(gender)) queryParams.Add($"gender={Uri.EscapeDataString(gender)}");
        
        var queryString = string.Join("&", queryParams);

        var url = $"{baseUrl}?{queryString}";

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "No han sido encontraros personajes con estos criterios de busqueda.");
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return NoContent();
            }

            var characters = JsonSerializer.Deserialize<ApiModels.ApiResponse<ApiModels.Character>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (characters == null || characters.Results == null || characters.Results.Count == 0)
            {
                return NoContent();
            }

            return Ok(characters.Results);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
