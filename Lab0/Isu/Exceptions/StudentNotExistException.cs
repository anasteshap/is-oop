using Isu.Entities;
namespace Isu.Exceptions;

public class StudentNotExistException : Exception
{
    private StudentNotExistException(string message)
        : base(message) { }

    public static StudentNotExistException StudentNotExist(int id)
    {
        return new StudentNotExistException($"Student with id {id} isn't exists");
    }
}