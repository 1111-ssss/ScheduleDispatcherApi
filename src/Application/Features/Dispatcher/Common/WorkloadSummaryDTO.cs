namespace Application.Features.Dispatcher.Common;

public record WorkloadSummaryDTO(
    List<WorkloadDTO> WorkloadList,
    List<RemovalDTO> RemovalList
);