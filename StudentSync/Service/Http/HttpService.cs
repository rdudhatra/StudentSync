using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudentSync.Data.Models;
using StudentSync.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudentSync.Service.Http
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly ILogger<HttpService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private readonly List<string> _identitiesURLs = new()
        {
            "login",
            "confirm-email",
            "confirm-register",
            "forgot-password",
            "google-signin",
            "logout",
            "register",
            "tokens/refresh",
            "reset-password",
            "forgot-password-confirm"
        };

        public HttpService(HttpClient httpClient, NavigationManager navigationManager,
            ILogger<HttpService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _logger = logger;
            AddDefaultRequestHeaders();
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddDefaultRequestHeaders()
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("origin"))
            {
                _httpClient.DefaultRequestHeaders.Add("origin", GetBaseURI());
            }
        }

        private string GetBaseURI()
        {
            try
            {
                return _navigationManager.BaseUri;
            }
            catch
            {
                return string.Empty;
            }
        }

        private async Task<string> GetAuthTokenAsync()
        {
            // Retrieve your token from cookies
            var authToken = _httpContextAccessor.HttpContext.Request.Cookies["authToken"];
            //authToken = authToken + "DD";
            return await Task.FromResult(authToken);
        }

        private void SetAuthHeader(string authToken)
        {
            // Remove any existing Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = null;

            // Set the new Authorization header with Bearer token
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url, CancellationToken cancellationToken = default)
        {
            var authToken = await GetAuthTokenAsync();
            SetAuthHeader(authToken);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendRequest(request, cancellationToken);
            return await CreateResponseAsync<T>(response, url, null, "GET");
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url, Dictionary<string, string> requestHeaders, CancellationToken cancellationToken = default)
        {
            var authToken = await GetAuthTokenAsync();
            SetAuthHeader(authToken);

            if (requestHeaders != null)
            {
                foreach (var item in requestHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Remove(item.Key);
                    _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendRequest(request, cancellationToken);
            return await CreateResponseAsync<T>(response, url, null, "GET");
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T data, CancellationToken cancellationToken = default)
        {
            var authToken = await GetAuthTokenAsync();
            SetAuthHeader(authToken);

            var dataJson = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(dataJson, Encoding.UTF8, "application/json")
            };
            var response = await SendRequest(request, cancellationToken);
            return await CreateResponseAsync<object>(response, url, dataJson, "POST");
        }

        public async Task<HttpResponseWrapper<object>> Post(string url, MultipartFormDataContent data, CancellationToken cancellationToken = default)
        {
            var authToken = await GetAuthTokenAsync();
            SetAuthHeader(authToken);

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = data
            };
            var response = await SendRequest(request, cancellationToken);
            return await CreateResponseAsync<object>(response, url, null, "POST");
        }

        public async Task<HttpResponseWrapper<object>> Put(string url, MultipartFormDataContent data, CancellationToken cancellationToken = default)
        {
            var authToken = await GetAuthTokenAsync();
            SetAuthHeader(authToken);

            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = data
            };
            var response = await SendRequest(request, cancellationToken);
            return await CreateResponseAsync<object>(response, url, null, "PUT");
        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T data, CancellationToken cancellationToken = default)
        {
            var authToken = await GetAuthTokenAsync();
            SetAuthHeader(authToken);

            var dataJson = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(dataJson, Encoding.UTF8, "application/json")
            };
            var response = await SendRequest(request, cancellationToken);
            return await CreateResponseAsync<object>(response, url, dataJson, "PUT");
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data, CancellationToken cancellationToken = default)
        {
            var authToken = await GetAuthTokenAsync();
            SetAuthHeader(authToken);

            var dataJson = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(dataJson, Encoding.UTF8, "application/json")
            };
            var response = await SendRequest(request, cancellationToken);
            return await CreateResponseAsync<TResponse>(response, url, dataJson, "POST");
        }

        public async Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T data, CancellationToken cancellationToken = default)
        {
            var authToken = await GetAuthTokenAsync();
            SetAuthHeader(authToken);

            var dataJson = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(dataJson, Encoding.UTF8, "application/json")
            };
            var response = await SendRequest(request, cancellationToken);
            return await CreateResponseAsync<TResponse>(response, url, dataJson, "PUT");
        }

        public async Task<HttpResponseWrapper<object>> Delete(string url, CancellationToken cancellationToken = default)
        {
            var authToken = await GetAuthTokenAsync();
            SetAuthHeader(authToken);

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await SendRequest(request, cancellationToken);
            return await CreateResponseAsync<object>(response, url, null, "DELETE");
        }

        private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            var apiURL = request.RequestUri?.ToString();

            var obj = new
            {
                ErrorCode = 500,
                Messages = new List<string> { "Something went wrong. Please try again." },
                Succeeded = false
            };

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            try
            {
                if (Constants.ANONYMOUSAPI.Any(a => apiURL?.Contains(a) == true))
                {
                    return await _httpClient.SendAsync(request, cancellationToken);
                }

                httpResponseMessage = await _httpClient.SendAsync(request, cancellationToken);
                return httpResponseMessage;
            }
            catch (OperationCanceledException)
            {
                httpResponseMessage.Content = new StringContent(JsonConvert.SerializeObject(obj));
                return httpResponseMessage;
            }
            catch (Exception ex)
            {
                if (ex.InnerException is System.Net.Sockets.SocketException socketException && socketException.ErrorCode == 10061)
                {
                    obj = new
                    {
                        ErrorCode = 502,
                        Messages = new List<string> { "Network error! Please try again." },
                        Succeeded = false
                    };
                    httpResponseMessage.StatusCode = HttpStatusCode.BadGateway;
                    _logger.LogError(ex, "Unable to connect HTTP connection.");
                }
                else
                {
                    _logger.LogError(ex, $"URL: {request.RequestUri} {Environment.NewLine} Message: Error into SendRequest method of HttpService class.");
                }
                httpResponseMessage.Content = new StringContent(JsonConvert.SerializeObject(obj));
                return httpResponseMessage;
            }
         }

        public async Task<HttpResponseWrapper<T>> CreateResponseAsync<T>(HttpResponseMessage response, string url, string requestContent, string method)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var responsemodel = JsonConvert.DeserializeObject<T>(content);
                    return new HttpResponseWrapper<T>(true, response , responsemodel);
                }

                ErrorResult result = null;
                try
                {
                    result = JsonConvert.DeserializeObject<ErrorResult>(content);
                }
                catch
                {
                }
                if (result != null && (result.ErrorCode < 200 || result.ErrorCode > 299))
                {
                    response.StatusCode = (HttpStatusCode)result.ErrorCode;
                }

                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _navigationManager.NavigateTo("/login");
                }
                // Provide more specific error handling for `401 Unauthorized`
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Optionally log the detailed response for debugging
                    _logger.LogWarning($"Unauthorized access attempt: {content}");

                    // You might want to clear the expired token or prompt the user
                    // _httpContextAccessor.HttpContext.Response.Cookies.Delete("authToken");
                    // _navigationManager.NavigateTo("/login");
                }

                var responseModel = JsonConvert.DeserializeObject<T>(content);
                return new HttpResponseWrapper<T>(false, response, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in CreateResponseAsync for URL {url}");
                return new HttpResponseWrapper<T>(false, response);
            }
        }
    }

    public class ErrorResult
    {
        public int ErrorCode { get; set; }
        public List<string> Messages { get; set; }
        public bool Succeeded { get; set; }
    }

    public interface IHttpService
    {
        Task<HttpResponseWrapper<T>> Get<T>(string url, CancellationToken cancellationToken = default);
        Task<HttpResponseWrapper<T>> Get<T>(string url, Dictionary<string, string> requestHeaders, CancellationToken cancellationToken = default);
        Task<HttpResponseWrapper<object>> Post<T>(string url, T data, CancellationToken cancellationToken = default);
        Task<HttpResponseWrapper<object>> Post(string url, MultipartFormDataContent data, CancellationToken cancellationToken = default);
        Task<HttpResponseWrapper<object>> Put(string url, MultipartFormDataContent data, CancellationToken cancellationToken = default);
        Task<HttpResponseWrapper<object>> Put<T>(string url, T data, CancellationToken cancellationToken = default);
        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data, CancellationToken cancellationToken = default);
        Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T data, CancellationToken cancellationToken = default);
        Task<HttpResponseWrapper<object>> Delete(string url, CancellationToken cancellationToken = default);
    }

    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(bool succeeded, HttpResponseMessage response, T data = default)
        {
            Succeeded = succeeded;
            Response = response;
            Data = data;
        }

        public bool Succeeded { get; set; }
        public HttpResponseMessage Response { get; set; }
        public T Data { get; set; }
    }
}

