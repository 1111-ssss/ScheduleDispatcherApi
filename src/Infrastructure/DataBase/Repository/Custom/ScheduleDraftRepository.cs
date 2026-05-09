using Application.Abstractions.Repository.Custom;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Entities;
using Domain.Specifications;
using Infrastructure.DataBase.Context;
using Infrastructure.DataBase.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Repository.Custom;

public class ScheduleDraftRepository : BaseRepository<DayScheduleEntity>, IScheduleDraftRepository
{
    public ScheduleDraftRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<DayScheduleEntity?> GetWithLessonsAsync(string groupName, DateOnly date, CancellationToken ct)
    {
        var spec = new DayScheduleWithLessonsSpec(groupName, date);
        return await DbContext.Set<DayScheduleEntity>()
            .WithSpecification(spec)
            .FirstOrDefaultAsync(ct);
    }

    public async Task SaveDraftAsync(string groupName, DateOnly date, List<LessonEntity> lessons, CancellationToken ct)
    {
        var existing = await GetWithLessonsAsync(groupName, date, ct);

        if (existing != null)
        {
            var oldLessons = DbContext.Set<LessonEntity>()
                .Where(l => l.Groupid == existing.Id);
            DbContext.RemoveRange(oldLessons);
            await DbContext.SaveChangesAsync(ct);
        }
        else
        {
            existing = new DayScheduleEntity { GroupName = groupName, Date = date };
            await DbContext.Set<DayScheduleEntity>().AddAsync(existing, ct);
            await DbContext.SaveChangesAsync(ct); 
        }

        foreach (var lesson in lessons)
        {
            lesson.Groupid = existing.Id; 
            await DbContext.Set<LessonEntity>().AddAsync(lesson, ct);
        }

        await DbContext.SaveChangesAsync(ct);
    }
}
