using Gantt_Chart_Backend.Data.Enums;

namespace Gantt_Chart_Backend.Data.DTOs;

public class ProjectResponse<T>
{
    public bool IsSuccess;
    public T? Result;
    public ResultType ResultType;

    public static ProjectResponse<T> Success(T? result) => new()
    {
        IsSuccess = true,
        Result = result,
        ResultType = ResultType.Success,
    };

    public static ProjectResponse<T> Forbidden () => new()
    {
        IsSuccess = false,
        Result = default,
        ResultType = ResultType.Forbidden
    };

    public static ProjectResponse<T> NotFound() => new()
    {
        IsSuccess = true,
        Result = default,
        ResultType = ResultType.NotFound
    };
}
