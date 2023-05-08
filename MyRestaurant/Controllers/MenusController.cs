using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestaurant.Models;

namespace MyRestaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly MenuDBContext _context;

        public MenusController(MenuDBContext context)
        {
            _context = context;
        }

        // GET: api/Menus
        [HttpGet]
        [EnableCors]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenus()
        {
            if (_context.Menus == null)
            {
               return NotFound();
            }
          
            List<Menu> menuList = new List<Menu>();

            foreach (var menu in _context.Menus)
            {
                if (menu.IsDeleted == false)
                {
                    menuList.Add(menu);
                }
            }
            return menuList;
        }

        // GET: api/Menus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> GetMenu(int id)
        {
          if (_context.Menus == null)
          {
              return NotFound();
          }
            var menu = await _context.Menus.FindAsync(id);

            if ((menu == null) || (menu.IsDeleted == true))
            {
                return NotFound();
            }

            return menu;
        }

        // PUT: api/Menus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutMenu(int id, Menu menu)
        {
            if (id != menu.MenuId)
            {
                return BadRequest();
            }

            _context.Entry(menu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Menus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Menu>> PostMenu(Menu menu)
        {
          if (_context.Menus == null)
          {
              return Problem("Entity set 'MenuDBContext.Menus'  is null.");
          }
            _context.Menus.Add(menu);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MenuExists(menu.MenuId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMenu", new { id = menu.MenuId }, menu);
        }

        // DELETE: api/Menus/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            if (_context.Menus == null)
            {
                return NotFound();
            }
            var menu = await _context.Menus.FindAsync(id);
            if ((menu == null) || (menu.IsDeleted ?? true))
            {
                return NotFound();
            }

            List<MenuCategory> menuCategories = await _context.MenuCategories.ToListAsync();
            List<int?> filteredCategories = new List<int?>();
            foreach (var menuCategory in menuCategories)
            {
                if(menuCategory.MenuId == id && menuCategory.IsDeleted == false)
                {
                    filteredCategories.Add(menuCategory.Id);
                    menuCategory.IsDeleted = true;
                    _context.MenuCategories.Update(menuCategory);
                    _context.SaveChanges();
                }
            }

            List<Category> categories = await _context.Categories.ToListAsync();
            categories.ForEach(category =>
            {
                if (filteredCategories.Contains(category.CategoryId))
                {
                    category.IsDeleted = true;
                    _context.Categories.Update(category);
                    _context.SaveChanges();
                }
            });

            menu.IsDeleted = true;
            _context.Menus.Update(menu);
            _context.SaveChanges();

            *//*_context.Menus.Remove(menu);
            await _context.SaveChangesAsync();*//*

            return NoContent();
        }*/

        private bool MenuExists(int id)
        {
            return (_context.Menus?.Any(e => e.MenuId == id)).GetValueOrDefault();
        }
    }
}
