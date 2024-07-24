namespace StudentSync.Controllers
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public List<string> Messages { get; set; }
        public bool Succeeded { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
    }

} 