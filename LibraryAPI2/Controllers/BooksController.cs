using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryAPI2.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace LibraryAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public HttpResponseMessage Validate([FromBody] Books b)
        {
            if (ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        public BooksController(CoreDbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Books>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [Route("[action]/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Books>> GetBooks(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }


        [Route("[action]/{id}")]
        [HttpGet("{id}")]
        public List<Books> GetBooksByLibrary(int id)
        {
            List<Books> bookList = new List<Books>();
            int ID = id;
            var books = _context.BookLibraryAssociation.Where(b=>b.LibraryId==ID).ToList();

            foreach(var book in books)
            {
                bookList.Add(_context.Books.Where(b => b.BookId == book.BookId).FirstOrDefault());
            }
            return bookList;
        }

        [Route("[action]/{id}")]
        [HttpGet("{id}")]
        public List<UserBookAssociation> GetBooksByUser(int id)
        {
            int ID = id;
            List<UserBookAssociation> booksTaken = new List<UserBookAssociation>();
            booksTaken = _context.UserBookAssociation.Where(u => u.UserId == ID).OrderByDescending(u=>u.DueDate).ToList();

            return booksTaken;
        }

        [Route("[action]")]
        [HttpGet]
        public List<Books> GetBooksForCheckout()
        {
            List<Books> booksAvailable = new List<Books>();
            var books = _context.BookLibraryAssociation.Where(b => b.IsCheckedOut == false && b.IsAvailable==true).ToList();
            foreach (var book in books)
            {
                booksAvailable.Add(_context.Books.Where(b => b.BookId == book.BookId).FirstOrDefault());
            }

            return booksAvailable;
        }

        [HttpGet("GetAvailability/{id}")]
        public ActionResult<bool> GetAvailability(int id)
        {

                var book = _context.BookLibraryAssociation.Where(i => i.BookId == id).FirstOrDefault();
                return book.IsAvailable;

        }
        // PUT: api/Books/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooks(int id, Books books)
        {
            if (id != books.BookId)
            {
                return BadRequest();
            }

            _context.Entry(books).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksExists(id))
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
        [HttpPut("ChangeAvailability")]
        public ActionResult<bool> PutBookLibraryAssociation([FromBody] BookAvailability json)
        {

            var book = _context.BookLibraryAssociation.FirstOrDefault(x => x.BookId == json.id);

            if (book != null)
            {
                book.IsAvailable = json.isAvailable;
                _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest(false);
            }
            return Ok(true);
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> PutUnavailable([FromBody] int id)
        {
            /*if (id != books.BookId)
            {
                return BadRequest();
            }*/
            var book = await _context.BookLibraryAssociation.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                book.IsAvailable = false;
                _context.Entry(book).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> PutAvailable([FromBody] int id)
        {
            /*if (id != books.BookId)
            {
                return BadRequest();
            }*/
            var book = await _context.BookLibraryAssociation.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                book.IsAvailable = true;
                _context.Entry(book).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }
        [Route("[action]/{id}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheckOut(int id, [FromBody]int user)
        {
            /*if (id != books.BookId)
            {
                return BadRequest();
            }*/
            var book = await _context.BookLibraryAssociation.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                book.IsCheckedOut = true;
                _context.Entry(book).State = EntityState.Modified;

                UserBookAssociation obj = new UserBookAssociation();

                obj.Id = id;
                obj.UserId = user;
                obj.BookLibraryAssociationId = id;
                DateTime dt = DateTime.Now;
                TimeSpan ts = new TimeSpan(90,0,0,0,0);
                DateTime newDate = dt.Add(ts);
                obj.DueDate = newDate;

                _context.UserBookAssociation.Add(obj);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }
        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> PutReturnBook([FromBody]int id)
        {
            /*if (id != books.BookId)
            {
                return BadRequest();
            }*/
            var book = await _context.BookLibraryAssociation.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                book.IsCheckedOut = false;
                var issuance= await _context.UserBookAssociation.FindAsync(id);
                _context.UserBookAssociation.Remove(issuance);
                _context.Entry(book).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }
        // POST: api/Books
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Route("[action]/{id}")]
        [HttpPost]
        public async Task<ActionResult<Books>> PostBooks(int id,Books books)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Books.Add(books);
                    BookLibraryAssociation obj = new BookLibraryAssociation();
                    obj.BookLibraryAssociationId = books.BookId;
                    obj.BookId = books.BookId;
                    obj.LibraryId = id;
                    obj.IsAvailable = true;
                    obj.IsCheckedOut = false;

                    _context.BookLibraryAssociation.Add(obj);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (BooksExists(books.BookId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction(nameof(GetBooks), new { id = books.BookId }, books);
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Books>> DeleteBooks(int id)
        {
            var books = await _context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }

            _context.Books.Remove(books);
            await _context.SaveChangesAsync();

            return books;
        }

        private bool BooksExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
