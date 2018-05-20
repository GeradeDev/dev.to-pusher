using Newtonsoft.Json;

namespace AwsDotnetCsharp
{
    public class FeedMessage
    {
        [JsonProperty("items")]
        public string[] Items { get; set; }
    }
}