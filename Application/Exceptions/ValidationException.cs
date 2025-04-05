using System.Net;

namespace Application.Exceptions;

public class ValidationException : BaseApplicationException
{
    public ValidationException(string message)
        : base(message, HttpStatusCode.BadRequest) { }
}