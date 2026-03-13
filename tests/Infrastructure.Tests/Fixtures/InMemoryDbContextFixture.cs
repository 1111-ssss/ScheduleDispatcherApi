using Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Tests.Fixtures;

public class InMemoryDbContextFixture
{
    public string CreateDatabaseName() => Guid.NewGuid().ToString();

    public AppDbContext CreateContext(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName ?? CreateDatabaseName())
            .Options;

        return new AppDbContext(options);
    }

    public void Seed(AppDbContext context)
    {
        var specialty1 = new Specialty
        {
            CnSpec = "09.02.07",
            Name = "Информационные системы",
            Fullname = "Информационные системы и программирование",
            CnT = "T001"
        };

        var specialty2 = new Specialty
        {
            CnSpec = "10.02.01",
            Name = "Кибербезопасность",
            Fullname = "Обеспечение информационной безопасности",
            CnT = "T002"
        };

        context.Specialties.AddRange(specialty1, specialty2);
        context.SaveChanges();

        var employer1 = new Employer
        {
            CnE = "E001",
            Surname = "Иванов",
            Name = "Иван",
            FatherName = "Иванович"
        };

        var employer2 = new Employer
        {
            CnE = "E002",
            Surname = "Петров",
            Name = "Петр",
            FatherName = "Петрович"
        };

        context.Employers.AddRange(employer1, employer2);
        context.SaveChanges();

        var teacher1 = new Teacher
        {
            CnT = "T001",
            CnE = "E001",
            CnENavigation = employer1
        };

        var teacher2 = new Teacher
        {
            CnT = "T002",
            CnE = "E002",
            CnENavigation = employer2
        };

        var teacher3 = new Teacher
        {
            CnT = "T003",
            CnE = "E001",
            CnENavigation = employer1
        };

        context.Teachers.AddRange(teacher1, teacher2, teacher3);
        context.SaveChanges();

        var group1 = new Group
        {
            CnG = "П21",
            Name = "П21",
            Cours = 2,
            CnSpec = "09.02.07"
        };

        var group2 = new Group
        {
            CnG = "П22",
            Name = "П22",
            Cours = 2,
            CnSpec = "09.02.07"
        };

        var group3 = new Group
        {
            CnG = "П23",
            Name = "П23",
            Cours = 2,
            CnSpec = "10.02.01"
        };

        context.Groups.AddRange(group1, group2, group3);
        context.SaveChanges();

        var subject1 = new Subject
        {
            Name = "Базы данных",
            Fullname = "Базы данных и СУБД",
            CnSpec = "09.02.07",
            Totalhourcount = 100,
            Practichourcount = 40,
            PcClassNeed = true,
            Optional = false
        };

        var subject2 = new Subject
        {
            Name = "Веб-программирование",
            Fullname = "Веб-программирование",
            CnSpec = "09.02.07",
            Totalhourcount = 120,
            Practichourcount = 60,
            PcClassNeed = true,
            Optional = false
        };

        var subject3 = new Subject
        {
            Name = "Криптография",
            Fullname = "Основы криптографии",
            CnSpec = "10.02.01",
            Totalhourcount = 80,
            Practichourcount = 30,
            PcClassNeed = false,
            Optional = true
        };

        context.Subjects.AddRange(subject1, subject2, subject3);
        context.SaveChanges();

        var subjectTeacher1 = new SubjectTeacher
        {
            IdSubject = subject1.IdSubject,
            CnG = group1.CnG,
            CnT = teacher1.CnT,
            IdSubjectNavigation = subject1,
            CnGNavigation = group1,
            CnTNavigation = teacher1
        };

        var subjectTeacher2 = new SubjectTeacher
        {
            IdSubject = subject1.IdSubject,
            CnG = group2.CnG,
            CnT = teacher1.CnT,
            IdSubjectNavigation = subject1,
            CnGNavigation = group2,
            CnTNavigation = teacher1
        };

        var subjectTeacher3 = new SubjectTeacher
        {
            IdSubject = subject2.IdSubject,
            CnG = group1.CnG,
            CnT = teacher2.CnT,
            IdSubjectNavigation = subject2,
            CnGNavigation = group1,
            CnTNavigation = teacher2
        };

        var subjectTeacher4 = new SubjectTeacher
        {
            IdSubject = subject3.IdSubject,
            CnG = group3.CnG,
            CnT = teacher3.CnT,
            IdSubjectNavigation = subject3,
            CnGNavigation = group3,
            CnTNavigation = teacher3
        };

        context.SubjectTeachers.AddRange(subjectTeacher1, subjectTeacher2, subjectTeacher3, subjectTeacher4);
        context.SaveChanges();

        var semester1 = new SubjectTeacherSemester
        {
            IdsubjectTeacher = subjectTeacher1.IdsubjectTeacher,
            Semester1 = true,
            Semester2 = false,
            IdsubjectTeacherNavigation = subjectTeacher1
        };

        var semester2 = new SubjectTeacherSemester
        {
            IdsubjectTeacher = subjectTeacher2.IdsubjectTeacher,
            Semester1 = true,
            Semester2 = true,
            IdsubjectTeacherNavigation = subjectTeacher2
        };

        var semester3 = new SubjectTeacherSemester
        {
            IdsubjectTeacher = subjectTeacher3.IdsubjectTeacher,
            Semester1 = false,
            Semester2 = true,
            IdsubjectTeacherNavigation = subjectTeacher3
        };

        var semester4 = new SubjectTeacherSemester
        {
            IdsubjectTeacher = subjectTeacher4.IdsubjectTeacher,
            Semester1 = true,
            Semester2 = false,
            IdsubjectTeacherNavigation = subjectTeacher4
        };

        context.SubjectTeacherSemesters.AddRange(semester1, semester2, semester3, semester4);
        context.SaveChanges();

        var schedule1 = new Schedule
        {
            IdSchedule = 1,
            Lessonnumber = 1,
            Date1 = new DateTime(2025, 9, 1),
            IsPractical = false,
            IsOver = false
        };

        var schedule2 = new Schedule
        {
            IdSchedule = 2,
            Lessonnumber = 2,
            Date1 = new DateTime(2025, 9, 2),
            IsPractical = true,
            IsOver = false
        };

        var schedule3 = new Schedule
        {
            IdSchedule = 3,
            Lessonnumber = 1,
            Date1 = new DateTime(2025, 9, 3),
            IsPractical = false,
            IsOver = false
        };

        var schedule4 = new Schedule
        {
            IdSchedule = 4,
            Lessonnumber = 3,
            Date1 = new DateTime(2025, 9, 4),
            IsPractical = true,
            IsOver = false
        };

        context.Schedules.AddRange(schedule1, schedule2, schedule3, schedule4);
        context.SaveChanges();

        var removal1 = new Removal
        {
            IdRemoval = 1,
            IdSchedule1 = 1,
            IdSchedule2 = 2
        };

        var removal2 = new Removal
        {
            IdRemoval = 2,
            IdSchedule1 = 3,
            IdSchedule2 = 4
        };

        context.Removals.AddRange(removal1, removal2);
        context.SaveChanges();

        schedule1.RemovalAsFirst = removal1;
        schedule2.RemovalAsSecond = removal1;
        schedule3.RemovalAsFirst = removal2;
        schedule4.RemovalAsSecond = removal2;

        context.Schedules.UpdateRange(schedule1, schedule2, schedule3, schedule4);
        context.SaveChanges();

        var subjectTeacherSchedule1 = new SubjectTeacherSchedule
        {
            IdsubjectTeacher = subjectTeacher1.IdsubjectTeacher,
            IdSchedule = schedule1.IdSchedule,
            GroupSplit = false,
            LectureHours1term = 20,
            PracticalHours1term = 10,
            IdsubjectTeacherNavigation = subjectTeacher1,
            IdScheduleNavigation = schedule1
        };

        var subjectTeacherSchedule2 = new SubjectTeacherSchedule
        {
            IdsubjectTeacher = subjectTeacher2.IdsubjectTeacher,
            IdSchedule = schedule2.IdSchedule,
            GroupSplit = true,
            LectureHours1term = 15,
            PracticalHours1term = 15,
            IdsubjectTeacherNavigation = subjectTeacher2,
            IdScheduleNavigation = schedule2
        };

        var subjectTeacherSchedule3 = new SubjectTeacherSchedule
        {
            IdsubjectTeacher = subjectTeacher3.IdsubjectTeacher,
            IdSchedule = schedule3.IdSchedule,
            GroupSplit = false,
            LectureHours2term = 25,
            PracticalHours2term = 20,
            IdsubjectTeacherNavigation = subjectTeacher3,
            IdScheduleNavigation = schedule3
        };

        var subjectTeacherSchedule4 = new SubjectTeacherSchedule
        {
            IdsubjectTeacher = subjectTeacher4.IdsubjectTeacher,
            IdSchedule = schedule4.IdSchedule,
            GroupSplit = true,
            LectureHours1term = 10,
            PracticalHours1term = 5,
            IdsubjectTeacherNavigation = subjectTeacher4,
            IdScheduleNavigation = schedule4
        };

        context.SubjectTeacherSchedules.AddRange(subjectTeacherSchedule1, subjectTeacherSchedule2, subjectTeacherSchedule3, subjectTeacherSchedule4);
        context.SaveChanges();
    }
}
