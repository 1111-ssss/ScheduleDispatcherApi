using Ardalis.Specification;

namespace Application.Abstractions.Repository.Base;

public interface IBaseRepository<T> : IRepositoryBase<T> where T : class
{

}