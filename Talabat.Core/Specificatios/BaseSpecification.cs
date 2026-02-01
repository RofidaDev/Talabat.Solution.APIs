using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specificatios
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get ; set; }
        public List<Expression<Func<T, object>>> Includes { get ; set ; } = new List<Expression<Func<T, object>>> ();
        public Expression<Func<T, object>> OrderBy { get ; set; }
        public Expression<Func<T, object>> OrderByDesc { get ; set; }
        public int Skip { get; set ; }
        public int Take { get; set  ; }
        public bool IsPaginationEnabled { get ; set ; }    //false
 
        public BaseSpecification()  //It's no longer useful
        {
           // Criteria = null
        }
        public BaseSpecification(Expression<Func<T,bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;  //p => p.Id == 4
        }
        public void AddOrderBy(Expression<Func<T,object>> orderExpression)   //just setter
        {
            OrderBy = orderExpression;

        }
        public void AddOrderByDesc(Expression<Func<T, object>> orderExpression)   //just setter
        {
            OrderByDesc = orderExpression;

        }
        public void ApplyPagination(int skip,int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }

    }
}
