using Application.Features.Dispatcher.Common;
using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Dispatcher.SaveWorkload;

public record SaveWorkloadCommand(
    string Lesson,
    string Teacher,
    string Group,
    int Semester,
    WorkloadSummaryDTO WorkloadSummary
) : IRequest<Result>;