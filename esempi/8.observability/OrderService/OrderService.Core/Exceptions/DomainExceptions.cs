// Core/Exceptions/DomainExceptions.cs
namespace OrderService.Core.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
