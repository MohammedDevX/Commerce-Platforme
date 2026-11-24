using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Companies.Commands.Deactivate
{
    public class DeactivateCompanyHandler
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateCompanyHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeactivateCompanyCommand command, CancellationToken ct)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(command.Id, ct)
                ?? throw new NotFoundException(nameof(Company), command.Id);

            company.Deactivate();

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
