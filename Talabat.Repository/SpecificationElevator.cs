using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Specificatios;

namespace Talabat.Repository
{
    internal static class SpecificationElevator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputquery , ISpecification<T> spec)
        {
            var query = inputquery; //dbcontext.Set<Product>()
            if(spec.Criteria is not null)  //p => p.Id == 1 
               query = query.Where(spec.Criteria);
            //query = _dbcontext.Set<Product>().Where(P =>p.id == 1)
            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if(spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);
            if(spec.IsPaginationEnabled)
                query=query.Skip(spec.Skip).Take(spec.Take);
            query = spec.Includes.Aggregate(query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));
            //Includes
            //1. p=>p.Brand
            //2. p=>p.Category
            //_dbcontext.Set<Product>().Where(P =>p.id == 1).OrderBy(P=>P.Name).Include(P=>P.Brand)
            //_dbcontext.Set<Product>().Where(P =>p.id == 1).OrderBy(P=>P.Name).Include(P=>P.Brand).Include(P=>P.Category)
            return query;
        }
    }
}
