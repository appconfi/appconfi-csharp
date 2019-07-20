namespace Appconfi
{
    using RestSharp;
    using System;

    public class BadRequestException<T> :Exception
    {
        public BadRequestException(IRestResponse<T> response): base(response.Content)
        {
            Response = response; 
        }

        public IRestResponse<T> Response { get; }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(IRestResponse response) : base(response.Content)
        {
            Response = response;
        }

        public IRestResponse Response { get; }
    }
}
