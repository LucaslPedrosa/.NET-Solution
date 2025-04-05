using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Application.Exceptions;

public class BaseApplicationException : Exception
{
    public HttpStatusCode HttpStatusCode { get; }

    public BaseApplicationException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        : base(message)
    {
        HttpStatusCode = httpStatusCode;
    }
}