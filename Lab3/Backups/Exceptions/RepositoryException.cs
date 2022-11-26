namespace Backups.Exceptions;

public class RepositoryException : Exception
{
    private RepositoryException(string message)
        : base(message) { }

    public static RepositoryException DirectoryDoesNotExist(string path)
    {
        return new RepositoryException($"Directory {path} doesn't exist");
    }

    public static RepositoryException FileDoesNotExist(string path)
    {
        return new RepositoryException($"File {path} doesn't exist");
    }

    public static RepositoryException ObjectDoesNotExist(string path)
    {
        return new RepositoryException($"Object {path} doesn't exist");
    }
}
