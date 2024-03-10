namespace DotPix.Exceptions;

public class ConflictException(string message = "Conflict occurred. Please check and try again.") : Exception(message);