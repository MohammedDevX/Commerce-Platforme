using Catalogue.Application.Common.Models;
using Catalogue.Application.Companies.Command.Create;
using Catalogue.Application.Companies.Commands.Activate;
using Catalogue.Application.Companies.Commands.Deactivate;
using Catalogue.Application.Companies.Commands.Delete;
using Catalogue.Application.Companies.Commands.Update;
using Catalogue.Application.Companies.Queries.GetCompanies;
using Catalogue.Application.Companies.Queries.GetCompanyById;
using CommercePlatform.Api.src.Dtos.Requests.Companies;
using Microsoft.AspNetCore.Mvc;

namespace CommercePlatform.Api.src.Modules.Catalogue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CreateCompanyHandler _createCompanyHandler;
        private readonly GetCompanyByIdHandler _getCompanyByIdHandler;
        private readonly GetCompaniesHandler _getCompaniesHandler;
        private readonly UpdateCompanyHandler _updateCompanyHandler;
        private readonly DeleteCompanyHandler _deleteCompanyHandler;
        private readonly DeactivateCompanyHandler _deactivateCompanyHandler;
        private readonly ActivateCompanyHandler _activateCompanyHandler;

        public CompanyController(
            CreateCompanyHandler createCompanyHandler,
            GetCompanyByIdHandler getCompanyByIdHandler,
            GetCompaniesHandler getCompaniesHandler,
            UpdateCompanyHandler updateCompanyHandler,
            DeleteCompanyHandler deleteCompanyHandler,
            DeactivateCompanyHandler deactivateCompanyHandler,
            ActivateCompanyHandler activateCompanyHandler)
        {
            _createCompanyHandler = createCompanyHandler;
            _getCompanyByIdHandler = getCompanyByIdHandler;
            _getCompaniesHandler = getCompaniesHandler;
            _updateCompanyHandler = updateCompanyHandler;
            _deleteCompanyHandler = deleteCompanyHandler;
            _deactivateCompanyHandler = deactivateCompanyHandler;
            _activateCompanyHandler = activateCompanyHandler;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCompanyByIdAsync(Guid id, CancellationToken ct)
        {
            var query = new GetCompanyByIdQuery(id);
            var companyDetails = await _getCompanyByIdHandler.Handle(query, ct);

            return Ok(companyDetails);
        }

        [HttpGet]
        public async Task<ActionResult<PageResult<CompanyListItemDto>>> GetAllAsync(
            [FromQuery] GetCompaniesQuery query,
            CancellationToken ct)
        {
            var result = await _getCompaniesHandler.Handle(query, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddCompanyAsync(CreateCompanyRequestDto dto, CancellationToken ct)
        {
            var command = new CreateCompanyCommand(dto.Name);
            var response = await _createCompanyHandler.Handle(command, ct);

            return CreatedAtAction(nameof(GetCompanyByIdAsync), new { response.Id}, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(UpdateCompanyRequestDto dto, CancellationToken ct)
        {
            var command = new UpdateCompanyCommand(dto.Id, dto.Name);
            await _updateCompanyHandler.Handle(command, ct);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(DeleteCompanyCommand command, CancellationToken ct)
        {
            await _deleteCompanyHandler.Handle(command, ct);
            return NoContent();
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult> DeactivateAsync(DeactivateCompanyCommand command, CancellationToken ct)
        {
            await _deactivateCompanyHandler.Handle(command, ct);

            return NoContent();
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult> ActivateAsync(ActivateCompanyCommand command, CancellationToken ct)
        {
            await _activateCompanyHandler.Handle(command, ct);

            return NoContent();
        }
    }
}
