﻿using Library.Core;
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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LibraryContext _context;

        public CategoryRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            var categoryAdded = await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return categoryAdded.Entity;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var categoryToDelete = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (categoryToDelete == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<Category>> GetCategoriesAsync(int page, int nr)
        {
            var count = _context.Categories.Count();
            var totalPages = (int)Math.Ceiling(count / (double)nr);
            var categories = await _context.Categories.Skip((page - 1) * nr).Take(nr).ToListAsync();

            return new PaginatedList<Category>(categories, page, totalPages);
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var categoryToUpdate = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
            if (categoryToUpdate == null)
            {
                throw new KeyNotFoundException();
            }

            categoryToUpdate.Name = category.Name;
            var categoryUpdated = _context.Update(categoryToUpdate);
            await _context.SaveChangesAsync();
            return categoryUpdated.Entity;
        }
    }
}
