using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications;

public class DayScheduleWithLessonsSpec : Specification<DayScheduleEntity>
{
    public DayScheduleWithLessonsSpec(string groupName, DateOnly date)
    {
        Query.Where(d => d.GroupName == groupName && d.Date == date)
             .Include(d => d.Lessons)
             .AsNoTracking();
    }
}
