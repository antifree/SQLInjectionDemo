using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SQLInjectionDemo.Data;
using SQLInjectionDemo.Models;

namespace SQLInjectionDemo.Controllers
{

    [Authorize]
    public class FileController : Controller
    {

        private readonly IFileItemRepository _repo;

        public FileController(IFileItemRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index([FromQuery]string search = "")
        {
            var files = !string.IsNullOrEmpty(search)
                ? _repo.GetAllByName(search)
                : _repo.GetAll();

            return View(new FilesViewModel { 
                Search = search,
                Files = files
            });
        }

        public IActionResult Get(string id)
        {
            var file = _repo.Get(id);
            if (file == null)
                return NotFound();
            return File(file.Content, file.ContentType, file.Name);
        }

        [HttpPost]
        public IActionResult Add(IFormFile uploadedFile)
        {
            var fileName = System.IO.Path.GetFileName(uploadedFile.FileName);
            var ms = new MemoryStream();
            uploadedFile.OpenReadStream().CopyTo(ms);
            var file = new FileItem { Name = fileName, ContentType = uploadedFile.ContentType, Content = ms.ToArray() };
            _repo.Add(file);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
