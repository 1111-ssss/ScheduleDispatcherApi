using Application.Abstractions.Repository.Base;
using Domain.Entities;
using Domain.Model.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Dispatcher.GetCabinets;

public class GetCabinetsQueryHandler : IRequestHandler<GetCabinetsQuery, Result<Dictionary<string, List<string>>>>
    {
    private readonly ILogger<GetCabinetsQueryHandler> _logger;
    private readonly IBaseRepository<Classroom> _classroomRepository;

    public GetCabinetsQueryHandler(
        ILogger<GetCabinetsQueryHandler> logger,
        IBaseRepository<Classroom> classroomRepository
    )
    {
        _logger = logger;
        _classroomRepository = classroomRepository;
    }

    public async Task<Result<Dictionary<string, List<string>>>> Handle(GetCabinetsQuery query, CancellationToken ct)
    {
        var rooms = await _classroomRepository.ListAsync(ct);

        static string Display(Classroom c) =>
            !string.IsNullOrWhiteSpace(c.Name) ? c.Name! : c.ClassroomNumber?.ToString() ?? string.Empty;

        var result = new Dictionary<string, List<string>>
        {
            ["Компьютерные"] = rooms
                .Where(c => c.IsPcClassroom == true)
                .Select(Display)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct()
                .OrderBy(name => name)
                .ToList(),
            ["Лекционные"] = rooms
                .Where(c => c.IsPcClassroom != true)
                .Select(Display)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct()
                .OrderBy(name => name)
                .ToList()
        };

        return Result<Dictionary<string, List<string>>>.Success(result);
    }
    }
