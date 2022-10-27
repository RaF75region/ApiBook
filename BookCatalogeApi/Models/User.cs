using System;
namespace BookCatalogeApi.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role RoleUser { get; set; }
    }
}

