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
    public class TypesController : ControllerBase
    {
        private readonly RedBookBaseContext _context;

        public TypesController(RedBookBaseContext context)
        {
            _context = context;
        }

        // GET: api/Types
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Model.Type>>> GetType()
        {
            return await _context.Type.ToListAsync();
        }

        // GET: api/Types/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Model.Type>> GetType(int id)
        {
            var @type = await _context.Type.FindAsync(id);

            if (@type == null)
            {
                return NotFound();
            }

            return @type;
        }

        // PUT: api/Types/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutType(int id, Model.Type @type)
        {
            if (id != @type.Id)
            {
                return BadRequest();
            }

            _context.Entry(@type).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeExists(id))
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

        // POST: api/Types
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Model.Type>> PostType(Model.Type @type)
        {
            _context.Type.Add(@type);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetType", new { id = @type.Id }, @type);
        }

        // DELETE: api/Types/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Model.Type>> DeleteType(int id)
        {
            var @type = await _context.Type.FindAsync(id);
            if (@type == null)
            {
                return NotFound();
            }

            _context.Type.Remove(@type);
            await _context.SaveChangesAsync();

            return @type;
        }

        private bool TypeExists(int id)
        {
            return _context.Type.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("GetTypes")]
        public ActionResult<IEnumerable<Model.Type>> GetType(string kingdom)
        {
            if (kingdom == null)
            {
                IEnumerable<Model.Type> items = _context.Type;

                return items.ToArray();
            }
            else
            {
                IEnumerable<Model.Type> items = _context.Type.Where(x => x.Kingdom.Name == kingdom);

                return items.ToArray();
            }

        }

        [HttpGet]
        [Route("GetNameTypes")]
        public ActionResult<IEnumerable<string>> GetNameType(string kingdom)
        {
            if (kingdom == null)
            {
                IEnumerable<Model.Type> items = _context.Type.ToArray();
                List<string> itemsName = new List<string>();
                foreach (var item in items)
                {
                    itemsName.Add(item.Name);
                }
                return itemsName.ToArray();
            }
            else
            {
                IEnumerable<Model.Type> items = _context.Type.Where(x => x.Kingdom.Name == kingdom);
                List<string> itemsName = new List<string>();
                foreach (var item in items)
                {
                    itemsName.Add(item.Name);
                }
                return itemsName.ToArray();
            }
           
        }
    }
}
