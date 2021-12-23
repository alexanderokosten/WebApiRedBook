using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RedBookWebApplication.Helper;
using RedBookWebApplication.Model;

namespace RedBookWebApplication.Controllers
{
    public class TypesController : Controller
    {
        private readonly RedBookBaseContext _context;
        RedBookApi api = new RedBookApi();
        public TypesController(RedBookBaseContext context)
        {
            _context = context;
        }

        // GET: Types
        public async Task<IActionResult> Index()
        {
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Types");
            HttpResponseMessage res2 = await client.GetAsync("api/Kingdoms");
          
            if (res.IsSuccessStatusCode && res2.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                var result2 = res2.Content.ReadAsStringAsync().Result;
             
             
                List<Kingdom> KingdomClasses = JsonConvert.DeserializeObject<List<Kingdom>>(result2);
               
                List<Model.Type> items = JsonConvert.DeserializeObject<List<Model.Type>>(result);
                foreach (var item in items)
                {           
                    var kingdom = KingdomClasses.FirstOrDefault(x => x.Id == item.KingdomId);
                    item.Kingdom = kingdom;
                }


                return View(items);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/Types/{id}");
            HttpResponseMessage res2 = await client.GetAsync("api/Kingdoms");

            if (res.IsSuccessStatusCode && res2.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                var result2 = res2.Content.ReadAsStringAsync().Result;


                List<Kingdom> KingdomClasses = JsonConvert.DeserializeObject<List<Kingdom>>(result2);

                Model.Type item = JsonConvert.DeserializeObject<Model.Type>(result);
               
                    var kingdom = KingdomClasses.FirstOrDefault(x => x.Id == item.KingdomId);
                    item.Kingdom = kingdom;
                


                return View(item);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Types/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            HttpClient client = api.Initial();
       
            HttpResponseMessage res2 = await client.GetAsync("api/Kingdoms");

            if (res2.IsSuccessStatusCode)
            {     
                var result2 = res2.Content.ReadAsStringAsync().Result;
                List<Kingdom> KingdomClasses = JsonConvert.DeserializeObject<List<Kingdom>>(result2);
                ViewData["KingdomId"] = new SelectList(KingdomClasses, "Id", "Name");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
          
        }

        // POST: Types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,KingdomId")] Model.Type @type)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = api.Initial();

                string token = User.Claims.FirstOrDefault(x => x.Type == "token").Value;
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post,
                   "api/Types/");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string json = JsonConvert.SerializeObject(@type);
                httpRequest.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await client.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {


                    return RedirectToAction("", "Types");
                }
                else
                {
                    return RedirectToAction("", "Home");
                }
            }
            else
            {
                return BadRequest();
            }
         
        }

        // GET: Types/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Model.Type itemMain = new Model.Type();
            List<Kingdom> items = new List<Kingdom>();
          
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/Types/{id}");
            HttpResponseMessage res2 = await client.GetAsync("api/Kingdoms");
           
            if (res.IsSuccessStatusCode)
            {
                var result = res2.Content.ReadAsStringAsync().Result;
                
                var result3 = res.Content.ReadAsStringAsync().Result;
                itemMain = JsonConvert.DeserializeObject<Model.Type>(result3);
                items = JsonConvert.DeserializeObject<List<Kingdom>>(result);
            
                if (itemMain == null)
                {
                    return NotFound();
                }

            }
          
            var kingdom = items.FirstOrDefault(x => x.Id == itemMain.KingdomId);
        
            itemMain.Kingdom = kingdom;
            ViewData["KingdomId"] = new SelectList(_context.Kingdom, "Id", "Name", itemMain.KingdomId);
            return View(itemMain);
        }

        // POST: Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,KingdomId")] Model.Type @type)
        {
            if (id != @type.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                HttpClient client = api.Initial();

                string token = User.Claims.FirstOrDefault(x => x.Type == "token").Value;
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Put,
                   $"api/Types/{id}");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string json = JsonConvert.SerializeObject(@type);
                httpRequest.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {


                    return RedirectToAction("", "Types");
                }
                else
                {
                    return RedirectToAction("", "Home");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        // GET: Types/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            Model.Type itemMain = new Model.Type();
            List<Kingdom> items = new List<Kingdom>();

            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/Types/{id}");
            HttpResponseMessage res2 = await client.GetAsync("api/Kingdoms");

            if (res.IsSuccessStatusCode)
            {
                var result = res2.Content.ReadAsStringAsync().Result;

                var result3 = res.Content.ReadAsStringAsync().Result;
                itemMain = JsonConvert.DeserializeObject<Model.Type>(result3);
                items = JsonConvert.DeserializeObject<List<Kingdom>>(result);

                if (itemMain == null)
                {
                    return NotFound();
                }

            }

            var kingdom = items.FirstOrDefault(x => x.Id == itemMain.KingdomId);

            itemMain.Kingdom = kingdom;

            return View(itemMain);
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = api.Initial();
            string token = User.Claims.FirstOrDefault(x => x.Type == "token").Value;
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Delete,
               $"api/Types/{id}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.SendAsync(httpRequest);
            if (response.IsSuccessStatusCode)
            {


                return RedirectToAction("", "Types");
            }
            else
            {
                return RedirectToAction("", "Home");
            }
        }

    
    }
}
