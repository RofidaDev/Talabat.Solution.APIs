using System.Collections;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork  //unit of work between services and dbContext
    {
        private readonly StoreContext _dbContext;
        /*private Dictionary<string, IGenaricRepository<BaseEntity>> _repositories;*/ //Generic //or
        private Hashtable _repositories;  // like dict but non generic [object,object]
       
        public UnitOfWork(StoreContext dbContext) //Ask CLR for creating obj from DbContext implicitly
        {
            _dbContext = dbContext;
            //_repositories = new Dictionary<string, IGenaricRepository<BaseEntity>>();
            _repositories = new Hashtable();
        }
        IGenaricRepository<TEntity> IUnitOfWork.Repository<TEntity>()
        {
            var key = typeof(TEntity).Name;  // Order
            if (!_repositories.ContainsKey(key))
            {
                var repository = new GenaricRepository<TEntity>(_dbContext);
                _repositories.Add(key, repository);
            }
            return _repositories[key] as IGenaricRepository<TEntity>;
                
        }
        public async Task<int> CompleteAsync()
          => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()  //Resources Clean up(close connections,clean memory...)
          => await _dbContext.DisposeAsync();

       
    }
}
