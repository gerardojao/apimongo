//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;

//namespace ApiBase.Data
//{
//    public class Repository<TDbContext> : IRepository where TDbContext : DbContext
//    {
     
//        private TDbContext _dbContext;

//        public Repository(TDbContext context, IConfiguration appsettings, AppDbContext appcontext)
//        {
//            this._dbContext = context;
           
//        }
        
//        public async Task<int> CreateAsync<T>(T entity) where T : class
//        {
//            this._dbContext.Set<T>().Add(entity);
//            await this._dbContext.SaveChangesAsync();
//            var IdProperty = entity.GetType().GetProperty("Id").GetValue(entity, null);
//            return (int)IdProperty;
//        }

//        public async Task<string> CreateAsyncString<T>(T entity) where T : class
//        {
//            this._dbContext.Set<T>().Add(entity);
//            await this._dbContext.SaveChangesAsync();
//            var IdProperty = entity.GetType().GetProperty("Id").GetValue(entity, null);
//            return IdProperty.ToString();
//        }
        
//        public async Task<List<T>> SelectAll<T>() where T : class
//        {
//            return await this._dbContext.Set<T>().ToListAsync();
//        }

//        public async Task DeleteAsync<T>(T entity) where T : class
//        {
//            this._dbContext.Set<T>().Update(entity);
//            _ = await this._dbContext.SaveChangesAsync();
//        }

//        public async Task<T> SelectById<T>(int Id) where T : class
//        {
//            return await this._dbContext.Set<T>().FindAsync(Id);
//        }

//        public async Task<List<T>> SelectByRange<T>(int pageNumber, int pagSize) where T : class
//        {
//            return await this._dbContext
//                    .Set<T>()
//                    .Skip((pageNumber - 1) * pagSize)
//                    .Take(pagSize)
//                    .ToListAsync();
//        }

//        public async Task UpdateAsync<T>(T entity) where T : class
//        {
//            this._dbContext.Set<T>().Update(entity);
//            _ = await this._dbContext.SaveChangesAsync();
//        }


//    //public async Task UpdateUserRegisterAsync(UsersVr u, UserVrUpdateModel uu)
//        //{
//        //    if (uu.FullName != null)
//        //    { u.FullName = uu.FullName; }
          
//        //    if (uu.Email != null)
//        //    { u.Email = uu.Email; }
           
//        //    this._dbContext.Set<UsersVr>().Update(u);
//        //    _ = await this._dbContext.SaveChangesAsync();
//        //}
//        //public async Task UpdateUserAsync(UsersVr u, UserVrLocation ua, UserVrUpdateModel uu)
//        //{
//        //    if (uu.FullName != null)
//        //    { u.FullName = uu.FullName; }
           
//        //    if (uu.Email != null)
//        //    { u.Email = uu.Email; }
           

//        //    if (uu.Province != null)
//        //    { ua.Province = uu.Province; }
           
//        //    if (uu.Colony != null)
//        //    { ua.Colony = uu.Colony; }
//        //    if (uu.PostalCode != 0)
//        //    { ua.PostalCode = uu.PostalCode; }
//        //    if (uu.City != null)
//        //    { ua.City = uu.City; }
//        //    if (uu.State != null)
//        //    { ua.State = uu.State; }
           

//        //    this._dbContext.Set<UsersVr>().Update(u);
//        //    _ = await this._dbContext.SaveChangesAsync();
//        //    this._dbContext.Set<UserVrLocation>().Update(ua);
//        //    _ = await this._dbContext.SaveChangesAsync();
//        //}

//        //public async Task UpdateUserProfileAsync(UsersVr u, UserVrLocation ua, UpdateModel uu)
//        //{
//        //    if (uu.FullName != null)
//        //    { u.FullName = uu.FullName; }           
//        //    if (uu.Email != null)
//        //    { u.Email = uu.Email; }         
//        //    this._dbContext.Set<UsersVr>().Update(u);
//        //    _ = await this._dbContext.SaveChangesAsync();
//        //    this._dbContext.Set<UserVrLocation>().Update(ua);
//        //    _ = await this._dbContext.SaveChangesAsync();
//        //}

//        //Task IRepository.CreateUser(UsersVr user)
//        //{
//        //    throw new NotImplementedException();
//        //}
//    }
//}
