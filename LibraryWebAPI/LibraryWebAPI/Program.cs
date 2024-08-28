using FluentValidation;
using LibraryDataAcces.Data;
using LibraryDataAcces.Repozitories;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;


            // Add services to the container.
            builder.Services.AddDbContext<LibraryContext>(options => {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConection"));
            });

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
