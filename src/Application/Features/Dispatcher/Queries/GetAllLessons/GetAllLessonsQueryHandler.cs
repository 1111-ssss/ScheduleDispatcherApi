using Domain.Abstractions.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Dispatcher.GetAllLessons;

public class GetAllLessonsQueryHandler : IRequestHandler<GetAllLessonsQuery, Result<AllLessonsDTO>>
{
    private readonly ILogger<GetAllLessonsQueryHandler> _logger;

    public GetAllLessonsQueryHandler(
        ILogger<GetAllLessonsQueryHandler> logger)
    {
        _logger = logger;
    }

    public async Task<Result<AllLessonsDTO>> Handle(GetAllLessonsQuery query, CancellationToken ct)
    {
        throw new NotImplementedException("Get All Lessons Query еще не реализована");

        // return Result.Success();
    }
}