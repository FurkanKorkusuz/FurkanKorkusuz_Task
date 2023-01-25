using Core.Entities.Abstract;

namespace Core.DataAccess.EF
{
    public interface IRepository<T> where T : class, IEntity
    {
    }
}