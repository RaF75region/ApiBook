using System;
namespace BookCatalogeApi.Models
{
    public class ResultLogon
    {
        public string Name { get; set; }
        public string JwtToken { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

