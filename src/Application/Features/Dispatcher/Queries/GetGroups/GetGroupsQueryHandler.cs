using Application.Abstractions.Repository.Base;
using Domain.Entities;
using Domain.Model.Result;
using Domain.Specifications.Dispatcher;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Dispatcher.GetGroups;

public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, Result<List<string>>>
    {
    private readonly ILogger<GetGroupsQueryHandler> _logger;
    private readonly IBaseRepository<Group> _groupRepository;

    public GetGroupsQueryHandler(
        ILogger<GetGroupsQueryHandler> logger,
        IBaseRepository<Group> groupRepository
    )
    {
        _logger = logger;
        _groupRepository = groupRepository;
    }

    public async Task<Result<List<string>>> Handle(GetGroupsQuery query, CancellationToken ct)
    {
        var groups = await _groupRepository.ListAsync(new GetGroupsSpec(), ct);

        var names = groups
            .Select(g => g.Name)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name!)
            .Distinct()
            .OrderBy(name => name)
            .ToList();

        return Result<List<string>>.Success(names);
    }
    }
