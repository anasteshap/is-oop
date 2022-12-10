namespace Banks.Exceptions;

public class ClientException : Exception
{
    private ClientException(string message)
        : base(message)
    {
    }

    public static ClientException InvalidAddress()
    {
        return new ClientException("Invalid address");
    }

    public static ClientException InvalidPassport(string? message = null)
    {
        return new ClientException($"Invalid passport\n{message}");
    }

    public static ClientException ClientAlreadyExists(Guid id)
    {
        return new ClientException($"Client with id: {id.ToString()} already exists");
    }

    public static ClientException ClientDoesNotExist(Guid id)
    {
        return new ClientException($"Client with id: {id.ToString()} doesn't exist");
    }
}