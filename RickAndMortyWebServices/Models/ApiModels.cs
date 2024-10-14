using System.Text.Json.Serialization;

namespace RickAndMortyWebServices.Models
{
    public class ApiModels
    {
        public class ApiResponse<T>
        {
            public Info Info { get; set; }
            public List<T> Results { get; set; }
        }

        public class Info
        {
            public int Count { get; set; }
            public int Pages { get; set; }
            public string Next { get; set; }
            public string Prev { get; set; }
        }

        public class Character
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public string Species { get; set; }
            public string Gender { get; set; }
            public string Image { get; set; }
        }

        public class Episode
        {
            public int Id { get; set; }
            public string Name { get; set; }
            [JsonPropertyName("air_date")]
            public string AirDate { get; set; }
            [JsonPropertyName("episode")]
            public string EpisodeCode { get; set; }
            public List<string> Characters { get; set; }
            public string Url { get; set; }
            public DateTime Created { get; set; }
        }

        public class Location
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Dimension { get; set; }
            public List<string> Residents { get; set; }
            public string Url { get; set; }
            public DateTime Created { get; set; }
        }
    }
}
