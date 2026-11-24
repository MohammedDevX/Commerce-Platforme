using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Companies.Commands.Activate
{
    public class ActivateCompanyHandler
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateCompanyHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ActivateCompanyCommand command, CancellationToken ct)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(command.Id, ct)
                ?? throw new NotFoundException(nameof(Company), command.Id);

            company.Activate();

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
