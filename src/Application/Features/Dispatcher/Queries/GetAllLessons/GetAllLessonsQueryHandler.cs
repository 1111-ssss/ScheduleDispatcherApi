using Application.Abstractions.Repository.Custom;
using Domain.Model.Result;
using Domain.Specifications.Dispatcher;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Dispatcher.GetAllLessons;

public class GetAllLessonsQueryHandler : IRequestHandler<GetAllLessonsQuery, Result<AllLessonsDTO>>
{
    private readonly ILogger<GetAllLessonsQueryHandler> _logger;
    private readonly ISubjectRepository _subjectRepository;

    public GetAllLessonsQueryHandler(
        ILogger<GetAllLessonsQueryHandler> logger,
        ISubjectRepository subjectRepository
    )
    {
        _logger = logger;
        _subjectRepository = subjectRepository;
    }

    public async Task<Result<AllLessonsDTO>> Handle(GetAllLessonsQuery query, CancellationToken ct)
    {
        var subjects = await _subjectRepository.GetAllLessons(new GetAllLessonsSpec(), ct);

        return Result<AllLessonsDTO>.Success(
            new AllLessonsDTO(
                Lessons: subjects
            )
        );
    }
}