using AutoMapper;
using LeadwaycanteenApi.Entities.Dtos.GeneralDto;
using LeadwaycanteenApi.Entities.Dtos.ResponseDto;

namespace LeadwaycanteenApi.Profiler
{
    /// <summary>
    /// Defines object-to-object mappings for authentication-related entities using AutoMapper.
    /// </summary>
    public class VendorProfiler : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthProfiler"/> class and defines mappings
        /// between models and data transfer objects (DTOs).
        /// </summary>
        public VendorProfiler()
        {
            CreateMap<VendorResponse, VendorDto>();
            CreateMap<VendorDto, VendorResponse>();
            CreateMap<VendorDto, VendorResponseDto>();
            CreateMap<VendorResponseDto, VendorDto>();
        }
    }
}
