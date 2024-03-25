namespace DotPixApi.Exceptions;

public class AccountNotFoundException(string message = "Account not found for this user")
    : Exception(message);