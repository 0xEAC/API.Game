using System.Text.Json.Serialization;

namespace Learning.Models
{
  public class Animal
  {
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("age")]
    public int Age { get; set; }
    [JsonPropertyName("health")]
    public int Health { get; set; }
    [JsonPropertyName("hunger")]
    public int Hunger { get; set; }
    [JsonPropertyName("hungry")]
    public bool Hungry { get; set; }
    [JsonPropertyName("last_fed")]
    public DateTime lastFed { get; set; }
  }
}
