using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using RedBookWebApplication.Helper;
using RedBookWebApplication.Model;

namespace RedBookWebApplication.Controllers
{

   

    public class ItemsController : Controller
    {
        
        RedBookApi api = new RedBookApi();
        public ItemsController()
        {
        
        
        }

        // GET: Items
        public async Task<IActionResult> Index(int pageIndex, int sortingValue)
        {
            HttpClient client = api.Initial();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
           
            int i = pageIndex;
            HttpResponseMessage res = await client.GetAsync($"api/Items?currentPageIndex={pageIndex}&sorting={sortingValue}");

          




            HttpResponseMessage res2 = await client.GetAsync("api/Items/GetClassItem");
            HttpResponseMessage res3 = await client.GetAsync("api/Items/GetStatusItem");
            //HttpResponseMessage res4 = await client.GetAsync($"api/Items");
            if (res.IsSuccessStatusCode && res2.IsSuccessStatusCode && res3.IsSuccessStatusCode/*&&res4.IsSuccessStatusCode*/)
            {       
                var result = res.Content.ReadAsStringAsync().Result;
                var result2 = res2.Content.ReadAsStringAsync().Result;
                var result3 = res3.Content.ReadAsStringAsync().Result;
                //var result4 = res4.Content.ReadAsStringAsync().Result;
              

                List<ClassItem> classItems = new List<ClassItem>();
                List<Status> statusItems = new List<Status>();   
                List<ClassItem> itemsClasses = JsonConvert.DeserializeObject<List<ClassItem>>(result2);
                List<Status> itemsStatus = JsonConvert.DeserializeObject<List<Status>>(result3);
                GetItemsModel items = JsonConvert.DeserializeObject<GetItemsModel>(result);
               


                foreach (var item in items.Items)
                {


                    var status = itemsStatus.FirstOrDefault(x => x.Id == item.StatusId);
                    var classes = itemsClasses.FirstOrDefault(x => x.Id == item.ClassItemId);
                    item.Status = status;
                    item.ClassItem = classes;
                  

                }


                return View(items);

            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Item itemMain = new Item();
            List<ClassItem> items = new List<ClassItem>();
            List<Status> items2 = new List<Status>();
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/Items/{id}");
            HttpResponseMessage res2 = await client.GetAsync("api/ClassItems");
            HttpResponseMessage res3 = await client.GetAsync("api/ClassItems/getStatus");
            if (res.IsSuccessStatusCode)
            {
                var result = res2.Content.ReadAsStringAsync().Result;
                var result2 = res3.Content.ReadAsStringAsync().Result;
                var result3 = res.Content.ReadAsStringAsync().Result;
                itemMain = JsonConvert.DeserializeObject<Item>(result3);
                items = JsonConvert.DeserializeObject<List<ClassItem>>(result);
                items2 = JsonConvert.DeserializeObject<List<Status>>(result2);
                if (itemMain == null)
                {
                    return NotFound();
                }
            }
            var status = items2.FirstOrDefault(x => x.Id == itemMain.StatusId);
            var classes = items.FirstOrDefault(x => x.Id == itemMain.ClassItemId);
            itemMain.Status = status;
            itemMain.ClassItem = classes;
            return View(itemMain);
        }

        // GET: Items/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
          
            List<ClassItem> items = new List<ClassItem>();
            List<Status> items2 = new List<Status>();
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/ClassItems");
            HttpResponseMessage res2 = await client.GetAsync("api/ClassItems/getStatus");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                var result2 = res2.Content.ReadAsStringAsync().Result;
                items = JsonConvert.DeserializeObject<List<ClassItem>>(result);
                items2 = JsonConvert.DeserializeObject<List<Status>>(result2);

            }

           
            ViewData["ClassItemId"] = new SelectList(items, "Id", "Name");
            ViewData["StatusId"] = new SelectList(items2, "Id", "Name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Spread,Number,Biology,LimitingFactors,SecurityMeasures,ClassItemId,StatusId,Image")] Item item)
        { 
            HttpClient client = api.Initial();

            string token = User.Claims.FirstOrDefault(x => x.Type == "token").Value;
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post,
               "api/Items/");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(item);
            httpRequest.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.SendAsync(httpRequest);
            if (response.IsSuccessStatusCode)
            {

           
                return RedirectToAction("", "Items");
            }
            else
            {
                return RedirectToAction("", "Home");
            }
        }

        //// GET: Items/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            } 
            Item itemMain = new Item();
            List<ClassItem> items = new List<ClassItem>();
            List<Status> items2 = new List<Status>();
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/Items/{id}");
            HttpResponseMessage res2 = await client.GetAsync("api/ClassItems");
            HttpResponseMessage res3 = await client.GetAsync("api/ClassItems/getStatus");
            if (res.IsSuccessStatusCode)
            {
                var result = res2.Content.ReadAsStringAsync().Result;
                var result2 = res3.Content.ReadAsStringAsync().Result;
                var result3 = res.Content.ReadAsStringAsync().Result;
                itemMain = JsonConvert.DeserializeObject<Item>(result3);
                items = JsonConvert.DeserializeObject<List<ClassItem>>(result);
                items2 = JsonConvert.DeserializeObject<List<Status>>(result2);
                if (itemMain == null)
                {
                    return NotFound();
                }

            }
            var status = items2.FirstOrDefault(x => x.Id == itemMain.StatusId);
            var classes = items.FirstOrDefault(x => x.Id == itemMain.ClassItemId);
            itemMain.Status = status;
            itemMain.ClassItem = classes;



            ViewData["ClassItemId"] = new SelectList(items, "Id", "Name",itemMain.ClassItemId);
            ViewData["StatusId"] = new SelectList(items2, "Id", "Name",itemMain.StatusId);
            return View(itemMain);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Spread,Number,Biology,LimitingFactors,SecurityMeasures,ClassItemId,StatusId,Image")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                HttpClient client = api.Initial();

                string token = User.Claims.FirstOrDefault(x => x.Type == "token").Value;
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Put,
                   $"api/Items/{id}");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string json = JsonConvert.SerializeObject(item);
                httpRequest.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
              
                var response = await client.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {


                    return RedirectToAction("", "Items");
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

        // GET: Items/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var item = await _context.Item.FindAsync(id);

            Item itemMain = new Item();




            List<ClassItem> items = new List<ClassItem>();
            List<Status> items2 = new List<Status>();
            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/Items/{id}");
            HttpResponseMessage res2 = await client.GetAsync("api/ClassItems");
            HttpResponseMessage res3 = await client.GetAsync("api/ClassItems/getStatus");
            if (res.IsSuccessStatusCode)
            {
                var result = res2.Content.ReadAsStringAsync().Result;
                var result2 = res3.Content.ReadAsStringAsync().Result;
                var result3 = res.Content.ReadAsStringAsync().Result;
                itemMain = JsonConvert.DeserializeObject<Item>(result3);
                items = JsonConvert.DeserializeObject<List<ClassItem>>(result);
                items2 = JsonConvert.DeserializeObject<List<Status>>(result2);
                if (itemMain == null)
                {
                    return NotFound();
                }

            }
            var status = items2.FirstOrDefault(x => x.Id == itemMain.StatusId);
            var classes = items.FirstOrDefault(x => x.Id == itemMain.ClassItemId);
            itemMain.Status = status;
            itemMain.ClassItem = classes;
            if (itemMain == null)
            {
                return NotFound();
            }

            return View(itemMain);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = api.Initial();
            string token = User.Claims.FirstOrDefault(x => x.Type == "token").Value;
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Delete,
               $"api/Items/{id}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.SendAsync(httpRequest);
            if (response.IsSuccessStatusCode)
            {


                return RedirectToAction("", "Items");
            }
            else
            {
                return RedirectToAction("", "Home");
            }
        }
    }
}
