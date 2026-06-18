using Domain.Model.Result;
using MediatR;

namespace Application.Features.Dispatcher.GetTeachers;

public record GetTeachersQuery(
    string? Group,
    string? Lesson,
    int? Semester) : IRequest<Result<List<string>>>;
