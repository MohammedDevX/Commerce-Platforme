using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Domaine.Entities;
using CommercePlatform.Api.src.Dtos.Responses;

namespace Catalogue.Application.Companies.Command.Create
{
    public class CreateCompanyHandler
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCompanyHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateCompanyResponseDto> Handle(CreateCompanyCommand command, CancellationToken ct)
        {
            Company company = new(command.Name);
            _companyRepository.Add(company, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateCompanyResponseDto(company.Id, company.Name);
        }
    }
}
