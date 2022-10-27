using System;
using BookCatalogeApi.Context;

namespace BookCatalogeApi.Models
{
    public class Repository:IRepository
    {
        private readonly ContextDataBase context;

        public Repository(ContextDataBase contextDataBase)
        {
            this.context = contextDataBase;
        }

        public IQueryable<Book> Books => context.Books;

    }
}

