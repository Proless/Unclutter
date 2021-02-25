using System.Collections.Generic;

namespace Unclutter.Services.Data
{
    public interface IDataRepository<TEntity, in TKey>
    {
        IEnumerable<TEntity> ReadAll();
        TEntity ReadById(TKey id);
        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Delete(TEntity entity);
    }
}
