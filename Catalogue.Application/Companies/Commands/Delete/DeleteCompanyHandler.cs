using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Companies.Commands.Delete
{
    public class DeleteCompanyHandler
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCompanyHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCompanyCommand command, CancellationToken ct)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(command.Id, ct)
                ?? throw new NotFoundException(nameof(Company), command.Id);

            _companyRepository.Delete(company);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
