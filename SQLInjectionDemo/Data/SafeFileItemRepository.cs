using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using SQLInjectionDemo.Common;
using SQLInjectionDemo.Models;

namespace SQLInjectionDemo.Data
{
    public class SafeFileItemRepository : BaseFileItemRepository
    {
        public SafeFileItemRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {

        }

        public override List<FileItem> GetAll()
        {
            return DbContext.Set<FileItem>()
                .FromSqlRaw($"SELECT * FROM dbo.Files WHERE UserId = @user_id", new SqlParameter("@user_id", User.GetUserId()))
                .ToList();
        }

        public override List<FileItem> GetAllByName(string name)
        {
            return DbContext.Set<FileItem>()
                 .FromSqlRaw($"SELECT * FROM dbo.Files WHERE UserId = @user_id AND Name LIKE '%@name%'",
                     new SqlParameter("@user_id", User.GetUserId()),
                     new SqlParameter("@name", name))
                 .ToList();
        }

        public override FileItem Get(string id)
        {
            return DbContext.Set<FileItem>()
                .FromSqlRaw($"SELECT * FROM dbo.Files WHERE UserId = @user_id AND Id = @id",
                    new SqlParameter("@user_id", User.GetUserId()),
                    new SqlParameter("@id", id))
                .FirstOrDefault();
        }

        public override FileItem Add(FileItem entity)
        {
            entity.Id = GenerateId();
            entity.UserId = User.GetUserId();
            var sql = $"INSERT dbo.Files (Id, Name, ContentType, Content, UserId) VALUES (@id, @name, @content_type, @content, @user_id)";
            DbContext.Database.ExecuteSqlRaw(sql,
                new SqlParameter("@id", entity.Id),
                new SqlParameter("@name", entity.Name),
                new SqlParameter("@content_type", entity.ContentType),
                new SqlParameter("@content", SqlDbType.VarBinary) { Value = entity.Content },
                new SqlParameter("@user_id", entity.UserId));

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
            var sql = $"UPDATE dbo.Files SET Name=@name, ContentType=@content_type, Content=@content" +
                $" WHERE UserId=@user_id AND Id=@id";

            DbContext.Database.ExecuteSqlRaw(sql,
              new SqlParameter("@name", entity.Name),
              new SqlParameter("@content_type", entity.ContentType),
              new SqlParameter("@content", SqlDbType.VarBinary) { Value = entity.Content },
              new SqlParameter("@user_id", entity.UserId),
              new SqlParameter("@id", entity.Id));

            return entity;
        }
    }
}
