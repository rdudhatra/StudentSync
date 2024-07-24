using System.Collections.Generic;

namespace StudentSync.Service.Http
{
    public class BadRequestError
    {
        public string Status { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
 