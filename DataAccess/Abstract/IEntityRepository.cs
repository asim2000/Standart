using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IEntityRepository<Entity>
    {
        Task<List<Entity>> GetAll(Expression<Func<Entity, bool>> filter = null);
        Task<Entity> Get(Expression<Func<Entity, bool>> filter);
        Task Create(Entity entity);
        Task Update(Entity entity);
        Task Delete(Entity entity);
    }
}
