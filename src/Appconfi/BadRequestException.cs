namespace Appconfi
{
    using System;
    using System.Net.Http;

    public class BadRequestException : Exception
    {
        public BadRequestException(HttpResponseMessage response) : base(response.ReasonPhrase)
        {
            Response = response;
        }

        public HttpResponseMessage Response { get; }
    }
}
