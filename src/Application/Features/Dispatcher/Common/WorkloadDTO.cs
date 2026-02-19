namespace Application.Features.Dispatcher.Common;

public record WorkloadDTO(
    string LessonName,
    int LessonIndex,
    bool IsLessonSplit,
    DateTime LessonDate,
    int RemovalIndex
);