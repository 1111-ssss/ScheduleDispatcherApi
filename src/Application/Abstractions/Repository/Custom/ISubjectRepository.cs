using Application.Abstractions.Repository.Base;
using Application.Features.Dispatcher.GetAllLessons;
using Ardalis.Specification;
using Infrastructure.src.Domain.Entities;

namespace Application.Abstractions.Repository.Custom;

public interface ISubjectRepository : IBaseRepository<Subject>
{
    Task<List<LessonInfoDTO>> GetAllLessons(Specification<Subject> specification, CancellationToken ct = default);
}