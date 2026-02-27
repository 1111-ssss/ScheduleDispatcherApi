using Application.Abstractions.Interfaces.UnitOfWork;
using Domain.Model.CustomException;
using Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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
        try
        {
            return await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            throw new CustomDbException(ex.Message, pgEx.SqlState);
        }
    }
    public void Dispose()
    {
        _db.Dispose();
    }
}