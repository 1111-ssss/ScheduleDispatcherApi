using Application.Abstractions.Repository;
using Ardalis.Specification.EntityFrameworkCore;
using Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Repository;

public class BaseRepository<T> : RepositoryBase<T>, IBaseRepository<T> where T : class
{
    private readonly AppDbContext _db;

    private readonly DbSet<T> _dbSet;
    public BaseRepository(AppDbContext context) : base(context)
    {
        _db = context;
        _dbSet = context.Set<T>();
    }
}