//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;

//namespace StudentSync.Service.Http
//{
//    public interface IHttpService
//    {
//        Task<HttpResponseWrapper<object>> Delete(string url, CancellationToken cancellationToken = default);
//        Task<HttpResponseWrapper<T>> Get<T>(string url, CancellationToken cancellationToken = default);
//        Task<HttpResponseWrapper<T>> Get<T>(string url, Dictionary<string, string> requestHeaders, CancellationToken cancellationToken = default);
//        Task<HttpResponseWrapper<object>> Post<T>(string url, T data, CancellationToken cancellationToken = default);
//        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data, CancellationToken cancellationToken = default);
//        Task<HttpResponseWrapper<object>> Put<T>(string url, T data, CancellationToken cancellationToken = default);
//        Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T data, CancellationToken cancellationToken = default);
//        Task<HttpResponseWrapper<object>> Post(string url, MultipartFormDataContent data, CancellationToken cancellationToken = default);
//        Task<HttpResponseWrapper<object>> Put(string url, MultipartFormDataContent data, CancellationToken cancellationToken = default);
//    }
//}
