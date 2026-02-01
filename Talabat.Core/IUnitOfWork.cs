using System;
using System.Collections.Generic;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
namespace Talabat.Core
{
    public interface IUnitOfWork:IAsyncDisposable 
    {//properity signature for each and every repository(how many tables)
        IGenaricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;   // generate repositories
        Task<int> CompleteAsync();   //save changes


    }
}
