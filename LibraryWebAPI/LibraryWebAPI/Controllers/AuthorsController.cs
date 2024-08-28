using AutoMapper;
using FluentValidation;
using Library.Core;
using LibraryDataAcces.Models;
using LibraryDataAcces.Repozitories;
using LibraryWebAPI.Dtos.Author;
using LibraryWebAPI.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAuthorDto> _createValidator;

        public AuthorsController(
            IAuthorRepository authorRepository,
            IMapper mapper,
            IValidator<CreateAuthorDto> createValidator)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _createValidator = createValidator;
        }

        //    !!!

        //problema cu acest controller
        //imi pica tot api-ul din cauza acestei metode, daca o comentez restul merge bine
        //ma gandesc ca problema e in mapare,
        //dar la celelate controllere maparea merge bine cu PaginatedList

        [HttpGet]
        public async Task<PaginatedList<AuthorDto>> Get(int page, int nr)
        {
            var authors = await _authorRepository.GetAuthorsAsync(page, nr);

            var authorsDto = _mapper.Map<PaginatedList<AuthorDto>>(authors);
            return authorsDto;
        }


        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorDto>(author));
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthorDto author)
        {
            var validationResult = await _createValidator.ValidateAsync(author);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.GetErrorMessages());
            }
            var authorToCreate = _mapper.Map<Author>(author);

            var createResult = await _authorRepository.CreateAuthorAsync(authorToCreate);
            return Ok(_mapper.Map<AuthorDto>(createResult));
        }
        [HttpPut]
        public async Task<IActionResult> Update(AuthorDto author)
        {
            var authorToUpdate = _mapper.Map<Author>(author);

            try
            {
                var updateResult = await _authorRepository.UpdateAuthorAsync(authorToUpdate);
                return Ok(_mapper.Map<AuthorDto>(updateResult));
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _authorRepository.DeleteAuthorAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
