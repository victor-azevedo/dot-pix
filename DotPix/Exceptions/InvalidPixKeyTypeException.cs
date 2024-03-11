using DotPix.Models;

namespace DotPix.Exceptions;

public class InvalidPixKeyTypeException(
    string message = "Invalid value for 'Type'. Must be one of: 'CPF', 'Email', 'Phone' or 'Random'")
    : Exception(message);