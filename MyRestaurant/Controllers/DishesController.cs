﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestaurant.Models;

namespace MyRestaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly MenuDBContext _context;

        public DishesController(MenuDBContext context)
        {
            _context = context;
        }

        // GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
        {
            if (_context.Dishes == null)
            {
                return NotFound();
            }
            
            List<Dish> dishLish = new List<Dish>();
            foreach (var dish in _context.Dishes)
            {
                if(dish.IsDeleted == false)
                {
                    dishLish.Add(dish);
                }
            }
            return dishLish;
        }

        // GET: api/Dishes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
          if (_context.Dishes == null)
          {
              return NotFound();
          }
            var dish = await _context.Dishes.FindAsync(id);

            if ((dish == null) || (dish.IsDeleted == true))
            {
                return NotFound();
            }

            return dish;
        }

        // GET: using Category Id
        [HttpGet("CategoryId = {CategoryId}")]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes(int CategoryId)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }

            List<CategoryDish> categoryDishes = await _context.CategoryDishes.ToListAsync();
            List<int?> filteredDish = new List<int?>();

            categoryDishes.FindAll(categoryDish => categoryDish.CategoryId == CategoryId && categoryDish.IsDeleted == false).
                ForEach(catDish => filteredDish.Add(catDish.DishId));

            List<Dish> dishes = new List<Dish>();
            await _context.Dishes.ForEachAsync(dish =>
            {
                if(dish.IsDeleted == false && filteredDish.Contains(dish.DishId))
                {
                    dishes.Add(dish);
                }
            });

            return Ok(dishes);
        }

        // PUT: api/Dishes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            if (id != dish.DishId)
            {
                return BadRequest();
            }

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Dishes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dish>> PostDish(int categoryId, Dish dish)
        {
            if (_context.Dishes == null)
            {
                return Problem("Entity set 'MenuDBContext.Dishes'  is null.");
            }

            // adding dish to dish category
            _context.Dishes.Add(dish);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DishExists(dish.DishId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            // updating the category_dish table
            CategoryDish categoryDish = new CategoryDish();
            categoryDish.CategoryId = categoryId;
            categoryDish.DishId = dish.DishId;
            _context.CategoryDishes.Add(categoryDish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDish", new { id = dish.DishId }, dish);
        }

        // DELETE: api/Dishes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            if (_context.Dishes == null)
            {
                return NotFound();
            }
            var dish = await _context.Dishes.FindAsync(id);
            if ((dish == null) || (dish.IsDeleted == true))
            {
                return NotFound();
            }

            List<CategoryDish> categoryDishes = await _context.CategoryDishes.ToListAsync();
            foreach (var categoryDish in categoryDishes)
            {
                if (categoryDish.DishId == id)
                {
                    categoryDish.IsDeleted = true;
                    _context.CategoryDishes.Update(categoryDish);
                    _context.SaveChanges();
                }
            }

            dish.IsDeleted = true;
            _context.Dishes.Update(dish);
            _context.SaveChanges();

            /*_context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();*/

            return Ok(new {response = "Deleted successfully"});
        }

        private bool DishExists(int id)
        {
            return (_context.Dishes?.Any(e => e.DishId == id)).GetValueOrDefault();
        }
    }
}
