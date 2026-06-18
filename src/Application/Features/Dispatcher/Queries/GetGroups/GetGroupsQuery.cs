using Domain.Model.Result;
using MediatR;

namespace Application.Features.Dispatcher.GetGroups;

public record GetGroupsQuery() : IRequest<Result<List<string>>>;
