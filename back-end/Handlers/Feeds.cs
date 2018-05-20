using Newtonsoft.Json;

namespace AwsDotnetCsharp
{
    public partial class AppointmentController
    {
        public class Feeds
        {
            [JsonProperty("permission")]
            public Permission Permission { get; set; }
        }
    }
}