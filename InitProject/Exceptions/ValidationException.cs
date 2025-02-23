using InitProject.Models;

namespace InitProject.Exceptions;
public class ValidationException : Exception
{
    public readonly List<ValidationResponse> validationResponses;

    public ValidationException(){
        validationResponses = new List<ValidationResponse>();
    }
}