namespace Application.Abstractions.Interfaces.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    public Task<int> CommitAsync(CancellationToken ct = default);
    // public Task<int> SaveChangesAsync(CancellationToken ct = default);
}