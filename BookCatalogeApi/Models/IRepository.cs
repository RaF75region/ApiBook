using System;
namespace BookCatalogeApi.Models
{
    public interface IRepository
    {
        IQueryable<Book> Books { get; }
    }
}

