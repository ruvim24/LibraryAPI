using AutoMapper;
using Library.Core;
using LibraryDataAcces.Models;
using LibraryWebAPI.Dtos.Author;
using LibraryWebAPI.Dtos.Book;

namespace LibraryWebAPI.Profiles
{
    public class BookProfiles : Profile
    {
        public BookProfiles()
        {
            CreateMap<BookDto, Book>().ReverseMap();
            CreateMap<CreateBookDto, Book>().ReverseMap();
            CreateMap<PaginatedList<Book>, PaginatedList<BookDto>>();

        }
    }
}
