namespace Application.Features.Dispatcher.GetAllLessons;

public record AllLessonsDTO(
    List<LessonInfoDTO> Lessons
);