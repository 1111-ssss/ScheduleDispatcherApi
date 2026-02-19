namespace Application.Features.Dispatcher.Common;

public record RemovalDTO(
    int RemovalIndex,
    int FirstLessonIndex,
    int SecondLessonIndex
);