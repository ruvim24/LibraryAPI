﻿namespace LibraryWebAPI.Dtos.Book
{
    public class BookDto
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public int AuthorId { get; set; }
    }
}
