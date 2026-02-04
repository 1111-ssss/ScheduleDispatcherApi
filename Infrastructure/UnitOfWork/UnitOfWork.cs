using Application.Abstractions.Interfaces.UnitOfWork;
using Infrastructure.DataBase.Context;

namespace Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    public UnitOfWork(AppDbContext courceDbContext)
    {
        _db = courceDbContext;
    }
    public async Task<int> CommitAsync(CancellationToken ct = default)
    {
        return await _db.SaveChangesAsync(ct);
    }
    public void Dispose()
    {
        _db.Dispose();
    }
}