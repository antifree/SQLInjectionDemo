using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQLInjectionDemo.Models
{
    public class FileItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public int UserId { get; set; }

        public IdentityUser User { get; set; }
    } 
}
