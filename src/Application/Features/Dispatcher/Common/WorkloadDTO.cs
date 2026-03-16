namespace Application.Features.Dispatcher.Common;

public record WorkloadDTO(
    string LessonName,
    int LessonIndex,
    bool IsLessonSplit,
    DateTime LessonDate1,
    DateTime LessonDate2,
    int RemovalIndex
);