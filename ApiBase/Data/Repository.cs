using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmstelAPI.Models;
using ApiBase.Controllers;
using ApiBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApiBase.Data
{
    public class Repository<TDbContext> : IRepository where TDbContext : DbContext
    {
        private readonly IConfiguration _appsettings;
        private readonly AppDbContext _context;
        private TDbContext _dbContext;

        public Repository(TDbContext context, IConfiguration appsettings, AppDbContext appcontext)
        {
            this._dbContext = context;
            this._appsettings = appsettings;
            this._context = appcontext;
        }
        
        public async Task<int> CreateAsync<T>(T entity) where T : class
        {
            this._dbContext.Set<T>().Add(entity);
            await this._dbContext.SaveChangesAsync();
            var IdProperty = entity.GetType().GetProperty("Id").GetValue(entity, null);
            return (int)IdProperty;
        }

        public async Task<string> CreateAsyncString<T>(T entity) where T : class
        {
            this._dbContext.Set<T>().Add(entity);
            await this._dbContext.SaveChangesAsync();
            var IdProperty = entity.GetType().GetProperty("Id").GetValue(entity, null);
            return IdProperty.ToString();
        }
        
        public async Task<List<T>> SelectAll<T>() where T : class
        {
            return await this._dbContext.Set<T>().ToListAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            this._dbContext.Set<T>().Update(entity);
            _ = await this._dbContext.SaveChangesAsync();
        }

        public async Task<T> SelectById<T>(int Id) where T : class
        {
            return await this._dbContext.Set<T>().FindAsync(Id);
        }

        public async Task<List<T>> SelectByRange<T>(int pageNumber, int pagSize) where T : class
        {
            return await this._dbContext
                    .Set<T>()
                    .Skip((pageNumber - 1) * pagSize)
                    .Take(pagSize)
                    .ToListAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            this._dbContext.Set<T>().Update(entity);
            _ = await this._dbContext.SaveChangesAsync();
        }

        //public async Task<bool> CreateUser(UsersVr entity)
        //{
        //    this._dbContext.Set<UsersVr>().Add(entity);
        //    var created = await this._dbContext.SaveChangesAsync();
        //    return created > 0;
        //}

        //public async Task<bool> VerifyUser(UsersVr entity)
        //{
        //    this._dbContext.Set<UsersVr>().Update(entity);
        //    var created = await this._dbContext.SaveChangesAsync();
        //    return created > 0;
        //}

        //public async Task UpdateUserRegisterAsync(UsersVr u, UserUpdateModel uu)
        //{
        //    if (uu.FirstName != null)
        //    { u.FirstName = uu.FirstName; }
        //    if (uu.LastName != null)
        //    { u.LastName = uu.LastName; }
        //    if (uu.Email != null)
        //    { u.Email = uu.Email; }
           
        //    this._dbContext.Set<UsersVr>().Update(u);
        //    _ = await this._dbContext.SaveChangesAsync();
        //}
        //public async Task UpdateUserAsync(UsersVr u, AwardUserAddress ua, UserUpdateModel uu)
        //{
        //    if (uu.FirstName != null)
        //    { u.FirstName = uu.FirstName; }
        //    if (uu.LastName != null)
        //    { u.LastName = uu.LastName; }
        //    if (uu.Email != null)
        //    { u.Email = uu.Email; }
          

        //    if (uu.Municipio != null)
        //    { ua.Province = uu.Municipio; }
           
        //    if (uu.PostalCode != 0)
        //    { ua.PostalCode = uu.PostalCode; }
        //    if (uu.City != null)
        //    { ua.City = uu.City; }
        //    if (uu.State != null)
        //    { ua.State = uu.State; }
            

        //    this._dbContext.Set<UsersVr>().Update(u);
        //    _ = await this._dbContext.SaveChangesAsync();
        //    this._dbContext.Set<AwardUserAddress>().Update(ua);
        //    _ = await this._dbContext.SaveChangesAsync();
        //}

        //public async Task UpdateUserProfileAsync(UsersVr u, AwardUserAddress ua, UpdateModel uu)
        //{
        //    if (uu.FirstName != null)
        //    { u.FirstName = uu.FirstName; }
        //    if (uu.LastName != null)
        //    { u.LastName = uu.LastName; }
        //    if (uu.Email != null)
        //    { u.Email = uu.Email; }
         

        //    if (uu.Street != null)
        //    { ua.Street = uu.Street; }
        //    if (uu.ExtNumber != null)
        //    { ua.ExtNumber = uu.ExtNumber; }
        //    if (uu.IntNumber != null)
        //    { ua.IntNumber = uu.IntNumber; }
        //    if (uu.Referency != null)
        //    { ua.Referency = uu.Referency; }

        //    this._dbContext.Set<UsersVr>().Update(u);
        //    _ = await this._dbContext.SaveChangesAsync();
        //    this._dbContext.Set<AwardUserAddress>().Update(ua);
        //    _ = await this._dbContext.SaveChangesAsync();
        //}

    }
}
