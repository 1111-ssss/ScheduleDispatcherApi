using Domain.Model.Result;
using MediatR;

namespace Application.Features.Dispatcher.GetAllLessons;

public record GetAllLessonsQuery() : IRequest<Result<AllLessonsDTO>>;