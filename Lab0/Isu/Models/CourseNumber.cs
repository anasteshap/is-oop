using Isu.Exceptions;

namespace Isu.Models;

public class CourseNumber
{
    public CourseNumber(int number)
    {
        if (number is > 4 or < 1)
        {
            throw new CourseNumberException($"Course {number} isn't exist");
        }

        Number = number;
    }

    public int Number { get; }
}