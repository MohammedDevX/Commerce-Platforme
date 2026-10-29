namespace Catalogue.Application.Common.Exceptions
{
    public sealed class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {}

        public ConflictException(string entityName, Guid key) : base($"{entityName} with key {key} already existe")
        {}

    }
}
