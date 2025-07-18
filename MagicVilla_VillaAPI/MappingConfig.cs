﻿using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        //villa
        CreateMap<Villa, VillaDTO>().ReverseMap();

        CreateMap<Villa, VillaCreateDTO>().ReverseMap();

        CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

        //villanumber
        CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();

        CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();

        CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();

        //Autentication
        CreateMap<LocalUser, LoginRequestDTO>().ReverseMap();
        CreateMap<LocalUser, RegistrationRequestDTO>().ReverseMap();

        CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        CreateMap<ApplicationUser, RegistrationRequestDTO>().ReverseMap();

    }
}
