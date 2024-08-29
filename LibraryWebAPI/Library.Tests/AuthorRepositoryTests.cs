using LibraryDataAcces.Data;
using LibraryDataAcces.Models;
using LibraryDataAcces.Repozitories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests
{
    public class AuthorRepositoryTests
    {
        private List<Author> _authors = new List<Author>()
        {
            new Author(){AuthorName = "testName1", AuthorId = 1},
            new Author(){AuthorName = "testName2", AuthorId = 2},
            new Author(){AuthorName = "testName3", AuthorId = 3},
            new Author(){AuthorName = "testName4", AuthorId = 4},


        };

        private int page = 1;
        private int pageSize = 10;
        //Task<List<Author>> GetAuthorsAsync();
        [Fact]
        public async Task AuthorRepositoryGetAuthors_ShouldReturnAuthors_IfAny()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            var repo = new AuthorRepository(context);

            context.Authors.AddRange(_authors);
            context.SaveChanges();

            var returnedAuthors = await repo.GetAuthorsAsync(page, pageSize);

            Assert.NotNull(returnedAuthors);
            Assert.True(returnedAuthors.Items.Any());
            Assert.Equal(returnedAuthors.Items, _authors);
            Assert.True(returnedAuthors.Items.Count == _authors.Count);
        }
        [Fact]
        public async Task AuthorRepositoryGetAuthors_ShouldReturnZeroAuthors_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            var repo = new AuthorRepository(context);

            var returnedAuthors = await repo.GetAuthorsAsync(page, pageSize);

            Assert.True(!context.Authors.Any());
            Assert.NotNull(returnedAuthors);
            Assert.True(returnedAuthors.Items.Count == 0);

        }

        [Fact]
        public async Task AuthorRepositoryGetAuthorsById_ShouldReturnAuthor_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            var repo = new AuthorRepository(context);
            context.Authors.AddRange(_authors);
            context.SaveChanges();

            var returnedAuthor = await repo.GetAuthorByIdAsync(_authors.ElementAt(0).AuthorId);
            Assert.NotNull(returnedAuthor);
            Assert.True(_authors.ElementAt(0).Equals(returnedAuthor));
        }
        [Fact]
        public async Task AuthorRepositoryGetAuthorsById_ShouldReturnNull_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            var repo = new AuthorRepository(context);

            var returnedAuthor = await repo.GetAuthorByIdAsync(_authors.ElementAt(0).AuthorId);

            Assert.Null(returnedAuthor);

        }


        [Fact]
        public async Task AuthorRepositoryCreateAuthor_ShouldCreateCategory_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            var repo = new AuthorRepository(context);

            var author = new Author() { AuthorName = "testName1", AuthorId = 1 };

            var createdAuthor = await repo.CreateAuthorAsync(author);
            Assert.NotNull(createdAuthor);
            Assert.Equal(createdAuthor.AuthorId, author.AuthorId);
        }
        [Fact]
        public async Task AuthorRepositoryCreateAuthor_ShouldThrowException_IfExist()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            context.Authors.Add(_authors.ElementAt(0));
            var repo = new AuthorRepository(context);

            var author = new Author() { AuthorName = "Test", AuthorId = 1 };
            Assert.ThrowsAsync<Exception>(async () => await repo.CreateAuthorAsync(author));

        }


        [Fact]
        public async Task AuthorRepositoryUpdateBook_ShouldUpdateBook_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            context.Authors.AddRange(_authors);
            context.SaveChanges();

            var repo = new AuthorRepository(context);
            var authorToBeUpdated = new Author() {AuthorId = 1, AuthorName = "Test" };
            await repo.UpdateAuthorAsync(authorToBeUpdated);

            var returnedUpdatedAuthor = context.Authors.FirstOrDefault(x => x.AuthorId == authorToBeUpdated.AuthorId);
            Assert.NotNull(returnedUpdatedAuthor);
            Assert.NotSame(returnedUpdatedAuthor, authorToBeUpdated);
            Assert.True(
                returnedUpdatedAuthor.AuthorName == authorToBeUpdated.AuthorName
                && returnedUpdatedAuthor.AuthorId == authorToBeUpdated.AuthorId
                );
        }

        [Fact]
        public async Task AuthorRepositoryUpdateBook_ShouldThrowNotFound_IfNotFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            var repo = new AuthorRepository(context);

            var authorToBeUpdated = new Author() { AuthorName = "test", AuthorId = 39};
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await repo.UpdateAuthorAsync(authorToBeUpdated));

        }


        [Fact]
        public async Task AuthorRepositoryRemoveAuthor_ShouldRemoveAuthor_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            var repo = new AuthorRepository(context);

            var author = new Author() { AuthorName = "test", AuthorId = 39 };
            context.Authors.Add(author);
            context.SaveChanges();

            await repo.DeleteAuthorAsync(author.AuthorId);
            Assert.True(!context.Authors.Any(x => x.AuthorId == author.AuthorId));
            Assert.Null(context.Authors.FirstOrDefault(x => x.AuthorId == author.AuthorId));
        }
        [Fact]
        public async Task AuthorRepositoryRemoveAuthor_ShouldReturnNotFoundException_IfNotFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            var repo = new AuthorRepository(context);
            Assert.True(!context.Authors.Any());
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await repo.DeleteAuthorAsync(2));
        }
    }

}

