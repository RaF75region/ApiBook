using System;
using System.ComponentModel.DataAnnotations;

namespace BookCatalogeApi.Models
{
    public class Book
    {
        public long BookID { get; set; }
        public byte[] Cover { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public string Category { get; set; }
    }

    public class BookAdd {
        public long? Book_ID { get; set; }
        public IFormFile Cover { get; set; }
        public string Author { get; set; }
        public DateTime Year { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public CategoryBook Category { get; set; }
    }

    public class BookMessage
    {
        public long? Book_ID { get; set; }
        public bool Succes { get; set; }
        public string Message { get; set; }
    }

    public class BookFavorites
    {
        public long BookFavoritesID { get; set; }
        public string User_ID { get; set; }
        public long Book_ID { get; set; }
    }

    public enum CategoryBook
    {
        Comics,
        Computers,
        Health,
        History,
        Cooking
    }

}

