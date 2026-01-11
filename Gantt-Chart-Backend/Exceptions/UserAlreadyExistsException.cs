namespace Gantt_Chart_Backend.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message = "User already exists") 
        : base(message) { }
}