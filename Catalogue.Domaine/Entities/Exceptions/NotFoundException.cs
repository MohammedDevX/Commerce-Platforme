namespace Catalogue.Domaine.Entities.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, Guid id) : base($"{entity} whit Id : {id} not found")
        {}
    }
}
