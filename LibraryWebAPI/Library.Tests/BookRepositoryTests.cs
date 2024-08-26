using LibraryDataAcces.Data;
using LibraryDataAcces.Models;
using LibraryDataAcces.Repozitories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests
{
    public class BookRepositoryTests
    {
        private List<Book> _books = new List<Book>
        {
            new Book(){Title = "TitleTest", Price = 123, AuthorId = 1, BookId = 1, CategoryId = 1},
            new Book(){Title = "TitleTest1", Price = 123, AuthorId = 2, BookId = 2, CategoryId = 2},
            new Book(){Title = "TitleTest2", Price = 123, AuthorId = 3, BookId = 3, CategoryId = 3},
            new Book(){Title = "TitleTest3", Price = 123, AuthorId = 4, BookId = 4, CategoryId = 4},
            new Book(){Title = "TitleTest4", Price = 123, AuthorId = 5, BookId = 5, CategoryId = 5},
            new Book(){Title = "TitleTest5", Price = 123, AuthorId = 6, BookId = 6, CategoryId = 6},

        };
        
        [Fact]
        public async Task BookRepositoryGetAllBooks_ShouldReturnZero_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            
            var context = new LibraryContext(options);
            IBookRepository repo = new BookRepository(context);

            Assert.True(!context.Books.Any());
            var books = await repo.GetAllBooksAsync();
            Assert.True(books.Count == 0);

        }
        [Fact]
        public async Task BookRepositoryGetAllBooks_ShouldReturnAllBooks_IfExist()
        {
            var option = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase (Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(option);
            context.Books.AddRange(_books);
            context.SaveChanges();

            var repo = new BookRepository(context);

            Assert.True(context.Books.Any());
            var books = await repo.GetAllBooksAsync();
            Assert.True(books.Any());
            Assert.True(books.Count ==  _books.Count);
            Assert.Equal(books, _books);
        }
        [Fact]
        public async Task BookRepositoryCreateBook_ShouldReturnBook_IfNone() 
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);

            var repo = new BookRepository(context);
            var book = new Book() { Title = "Test", Price = 123, AuthorId = 321, BookId = 1, CategoryId = 1 };

            Assert.True (!context.Books.Any(x=> x.BookId == book.BookId));
            var TBook = await repo.CreateBookAsync(book);
            Assert.NotNull(TBook);
            Assert.True(context.Books.Any(x => x.BookId == book.BookId));

        }
        [Fact]
        public async Task BookRepositoryCreateBook_ShouldThrowException_IfExist()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            var book = new Book() { Title = "Test", Price = 123, AuthorId = 321, BookId = 1, CategoryId = 1 };
            context.Books.Add(book);
            context.SaveChanges();

            var repo = new BookRepository(context);


            Assert.True(context.Books.Any(x => x.BookId == book.BookId));
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await repo.CreateBookAsync(book));


        }
        [Fact]
        public async Task BookRepositoryGetBookById_ShouldReturnBook_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);

            var book = new Book() { Title = "Test", Price = 123, AuthorId = 321, BookId = 1, CategoryId = 1 };
            context.Books.Add(book);
            context.SaveChanges();
            var repo = new BookRepository(context);
            var returnedBook = await repo.GetBookByIdAsync(book.BookId);
            Assert.NotNull(returnedBook);
            Assert.Equal(returnedBook, book);
        }
        [Fact]
        public async Task BookRepositoryGetBookById_ShouldReturnNull_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase (Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            var repo = new BookRepository(context);

            var returnedBook = await repo.GetBookByIdAsync(74);

            Assert.Null(returnedBook);
        }

        [Fact]
        public async Task BookRepositoryRemoveBook_ShouldRemoveBook_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            var repo = new BookRepository(context);

            var book = new Book() { Title = "Test", Price = 123, AuthorId = 321, BookId = 1, CategoryId = 1 };
            context.Books.Add(book);
            context.SaveChanges();

            await repo.RemoveBookAsync(book.BookId);
            Assert.True(!context.Books.Any(x => x.BookId == book.BookId));
            Assert.Null(context.Books.FirstOrDefault(x => x.BookId == book.BookId));   
        }
        [Fact]
        public async Task BookRepositoryRemoveBook_ShouldReturnNotFoundException_IfNotFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            var repo = new BookRepository(context);
            Assert.True(!context.Books.Any());
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await repo.RemoveBookAsync(2));
        }

        [Fact]
        public async Task BookRepositoryUpdateBook_ShouldUpdateBookIfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            context.Books.AddRange(_books);
            context.SaveChanges();

            var repo = new BookRepository(context);
            var bookToBeUpdated = new Book() { Title = "Test", Price = 123, AuthorId = 321, BookId = 1, CategoryId = 34 };
            await repo.UpdateBookAsync(bookToBeUpdated);

            var returnedUpdatedBook = context.Books.FirstOrDefault(x => x.BookId == bookToBeUpdated.BookId);
            Assert.NotNull(returnedUpdatedBook);
            Assert.NotSame(returnedUpdatedBook, bookToBeUpdated);
            Assert.True(
                returnedUpdatedBook.Title == bookToBeUpdated.Title 
                && returnedUpdatedBook.Price == bookToBeUpdated.Price
                && returnedUpdatedBook.AuthorId == bookToBeUpdated.AuthorId
                && returnedUpdatedBook.CategoryId == bookToBeUpdated.CategoryId
                );
        }

        [Fact]
        public async Task BookRepositoryUpdateBook_ShouldThrowNotFoundException_IfNotFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            var repo = new BookRepository(context);

            var bookToBeUpdated = new Book() { Title = "Test", Price = 123, AuthorId = 321, BookId = 1, CategoryId = 34 };
            Assert.ThrowsAsync<KeyNotFoundException>(async ()=> await repo.UpdateBookAsync(bookToBeUpdated));

        }
    }
}
