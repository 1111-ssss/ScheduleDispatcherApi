namespace Application.Features.Dispatcher.Common;

public record LessonDTO(
    DateTime StartTime,
    DateTime? EndTime,
    string Lesson1Name,
    string? Lesson2Name,
    string Teacher1FullName,
    string? Teacher2FullName,
    string Classroom1,
    string? Classroom2
);