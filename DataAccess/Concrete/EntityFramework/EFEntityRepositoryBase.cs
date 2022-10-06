using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFEntityRepositoryBase<Entity,Context> where Entity : class
        where Context : DbContext, new()
    {
        private readonly Context context;
        public EFEntityRepositoryBase()
        {
            this.context = new Context();
        }

        public async Task Create(Entity entity)
        {
            context.Entry<Entity>(entity).State = EntityState.Added;
            await context.SaveChangesAsync();
        }

        public async Task Delete(Entity entity)
        {
            context.Entry<Entity>(entity).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        public virtual async Task<Entity> Get(Expression<Func<Entity, bool>> filter)
        {
            return await context.Set<Entity>().Where(filter).FirstOrDefaultAsync();
        }

        public virtual async Task<List<Entity>> GetAll(Expression<Func<Entity, bool>> filter = null)
        {
            return filter == null
                ? await context.Set<Entity>().ToListAsync()
                : await context.Set<Entity>().Where(filter).ToListAsync();
        }
        public async Task Update(Entity entity)
        {
            context.Entry<Entity>(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

    }
}
