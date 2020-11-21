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
    public class UsersController : Controller
    {
        /*public IActionResult Index()
        {
            return View();
        }*/
        private HttpClient client = new HttpClient();

        public static int user;

        public class userId
        {
            public int id { get; set; }
        }


        public UsersController()
        {
            var httpClientHandler = new HttpClientHandler { Proxy = WebRequest.GetSystemWebProxy() };
            client = new HttpClient(httpClientHandler);
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("text/plain"));
        }
        public IActionResult UserHome(UserId UserId)
        {
            user = UserId.id;
            HttpResponseMessage res= client.
                    GetAsync("api/User/"+user.ToString()).Result;
            if (!res.IsSuccessStatusCode)
                return View("Register");
            HttpResponseMessage response = client.
                    GetAsync("api/Books/GetBooksForCheckout").Result;
            List<Books> data = response.Content.
                         ReadAsAsync<List<Books>>().Result;
            List<BooksForUser> books = new List<BooksForUser>();
            for (int i = 0; i < data.Count; i++)
            {
                books.Add(new BooksForUser { BookId = data[i].BookId, Title = data[i].Title, Author = data[i].Author, Genre = data[i].Genre});
            }

            return View(books);
        }

        public IActionResult CheckOut()
        {
            return View();
        }
        public IActionResult CheckoutBook(int BookId)
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
            string json = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = client.PutAsync("api/Books/PutCheckOut/"+BookId.ToString(), httpContent);
            return RedirectToAction("ListBooksCheckedOut");


        }
        public IActionResult Return()
        {
            return View();
        }
        public IActionResult ReturnBook(int BookId)
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
            var httpResponse = client.PutAsync("api/Books/PutReturnBook", httpContent);
            return RedirectToAction("ListBooksCheckedOut");


        }
        public IActionResult ListBooksCheckedOut()
        {
            HttpResponseMessage response = client.
                   GetAsync("api/Books/GetBooksByUser/"+user.ToString()).Result;
            List<UserBookAssociation> data = response.Content.
                         ReadAsAsync<List<UserBookAssociation>>().Result;
            List<Books> books1 = new List<Books>();
            List<BooksCheckedOut> books2 = new List<BooksCheckedOut>();
            foreach (var book in data)
            {
                HttpResponseMessage response2 = client.
                     GetAsync("api/Books/GetBooks/" + book.Id.ToString()).Result;
                Books data2 = response2.Content.
                         ReadAsAsync<Books>().Result;
                books1.Add(data2);
            }
            for (int i = 0; i < books1.Count; i++)
            {
                books2.Add(new BooksCheckedOut { UserId=data[i].UserId, BookId = books1[i].BookId, Title = books1[i].Title, Author = books1[i].Author, DueDate = data[i].DueDate });
            }
            return View(books2);
        }

    }
}
