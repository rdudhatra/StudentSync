using Newtonsoft.Json;

using System.Collections.Generic;

namespace StudentSync.Service.Http
{
    public class APIError
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("exception")]
        public string Exception { get; set; }

        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("messages")]
        public List<string> Messages { get; set; }

        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }
    }
}
 