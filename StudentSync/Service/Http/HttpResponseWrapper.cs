
//using Microsoft.AspNetCore.Components;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;


//namespace StudentSync.Service.Http
//{
//    public class HttpResponseWrapper<T>
//    {
//        public readonly NavigationManager _navigationManager;

//        public HttpResponseWrapper(bool success, HttpResponseMessage httpResponseMessage, T? responseModel)
//        {
//            Success = success;
//            HttpResponseMessage = httpResponseMessage;
//        }

//        public bool Success { get; set; }

//        public HttpResponseMessage HttpResponseMessage { get; set; }

//        public async Task<(T Data, bool IsSucceeded, string message)> GetResult()
//        {
//            try
//            {
//                var responseString = await HttpResponseMessage.Content.ReadAsStringAsync();
//                if (!string.IsNullOrWhiteSpace(responseString))
//                {
//                    try
//                    {
//                        var responseObject = JsonConvert.DeserializeObject<Response<T>>(responseString);
//                        return (responseObject.Data, responseObject.Succeeded, responseObject.Messages?.Count > 0 ? string.Join("\n", responseObject.Messages) : string.Empty);

//                    }
//                    catch
//                    {
//                        var responseObject = JsonConvert.DeserializeObject<ExtendedResult>(responseString);
//                        return (default(T), responseObject.Succeeded, responseObject.Messages?.Count > 0 ? string.Join("\n", responseObject.Messages) : string.Empty);
//                    }
//                }
//                else
//                    return (default(T), false, "");
//            }
//            catch
//            {
//                return (default(T), false, "");
//            }
//        }

//        public async Task<T> GetResultSet()
//        {
//            try
//            {
//                var responseString = await HttpResponseMessage.Content.ReadAsStringAsync();
//                return JsonConvert.DeserializeObject<T>(responseString);
//            }
//            catch
//            {
//                return default;
//            }
//        }

//        public async Task<List<string>> GetErrors()
//        {
//            List<string> errors = new();
//            if (HttpResponseMessage.StatusCode == HttpStatusCode.InternalServerError) { }
//            var response = await HttpResponseMessage.Content.ReadAsStringAsync();
//            ErrorResult result = null;
//            try
//            {
//                result = JsonConvert.DeserializeObject<ErrorResult>(response);
//            }
//            catch (Exception)
//            {
//                result = new ErrorResult()
//                {
//                    ErrorCode = (int)HttpResponseMessage.StatusCode,
//                    Succeeded = false
//                };
//            }
//            if (HttpResponseMessage.IsSuccessStatusCode)
//            {
//                HttpResponseMessage.StatusCode = (HttpStatusCode)result.ErrorCode;
//            }
//            if (result != null)
//                BindErrorMessage(result, errors);
//            else
//            {
//                if (!string.IsNullOrWhiteSpace(HttpResponseMessage.ReasonPhrase))
//                    errors.Add(HttpResponseMessage.ReasonPhrase);
//                else
//                    errors.Add(HttpResponseMessage.StatusCode.ToString());
//            }
           

//            return errors;
//        }

//        private static void BindErrorMessage(ErrorResult result, List<string> errors)
//        {
//            if (result?.Messages?.Count > 0)
//            {
//                errors.AddRange(result.Messages);
//            }
//            else if (!string.IsNullOrWhiteSpace(result.Exception))
//            {
//                errors.Add(result.Exception);
//            }
//            else
//            {
//                errors.Add("An internal error has occurred");
//            }
//        }

//        public async Task<PaginatedResult<T>> GetListResult()
//        {
//            var responseString = await HttpResponseMessage.Content.ReadAsStringAsync();
//            return JsonConvert.DeserializeObject<PaginatedResult<T>>(responseString);
//        }

//        public async Task<CampaignPaginatedResult<T>> GetCampaignListResult()
//        {
//            var responseString = await HttpResponseMessage.Content.ReadAsStringAsync();
//            return JsonConvert.DeserializeObject<CampaignPaginatedResult<T>>(responseString);
//        }
//    }

//    public class ErrorResult : Result
//    {
//        public string Source { get; set; }
//        public string Exception { get; set; }
//        public int ErrorCode { get; set; }
//    }

//    public class Response<T>
//    {

//        [JsonProperty("source")]
//        public string Source { get; set; }

//        [JsonProperty("exception")]
//        public string Exception { get; set; }

//        [JsonProperty("errorCode")]
//        public int ErrorCode { get; set; }

//        [JsonProperty("data")]
//        public T Data { get; set; }

//        [JsonProperty("messages")]
//        public List<string> Messages { get; set; }

//        [JsonProperty("succeeded")]
//        public bool Succeeded { get; set; }
//    }

//    public class Result : IResult
//    {
//        public Result()
//        {
//        }

//        public List<string> Messages { get; set; } = new();

//        public bool Succeeded { get; set; }

//        public static IResult Fail()
//        {
//            return new Result { Succeeded = false };
//        }

//        public static IResult Fail(string message)
//        {
//            return new Result { Succeeded = false, Messages = new List<string> { message } };
//        }

//        public static IResult Fail(List<string> messages)
//        {
//            return new Result { Succeeded = false, Messages = messages };
//        }

//        public static Task<IResult> FailAsync()
//        {
//            return Task.FromResult(Fail());
//        }

//        public static Task<IResult> FailAsync(string message)
//        {
//            return Task.FromResult(Fail(message));
//        }

//        public static Task<IResult> FailAsync(List<string> messages)
//        {
//            return Task.FromResult(Fail(messages));
//        }

//        public static Task<ErrorResult> ReturnError(string message)
//        {
//            return Task.FromResult(new ErrorResult()
//            {
//                Messages = new List<string>() { message },
//                ErrorCode = 500
//            });
//        }

//        public static Task<ErrorResult> ReturnError(string message, int errorCode)
//        {
//            return Task.FromResult(new ErrorResult()
//            {
//                Messages = new List<string>() { message },
//                ErrorCode = errorCode
//            });
//        }

//        public static IResult Success()
//        {
//            return new Result { Succeeded = true };
//        }

//        public static IResult Success(string message)
//        {
//            return new Result { Succeeded = true, Messages = new List<string> { message } };
//        }

//        public static IResult Success(List<string> messages)
//        {
//            return new Result { Succeeded = true, Messages = messages };
//        }

//        public static Task<IResult> SuccessAsync()
//        {
//            return Task.FromResult(Success());
//        }

//        public static Task<IResult> SuccessAsync(string message)
//        {
//            return Task.FromResult(Success(message));
//        }

//        public static Task<IResult> SuccessAsync(List<string> messages)
//        {
//            return Task.FromResult(Success(messages));
//        }
//    }

//    public interface IResult
//    {
//        List<string> Messages { get; set; }

//        bool Succeeded { get; set; }
//    }

//    public class PaginatedResult<T> : Result
//    {
//        public PaginatedResult(List<T> data)
//        {
//            Data = data;
//        }

//        public List<T> Data { get; set; }

//        internal PaginatedResult(bool succeeded, List<T> data = default, List<string> messages = null, int count = 0, int page = 1, int pageSize = 10)
//        {
//            Data = data;
//            CurrentPage = page;
//            Succeeded = succeeded;
//            PageSize = pageSize;
//            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
//            TotalCount = count;
//        }

//        public static PaginatedResult<T> Failure(List<string> messages)
//        {
//            return new(false, default, messages);
//        }

//        public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize)
//        {
//            return new(true, data, null, count, page, pageSize);
//        }

//        public int CurrentPage { get; set; }

//        public int TotalPages { get; set; }

//        public int TotalCount { get; set; }
//        public int PageSize { get; set; }

//        public bool HasPreviousPage => CurrentPage > 1;

//        public bool HasNextPage => CurrentPage < TotalPages;
//    }

//    public class CampaignPaginatedResult<T> : Result
//    {
//        public CampaignPaginatedResult(List<T> data)
//        {
//            Data = data;
//        }

//        public List<T> Data { get; set; }

//        internal CampaignPaginatedResult(bool succeeded, List<T> data = default, List<string> messages = null, int count = 0, int page = 1, int pageSize = 10)
//        {
//            Data = data;
//            CurrentPage = page;
//            Succeeded = succeeded;
//            PageSize = pageSize;
//            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
//            TotalCount = count;
//        }

//        public static PaginatedResult<T> Failure(List<string> messages)
//        {
//            return new(false, default, messages);
//        }

//        public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize)
//        {
//            return new(true, data, null, count, page, pageSize);
//        }

//        public int CurrentPage { get; set; }

//        public int TotalPages { get; set; }

//        public int TotalCount { get; set; }
//        public int TotalDrafts { get; set; }
//        public int TotalScheduled { get; set; }
//        public int TotalSent { get; set; }
//        public int TotalDeleted { get; set; }
//        public int TotalSending { get; set; }
//        public int TotalDelayed { get; set; }
//        public int TotalError { get; set; }
//        public int TotalSendingTest { get; set; }
//        public int TotalWaiting4Test { get; set; }
//        public int PageSize { get; set; }

//        public bool HasPreviousPage => CurrentPage > 1;

//        public bool HasNextPage => CurrentPage < TotalPages;
//    }

//    public class Data
//    {
//    }

//    public class ExtendedResult : Result
//    {
//        public Data Data { get; set; }
//    }
//}
