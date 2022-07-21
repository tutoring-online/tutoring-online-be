using Newtonsoft.Json;

namespace DataAccess.Models;

public class GoogleNotification
{
    public class DataPayload
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    [JsonProperty("priority")]
    public string Priority { get; set; } = "high";

    [JsonProperty("data")]
    public DataPayload Data { get; set; }
}