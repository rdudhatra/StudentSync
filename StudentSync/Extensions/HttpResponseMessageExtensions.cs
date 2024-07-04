using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StudentSync.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<List<string>> GetErrors(this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage), "HttpResponseMessage cannot be null.");
            }


            List<string> errors = new();
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            ErrorResult result = null;

            try
            {
                result = JsonConvert.DeserializeObject<ErrorResult>(responseContent);
            }
            catch
            {
                result = new ErrorResult()
                {
                    ErrorCode = (int)httpResponseMessage.StatusCode,
                    Succeeded = false
                };
            }

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                if (result != null)
                {
                    BindErrorMessage(result, errors);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(httpResponseMessage.ReasonPhrase))
                    {
                        errors.Add(httpResponseMessage.ReasonPhrase);
                    }
                    else
                    {
                        errors.Add(httpResponseMessage.StatusCode.ToString());
                    }
                }

                if (httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError)
                {
                    errors.Add("An internal server error has occurred.");
                }
            }

            return errors;
        }

        private static void BindErrorMessage(ErrorResult result, List<string> errors)
        {
            if (result?.Messages?.Count > 0)
            {
                errors.AddRange(result.Messages);
            }
            else if (!string.IsNullOrWhiteSpace(result.Exception))
            {
                errors.Add(result.Exception);
            }
            else
            {
                errors.Add("An internal error has occurred");
            }
        }

        private class ErrorResult
        {
            public int ErrorCode { get; set; }
            public bool Succeeded { get; set; }
            public List<string> Messages { get; set; }
            public string Exception { get; set; }
        }
    }
}
