using AutoMapper;
using Library.Core;
using LibraryDataAcces.Models;
using LibraryWebAPI.Dtos.Author;

namespace LibraryWebAPI.Profiles
{
    public class AuthorProfiles : Profile
    {
        public AuthorProfiles()
        {
            CreateMap<AuthorDto, Author>().ReverseMap();
            CreateMap<CreateAuthorDto, Author>().ReverseMap();
            CreateMap<PaginatedList<Author>, PaginatedList<AuthorDto>>();
        }
    }
}


