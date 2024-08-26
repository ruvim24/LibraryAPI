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
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _context;

        public AuthorRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task<List<Author>> GetAuthorsAsync()
        {
            return  _context.Authors.ToList();
        }
        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            var returnedAuthor = _context.Authors.FirstOrDefault(x => x.AuthorId == id);
            if (returnedAuthor == null)
            {
                return null;
            }
            return returnedAuthor;
        }
        public async Task<Author> CreateAuthorAsync(Author author)
        {
            var returnedAuthor = await _context.Authors.FirstOrDefaultAsync(x => x.AuthorId ==  author.AuthorId);
            if (returnedAuthor != null)
            {
                throw new Exception("Exiting author");
            }
            var createdAuhtor =  await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return createdAuhtor.Entity;
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = _context.Authors.FirstOrDefault(x => x.AuthorId == id);
            if (author == null)
            {
                throw new KeyNotFoundException();
            }
            _context.Authors.Remove(author);
            _context.SaveChanges();
        }

        public async Task<Author> UpdateAuthorAsync(Author author)
        {
            var authorToBeUpdated = await _context.Authors.FirstOrDefaultAsync(x => x.AuthorId == author.AuthorId);
            if (authorToBeUpdated == null)
            {
                throw new KeyNotFoundException();
            }
            authorToBeUpdated.AuthorName = author.AuthorName;
            _context.SaveChanges();
            return authorToBeUpdated;
        }
    }
}
