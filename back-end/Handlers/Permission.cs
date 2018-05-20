using Newtonsoft.Json;

namespace AwsDotnetCsharp
{
    public class Permission
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }
    }
}