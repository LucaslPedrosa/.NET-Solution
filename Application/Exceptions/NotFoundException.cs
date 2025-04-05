using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Application.Exceptions;

public class NotFoundException : BaseApplicationException
{
    public NotFoundException(string message)
        : base(message, HttpStatusCode.NotFound) { }
}
