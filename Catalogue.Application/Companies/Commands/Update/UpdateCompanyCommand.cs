namespace Catalogue.Application.Companies.Commands.Update
{
    public sealed record UpdateCompanyCommand(Guid Id, string Name)
    {
    }
}
