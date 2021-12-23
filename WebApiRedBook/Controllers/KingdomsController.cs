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
    public class KingdomsController : ControllerBase
    {
        private readonly RedBookBaseContext _context;

        public KingdomsController(RedBookBaseContext context)
        {
            _context = context;
        }

        // GET: api/Kingdoms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kingdom>>> GetKingdom()
        {
            GetKingdomModel kingdomModel = new GetKingdomModel();
            kingdomModel.Items = (from customer in this._context.Kingdom
                                  select customer)
                                 .AsQueryable();
           

            //return View(a);
            return await kingdomModel.Items.ToListAsync();
        }

        // GET: api/Kingdoms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kingdom>> GetKingdom(int id)
        {
            var kingdom = await _context.Kingdom.FindAsync(id);

            if (kingdom == null)
            {
                return NotFound();
            }

            return kingdom;
        }

        // PUT: api/Kingdoms/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKingdom(int id, Kingdom kingdom)
        {
            if (id != kingdom.Id)
            {
                return BadRequest();
            }

            _context.Entry(kingdom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KingdomExists(id))
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

        // POST: api/Kingdoms
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Kingdom>> PostKingdom(Kingdom kingdom)
        {
            _context.Kingdom.Add(kingdom);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Kingdoms/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Kingdom>> DeleteKingdom(int id)
        {
            var kingdom = await _context.Kingdom.FindAsync(id);
            if (kingdom == null)
            {
                return NotFound();
            }

            _context.Kingdom.Remove(kingdom);
            await _context.SaveChangesAsync();

            return kingdom;
        }

        private bool KingdomExists(int id)
        {
            return _context.Kingdom.Any(e => e.Id == id);
        }
        [HttpGet]
        [Route("GetNameKingdoms")]
        public ActionResult<IEnumerable<string>> GetNameKingdoms()
        {
            IEnumerable<Kingdom> items = _context.Kingdom.ToArray();
            List<string> itemsName = new List<string>();
            foreach (var item in items)
            {
                itemsName.Add(item.Name);
            }
            return itemsName.ToArray();
        }
    }
}
