namespace Application.Features.Dispatcher.GetAllLessons;

public record LessonInfoDTO(
    string LessonName,
    int Semester,
    int Course,
    Dictionary<string, List<string>> Groups
);