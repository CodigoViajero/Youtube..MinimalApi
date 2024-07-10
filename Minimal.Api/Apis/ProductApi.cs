using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using AutoMapper;
using Azure;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Minimal.Api.Data;
using Minimal.Api.Dtos;
using Minimal.Api.Model;
using Newtonsoft.Json;

namespace Minimal.Api.Apis;

public static class ProductApi
{
    public static WebApplication MapProductApi(this WebApplication app)
    {
        var api = app.MapGroup("/api/product").RequireAuthorization("MyPolicy");

        api.MapGet("", GetAllProducts);

        api.MapGet("/{id}", GetProductById).WithName("GetProductById");

        api.MapPost("", AddProduct);
 
        api.MapPut("/{id}", UpdateProduct);

        api.MapPatch("/{id}", PartialUpdate);

        api.MapDelete("/{id}", DeleteProduct);

        return app;
    }

    private static async Task<IResult> GetAllProducts(IRepo repo, IMapper mapper)
    {
        var result = await repo.GetAllAsync();
        return Results.Ok(mapper.Map<IEnumerable<ProductReadDto>>(result));
    }

    private static async Task<IResult> GetProductById(int id, IRepo repo, IMapper mapper)
    {
        var product = await repo.GetBydId(id);
        if (product == null)
        {
            return Results.NotFound(product);
        }
        return Results.Ok(mapper.Map<ProductReadDto>(product));
    }

    private static async Task<IResult> AddProduct([FromBody] ProductDto productDto, IMapper mapper, IRepo repo, IValidator<ProductDto> validator)
    {
        var state = await validator.ValidateAsync(productDto);
        if (!state.IsValid)
        {
            return Results.ValidationProblem(state.ToDictionary());
        }
        var product = mapper.Map<Product>(productDto);
        await repo.AddAsync(product);

        var productReadDto = mapper.Map<ProductReadDto>(product);
        return Results.CreatedAtRoute(nameof(GetProductById), new { Id = product.Id }, productReadDto);
    }

    private static async Task<IResult> UpdateProduct(int id, [FromBody] ProductDto productDto, IValidator<ProductDto> validator, IMapper mapper, IRepo repo)
    {
        var state = await validator.ValidateAsync(productDto);
        if (!state.IsValid)
        {
            return Results.ValidationProblem(state.ToDictionary());
        }

        var productModel = await repo.GetBydId(id);
        if (productModel == null)
        {
            return Results.NotFound();
        }

        mapper.Map(productDto, productModel);
        await repo.UpdateAsync(productModel);

        return Results.NoContent();
    }

    public static async Task<IResult> PartialUpdate(int id, [FromBody] JsonElement jsonElement, IMapper mapper, IRepo repo)
    {
        var productModel = await repo.GetBydId(id);
        if (productModel == null)
        {
            return Results.NotFound();
        }
        var json = jsonElement.GetRawText();
        var patchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<ProductDto>>(json);

        var productToPatch = mapper.Map<ProductDto>(productModel);

        try
        {
            patchDocument!.ApplyTo(productToPatch);
        }
        catch (JsonPatchException e)
        {
            return Results.BadRequest(new { statusCode = 400, message = e.Message });
        }

        mapper.Map(productToPatch, productModel);

        await repo.UpdateAsync(productModel);

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteProduct(int id, IRepo repo)
    {
        var product = await repo.GetBydId(id);
        await repo.DeleteAsync(product);

        return Results.NoContent();
    }
}