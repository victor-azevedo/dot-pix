namespace DotPix.Exceptions;

public class PixKeyNotFoundException(
    string message = "Key value not found. Please check and try again.") : Exception(message);