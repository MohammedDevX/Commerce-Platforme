using Catalogue.Domaine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Products.Commands.Create
{
    public sealed record CreateProductResponseDto(Guid Id, 
        Guid CompanyId, 
        string Name, 
        string Status,
        string Description,
        Guid BrandId)
    {
    }
}
