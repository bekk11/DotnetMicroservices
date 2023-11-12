using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.SyncDataServices.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepo _platformRepo;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepo platformRepo, IMapper mapper)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
    }

    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        PlatformResponse response = new PlatformResponse();
        IEnumerable<Platform> platforms = _platformRepo.GetAllPlatforms();

        foreach (var platform in platforms)
        {
            response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));
        }

        return Task.FromResult(response);
    }
}