using AutoMapper;
using Minimal.Api.Model;

namespace Minimal.Api.Dtos;

public class Profiles : Profile
{
    public Profiles()
    {
        // Origen => Destino
        CreateMap<Product, ProductReadDto>();
        CreateMap<ProductDto, Product>();
        CreateMap<Product, ProductDto>();
    }
}