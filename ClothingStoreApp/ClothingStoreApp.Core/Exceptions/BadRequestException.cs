using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothingStoreApp.Core.Exceptions;
public class BadRequestException : Exception
{
    public string Param { get; }
    public BadRequestException(string message, string param) : base(message)
    {
        Param = param;
    }

}