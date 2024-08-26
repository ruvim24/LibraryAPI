using LibraryDataAcces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDataAcces.Repozitories
{
    public interface IBookRepository
    {
        public Task<Book> CreateBookAsync(Book book);
        public Task<List<Book>> GetAllBooksAsync();
        public Task<Book?> GetBookByIdAsync(int id);
        public Task RemoveBookAsync(int id);
        public Task UpdateBookAsync(Book book);
        
    }
}
