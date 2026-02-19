namespace Application.Features.Dispatcher.GetAllLessons;

public record LessonInfoDTO(
    string LessonName,
    int Semester,
    int Course,
    List<string> Groups
);