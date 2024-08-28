using AutoMapper;
using Library.Core;
using LibraryDataAcces.Models;
using LibraryWebAPI.Dtos.Author;
using LibraryWebAPI.Dtos.Category;

namespace LibraryWebAPI.Profiles
{
    public class CategoryProfiles : Profile
    {
        public CategoryProfiles()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>().ReverseMap();
            CreateMap<PaginatedList<Category>, PaginatedList<CategoryDto>>();

        }
    }
}
