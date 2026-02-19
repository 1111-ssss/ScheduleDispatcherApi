using Application.Features.Dispatcher.Common;
using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Dispatcher.GetWorkload;

public record GetWorkloadQuery(
    string Lesson,
    string Teacher,
    string Group,
    int Semester
) : IRequest<Result<WorkloadSummaryDTO>>;