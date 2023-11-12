using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Profiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformReadDto, PlatformPublishDto>();
        CreateMap<Platform, GrpcPlatformModel>()
            .ForMember(x => x.PlatformId, opt => opt.MapFrom(src => src.Id))
            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(x => x.Publisher, opt => opt.MapFrom(src => src.Publisher));
    }
}