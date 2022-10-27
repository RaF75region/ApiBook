using System;
using Microsoft.EntityFrameworkCore;
using BookCatalogeApi.Models;

namespace BookCatalogeApi.Context
{
    public class ContextDataBase: DbContext
    {
        public ContextDataBase(DbContextOptions<ContextDataBase> options) : base(options)
        {
           
        }

        public DbSet<Book> Books { get; set; }
    }
}

