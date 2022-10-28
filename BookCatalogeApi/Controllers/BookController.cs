using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCatalogeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BookCatalogeApi.Controllers
{
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IRepository repository;
        private readonly SignInManager<IdentityUser> signInManager;

        public BookController(IRepository repository, SignInManager<IdentityUser> signInManager) {
            this.signInManager = signInManager;
            this.repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        [Route("AddOrChangeBook")]
        public async Task<BookMessage> AddOrChangeBook([FromForm] BookAdd book)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                if (book.Book_ID is null)
                {
                    await book.Cover.CopyToAsync(memoryStream);
                    Book bk = new Book
                    {
                        Author = book.Author,
                        Cover = memoryStream.ToArray(),
                        Year = book.Year.Year,
                        Title = book.Title,
                        Description = book.Description,
                        Category = book.Category.ToString(),
                        Number = book.Number
                    };
                    await repository.BookAdd(bk);
                    return new BookMessage() { Succes = true, Message = "Книга добавлена в базу данных" };
                }
                else
                {
                    Book bk = repository.Books.Where(opt => opt.BookID == book.Book_ID).FirstOrDefault();
                    await book.Cover.CopyToAsync(memoryStream);
                    if (bk is not null)
                    {
                        bk.Author = book.Author;
                        bk.Cover = memoryStream.ToArray();
                        bk.Category = book.Category.ToString();
                        bk.Description = book.Description;
                        bk.Number = book.Number;
                        bk.Year = book.Year.Year;
                        bk.Title = book.Title;
                        await repository.BookChange();
                        return new BookMessage() { Book_ID = bk.BookID, Succes = true, Message = $"Данные книги {bk.BookID} изменены" };
                    }
                    
                }

            }
            return new BookMessage() { Succes = false, Message = $"Ошибка добавления или изменения данных" };
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        [Route("BookDelete")]
        public async Task<BookMessage> BookDelete([FromForm] long idBook)
        {
            if (ModelState.IsValid)
            {
                Book bk = repository.Books.Where(opt => opt.BookID == idBook).FirstOrDefault();
                if(bk is not null)
                {
                    await repository.BookDelete(bk);
                }
                return new BookMessage() { Book_ID=bk.BookID, Succes=true, Message=$"Книга удалена"};
            }
            return new BookMessage() { Succes = false, Message = $"Ошибка удаления" };
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        [Route("BookSearch")]
        public Book[] BookSearch(string searchtxt)
        {
            if (!string.IsNullOrEmpty(searchtxt))
                return repository.Books.Where(opt =>
                opt.Category.Contains(searchtxt.ToLower()) ||
                opt.Description.Contains(searchtxt.ToLower()) ||
                opt.Author.Contains(searchtxt.ToLower()) ||
                opt.Title.Contains(searchtxt.ToLower()) ||
                opt.Year.ToString().Contains(searchtxt.ToLower())
            ).ToArray();
            else
                return repository.Books.ToArray();
        }

        [HttpGet]
       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        [Route("BookSearchCategory")]
        public Book[] BookSearchCategory(CategoryBook? searchCategory)
        {
            if(searchCategory is not null)
                return repository.Books.Where(opt =>
                    opt.Category == searchCategory.ToString())
                    .ToArray();
            else
                return repository.Books.ToArray();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="user")]
        [Route("BookAddFavorites")]
        public async Task<BookMessage> BookAddFavorites(long id)
        {
            if (ModelState.IsValid)
            {
                Book book = repository.Books.Where(opt => opt.BookID == id).FirstOrDefault();
                if (book is not null)
                {
                    var obj = new BookFavorites()
                    {
                        Book_ID = id,
                        User_ID = User.Identity.Name
                    };
                    await repository.BookAddFavorites(obj);
                    return new BookMessage() { Book_ID = id, Succes = true, Message = "Книга добавлена в избранное" };
                }
            }
            return new BookMessage() { Book_ID = id, Succes = false, Message = "Книга не добавлена в избранное" };
        }
    }
}

