using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI2.Models;
using System.Net.Http;
using System.Net.Security;
using Newtonsoft.Json;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyModel;
using Books = LibraryAPI2.Models.Books;


namespace LMSApp.Controllers
{
    public class AdminController : Controller
    {
        private HttpClient client = new HttpClient();

        public AdminController()
        {
            var httpClientHandler = new HttpClientHandler { Proxy = WebRequest.GetSystemWebProxy() };
            client = new HttpClient(httpClientHandler);
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public class libId
        {
            public int id { get; set; }
        }

        public static int lib;

        /*public IActionResult Index()
        {
            return View();
        }*/

        [HttpGet]
        public IActionResult ListBooks(libId json)
        {
            /*_oBooks = new List<Books>();
            using(var httpClient=new HttpClient(_clientHandler))
            {
                using (var response = httpClient.GetAsync("https://localhost:5001/api/Books/GetBooksByLibrary/" + LibraryId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oBooks = JsonConvert.DeserializeObject<List<Books>>(apiResponse);
                }
            }*/
            lib = json.id;
            HttpResponseMessage response = client.
                     GetAsync("api/Books/GetBooksByLibrary/" + json.id.ToString()).Result;
            List<Books> data = response.Content.
                         ReadAsAsync<List<Books>>().Result;
            List<bool> avails = new List<bool>();
            List<BookWithStatus> bws = new List<BookWithStatus>();
            foreach (var book in data)
            {
                HttpResponseMessage response2 = client.
                     GetAsync("api/Books/GetAvailability/" + book.BookId.ToString()).Result;
                bool data2 = response2.Content.
                         ReadAsAsync<bool>().Result;
                avails.Add(data2);
            }
            for (int i = 0; i < data.Count; i++)
            {
                bws.Add(new BookWithStatus { BookId = data[i].BookId, Title = data[i].Title, Author = data[i].Author, Price = data[i].Price, Genre = data[i].Genre, isAvailable = avails[i] });
            }


            return View(bws);
            
        }

        /*[HttpGet]
         public async Task<Books> GetBooks(int BookId)
         {
             _oBook = new Books();
             using (var httpClient = new HttpClient(_clientHandler))
             {
                 using (var response = await httpClient.GetAsync("https://localhost:5001/api/Books/GetBooks/" + BookId))
                 {
                     string apiResponse = await response.Content.ReadAsStringAsync();
                     _oBook = JsonConvert.DeserializeObject<Books>(apiResponse);
                 }
             }
             return _oBook;
         }*/
        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostBooks(Books book)
        {
            /*_oBook = new Books();
            using (var httpClient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(book),Encoding.UTF8,"application/json");

                using (var response = await httpClient.PostAsync("https://localhost:5001/api/Books/PostBooks",content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oBook = JsonConvert.DeserializeObject<Books>(apiResponse);
                }
            }
            return _oBook;*/
            string json1 = JsonConvert.SerializeObject(book);
            var httpContent = new StringContent(json1, Encoding.UTF8, "application/json");
            var httpResponse = client.PostAsync("api/Books/PostBooks/" + lib.ToString(), httpContent);
            return RedirectToAction("ListBooks", new libId { id = lib });
        }
        public ActionResult MakeAvailable()
        {
            return View();
        }
        public ActionResult MakeUnavailable()
        {
            return View();
        }
        /*public IActionResult ChangeAvailability(BookAvailability bookAvailability)
        {
            string json = JsonConvert.SerializeObject(bookAvailability);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = client.PutAsync("api/Books/ChangeAvailability", httpContent);
            return RedirectToAction("ListBooks", new libId { id = Statics.libraryId });
        }*/
        
        public IActionResult PutUnavailable(int BookId)
        {
             /*_oBook = new Books();
             using (var httpClient = new HttpClient(_clientHandler))
             {
                 StringContent content = new StringContent(JsonConvert.SerializeObject(BookId), Encoding.UTF8, "application/json");

                 using (var response = await httpClient.PutAsync("https://localhost:5001/api/Books/PutUnavailable", content))
                 {
                     string apiResponse = await response.Content.ReadAsStringAsync();
                     _oBook = JsonConvert.DeserializeObject<Books>(apiResponse);
                 }
             }
             return _oBook;*/
            string json = JsonConvert.SerializeObject(BookId);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = client.PutAsync("api/Books/PutUnavailable",httpContent);
            return RedirectToAction("ListBooks", new libId { id = lib });

        }

        public IActionResult PutAvailable(int BookId)
        {
            /*_oBook = new Books();
            using (var httpClient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(BookId), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://localhost:5001/api/Books/PutAvailable", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oBook = JsonConvert.DeserializeObject<Books>(apiResponse);
                }
            }
            return _oBook;*/
            string json = JsonConvert.SerializeObject(BookId);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = client.PutAsync("api/Books/PutAvailable",httpContent);
            return RedirectToAction("ListBooks", new libId { id = lib });


        }

    }
}
