﻿using Library.Core;
using LibraryDataAcces.Data;
using LibraryDataAcces.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDataAcces.Repozitories
{
    public class BookRepository : IBookRepository
    {
        public readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task<PaginatedList<Book>> GetAllBooksAsync(int page, int nr)
        {
            var count = _context.Books.Count();
            var totalPages = (int)Math.Ceiling(count / (double)nr);
            var books = await _context.Books.Skip((page - 1) * nr).Take(nr).ToListAsync();

            return new PaginatedList<Book>(books, page, totalPages);
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            var temp = _context.Books.FirstOrDefault(x => x.BookId == book.BookId);
            if (temp != null)
            {
                throw new KeyNotFoundException();
            }
            _context.Books.Add(book);
            _context.SaveChanges();
            return await _context.Books.FirstOrDefaultAsync(x => x.BookId == book.BookId);
            
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            var returnedBook = _context.Books.FirstOrDefault(x => x.BookId == id);
            if(returnedBook == null)
            {
                return null;
            }
            return returnedBook;
        }

        public async Task RemoveBookAsync(int id)
        {
            var returnedBook = _context.Books.FirstOrDefault(x => x.BookId == id);
            if (returnedBook == null)
            {
                throw new KeyNotFoundException();
            }
            _context.Books.Remove(returnedBook);
            _context.SaveChanges();
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            var returnedBook = _context.Books.FirstOrDefault(x => x.BookId == book.BookId);
            if (returnedBook == null)
            {
                throw new KeyNotFoundException();
            }
            returnedBook.AuthorId = book.AuthorId;
            returnedBook.CategoryId = book.CategoryId;
            returnedBook.Title = book.Title;
            returnedBook.Price = book.Price;
            
            
            var updatedBook = _context.Books.Update(returnedBook);
            _context.SaveChanges();
            return updatedBook.Entity;
        }
    }
}
