using Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IBaseRespository<T> where T: AbstractMongoEntity, new()
    {
        T GetById(string id);
        List<T> GetList(Expression<Func<T, bool>> expr = null);
        T GetOne(Expression<Func<T, bool>> expr);
        void Create(T entity);
        void Update(string id, T entity);
        void Delete(T entity);
        void Delete(string id);
    }
}
