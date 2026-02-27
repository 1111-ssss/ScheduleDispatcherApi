using Application.Abstractions.Interfaces.UnitOfWork;
using Domain.Model.Result;
using MediatR;

namespace Application.Features.Sync.SyncData;

public class SyncDataCommandHandler : IRequestHandler<SyncDataCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    public SyncDataCommandHandler(
        IUnitOfWork unitOfWork
    )
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SyncDataCommand request, CancellationToken ct)
    {
        throw new NotImplementedException("Синхронизация с Единым Колледжем еще не реализована.");

        // return Result.Success();
    }
}