using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApiRedBook.Model;

namespace WebApiRedBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly RedBookBaseContext _context;

        private readonly IConfiguration _config;
        public ItemsController(RedBookBaseContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/Items
        

        [HttpGet("[action]")]
        public async Task<ActionResult<List<ClassItem>>> GetClassItem()
        {
            List<ClassItem> classItems = new List<ClassItem>();
            classItems = _context.ClassItem.ToList();
            return  classItems.ToList();
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<List<Status>>> GetStatusItem()
        {
            List<Status> statusItems = new List<Status>();
            statusItems = _context.Status.ToList();
            return statusItems.ToList();
        }
        [HttpGet]
        public async Task<ActionResult<GetItemsModel>> GetItem(int currentPageIndex, int count, int sorting)
        {
            if (currentPageIndex == 0)
            {
                currentPageIndex = 1;
            }
            if (count == 0)
            {
                count = 5;
            }
            int maxRows = count;

            var Items = (from customer in this._context.Item
                         select customer).ToArray();


            GetItemsModel itemsModel = new GetItemsModel();

            double pageCount = (double)((decimal)this._context.Item.Count() / Convert.ToDecimal(maxRows));
            itemsModel.PageCount = (int)Math.Ceiling(pageCount);
            itemsModel.CurrentPageIndex = currentPageIndex;
            itemsModel.Items = (from customer in this._context.Item
                                select customer)
                    .OrderBy(customer => customer.Id)
                    .Skip((currentPageIndex - 1) * maxRows)
                    .Take(maxRows).ToList();
            if (sorting == 0)
            {
                itemsModel.Items = itemsModel.Items.OrderBy(x=>x.Name); //По возрастанию 
            }
            else if(sorting==1)
            {
                itemsModel.Items = itemsModel.Items.OrderByDescending(x=>x.Name); //По убыванию
            }
            return  itemsModel;
        }

       

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        { 
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutItem(int id, [FromBody]Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            _context.Item.Update(item);
           

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

         
        }

        // POST: api/Items
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
     
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Item>> PostItem(Item item)
        { 
                _context.Item.Add(item);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetItem", new { id = item.Id }, item);       
        }

        // DELETE: api/Items/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }
    }
}
