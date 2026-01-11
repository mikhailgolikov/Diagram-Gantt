namespace Gantt_Chart_Backend.Exceptions;

public class DependenceAlreadyExistsException(string message = "Dependence already exists") 
    : Exception(message);