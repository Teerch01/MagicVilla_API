using AutoMapper;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        //villa
        CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();

        CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

        //villanumber
        CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();

        CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();

    }
}
