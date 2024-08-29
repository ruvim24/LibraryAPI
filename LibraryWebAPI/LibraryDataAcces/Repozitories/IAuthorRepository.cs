using Library.Core;
using LibraryDataAcces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDataAcces.Repozitories
{
    public interface IAuthorRepository
    {
        Task<PaginatedList<Author>> GetAuthorsAsync(int page, int nr);

        Task<Author?> GetAuthorByIdAsync(int id);

        Task<Author> CreateAuthorAsync(Author author);

        Task<Author> UpdateAuthorAsync(Author author);

        Task DeleteAuthorAsync(int id);
    }
}
