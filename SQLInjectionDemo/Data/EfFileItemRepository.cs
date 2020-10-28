using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using SQLInjectionDemo.Common;
using SQLInjectionDemo.Models;

namespace SQLInjectionDemo.Data
{
    public class EfFileItemRepository: BaseFileItemRepository
    {
        public EfFileItemRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {

        }

        public override List<FileItem> GetAll()
        {
            return DbContext.Set<FileItem>()
                .Where(f => f.UserId == User.GetUserId())
                .ToList();
        }

        public override List<FileItem> GetAllByName(string name)
        {
            return DbContext.Set<FileItem>()
                .Where(f => f.UserId == User.GetUserId()
                    && f.Name.Contains(name))
                .ToList();
        }

        public override FileItem Get(string id)
        {
            return DbContext.Set<FileItem>()
                .FirstOrDefault(f => f.UserId == User.GetUserId() && f.Id == id);
        }

        public override FileItem Add(FileItem entity)
        {
            entity.Id = GenerateId();
            entity.UserId = User.GetUserId();
            DbContext.Set<FileItem>().Add(entity);
            DbContext.SaveChanges();

            return entity;
        }

        public override FileItem Delete(string id)
        {
            var file = Get(id);

            DbContext.Set<FileItem>().Remove(file);
            DbContext.SaveChanges();

            return file;
        }

        public override FileItem Update(string id, FileItem entity)
        {
            entity.Id = id;
            entity.UserId = User.GetUserId();
            DbContext.Set<FileItem>().Update(entity);
            DbContext.SaveChanges();

            return entity;
        }
    }
}
