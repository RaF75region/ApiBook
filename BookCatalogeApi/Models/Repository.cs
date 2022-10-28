using System;
using BookCatalogeApi.Context;

namespace BookCatalogeApi.Models
{
    public class Repository : IRepository
    {
        private readonly ContextDataBase context;

        public Repository(ContextDataBase contextDataBase)
        {
            this.context = contextDataBase;
        }

        public IQueryable<Book> Books => context.Books;

        public Task BookAdd(Book book)
        {
           return Task.Run(() => {
                context.Books.Add(book);
                context.SaveChanges();
                });
        }

        public async Task BookChange() => await context.SaveChangesAsync();

        public Task BookDelete(Book book)
        {
            return Task.Run(() => {
                context.Books.Remove(book);
                context.SaveChanges();
            });
        }

        public Task<IQueryable<Book>> BooksSearch(string search)
        {
            return Task.Run(()=>
            context.Books.Where(opt => 
                opt.Category.Contains(search.ToLower()) ||
                opt.Description.Contains(search.ToLower())||
                opt.Author.Contains(search.ToLower())||
                opt.Title.Contains(search.ToLower()) ||
                opt.Year.ToString().Contains(search.ToLower())
            ));
        }

        public Task BookAddFavorites(BookFavorites bookFavorites)
        {
            return Task.Run(()=>
            {
                context.BookFavorites.Add(bookFavorites);
                context.SaveChanges();
            });
        }
    }
}

