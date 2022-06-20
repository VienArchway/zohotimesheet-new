using Newtonsoft.Json;

namespace api.Models;

public record ZohoUser
{
    [JsonProperty(PropertyName = "First_Name")]
    public string? FirstName { get; init; }
    
    public string? Email { get; init; }
    
    [JsonProperty(PropertyName = "Last_Name")]
    public string? LastName { get; init; }
    
    [JsonProperty(PropertyName = "Display_Name")]
    public string? DisplayName { get; init; }
    
    public string? Zuid { get; init; }
};