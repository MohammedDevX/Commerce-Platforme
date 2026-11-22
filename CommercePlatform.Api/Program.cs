using Catalogue.Application.Common.Abstractions.Persistence;
using Catalogue.Application.Common.Abstractions.Persistence.Queries;
using Catalogue.Application.Common.Abstractions.Persistence.Repositories;
using Catalogue.Application.Common.Abstractions.Storage;
using Catalogue.Application.ProductImage.Create;
using Catalogue.Application.Products.Commands.Activate;
using Catalogue.Application.Products.Commands.Create;
using Catalogue.Application.Products.Commands.Desactivate;
using Catalogue.Application.Products.Commands.Update;
using Catalogue.Application.Products.Queries.GetProductById;
using Catalogue.Application.Products.Queries.GetProducts;
using Catalogue.Application.ProductVariants.Commands.Create;
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

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductQueries, ProductQueries>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ActivateProductHandler>();
builder.Services.AddScoped<DesactivateProductHandler>();
builder.Services.AddScoped<CreateProductHandler>();
builder.Services.AddScoped<UpdateProductHandler>();
builder.Services.AddScoped<GetProductByIdHandler>();
builder.Services.AddScoped<GetProductsHandler>();
builder.Services.AddScoped<CreateProductVariantHandler>();
builder.Services.AddScoped<IFileStorage, LocalFileStorage>();
builder.Services.AddScoped<UploadImageHandler>();


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
