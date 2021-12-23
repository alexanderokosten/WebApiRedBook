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
    public class ClassItemsController : Controller
    {
        private readonly RedBookBaseContext _context;
        RedBookApi api = new RedBookApi();
        public ClassItemsController(RedBookBaseContext context)
        {
            _context = context;
        }

        // GET: ClassItems
        public async Task<IActionResult> Index()
        {
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/ClassItems");
            HttpResponseMessage res2 = await client.GetAsync("api/Types");

            if (res.IsSuccessStatusCode && res2.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                var result2 = res2.Content.ReadAsStringAsync().Result;


                List<Model.Type> TypesClasses = JsonConvert.DeserializeObject<List<Model.Type>>(result2);

                List<ClassItem> items = JsonConvert.DeserializeObject<List<ClassItem>>(result);
                foreach (var item in items)
                {
                    var kingdom = TypesClasses.FirstOrDefault(x => x.Id == item.TypeId);
                    item.Type = kingdom;
                }


                return View(items);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: ClassItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/ClassItems/{id}");
            HttpResponseMessage res2 = await client.GetAsync("api/Types");

            if (res.IsSuccessStatusCode && res2.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                var result2 = res2.Content.ReadAsStringAsync().Result;


                List<Model.Type> TypesClasses = JsonConvert.DeserializeObject<List<Model.Type>>(result2);

                ClassItem item = JsonConvert.DeserializeObject<ClassItem>(result);

                var type = TypesClasses.FirstOrDefault(x => x.Id == item.TypeId);
                item.Type = type;



                return View(item);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: ClassItems/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            HttpClient client = api.Initial();

            HttpResponseMessage res2 = await client.GetAsync("api/Types");

            if (res2.IsSuccessStatusCode)
            {
                var result2 = res2.Content.ReadAsStringAsync().Result;
                List<Model.Type> typeClasses = JsonConvert.DeserializeObject<List<Model.Type>>(result2);
                ViewData["TypeId"] = new SelectList(typeClasses, "Id", "Name");
                return View();
                //ViewData["KingdomId"] = new SelectList(KingdomClasses, "Id", "Name");
                //return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // POST: ClassItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,TypeId")] ClassItem classItem)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = api.Initial();

                string token = User.Claims.FirstOrDefault(x => x.Type == "token").Value;
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post,
                   "api/ClassItems/");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string json = JsonConvert.SerializeObject(classItem);
                httpRequest.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await client.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {


                    return RedirectToAction("", "ClassItems");
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

        // GET: ClassItems/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ClassItem itemMain = new Model.ClassItem();
            List<Model.Type> items = new List<Model.Type>();

            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/ClassItems/{id}");
            HttpResponseMessage res2 = await client.GetAsync("api/Types");

            if (res.IsSuccessStatusCode)
            {
                var result = res2.Content.ReadAsStringAsync().Result;

                var result3 = res.Content.ReadAsStringAsync().Result;
                itemMain = JsonConvert.DeserializeObject<ClassItem>(result3);
                items = JsonConvert.DeserializeObject<List<Model.Type>>(result);

                if (itemMain == null)
                {
                    return NotFound();
                }

            }

            var type = items.FirstOrDefault(x => x.Id == itemMain.TypeId);

            itemMain.Type = type;
            ViewData["TypeId"] = new SelectList(items, "Id", "Name", itemMain.TypeId);
            return View(itemMain);
        }

        // POST: ClassItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,TypeId")] ClassItem classItem)
        {
            if (id != classItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                HttpClient client = api.Initial();

                string token = User.Claims.FirstOrDefault(x => x.Type == "token").Value;
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Put,
                   $"api/ClassItems/{id}");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string json = JsonConvert.SerializeObject(classItem);
                httpRequest.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {


                    return RedirectToAction("", "ClassItems");
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

        // GET: ClassItems/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClassItem itemMain = new ClassItem();
            List<Model.Type> items = new List<Model.Type>();

            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/ClassItems/{id}");
            HttpResponseMessage res2 = await client.GetAsync("api/Types");

            if (res.IsSuccessStatusCode)
            {
                var result = res2.Content.ReadAsStringAsync().Result;

                var result3 = res.Content.ReadAsStringAsync().Result;
                itemMain = JsonConvert.DeserializeObject<ClassItem>(result3);
                items = JsonConvert.DeserializeObject<List<Model.Type>>(result);

                if (itemMain == null)
                {
                    return NotFound();
                }

            }

            var type = items.FirstOrDefault(x => x.Id == itemMain.TypeId);

            itemMain.Type = type;

            return View(itemMain);
        }

        // POST: ClassItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = api.Initial();
            string token = User.Claims.FirstOrDefault(x => x.Type == "token").Value;
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Delete,
               $"api/ClassItems/{id}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.SendAsync(httpRequest);
            if (response.IsSuccessStatusCode)
            {


                return RedirectToAction("", "ClassItems");
            }
            else
            {
                return RedirectToAction("", "Home");
            }
        }

      
    }
}
