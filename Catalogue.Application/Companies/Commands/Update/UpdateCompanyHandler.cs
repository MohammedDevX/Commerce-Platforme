using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Exceptions;
using Catalogue.Domaine.Entities;

namespace Catalogue.Application.Companies.Commands.Update
{
    public class UpdateCompanyHandler
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCompanyHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCompanyCommand command, CancellationToken ct)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(command.Id, ct)
                ?? throw new NotFoundException(nameof(Company), command.Id);

            // Company already tracked by EF so he detect changes
            company.Rename(command.Name);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
