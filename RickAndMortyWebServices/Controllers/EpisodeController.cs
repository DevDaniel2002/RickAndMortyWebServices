using Microsoft.AspNetCore.Mvc;
using RickAndMortyWebServices.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class EpisodeController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private const string baseUrl = "https://rickandmortyapi.com/api/episode/";

    public EpisodeController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("all-episodes")]
    public async Task<IActionResult> GetAllEpisodes()
    {
        var url = $"{baseUrl}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Error fetching data from Rick and Morty API.");
        }

        var content = await response.Content.ReadAsStringAsync();
        var allEpisodes = JsonSerializer.Deserialize<ApiModels.ApiResponse<ApiModels.Episode>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (allEpisodes == null || allEpisodes.Results == null || allEpisodes.Results.Count == 0)
        {
            return NoContent();
        }

        return Ok(allEpisodes.Results);
    }

    [HttpGet("episode/{id}")]
    public async Task<IActionResult> GetEpisodeById(int id)
    {
        var url = $"{baseUrl}{id}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, $"Episode with ID {id} not found.");
        }

        var content = await response.Content.ReadAsStringAsync();
        var episode = JsonSerializer.Deserialize<ApiModels.Episode>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return Ok(episode);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchEpisodes([FromQuery] string? name = null, [FromQuery] string? episodeCode = null)
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(name)) queryParams.Add($"name={Uri.EscapeDataString(name)}");
        if (!string.IsNullOrEmpty(episodeCode)) queryParams.Add($"episode={Uri.EscapeDataString(episodeCode)}");

        var queryString = string.Join("&", queryParams);
        var url = $"{baseUrl}?{queryString}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "No episodes found with the given search criteria.");
        }

        var content = await response.Content.ReadAsStringAsync();
        var episodes = JsonSerializer.Deserialize<ApiModels.ApiResponse<ApiModels.Episode>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (episodes == null || episodes.Results == null || episodes.Results.Count == 0)
        {
            return NoContent();
        }

        return Ok(episodes.Results);
    }
}
