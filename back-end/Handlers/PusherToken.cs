using Newtonsoft.Json;

namespace AwsDotnetCsharp
{
    public partial class AppointmentController
    {
        public class PusherToken
        {
            [JsonProperty("app")]
            public string App { get; set; }

            [JsonProperty("iss")]
            public string Iss { get; set; }

            [JsonProperty("iat")]
            public long Iat { get; set; }

            [JsonProperty("exp")]
            public long Exp { get; set; }

            [JsonProperty("feeds")]
            public Feeds Feeds { get; set; }
        }
    }
}