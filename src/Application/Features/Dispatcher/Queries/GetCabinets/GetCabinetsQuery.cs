using Domain.Model.Result;
using MediatR;

namespace Application.Features.Dispatcher.GetCabinets;

public record GetCabinetsQuery() : IRequest<Result<Dictionary<string, List<string>>>>;
