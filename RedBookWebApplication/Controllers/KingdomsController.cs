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
    public class KingdomsController : Controller
    {
        private readonly RedBookBaseContext _context;
        RedBookApi api = new RedBookApi();
        public KingdomsController(RedBookBaseContext context)
        {
            _context = context;
        }

        // GET: Kingdoms
        public async Task<IActionResult> Index()
        {
            List<Kingdom> items = new List<Kingdom>();
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Kingdoms");
            if (res.IsSuccessStatusCode)
            {

                var result = res.Content.ReadAsStringAsync().Result;
                items = JsonConvert.DeserializeObject<List<Kingdom>>(result);

            }

            return View(items);
        }

        // GET: Kingdoms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Kingdom itemMain = new Kingdom();
         
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/Kingdoms/{id}");
          
            if (res.IsSuccessStatusCode)
            {
               
                var result3 = res.Content.ReadAsStringAsync().Result;
                itemMain = JsonConvert.DeserializeObject<Kingdom>(result3); 
                if (itemMain == null)
                {
                    return NotFound();
                }
            }
            return View(itemMain);
        }

        // GET: Kingdoms/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kingdoms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Kingdom kingdom)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = api.Initial();

                string token = User.Claims.FirstOrDefault(x => x.Type == "token").Value;
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post,
                   "api/Kingdoms/");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string json = JsonConvert.SerializeObject(kingdom);
                httpRequest.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await client.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {


                    return RedirectToAction("", "Kingdoms");
                }
                else
                {
                    return RedirectToAction("", "Home");
                }
            }
            return View(kingdom);
        }

        // GET: Kingdoms/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Kingdom itemMain = new Kingdom();
          
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/Kingdoms/{id}");
            
            if (res.IsSuccessStatusCode)
            {
              
                var result3 = res.Content.ReadAsStringAsync().Result;
                itemMain = JsonConvert.DeserializeObject<Kingdom>(result3);
              
                if (itemMain == null)
                {
                    return NotFound();
                }

            }
            return View(itemMain);
        }

        // POST: Kingdoms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Kingdom kingdom)
        {
            if (id != kingdom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                HttpClient client = api.Initial();

                string token = User.Claims.FirstOrDefault(x => x.Type == "token").Value;
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Put,
                   $"api/Kingdoms/{id}");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string json = JsonConvert.SerializeObject(kingdom);
                httpRequest.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {


                    return RedirectToAction("", "Kingdoms");
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

        // GET: Kingdoms/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var item = await _context.Item.FindAsync(id);

            Kingdom itemMain = new Kingdom();

           
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/Kingdoms/{id}");
         
            if (res.IsSuccessStatusCode)
            {     
                var result3 = res.Content.ReadAsStringAsync().Result;
                itemMain = JsonConvert.DeserializeObject<Kingdom>(result3);
             
                if (itemMain == null)
                {
                    return NotFound();
                }

            }
            if (itemMain == null)
            {
                return NotFound();
            }

            return View(itemMain);
        }

        // POST: Kingdoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = api.Initial();
            string token = User.Claims.FirstOrDefault(x => x.Type == "token").Value;
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Delete,
               $"api/Kingdoms/{id}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.SendAsync(httpRequest);
            if (response.IsSuccessStatusCode)
            {


                return RedirectToAction("", "Kingdoms");
            }
            else
            {
                return RedirectToAction("", "Home");
            }
        }

      
    }
}
