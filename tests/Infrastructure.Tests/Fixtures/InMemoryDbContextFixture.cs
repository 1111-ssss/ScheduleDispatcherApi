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

        var teacher1 = new Teacher
        {
            CnT = "T001",
            CnE = "E001"
        };

        var teacher2 = new Teacher
        {
            CnT = "T002",
            CnE = "E002"
        };

        var teacher3 = new Teacher
        {
            CnT = "T003",
            CnE = "E001"
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
            CnG = "П21",
            Name = "П21",
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
            CnT = teacher1.CnT
        };

        var subjectTeacher2 = new SubjectTeacher
        {
            IdSubject = subject1.IdSubject,
            CnG = group2.CnG,
            CnT = teacher1.CnT
        };

        var subjectTeacher3 = new SubjectTeacher
        {
            IdSubject = subject2.IdSubject,
            CnG = group1.CnG,
            CnT = teacher2.CnT
        };

        var subjectTeacher4 = new SubjectTeacher
        {
            IdSubject = subject3.IdSubject,
            CnG = group3.CnG,
            CnT = teacher3.CnT
        };

        context.SubjectTeachers.AddRange(subjectTeacher1, subjectTeacher2, subjectTeacher3, subjectTeacher4);
        context.SaveChanges();

        var semester1 = new SubjectTeacherSemester
        {
            IdsubjectTeacher = subjectTeacher1.IdsubjectTeacher,
            Semester1 = true,
            Semester2 = false
        };

        var semester2 = new SubjectTeacherSemester
        {
            IdsubjectTeacher = subjectTeacher2.IdsubjectTeacher,
            Semester1 = true,
            Semester2 = true
        };

        var semester3 = new SubjectTeacherSemester
        {
            IdsubjectTeacher = subjectTeacher3.IdsubjectTeacher,
            Semester1 = false,
            Semester2 = true
        };

        var semester4 = new SubjectTeacherSemester
        {
            IdsubjectTeacher = subjectTeacher4.IdsubjectTeacher,
            Semester1 = true,
            Semester2 = false
        };

        context.SubjectTeacherSemesters.AddRange(semester1, semester2, semester3, semester4);
        context.SaveChanges();
    }
}
