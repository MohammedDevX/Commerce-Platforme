using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Queries;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Abstractions.Storage;
using Catalogue.Application.Common.Options;
using Catalogue.Application.Companies.Command.Create;
using Catalogue.Application.Companies.Commands.Activate;
using Catalogue.Application.Companies.Commands.Deactivate;
using Catalogue.Application.Companies.Commands.Delete;
using Catalogue.Application.Companies.Commands.Update;
using Catalogue.Application.Companies.Queries.GetCompanies;
using Catalogue.Application.Companies.Queries.GetCompanyById;
using Catalogue.Application.Products.Commands.Activate;
using Catalogue.Application.Products.Commands.Create;
using Catalogue.Application.Products.Commands.Desactivate;
using Catalogue.Application.Products.Commands.ProductImage.AddImage;
using Catalogue.Application.Products.Commands.ProductImage.SetPrimaryImage;
using Catalogue.Application.Products.Commands.Update;
using Catalogue.Application.Products.Queries.GetProductById;
using Catalogue.Application.Products.Queries.GetProducts;
using Catalogue.Application.ProductVariants.Commands.Create;
using Catalogue.Infrastructure.Options;
using Catalogue.Infrastructure.Persistence;
using Catalogue.Infrastructure.Persistence.Queries;
using Catalogue.Infrastructure.Persistence.Repositories;
using Catalogue.Infrastructure.Persistence.Storage;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

string connectionString = builder.Configuration.GetConnectionString("catalogueConnectionString") ??
        throw new InvalidOperationException("Catalogue Connection String is missing!");
builder.Services.AddDbContext<CatalogueDbContext>(opt =>
{
    opt.UseSqlServer(connectionString);
});

// Repositories & Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

// Queries
builder.Services.AddScoped<IProductQueries, ProductQueries>();
builder.Services.AddScoped<ICompanyQueries, CompanyQueries>();

// Storage
builder.Services.AddScoped<IFileStorage, LocalFileStorage>();

// Handlers - Company
builder.Services.AddScoped<CreateCompanyHandler>();
builder.Services.AddScoped<GetCompanyByIdHandler>();
builder.Services.AddScoped<GetCompaniesHandler>();
builder.Services.AddScoped<UpdateCompanyHandler>();
builder.Services.AddScoped<DeleteCompanyHandler>();
builder.Services.AddScoped<DeactivateCompanyHandler>();
builder.Services.AddScoped<ActivateCompanyHandler>();

// Handlers - Product
builder.Services.AddScoped<CreateProductHandler>();
builder.Services.AddScoped<GetProductByIdHandler>();
builder.Services.AddScoped<GetProductsHandler>();
builder.Services.AddScoped<UpdateProductHandler>();
builder.Services.AddScoped<ActivateProductHandler>();
builder.Services.AddScoped<DesactivateProductHandler>();
builder.Services.AddScoped<AddProductImageHandler>();
builder.Services.AddScoped<SetPrimaryImageHandler>();

// Handlers - ProductVariant
builder.Services.AddScoped<CreateProductVariantHandler>();

// Options
builder.Services.AddOptions<ImageStorageOptions>()
    .Bind(builder.Configuration.GetSection(ImageStorageOptions.SectionName))
    .ValidateOnStart();

builder.Services.AddOptions<ImageStorageRulesOptions>()
    .Bind(builder.Configuration.GetSection(ImageStorageRulesOptions.SectionName))
    .ValidateOnStart();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
