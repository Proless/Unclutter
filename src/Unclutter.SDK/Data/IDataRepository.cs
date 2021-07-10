using System.Collections.Generic;

namespace Unclutter.SDK.Data
{
    public interface IDataRepository<TEntity, in TKey>
    {
        IEnumerable<TEntity> ReadAll();
        TEntity Read(TKey id);
        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Delete(TEntity entity);
    }
}
