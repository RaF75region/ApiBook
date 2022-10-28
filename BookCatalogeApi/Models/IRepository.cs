using System;
namespace BookCatalogeApi.Models
{
    public interface IRepository
    {
        IQueryable<Book> Books { get; }
        Task BookAdd(Book book);
        Task BookChange();
        Task BookDelete(Book book);
        Task<IQueryable<Book>> BooksSearch(string search);
        Task BookAddFavorites(BookFavorites bookFavorites);
    }
}

