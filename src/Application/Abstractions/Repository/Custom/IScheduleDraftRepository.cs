using Application.Abstractions.Repository.Base;
using Domain.Entities;

namespace Application.Abstractions.Repository.Custom;

public interface IScheduleDraftRepository : IBaseRepository<DayScheduleEntity>
{
    Task<DayScheduleEntity?> GetWithLessonsAsync(string groupName, DateOnly date, CancellationToken ct);
    Task SaveDraftAsync(string groupName, DateOnly date, List<LessonEntity> lessons, CancellationToken ct);
}
