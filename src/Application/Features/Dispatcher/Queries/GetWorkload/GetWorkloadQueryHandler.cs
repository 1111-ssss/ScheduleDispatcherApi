using Application.Abstractions.Repository.Base;
using Application.Features.Dispatcher.Common;
using Application.Features.Dispatcher.GetWorkload;
using Domain.Entities;
using Domain.Model.Result;
using Domain.Specifications.Dispatcher;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Dispatcher.SaveWorkload;

public class GetWorkloadQueryHandler : IRequestHandler<GetWorkloadQuery, Result<WorkloadSummaryDTO>>
{
    private readonly ILogger<GetWorkloadQueryHandler> _logger;
    private readonly IBaseRepository<Employer> _employerRepository;

    public GetWorkloadQueryHandler(
        ILogger<GetWorkloadQueryHandler> logger,
        IBaseRepository<Employer> employerRepository
    )
    {
        _logger = logger;
        _employerRepository = employerRepository;
    }

    public async Task<Result<WorkloadSummaryDTO>> Handle(GetWorkloadQuery query, CancellationToken ct)
    {
        var workloadResult = await _employerRepository.ListAsync(new GetWorkloadSpec(
            query.Teacher,
            query.Group,
            query.Semester,
            query.Lesson
        ));
        var employer = workloadResult.FirstOrDefault();

        if (employer == null) 
            return Result<WorkloadSummaryDTO>.Success(new WorkloadSummaryDTO(new(), new()));
        
        var allSchedules = employer.Teachers
            .SelectMany(t => t.SubjectTeachers)
            .Where(st => st.CnGNavigation.Name == query.Group && 
                        st.IdSubjectNavigation.Name == query.Lesson)
            .Where(st => query.Semester == 1 
                ? st.Subjectteachersemesters.Any(s => s.Semester1 == true)
                : st.Subjectteachersemesters.Any(s => s.Semester2 == true))
            .SelectMany(st => st.SubjectTeacherSchedules)
            .Select(sts => new 
            { 
                Schedule = sts.IdScheduleNavigation, 
                IsSplit = sts.GroupSplit,
                SubjectName = sts.IdsubjectTeacherNavigation.IdSubjectNavigation.Name 
            })
            .ToList();

        return Result<WorkloadSummaryDTO>.Success(new WorkloadSummaryDTO(
            WorkloadList: allSchedules.Select(item => new WorkloadDTO(
                item.SubjectName,
                item.Schedule.Lessonnumber ?? 0,
                item.IsSplit ?? false,
                item.Schedule.Date1 ?? DateTime.MinValue,
                item.Schedule.Date2 ?? DateTime.MinValue,
                item.Schedule.RemovalAsFirst?.IdRemoval ?? item.Schedule.RemovalAsSecond?.IdRemoval ?? 0
            )).ToList(),

            RemovalList: allSchedules
                .SelectMany(item => new[] { item.Schedule.RemovalAsFirst, item.Schedule.RemovalAsSecond })
                .Where(r => r != null)
                .DistinctBy(r => r!.IdRemoval)
                .Select(r => new RemovalDTO(
                    r!.IdRemoval,
                    r!.IdSchedule1,
                    r!.IdSchedule2
                )).ToList()
        ));
    }
}