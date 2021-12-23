using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRedBook.Model;

namespace WebApiRedBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassItemsController : ControllerBase
    {
        private readonly RedBookBaseContext _context;

        public ClassItemsController(RedBookBaseContext context)
        {
            _context = context;
        }

        [HttpGet("getStatus")]
        public async Task<ActionResult<IEnumerable<Status>>> GetStatus()
        {
            var classList = _context.Status;
            return await classList.ToArrayAsync();
       

        }
        // GET: api/ClassItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassItem>>> GetClassItem()
        {
            return await _context.ClassItem.ToListAsync();
        }
        // GET: api/ClassItems/Название
        [HttpGet("getClasses")]
        public async Task<ActionResult<IEnumerable<ClassItem>>> GetClassItem(string type)
        {
            if (type == null)
            {
                var classList = _context.ClassItem;

                return await classList.ToArrayAsync();
            }
            else
            {
                var classList = _context.ClassItem.Where(x => x.Type.Name == type);

                return await classList.ToArrayAsync();
            }
          
        }
        // GET: api/ClassItems/Название
        [HttpGet("getClassesName")]
        public async Task<ActionResult<List<string>>> GetClassItemName(string type)
        {
            if (type == null)
            {
                var classList = _context.ClassItem;
                List<string> listName = new List<string>();
                foreach (var item in classList)
                {
                    listName.Add(item.Name);
                }
                return listName.ToList();
            }
            else
            {
                var classList = _context.ClassItem.Where(x => x.Type.Name == type);
                List<string> listName = new List<string>();
                foreach (var item in classList)
                {
                    listName.Add(item.Name);
                }
                return listName.ToList();
            }
           
        }

        // GET: api/ClassItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassItem>> GetClassItem(int id)
        {
            var classItem = await _context.ClassItem.FindAsync(id);

            if (classItem == null)
            {
                return NotFound();
            }

            return classItem;
        }

        // PUT: api/ClassItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassItem(int id, ClassItem classItem)
        {
            if (id != classItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(classItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassItemExists(id))
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

        // POST: api/ClassItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ClassItem>> PostClassItem(ClassItem classItem)
        {
            _context.ClassItem.Add(classItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassItem", new { id = classItem.Id }, classItem);
        }

        // DELETE: api/ClassItems/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClassItem>> DeleteClassItem(int id)
        {
            var classItem = await _context.ClassItem.FindAsync(id);
            if (classItem == null)
            {
                return NotFound();
            }

            _context.ClassItem.Remove(classItem);
            await _context.SaveChangesAsync();

            return classItem;
        }

        private bool ClassItemExists(int id)
        {
            return _context.ClassItem.Any(e => e.Id == id);
        }
    }
}
