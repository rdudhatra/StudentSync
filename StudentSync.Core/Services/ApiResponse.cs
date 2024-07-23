namespace StudentSync.Core.Services
{
    internal class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public string Data { get; set; }
    }
}