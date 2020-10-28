using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using SQLInjectionDemo.Common;
using SQLInjectionDemo.Models;

namespace SQLInjectionDemo.Data
{
    public class UnsafeFileItemRepository : BaseFileItemRepository
    {
        public UnsafeFileItemRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {

        }

        public override List<FileItem> GetAll()
        {
            return DbContext.Set<FileItem>()
                .FromSqlRaw($"SELECT * FROM dbo.Files WHERE UserId = {User.GetUserId()}")
                .ToList();
        }

        public override List<FileItem> GetAllByName(string name)
        {
            return DbContext.Set<FileItem>()
                 .FromSqlRaw($"SELECT * FROM dbo.Files WHERE UserId = {User.GetUserId()} AND Name LIKE '%{name}%'")
                 .ToList();
        }

        public override FileItem Get(string id)
        {
            return DbContext.Set<FileItem>()
                .FromSqlRaw($"SELECT * FROM dbo.Files WHERE UserId = {User.GetUserId()} AND Id = '{id}'")
                .FirstOrDefault();
        }

        public override FileItem Add(FileItem entity)
        {
            entity.Id = GenerateId();
            entity.UserId = User.GetUserId();
            var sql = $"INSERT dbo.Files (Id, Name, ContentType, Content, UserId) VALUES ('{entity.Id}', '{entity.Name}', '{entity.ContentType}', 0x{string.Join("", entity.Content.Select(n => n.ToString("X2")))}, {entity.UserId})";
            DbContext.Database.ExecuteSqlRaw(sql);

            return entity;
        }

        public override FileItem Delete(string id)
        {
            var file = Get(id);

            var sql = $"DELETE FROM dbo.Files WHERE UserId={User.GetUserId()} AND Id='{id}'";
            DbContext.Database.ExecuteSqlRaw(sql);

            return file;
        }

        public override FileItem Update(string id, FileItem entity)
        {
            entity.Id = id;
            entity.UserId = User.GetUserId();
            var sql = $"UPDATE dbo.Files SET Name = '{entity.Name}', ContentType = '{entity.ContentType}', Content = 0x{string.Join("", entity.Content.Select(n => n.ToString("X2")))}" +
                $" WHERE UserId = {User.GetUserId()} AND Id = '{entity.Id}'";

            DbContext.Database.ExecuteSqlRaw(sql);

            return entity;
        }
    }
}
