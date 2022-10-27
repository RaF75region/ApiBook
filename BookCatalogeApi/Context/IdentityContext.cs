using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookCatalogeApi.Models;

namespace BookCatalogeApi.Context
{
    public class IdentityContext:IdentityDbContext<IdentityUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options):base(options)
        {
        }
    }
}

