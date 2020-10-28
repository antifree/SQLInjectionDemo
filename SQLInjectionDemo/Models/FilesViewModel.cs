using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SQLInjectionDemo.Models
{
    public class FilesViewModel
    {
        public string Search { get; set; }

        public IEnumerable<FileItem> Files { get; set; }

    }
}
