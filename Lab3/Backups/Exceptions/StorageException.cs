namespace Backups.Exceptions;

public class StorageException : Exception
{
    private StorageException(string message)
        : base(message) { }

    public static StorageException ZipStorageAlreadyExists(string path)
    {
        return new StorageException($"ZipStorage {path} already exists");
    }
}