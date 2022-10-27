using System;

namespace BookCatalogeApi.Models
{
    public class Book
    {
        public long BookID { get; set; }
        public string Cover { get; set; }
        public string Author { get; set; }
        public DateTime Year { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public string Category { get; set; }
    }
}

