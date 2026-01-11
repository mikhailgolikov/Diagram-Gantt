namespace Gantt_Chart_Backend.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message = "Resource not found") 
        : base(message) { }
}