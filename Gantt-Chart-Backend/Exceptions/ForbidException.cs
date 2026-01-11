namespace Gantt_Chart_Backend.Exceptions;

public class ForbidException : Exception
{
    public ForbidException (string message = "Forbidden") 
        : base(message) { }
}