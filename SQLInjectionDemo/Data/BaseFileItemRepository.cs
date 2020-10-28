using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using SQLInjectionDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SQLInjectionDemo.Data
{
    public abstract class BaseFileItemRepository : IFileItemRepository
    {
        protected readonly ClaimsPrincipal User;

        protected readonly ApplicationDbContext DbContext;

        public BaseFileItemRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            DbContext = dbContext;
            User = httpContextAccessor.HttpContext.User;
        }

        protected string GenerateId()
        {
            return shortid.ShortId.Generate(new shortid.Configuration.GenerationOptions 
            { 
                UseNumbers = true,
            });
        }

        public abstract List<FileItem> GetAll();

        public abstract List<FileItem> GetAllByName(string name);

        public abstract FileItem Get(string id);

        public abstract FileItem Add(FileItem entity);

        public abstract FileItem Update(string id, FileItem entity);

        public abstract FileItem Delete(string id);
    }
}
