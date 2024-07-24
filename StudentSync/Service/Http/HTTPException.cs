using System;

namespace StudentSync.Service.Http
{
    [Serializable]
    public class HTTPException : Exception
    {
        public HTTPException()
        {

        }
        public HTTPException(string message) : base(message)
        {

        }
    }
}
 