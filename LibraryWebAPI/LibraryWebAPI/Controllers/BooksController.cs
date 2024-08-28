using AutoMapper;
using FluentValidation;
using Library.Core;
using LibraryDataAcces.Models;
using LibraryDataAcces.Repozitories;
using LibraryWebAPI.Dtos.Book;
using LibraryWebAPI.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookDto> _createValidator;

        public BooksController(
            IBookRepository bookRepository,
            IMapper mapper,
            IValidator<CreateBookDto> createVaalidator)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _createValidator = createVaalidator;
        }

        [HttpGet]
        public async Task<PaginatedList<BookDto>> Get(int page, int nr)
        {
            var books = await _bookRepository.GetAllBooksAsync(page, nr);
            return _mapper.Map<PaginatedList<BookDto>>(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookDto>(book));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookDto book)
        {
            var validationResult = await _createValidator.ValidateAsync(book);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.GetErrorMessages());
            }

            var bookToCreate = _mapper.Map<Book>(book);

            var createResult = await _bookRepository.CreateBookAsync(bookToCreate);
            return Ok(_mapper.Map<BookDto>(createResult));
        }

        [HttpPut]
        public async Task<IActionResult> Update(BookDto book)
        {
            var bookToUpate = _mapper.Map<Book>(book);

            try
            {
                var updateResult = await _bookRepository.UpdateBookAsync(bookToUpate);
                return Ok(_mapper.Map<BookDto>(updateResult));
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
                await _bookRepository.RemoveBookAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
