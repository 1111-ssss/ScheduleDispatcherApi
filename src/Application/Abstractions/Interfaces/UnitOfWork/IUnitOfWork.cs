namespace Application.Abstractions.Interfaces.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    public Task<int> CommitAsync(CancellationToken ct = default);
    
}