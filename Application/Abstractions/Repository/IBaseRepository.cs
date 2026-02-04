using Ardalis.Specification;

namespace Application.Abstractions.Repository;

public interface IBaseRepository<T> : IRepositoryBase<T> where T : class
{

}