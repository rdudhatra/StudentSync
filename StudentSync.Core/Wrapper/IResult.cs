using System.Collections.Generic;

namespace StudentSync.Core.Wrapper
{
    public interface IResult
    {

        List<string> Messages { get; set; }

        public HttpResponseMessage HttpResponseMessage { get; set; }

        bool Succeeded { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}