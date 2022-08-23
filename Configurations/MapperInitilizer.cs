﻿using AutoMapper;
using HotelListing.Data;
using HotelListing.Dto;

namespace HotelListing.Configurations;

public class MapperInitilizer : Profile
{
    public MapperInitilizer()
    {
        CreateMap<Country, CountryDTO>().ReverseMap();
        CreateMap<Country, CreateCountryDTO>().ReverseMap();
        CreateMap<Hotel, HotelDTO>().ReverseMap();
        CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
    }
}