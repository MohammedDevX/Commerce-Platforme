namespace Catalogue.Application.Common.Exceptions
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string entityName, Guid key) : base($"{entityName} with key {key} not found")
        {}
    }
}
