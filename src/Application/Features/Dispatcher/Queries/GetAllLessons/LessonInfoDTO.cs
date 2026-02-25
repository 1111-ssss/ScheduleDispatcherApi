namespace Application.Features.Dispatcher.GetAllLessons;

public record LessonInfoDTO(
    string LessonName,
    bool Semester1,
    bool Semester2,
    int Course,
    Dictionary<string, List<string>> Groups
);